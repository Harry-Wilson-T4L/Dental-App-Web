using System;

namespace DentalDrill.CRM.Models.ViewModels.Reports.Corporate
{
    public class CorporateSurgeryReportBrandsFilter
    {
        public DateTime From { get; set; }

        public DateTime To { get; set; }

        public Guid[] Clients { get; set; }

        public ReportDateAggregate DateAggregate { get; set; }
    }
}