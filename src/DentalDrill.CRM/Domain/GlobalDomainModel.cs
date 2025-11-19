using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DentalDrill.CRM.Domain
{
    public class GlobalDomainModel
    {
        private readonly IServiceProvider serviceProvider;

        private GlobalReportsDomainModel reports;

        private GlobalDomainModel(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public GlobalReportsDomainModel Reports => this.reports ??= new GlobalReportsDomainModel(this.serviceProvider);

        public static Task<GlobalDomainModel> InitializeAsync(IServiceProvider serviceProvider)
        {
            var global = new GlobalDomainModel(serviceProvider);
            return Task.FromResult(global);
        }
    }
}
