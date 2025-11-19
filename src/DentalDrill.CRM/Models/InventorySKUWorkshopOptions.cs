using System;

namespace DentalDrill.CRM.Models
{
    public class InventorySKUWorkshopOptions
    {
        public Guid SKUId { get; set; }

        public InventorySKU SKU { get; set; }

        public Guid WorkshopId { get; set; }

        public Workshop Workshop { get; set; }

        public Decimal? WarningQuantity { get; set; }
    }
}
