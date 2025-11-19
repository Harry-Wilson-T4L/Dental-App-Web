using System;

namespace DentalDrill.CRM.Models
{
    public class DiagnosticCheckItemType
    {
        public Guid ItemId { get; set; }

        public DiagnosticCheckItem Item { get; set; }

        public Guid TypeId { get; set; }

        public DiagnosticCheckType Type { get; set; }

        public Int32 OrderNo { get; set; }
    }
}
