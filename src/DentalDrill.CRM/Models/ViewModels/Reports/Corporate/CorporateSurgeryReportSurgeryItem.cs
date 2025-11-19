using System;

namespace DentalDrill.CRM.Models.ViewModels.Reports.Corporate
{
    public class CorporateSurgeryReportSurgeryItem
    {
        public Guid ClientId { get; set; }

        public String ClientName { get; set; }

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
