using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DentalDrill.CRM.Models.ViewModels.Reports.Surgery
{
    public class SurgeryReportSurgeryItem
    {
        public String Brand { get; set; }

        public String Model { get; set; }

        public Int32 Year { get; set; }

        public Int32 Quarter { get; set; }

        public Int32 Month { get; set; }

        public Int32 Week { get; set; }

        public DateTime Date { get; set; }

        public Decimal Cost { get; set; }

        public Decimal Rating { get; set; }

        public Int32 Unrepaired { get; set; }

        public Int32 Handpieces { get; set; }
    }
}
