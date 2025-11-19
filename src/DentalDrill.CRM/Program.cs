using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DentalDrill.CRM.Data;
using DevGuild.AspNetCore.Services.Logging.Data;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace DentalDrill.CRM
{
    public class Program
    {
        public static async Task Main(String[] args)
        {
            var webHost = Program.CreateWebHostBuilder(args).Build();
            await webHost.MigrateDatabaseAsync();
            await webHost.RunAsync();
        }

        public static IWebHostBuilder CreateWebHostBuilder(String[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(configuration => { configuration.AddJsonFile("appsettings.User.json", optional: true, reloadOnChange: true); })
                .ConfigureLogging(logging => logging.AddRepositoryLogging())
                .UseStartup<Startup>();
    }
}
