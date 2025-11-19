using System;

namespace DentalDrill.CRM.Models
{
    public class InventorySKUWorkshopDescendantsWarnings
    {
        public Guid Id { get; set; }

        public Guid WorkshopId { get; set; }

        public Int32 HasDescendantsWithWarning { get; set; }
    }
}
