using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DentalDrill.CRM.Domain.Reporting;

namespace DentalDrill.CRM.Domain
{
    public class CorporateReportsDomainModel
    {
        private readonly IServiceProvider serviceProvider;
        private readonly CorporateDomainModel corporate;

        public CorporateReportsDomainModel(IServiceProvider serviceProvider, CorporateDomainModel corporate)
        {
            this.serviceProvider = serviceProvider;
            this.corporate = corporate;
        }

        public async Task<CorporateBrandsReportsDomainModel> PrepareBrandsReport(DateTime from, DateTime to, IReadOnlyList<Guid> clients)
        {
            var report = new CorporateBrandsReportsDomainModel(this.serviceProvider, this.corporate, from, to, clients);
            await report.PrepareQuery();

            return report;
        }

        public async Task<CorporateStatusesReportsDomainModel> PrepareStatusesReport(DateTime from, DateTime to, IReadOnlyList<Guid> clients)
        {
            var report = new CorporateStatusesReportsDomainModel(this.serviceProvider, this.corporate, from, to, clients);
            await report.PrepareQuery();

            return report;
        }
    }
}
