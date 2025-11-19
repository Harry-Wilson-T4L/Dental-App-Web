using System;

namespace DentalDrill.CRM.Models.ViewModels.Reports.Corporate
{
    public class CorporateSurgeryReportStatusesFilter
    {
        public DateTime From { get; set; }

        public DateTime To { get; set; }

        public Guid[] Clients { get; set; }
    }
}