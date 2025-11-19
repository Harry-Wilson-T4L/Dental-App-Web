using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DentalDrill.CRM.Models
{
    public class InventoryMovement
    {
        public Guid Id { get; set; }

        public Guid WorkshopId { get; set; }

        public Workshop Workshop { get; set; }

        public Guid SKUId { get; set; }

        public InventorySKU SKU { get; set; }

        public InventoryMovementDirection Direction { get; set; }

        public InventoryMovementType Type { get; set; }

        public InventoryMovementStatus Status { get; set; }

        public Decimal Quantity { get; set; }

        public Decimal QuantityAbsolute { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? CompletedOn { get; set; }

        public String Comment { get; set; }

        public Decimal? Price { get; set; }

        public ICollection<HandpieceRequiredPartMovement> RequiredParts { get; set; }
    }
}
