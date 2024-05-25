using MemoriesBackend.Infrastructure;

namespace MemoriesBackend.API

{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseWindowsService()
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                    config.AddEnvironmentVariables();
                })
                .ConfigureLogging((hostingContext, logging) =>
                {
                    logging.ClearProviders();
                    logging.AddConsole();
                    logging.AddDebug();
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    webBuilder.UseUrls("http://127.0.0.1:8887");

                    webBuilder.ConfigureKestrel((context, options) =>
                    {
                        // 5GB limit
                        options.Limits.MaxRequestBodySize = 5000L * 1024 * 1024;
                    });
                })
                .ConfigureServices((context, services) =>
                {
                    services.AddInfrastructure();
                });
    }
}