using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Identity.Identity.SeedDatabaseService;
using InfrastructureServices.Logging;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
namespace Web.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            Activity.DefaultIdFormat = ActivityIdFormat.W3C;
            var webHost = CreateHostBuilder(args).Build();

            #region Seeding Database

            using var scope = webHost.Services.CreateScope();

            var seedService = scope.ServiceProvider.GetRequiredService<ISeedDataBase>();

            await seedService.Seed();
            #endregion

            await CreateHostBuilder(args).Build().RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {

                    webBuilder.UseStartup<Startup>();
                }).UseSerilog(LoggingConfiguration.ConfigureLogger);
    }
}
