using System;

namespace DentalDrill.CRM.Models
{
    public class HandpieceDiagnostic
    {
        public Guid HandpieceId { get; set; }

        public Handpiece Handpiece { get; set; }

        public Guid ItemId { get; set; }

        public DiagnosticCheckItem Item { get; set; }

        public Int32 OrderNo { get; set; }
    }
}
