using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MSC.Server.Extensions;
using NLog;
using NLog.Web;
using System;
using System.Threading.Tasks;

namespace MSC.Server
{
    public class Program
    {
        public async static Task Main(string[] args)
        {
            LogManager.Setup().SetupExtensions(s =>
            {
                s.RegisterTarget<SignalRTarget>("SignalR");
            });
            var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
            try
            {
                logger.Debug("Server start, Init main.");
                await CreateHostBuilder(args).Build().RunAsync();
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
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureAppConfiguration((host, config) =>
                    {
                        config.AddJsonFile("RateLimitConfig.json", optional: true, reloadOnChange: true);
                    });
                    webBuilder.UseStartup<Startup>();
                })
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                })
                .UseNLog();
    }
}
