using System;
using System.Collections.Generic;

namespace DentalDrill.CRM.Models.ViewModels.Reports.Corporate
{
    public class CorporateSurgeryReportBrandAggregateItem
    {
        public String Brand { get; set; }

        public Dictionary<String, Decimal> RatingAverage { get; set; }

        public Dictionary<String, Decimal> CostSum { get; set; }

        public Dictionary<String, Decimal> CostAverage { get; set; }

        public Dictionary<String, Decimal> UnrepairedPercent { get; set; }

        public Dictionary<String, Int32> HandpiecesCount { get; set; }
    }
}