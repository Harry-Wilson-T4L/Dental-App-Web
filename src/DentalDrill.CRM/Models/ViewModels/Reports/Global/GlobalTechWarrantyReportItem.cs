using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DentalDrill.CRM.Models.ViewModels.Reports.Global
{
    public class GlobalTechWarrantyReportItem
    {
        public Guid HandpieceId { get; set; }

        public Guid JobId { get; set; }

        public Int64 JobNumber { get; set; }

        public DateTime JobReceived { get; set; }

        public Guid? RepairedById { get; set; }

        public String RepairedByName { get; set; }

        public String Brand { get; set; }

        public String Model { get; set; }

        public String Serial { get; set; }

        public Int32 HandpieceCount { get; set; }

        public Int32 Warranty { get; set; }

        public Int32? DaysPassed { get; set; }
    }
}
