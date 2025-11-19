using System;
using System.ComponentModel.DataAnnotations;

namespace DentalDrill.CRM.Models.ViewModels.InventoryMovements
{
    public class InventoryMovementReadModel
    {
        public Guid Id { get; set; }

        public DateTime? Date { get; set; }

        public DateTime? DateTime { get; set; }

        public Guid WorkshopId { get; set; }

        public String WorkshopName { get; set; }

        public Guid SKUId { get; set; }

        public String SKUName { get; set; }

        public Int32 SKUTypeOrderNo { get; set; }

        [MaxLength(80)]
        public Byte[] SKUOrderNo { get; set; }

        public Decimal Quantity { get; set; }

        public Decimal QuantityAbsolute { get; set; }

        public Decimal ShelfQuantity { get; set; }

        public InventoryMovementType Type { get; set; }

        public InventoryMovementStatus Status { get; set; }

        public Guid? HandpieceId { get; set; }

        public String HandpieceNumber { get; set; }

        public HandpieceStatus? HandpieceStatus { get; set; }

        public String HandpiecePartsComment { get; set; }

        public Guid? ClientId { get; set; }

        public String ClientFullName { get; set; }

        public String MovementComment { get; set; }

        public Decimal? AveragePrice { get; set; }

        public Decimal? MovementPrice { get; set; }

        public Decimal? FinalPrice { get; set; }

        public Decimal? TotalPrice { get; set; }

        public Decimal? TotalPriceAbsolute { get; set; }
    }
}
