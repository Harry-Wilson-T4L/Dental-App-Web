using System;

namespace DentalDrill.CRM.Models.ViewModels.Inventory
{
    public class InventorySKUAvailableStatsModel
    {
        public Guid Id { get; set; }

        public Guid TypeId { get; set; }

        public String TypeName { get; set; }

        public Byte[] OrderNo { get; set; }

        public String Name { get; set; }

        public Decimal TotalQuantity { get; set; }

        public Decimal ShelfQuantity { get; set; }

        public Decimal TrayQuantity { get; set; }

        public Decimal OrderedQuantity { get; set; }

        public Decimal RequestedQuantity { get; set; }
    }
}
