using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DentalDrill.CRM.Domain.Reporting;
using DentalDrill.CRM.Models.ViewModels.Reports.Global;

namespace DentalDrill.CRM.Domain
{
    public class GlobalReportsDomainModel
    {
        private readonly IServiceProvider serviceProvider;

        public GlobalReportsDomainModel(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public async Task<GlobalHandpiecesReportsDomainModel> PrepareHandpiecesReport(DateTime from, DateTime to, IEnumerable<String> groupingFields)
        {
            var report = new GlobalHandpiecesReportsDomainModel(this.serviceProvider, from, to, groupingFields);
            await report.PrepareQuery();

            return report;
        }

        public async Task<GlobalTechWarrantyReportsDomainModel> PrepareTechWarrantyReport(DateTime from, DateTime to)
        {
            var report = new GlobalTechWarrantyReportsDomainModel(this.serviceProvider, from, to);
            await report.PrepareQueryAsync();

            return report;
        }

        public async Task<GlobalBatchResultReportsDomainModel> PrepareBatchResultReport(DateTime from, DateTime to)
        {
            var report = new GlobalBatchResultReportsDomainModel(this.serviceProvider, from, to);
            await report.PrepareQueryAsync();

            return report;
        }
    }
}
