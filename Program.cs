using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace ZyxMeSocket.Ws
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((hostingContext, config) => { 
                //config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                config.AddJsonFile("appsettings.extrasettings.json", optional: true, reloadOnChange: true);
                config.AddJsonFile("appsettings.logsettings.json", optional: true, reloadOnChange: true);
                config.AddJsonFile("sqlfunctions.json", optional: true, reloadOnChange: true);
            })
            .UseContentRoot(Directory.GetCurrentDirectory())
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseIISIntegration();
                webBuilder.UseUrls("http://*:55957");
                webBuilder.UseStartup<Startup>();
            });
    }
}
