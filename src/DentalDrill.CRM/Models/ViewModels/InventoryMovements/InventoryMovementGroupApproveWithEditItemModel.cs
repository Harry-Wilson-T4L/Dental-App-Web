using System;

namespace DentalDrill.CRM.Models.ViewModels.InventoryMovements
{
    public class InventoryMovementGroupApproveWithEditItemModel
    {
        public Guid Id { get; set; }

        public Decimal? Quantity { get; set; }

        public Decimal? Price { get; set; }

        public InventoryMovementBulkEditStatus BulkEditStatus { get; set; }
    }
}
