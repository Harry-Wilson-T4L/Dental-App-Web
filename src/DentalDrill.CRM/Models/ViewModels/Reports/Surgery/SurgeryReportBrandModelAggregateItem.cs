using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DentalDrill.CRM.Models.ViewModels.Reports.Surgery
{
    public class SurgeryReportBrandModelAggregateItem
    {
        public String Brand { get; set; }

        public String Model { get; set; }

        public Dictionary<String, Decimal> RatingAverage { get; set; }

        public Dictionary<String, Decimal> CostSum { get; set; }

        public Dictionary<String, Decimal> CostAverage { get; set; }

        public Dictionary<String, Decimal> UnrepairedPercent { get; set; }

        public Dictionary<String, Int32> HandpiecesCount { get; set; }
    }
}
