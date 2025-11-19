using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DentalDrill.CRM.Models.ViewModels.Reports.Global
{
    public class GlobalBatchResultReportItem
    {
        public Guid JobId { get; set; }

        public Int64 JobNumber { get; set; }

        public DateTime CompletedFirst { get; set; }

        public Int32 CountOfHandpieces { get; set; }

        public Int32 CountOfDistinctDates { get; set; }

        public String ListOfDates { get; set; }
    }
}
