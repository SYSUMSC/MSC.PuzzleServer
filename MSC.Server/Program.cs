using AspNetCoreRateLimit;
using JavaScriptEngineSwitcher.Extensions.MsDependencyInjection;
using JavaScriptEngineSwitcher.V8;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MSC.Server.Extensions;
using MSC.Server.Hubs;
using MSC.Server.Middlewares;
using MSC.Server.Models;
using MSC.Server.Repositories;
using MSC.Server.Repositories.Interface;
using MSC.Server.Services;
using MSC.Server.Services.Interface;
using MSC.Server.Utils;
using NJsonSchema.Generation;
using NLog;
using NLog.Web;
using NLog.Targets;
using NSwag;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.Mvc;
using React.AspNet;

namespace MSC.Server;

class Program
{
    public static string[] DistJsFiles = Array.Empty<string>();
    public static string[] DistCssFiles = Array.Empty<string>();

    static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

        #region Configuration

        builder.Host.ConfigureAppConfiguration((host, config) =>
        {
            config.AddJsonFile("ratelimit.json", optional: true, reloadOnChange: true);
        });

        #endregion

        #region SignalR

        builder.Services.AddSignalR().AddJsonProtocol();
        builder.Services.AddSingleton<SignalRLoggingService>();

        #endregion SignalR

        #region Logging

        builder.Host.ConfigureLogging(logging =>
        {
            logging.ClearProviders();
            logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
        }).UseNLog();

        Target.Register<SignalRTarget>("SignalR");
        LogManager.Configuration.Variables["connectionString"] = builder.Configuration.GetConnectionString("DefaultConnection");

        #endregion

        #region AppDbContext

