using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DentalDrill.CRM.Data
{
    public static class ApplicationDbMigrationExtensions
    {
        public static async Task<IWebHost> MigrateDatabaseAsync(this IWebHost webHost)
        {
            using (var scope = webHost.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetService<ApplicationDbContext>();
                await dbContext.Database.MigrateAsync();

                var dbSeed = new ApplicationDbSeed(scope.ServiceProvider);
                await dbSeed.SeedAsync();
            }

            return webHost;
        }
    }
}
