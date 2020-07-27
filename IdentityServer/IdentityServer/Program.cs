using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace IdentityServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();

            Log.Logger = CreateSerilogLogger();
        }

        private static Serilog.ILogger CreateSerilogLogger()
        {
            var config = new LoggerConfiguration()
                .Enrich.WithProperty("ApplicationContext", "IdentityServerFormation")
                .Enrich.FromLogContext()
                .WriteTo.Console();
            return config.CreateLogger();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });


    }
}
