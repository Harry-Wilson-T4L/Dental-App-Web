using System;

namespace DentalDrill.CRM.Models
{
    public class InventorySKUWorkshopRequiredQuantity
    {
        public Guid Id { get; set; }

        public Guid WorkshopId { get; set; }

        public Decimal RequiredQuantity { get; set; }
    }
}
