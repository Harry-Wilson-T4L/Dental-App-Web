using System;

namespace DentalDrill.CRM.Models.ViewModels.Inventory
{
    public class InventorySKUIntermediateReadModel
    {
        public Guid Id { get; set; }

        public Guid TypeId { get; set; }

        public Guid? ParentId { get; set; }

        public Int32 OrderNo { get; set; }

        public Guid? ParentDefaultChildId { get; set; }

        public String Name { get; set; }

        public String Description { get; set; }

        public InventorySKUNodeType NodeType { get; set; }

        public Decimal? AveragePrice { get; set; }

        public Decimal? TotalQuantity { get; set; }

        public Decimal? ShelfQuantity { get; set; }

        public Decimal? TrayQuantity { get; set; }

        public Decimal? OrderedQuantity { get; set; }

        public Decimal? RequestedQuantity { get; set; }

        public Int32 HasWarning { get; set; }

        public Int32 HasDescendantsWithWarning { get; set; }
    }
}
