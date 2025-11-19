using System;
using System.Collections.Generic;

namespace DentalDrill.CRM.Models.ViewModels.Reports.Global
{
    public class GlobalHandpiecesReportAggregateItem
    {
        public Guid ClientId { get; set; }

        public String ClientName { get; set; }

        public String ServiceLevelName { get; set; }

        public String Brand { get; set; }

        public String Model { get; set; }

        public String RepairedByName { get; set; }

        public HandpieceStatus HandpieceStatus { get; set; }

        public Dictionary<String, Decimal> RatingAverage { get; set; }

        public Dictionary<String, Decimal> CostSum { get; set; }

        public Dictionary<String, Decimal> CostAverage { get; set; }

        public Dictionary<String, Decimal> UnrepairedPercent { get; set; }

        public Dictionary<String, Decimal> ReturnUnrepairedPercent { get; set; }

        public Dictionary<String, Int32> HandpiecesCount { get; set; }

        public Dictionary<String, Double?> TurnaroundAverage { get; set; }

        public Dictionary<String, Int32> WarrantyCount { get; set; }
    }
}