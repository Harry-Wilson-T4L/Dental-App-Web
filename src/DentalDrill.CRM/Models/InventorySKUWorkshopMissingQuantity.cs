using System;

namespace DentalDrill.CRM.Models
{
    public class InventorySKUWorkshopMissingQuantity
    {
        public Guid Id { get; set; }

        public Guid WorkshopId { get; set; }

        public Decimal MissingQuantity { get; set; }
    }
}
