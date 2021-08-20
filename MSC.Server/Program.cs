using AspNetCoreRateLimit;
using Microsoft.AspNetCore.Identity;
using MSC.Server.Extensions;
using MSC.Server.Hubs;
using MSC.Server.Middlewares;
using MSC.Server.Models;
using MSC.Server.Services;
using MSC.Server.Utils;
using NLog;
using NLog.Web;
using System;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using MSC.Server.Services.Interface;
using MSC.Server.Repositories.Interface;
using MSC.Server.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Host.ConfigureAppConfiguration((host, config) =>
{
    config.AddJsonFile("RateLimitConfig.json", optional: true, reloadOnChange: true);
}).ConfigureLogging(logging =>
{
    logging.ClearProviders();
    logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
}).UseNLog();

builder.Services.AddDbContext<AppDbContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
    provideropt => provideropt.EnableRetryOnFailure(3, TimeSpan.FromSeconds(10), null)));

#region OpenApiDocument
builder.Services.AddOpenApiDocument(settings =>
{
    settings.DocumentName = "v1";
    settings.Version = "v0.1";
    settings.Title = "MSC Puzzle API";
    settings.Description = "MSC Puzzle 接口文档";
    settings.UseControllerSummaryAsTagDescription = true;
});
#endregion

#region MemoryCache
builder.Services.AddMemoryCache();
#endregion

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
})
    .AddSignInManager<SignInManager<UserInfo>>()
    .AddUserManager<UserManager<UserInfo>>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddErrorDescriber<TranslatedIdentityErrorDescriber>()
    .AddDefaultTokenProviders();

builder.Services.Configure<DataProtectionTokenProviderOptions>(o =>
    o.TokenLifespan = TimeSpan.FromHours(3));

#endregion Identity

#region Nlog
Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
LogManager.Configuration.Variables["connectionString"] = builder.Configuration.GetConnectionString("DefaultConnection");
#endregion

#region IP Rate Limit
//从appsettings.json获取相应配置
builder.Services.Configure<IpRateLimitOptions>(builder.Configuration.GetSection("IpRateLimiting"));

//注入计数器和规则存储
builder.Services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();

builder.Services.AddSingleton<IIpPolicyStore, DistributedCacheIpPolicyStore>();

builder.Services.AddSingleton<IRateLimitCounterStore, DistributedCacheRateLimitCounterStore>();

builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
#endregion

#region Google reCaptcha v3
builder.Services.AddSingleton<IRecaptchaExtension, RecaptchaExtension>();
#endregion

#region Services and Repositories
builder.Services.AddTransient<IMailSender, MailSender>();

builder.Services.AddScoped<ILogRepository, LogRepository>();
builder.Services.AddScoped<IPuzzleRepository, PuzzleRepository>();
builder.Services.AddScoped<IRankRepository, RankRepository>();
builder.Services.AddScoped<ISubmissionRepository, SubmissionRepository>();
#endregion

#region SignalR
builder.Services.AddSignalR().AddJsonProtocol();

builder.Services.AddSingleton<SignalRLoggingService>();
#endregion

builder.Services.AddControllersWithViews();

LogManager.Setup().SetupExtensions(s =>
{
    s.RegisterTarget<SignalRTarget>("SignalR");
});

var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseOpenApi();
    app.UseSwaggerUi3();
}
else
{
    app.UseExceptionHandler("/Error");
}

app.UseMiddleware<ProxyMiddleware>();
app.UseIpRateLimiting();

app.UseStaticFiles();

app.UseRouting();

app.UseStaticFiles();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();
app.MapHub<LoggingHub>("/hub/log");
app.MapFallbackToFile("index.html");

try
{
    logger.Debug("Server start, Init main.");
    await app.RunAsync();
}
catch (Exception exception)
{
    logger.Error(exception, "Stopped program because of exception.");
    throw;
}
finally
{
    LogManager.Shutdown();
}
