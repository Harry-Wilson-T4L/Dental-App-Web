using System;
using System.Threading.Tasks;
using DentalDrill.CRM.Domain.Abstractions;
using DentalDrill.CRM.Domain.Abstractions.Reporting;
using DentalDrill.CRM.Domain.Reporting;

namespace DentalDrill.CRM.Domain
{
    public class ClientReportsDomainModel : IClientReports
    {
        private readonly IServiceProvider serviceProvider;
        private readonly IClient client;

        public ClientReportsDomainModel(IServiceProvider serviceProvider, IClient client)
        {
            this.serviceProvider = serviceProvider;
            this.client = client;
        }

        public async Task<IClientBrandsReports> PrepareBrandsReport(DateTime from, DateTime to)
        {
            var report = new ClientBrandsReportsDomainModel(this.serviceProvider, this.client, from, to);
            await report.PrepareQuery();

            return report;
        }

        public async Task<IClientStatusesReports> PrepareStatusesReport(DateTime from, DateTime to)
        {
            var report = new ClientStatusesReportsDomainModel(this.serviceProvider, this.client, from, to);
            await report.PrepareQuery();

            return report;
        }
    }
}
