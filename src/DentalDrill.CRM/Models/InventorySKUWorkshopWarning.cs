using System;

namespace DentalDrill.CRM.Models
{
    public class InventorySKUWorkshopWarning
    {
        public Guid Id { get; set; }

        public Guid WorkshopId { get; set; }

        public Int32 HasWarning { get; set; }
    }
}
