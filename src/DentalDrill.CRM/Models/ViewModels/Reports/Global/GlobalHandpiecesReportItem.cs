using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DentalDrill.CRM.Models.ViewModels.Reports.Global
{
    public class GlobalHandpiecesReportItem
    {
        public Guid NullId { get; set; }

        public Guid ClientId { get; set; }

        public String ClientName { get; set; }

        public String Brand { get; set; }

        public String Model { get; set; }

        public String ServiceLevelName { get; set; }

        public String RepairedByName { get; set; }

        public HandpieceStatus HandpieceStatus { get; set; }

        public Int32 Year { get; set; }

        public Int32 Quarter { get; set; }

        public Int32 Month { get; set; }

        public Int32 Week { get; set; }

        public DateTime Date { get; set; }

        public Decimal Cost { get; set; }

        public Decimal Rating { get; set; }

        public Int32 Unrepaired { get; set; }

        public Int32 ReturnUnrepaired { get; set; }

        public Int32 Handpieces { get; set; }

        public Int32? Turnaround { get; set; }

        public Int32 Warranty { get; set; }
    }
}
