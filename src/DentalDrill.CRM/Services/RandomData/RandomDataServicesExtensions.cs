using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DentalDrill.CRM.Models;
using DentalDrill.CRM.Services.Calculation;
using Microsoft.Extensions.DependencyInjection;

namespace DentalDrill.CRM.Services.RandomData
{
    public static class RandomDataServicesExtensions
    {
        public static IServiceCollection AddRandomServices(this IServiceCollection services)
        {
            services.AddSingleton<IGenericArithmeticService<Int32>, Int32ArithmeticService>();
            services.AddSingleton<IGenericArithmeticService<String>, InvalidArithmeticService<String>>();
            services.AddSingleton<IGenericArithmeticService<JobStatus>, InvalidArithmeticService<JobStatus>>();
            services.AddSingleton<IGenericArithmeticService<HandpieceStatus>, InvalidArithmeticService<HandpieceStatus>>();

            services.AddScoped<IRandomDataSource, RandomDataSource>();
            services.AddScoped(typeof(IRandomValueGenerationService<>), typeof(RandomValueGenerationService<>));

            return services;
        }
    }
}
