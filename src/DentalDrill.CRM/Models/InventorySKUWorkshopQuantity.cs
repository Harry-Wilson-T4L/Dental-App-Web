using System;

namespace DentalDrill.CRM.Models
{
    public class InventorySKUWorkshopQuantity
    {
        public Guid Id { get; set; }

        public Guid WorkshopId { get; set; }

        public Decimal TotalQuantity { get; set; }

        public Decimal ShelfQuantity { get; set; }

        public Decimal TrayQuantity { get; set; }

        public Decimal OrderedQuantity { get; set; }

        public Decimal RequestedQuantity { get; set; }
    }
}