        builder.Services.AddDbContext<AppDbContext>(
            options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
            provideropt => provideropt.EnableRetryOnFailure(3, TimeSpan.FromSeconds(10), null)));

        #endregion

        #region OpenApiDocument

        builder.Services.AddOpenApiDocument(settings =>
        {
            settings.DocumentName = "v1";
            settings.Version = "v0.1";
            settings.Title = "MSC Puzzle API";
            settings.Description = "MSC Puzzle 接口文档";
            settings.UseControllerSummaryAsTagDescription = true;
            settings.SerializerSettings = SystemTextJsonUtilities.ConvertJsonOptionsToNewtonsoftSettings(new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
            settings.DefaultReferenceTypeNullHandling = ReferenceTypeNullHandling.NotNull;
        });

        #endregion OpenApiDocument

        #region MemoryCache

        builder.Services.AddMemoryCache();

        #endregion MemoryCache

        #region Identity

        builder.Services.AddAuthentication(o =>
        {
            o.DefaultScheme = IdentityConstants.ApplicationScheme;
            o.DefaultSignInScheme = IdentityConstants.ExternalScheme;
        }).AddIdentityCookies();

        builder.Services.AddIdentityCore<UserInfo>(options =>
        {
            options.User.RequireUniqueEmail = true;
            options.Password.RequireNonAlphanumeric = false;
            options.SignIn.RequireConfirmedEmail = true;
        }).AddSignInManager<SignInManager<UserInfo>>()
        .AddUserManager<UserManager<UserInfo>>()
        .AddEntityFrameworkStores<AppDbContext>()
        .AddErrorDescriber<TranslatedIdentityErrorDescriber>()
        .AddDefaultTokenProviders();

        builder.Services.Configure<DataProtectionTokenProviderOptions>(o =>
            o.TokenLifespan = TimeSpan.FromHours(3));

        #endregion Identity

        #region IP Rate Limit

        //从appsettings.json获取相应配置
        builder.Services.Configure<IpRateLimitOptions>(builder.Configuration.GetSection("IpRateLimiting"));

        //注入计数器和规则存储
        builder.Services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();
        builder.Services.AddSingleton<IIpPolicyStore, DistributedCacheIpPolicyStore>();
        builder.Services.AddSingleton<IRateLimitCounterStore, DistributedCacheRateLimitCounterStore>();
        builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

        #endregion IP Rate Limit

        #region Services and Repositories

        builder.Services.AddTransient<IMailSender, MailSender>();

        builder.Services.AddScoped<ILogRepository, LogRepository>();
        builder.Services.AddScoped<IRankRepository, RankRepository>();
        builder.Services.AddScoped<IPuzzleRepository, PuzzleRepository>();
        builder.Services.AddScoped<ISubmissionRepository, SubmissionRepository>();
        builder.Services.AddScoped<IAnnouncementRepository, AnnouncementRepository>();

        #endregion Services and Repositories

        builder.Services.AddResponseCompression(options =>
        {
            options.Providers.Add<BrotliCompressionProvider>();
            options.Providers.Add<GzipCompressionProvider>();
            options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(new[] {
                    "image/svg+xml",
                    "image/png",
                    "font/woff",
                    "font/woff2",
                    "font/ttf",
                    "font/eof",
                    "image/x-icon" });
        });

        builder.Services.AddControllersWithViews().ConfigureApiBehaviorOptions(options =>
        {
            options.InvalidModelStateResponseFactory = context =>
            {
                var errmsg = context.ModelState.Values.FirstOrDefault()?.Errors.FirstOrDefault()?.ErrorMessage;
                return new JsonResult(new RequestResponse(errmsg ?? "验证失败"))
                {
                    StatusCode = 400
                };
            };
        });

        if (builder.Environment.IsProduction())
        {
            builder.Services.AddReact();

            builder.Services.AddJsEngineSwitcher(options =>
            {
                options.DefaultEngineName = V8JsEngine.EngineName;
            }).AddV8();
        }

        var app = builder.Build();

        app.UseResponseCompression();

        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseOpenApi();
        }
        else
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();

            const string distRoot = "wwwroot";
            if (Directory.Exists($"{distRoot}/static/js"))
            {
                DistJsFiles = 
                    Directory.GetFiles($"{distRoot}/static/js", "*.js")
                        .Select(file => $"~{file[(file.LastIndexOf(distRoot) + distRoot.Length)..]}")
                        .ToArray();
            }

            if (Directory.Exists($"{distRoot}/static/css"))
            {
                DistCssFiles = Directory.GetFiles($"{distRoot}/static/css", "*.css")
                        .Select(file => $"~{file[(file.LastIndexOf(distRoot) + distRoot.Length)..]}")
                        .ToArray();
            }

            if (DistJsFiles.Any())
            {
                app.UseReact(config =>
                {
                    config.LoadBabel = false;
                    config.LoadReact = false;
                    config.ReuseJavaScriptEngines = true;
                    config.AllowJavaScriptPrecompilation = true;
                    config.UseServerSideRendering = true;
                    foreach (var path in DistJsFiles)
                    {
                        config.AddScriptWithoutTransform(path);
                    }
                });
            }

            app.UseOpenApi(options =>
            {
                options.PostProcess += (document, _) =>
                {
                    document.Servers.Clear();
                    document.Servers.Add(new OpenApiServer
                    {
                        Url = "https://puzzle.sysums.club"
                    });
                };
            });
        }

        app.UseSwaggerUi3();

        app.UseMiddleware<ProxyMiddleware>();
        app.UseIpRateLimiting();

        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapHub<LoggingHub>("/hub/log");

            if (app.Environment.IsProduction())
            {
                endpoints.MapControllerRoute(
                    name: "frontend",
                    pattern: "{path?}",
                    defaults: new { Controller = "Home", Action = "Index" });
                endpoints.MapControllerRoute(
                    name: "frontend-params",
                    pattern: "{path?}/{**params}",
                    defaults: new { Controller = "Home", Action = "Index" });
            }
            else
            {
                endpoints.MapFallbackToFile("index.html");
            }
        });

        var logger = LogManager.GetLogger("Main");

        try
        {
            LogHelper.SystemLog(logger, "服务器初始化。");
            await app.RunAsync();
        }
        catch (Exception exception)
        {
            logger.Error(exception, "因异常，应用程序意外停止。");
            throw;
        }
        finally
        {
            LogManager.Shutdown();
        }

    }
}
