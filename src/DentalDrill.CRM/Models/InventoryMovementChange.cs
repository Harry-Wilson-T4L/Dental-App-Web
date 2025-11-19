using System;

namespace DentalDrill.CRM.Models
{
    public class InventoryMovementChange
    {
        public Guid Id { get; set; }

        public Guid SKUId { get; set; }

        public InventorySKU SKU { get; set; }

        public Guid? MovementId { get; set; }

        public DateTime ChangedOn { get; set; }

        public Guid ChangedById { get; set; }

        public Employee ChangedBy { get; set; }

        public ChangeAction Action { get; set; }

        public InventoryMovementStatus? OldStatus { get; set; }

        public InventoryMovementStatus? NewStatus { get; set; }

        public Decimal? OldQuantity { get; set; }

        public Decimal? NewQuantity { get; set; }

        public Decimal? OldPrice { get; set; }

        public Decimal? NewPrice { get; set; }

        public String OldComment { get; set; }

        public String NewComment { get; set; }

        public String OldContent { get; set; }

        public String NewContent { get; set; }
    }
}
