using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace DentalDrill.CRM.Extensions
{
    public static class ServiceProviderExtensions
    {
        public static Lazy<T> GetLazyService<T>(this IServiceProvider serviceProvider)
        {
            return new Lazy<T>(serviceProvider.GetRequiredService<T>);
        }
    }
}
