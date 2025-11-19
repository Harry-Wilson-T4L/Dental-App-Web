using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DentalDrill.CRM.Models.ViewModels
{
    public class HandpieceHistoryItemViewModel
    {
        public Guid Id { get; set; }

        public Int64 JobNumber { get; set; }

        public DateTime JobReceived { get; set; }

        public DateTime? CompletedOn { get; set; }

        public String DiagnosticReport { get; set; }

        public Int32 Rating { get; set; }

        public String TechnicianName { get; set; }

        public Decimal CostOfRepair { get; set; }
    }
}
