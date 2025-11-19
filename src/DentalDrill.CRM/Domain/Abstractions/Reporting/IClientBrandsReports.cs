using System.Collections.Generic;
using System.Threading.Tasks;
using DentalDrill.CRM.Models.ViewModels;
using DentalDrill.CRM.Models.ViewModels.Reports.Surgery;

namespace DentalDrill.CRM.Domain.Abstractions.Reporting
{
    public interface IClientBrandsReports
    {
        Task PrepareQuery();

        Task<List<SurgeryReportBrandEntireItem>> GetBrandsForEntirePeriodAsync();

        Task<List<SurgeryReportBrandAggregateItem>> GetBrandsAggregatedAsync(ReportDateAggregate dateAggregate);

        Task<List<SurgeryReportBrandAggregateItem>> GetBrandsAggregatedYearlyAsync();

        Task<List<SurgeryReportBrandAggregateItem>> GetBrandsAggregatedQuarterlyAsync();

        Task<List<SurgeryReportBrandAggregateItem>> GetBrandsAggregatedMonthlyAsync();

        Task<List<SurgeryReportBrandAggregateItem>> GetBrandsAggregatedWeeklyAsync();

        Task<List<SurgeryReportBrandAggregateItem>> GetBrandsAggregatedDailyAsync();

        Task<List<SurgeryReportBrandModelEntireItem>> GetBrandsAndModelsForEntirePeriodAsync();

        Task<List<SurgeryReportBrandModelAggregateItem>> GetBrandsAndModelsAggregatedAsync(ReportDateAggregate dateAggregate);

        Task<List<SurgeryReportBrandModelAggregateItem>> GetBrandsAndModelsAggregatedYearlyAsync();

        Task<List<SurgeryReportBrandModelAggregateItem>> GetBrandsAndModelsAggregatedQuarterlyAsync();

        Task<List<SurgeryReportBrandModelAggregateItem>> GetBrandsAndModelsAggregatedMonthlyAsync();

        Task<List<SurgeryReportBrandModelAggregateItem>> GetBrandsAndModelsAggregatedWeeklyAsync();

        Task<List<SurgeryReportBrandModelAggregateItem>> GetBrandsAndModelsAggregatedDailyAsync();
    }
}
