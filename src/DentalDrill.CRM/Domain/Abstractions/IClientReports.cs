using System;
using System.Threading.Tasks;
using DentalDrill.CRM.Domain.Abstractions.Reporting;

namespace DentalDrill.CRM.Domain.Abstractions
{
    public interface IClientReports
    {
        Task<IClientBrandsReports> PrepareBrandsReport(DateTime from, DateTime to);

        Task<IClientStatusesReports> PrepareStatusesReport(DateTime from, DateTime to);
    }
}
