using BasicWay.Services;
using BasicWay.Workers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BasicWay;

public static class Program
{
    public static async Task Main(string[] args)
    {
        await CreateHostBuilder(args).Build().RunAsync();
        Console.WriteLine("Press any key to exit !");
        Console.ReadKey();
    }

    private static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((_, configBuilder) =>
            {
                configBuilder.AddJsonFiles();
                configBuilder.AddUserSecrets();
                configBuilder.AddEnvironmentVariables();
                configBuilder.AddCommandLine(args);
            })
            .ConfigureLogging((hostingContext, loggingBuilder) =>
            {
                loggingBuilder.AddDefaultLogger();
                loggingBuilder.AddApplicationInsights(hostingContext.Configuration);
            })
            .ConfigureServices((_, services) =>
            {
                services.AddHostedService<HostedService>();
                services.AddTransient<IDummyService, DummyService>();
            })
            .UseConsoleLifetime()
            .UseWindowsService()
            .UseSystemd();

    extension(IConfigurationBuilder configBuilder)
    {
        private void AddJsonFiles()
        {
            configBuilder.SetBasePath(Directory.GetCurrentDirectory());
            var environment = Environment.GetEnvironmentVariable("ENVIRONMENT");
            configBuilder.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            configBuilder.AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true);
        }

        private void AddUserSecrets()
        {
            configBuilder.AddUserSecrets(typeof(Program).Assembly);
        }
    }

    extension(ILoggingBuilder loggingBuilder)
    {
        private void AddDefaultLogger()
        {
            var categoryName = typeof(Program).Namespace!;
            var services = loggingBuilder.Services;
            services.AddSingleton(serviceProvider =>
            {
                var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
                return loggerFactory.CreateLogger(categoryName);
            });
        }
        
        private void AddApplicationInsights(IConfiguration configuration)
        {
            const string key = "Logging:ApplicationInsights:ConnectionString";
            var connectionString = configuration.GetValue<string>(key);
            loggingBuilder.AddApplicationInsights(
                config => config.ConnectionString = connectionString,
                options => options.IncludeScopes = true);
        }
    }
}