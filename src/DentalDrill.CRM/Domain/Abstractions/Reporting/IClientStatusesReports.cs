using System.Collections.Generic;
using System.Threading.Tasks;
using DentalDrill.CRM.Models.ViewModels.Reports.Surgery;

namespace DentalDrill.CRM.Domain.Abstractions.Reporting
{
    public interface IClientStatusesReports
    {
        Task PrepareQuery();

        Task<List<SurgeryReportStatusItem>> GetBrandsForEntirePeriodAsync();
    }
}
