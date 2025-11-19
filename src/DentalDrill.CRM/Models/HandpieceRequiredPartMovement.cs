using System;

namespace DentalDrill.CRM.Models
{
    public class HandpieceRequiredPartMovement
    {
        public Guid RequiredPartId { get; set; }

        public HandpieceRequiredPart RequiredPart { get; set; }

        public Guid MovementId { get; set; }

        public InventoryMovement Movement { get; set; }
    }
}
