using System;

namespace DentalDrill.CRM.Models.ViewModels
{
    public class SurgeryMaintenanceHistoryItemViewModel
    {
        public Guid Id { get; set; }

        public String ImageUrl { get; set; }

        public String JobType { get; set; }

        public Int64 JobNumber { get; set; }

        public DateTime JobReceived { get; set; }

        public DateTime? CompletedOn { get; set; }

        public String DiagnosticReport { get; set; }

        public String ServiceLevel { get; set; }

        public Int32 Rating { get; set; }

        public Decimal CostOfRepair { get; set; }

        public String PublicComment { get; set; }
    }
}
