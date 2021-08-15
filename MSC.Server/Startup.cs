using AspNetCoreRateLimit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MSC.Server.Extensions;
using MSC.Server.Hubs;
using MSC.Server.Middlewares;
using MSC.Server.Models;
using MSC.Server.Services;
using MSC.Server.Utils;
using NLog;
using System;
using System.Text;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.EntityFrameworkCore;
using MSC.Server.Services.Interface;
using MSC.Server.Repositories.Interface;
using MSC.Server.Repositories;

namespace MSC.Server
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddDbContext<AppDbContext>(
                options =>
                    options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"),
                provideropt =>
                    provideropt.EnableRetryOnFailure(3, TimeSpan.FromSeconds(10), null)));

            // In production, the React files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/build";
            });

            #region Identity

            services.AddAuthentication(o =>
            {
                o.DefaultScheme = IdentityConstants.ApplicationScheme;
                o.DefaultSignInScheme = IdentityConstants.ExternalScheme;
            })
                .AddIdentityCookies();

            services.AddIdentityCore<UserInfo>(options =>
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

            services.Configure<DataProtectionTokenProviderOptions>(o =>
                o.TokenLifespan = TimeSpan.FromHours(3));

            #endregion Identity

            #region Nlog
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            LogManager.Configuration.Variables["connectionString"] = Configuration.GetConnectionString("DefaultConnection");
            #endregion

            #region IP Rate Limit
            //从appsettings.json获取相应配置
            services.Configure<IpRateLimitOptions>(Configuration.GetSection("IpRateLimiting"));

            //注入计数器和规则存储
            services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();

            services.AddSingleton<IIpPolicyStore, DistributedCacheIpPolicyStore>();

            services.AddSingleton<IRateLimitCounterStore, DistributedCacheRateLimitCounterStore>();

            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
            #endregion

            #region Google reCaptcha v3
            services.AddSingleton<IRecaptchaExtension, RecaptchaExtension>();
            #endregion

            #region Services and Repositories
            services.AddTransient<IMailSender, MailSender>();

            services.AddScoped<ILogRepository, LogRepository>();
            #endregion

            #region SignalR
            services.AddSignalR().AddNewtonsoftJsonProtocol();

            services.AddSingleton<SignalRLoggingService>();
            #endregion

            services.AddControllersWithViews().AddNewtonsoftJson();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            else
                app.UseExceptionHandler("/Error");

            app.UseMiddleware<ProxyMiddleware>();
            app.UseIpRateLimiting();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

                endpoints.MapHub<LoggingHub>("/hub/log");
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseReactDevelopmentServer(npmScript: "start");
                }
            });
        }
    }
}