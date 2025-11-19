using System;
using System.Linq;
using System.Threading.Tasks;

namespace DentalDrill.CRM.Models.ViewModels.Reports.Global
{
    public class GlobalHandpiecesReportEntireItem
    {
        public Guid ClientId { get; set; }

        public String ClientName { get; set; }

        public String ServiceLevelName { get; set; }

        public String Brand { get; set; }

        public String Model { get; set; }

        public String RepairedByName { get; set; }

        public HandpieceStatus HandpieceStatus { get; set; }

        public Decimal RatingAverage { get; set; }

        public Decimal CostSum { get; set; }

        public Decimal CostAverage { get; set; }

        public Decimal UnrepairedPercent { get; set; }

        public Decimal ReturnUnrepairedPercent { get; set; }

        public Int32 HandpiecesCount { get; set; }

        public Double? TurnaroundAverage { get; set; }

        public Int32 WarrantyCount { get; set; }
    }
}
