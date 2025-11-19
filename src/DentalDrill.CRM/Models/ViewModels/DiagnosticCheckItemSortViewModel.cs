using System;

namespace DentalDrill.CRM.Models.ViewModels
{
    public class DiagnosticCheckItemSortViewModel
    {
        public Guid TypeId { get; set; }

        public Guid ItemId { get; set; }

        public String Name { get; set; }

        public Int32 OrderNo { get; set; }
    }
}
