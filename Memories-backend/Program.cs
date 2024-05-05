using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Hosting;

namespace Memories_backend

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
                });
    }
}