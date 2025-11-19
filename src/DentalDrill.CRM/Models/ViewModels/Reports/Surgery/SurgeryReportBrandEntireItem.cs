using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DentalDrill.CRM.Models.ViewModels.Reports.Surgery
{
    public class SurgeryReportBrandEntireItem
    {
        public String Brand { get; set; }

        public Decimal RatingAverage { get; set; }

        public Decimal CostSum { get; set; }

        public Decimal CostAverage { get; set; }

        public Decimal UnrepairedPercent { get; set; }

        public Int32 HandpiecesCount { get; set; }
    }
}
