using System;

namespace DentalDrill.CRM.Models.ViewModels.Reports.Corporate
{
    public class CorporateSurgeryReportBrandEntireItem
    {
        public String Brand { get; set; }

        public Decimal RatingAverage { get; set; }

        public Decimal CostSum { get; set; }

        public Decimal CostAverage { get; set; }

        public Decimal UnrepairedPercent { get; set; }

        public Int32 HandpiecesCount { get; set; }
    }
}