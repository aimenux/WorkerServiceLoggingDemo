using System;
using System.IO;
using System.Threading.Tasks;
using AdvancedWay.Services;
using AdvancedWay.Workers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AdvancedWay
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            await CreateHostBuilder(args).Build().RunAsync();
            Console.WriteLine("Press any key to exit !");
            Console.ReadKey();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((_, config) =>
                {
                    config.AddCommandLine(args);
                    config.AddEnvironmentVariables();
                    config.SetBasePath(Directory.GetCurrentDirectory());
                    var environment = Environment.GetEnvironmentVariable("ENVIRONMENT");
                    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                    config.AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true);
                })
                .ConfigureLogging((hostingContext, loggingBuilder) =>
                {
                    loggingBuilder.AddNonGenericLogger();
                    loggingBuilder.AddConfiguration(hostingContext.Configuration.GetSection(@"Logging"));
                })
                .ConfigureServices((hostingContext, services) =>
                {
                    services.AddOptions();
                    services.AddHostedService<HostedService>();
                    services.AddTransient<IDummyService, DummyService>();
                    services.AddApplicationInsightsTelemetryWorkerService(GetInstrumentationKey(hostingContext));
                })
                .UseConsoleLifetime()
                .UseWindowsService()
                .UseSystemd();

        private static string GetInstrumentationKey(HostBuilderContext hostingContext)
        {
            const string key = "Logging:ApplicationInsights:InstrumentationKey";
            var instrumentationKey = hostingContext.Configuration.GetValue<string>(key);
            return instrumentationKey;
        }

        private static void AddNonGenericLogger(this ILoggingBuilder loggingBuilder)
        {
            var categoryName = typeof(Program).Namespace;
            var services = loggingBuilder.Services;
            services.AddSingleton(serviceProvider =>
            {
                var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
                return loggerFactory.CreateLogger(categoryName);
            });
        }
    }
}
