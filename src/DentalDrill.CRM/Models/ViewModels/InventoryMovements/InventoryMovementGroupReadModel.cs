using System;
using System.ComponentModel.DataAnnotations;

namespace DentalDrill.CRM.Models.ViewModels.InventoryMovements
{
    public class InventoryMovementGroupReadModel
    {
        public String Id { get; set; }

        public Guid WorkshopId { get; set; }

        public String WorkshopName { get; set; }

        public Guid SKUId { get; set; }

        public String SKUName { get; set; }

        public Int32 SKUTypeOrderNo { get; set; }

        [MaxLength(80)]
        public Byte[] SKUOrderNo { get; set; }

        public InventoryMovementType Type { get; set; }

        public InventoryMovementStatus Status { get; set; }

        public DateTime? MinDate { get; set; }

        public DateTime? MinDateTime { get; set; }

        public DateTime? MaxDate { get; set; }

        public DateTime? MaxDateTime { get; set; }

        public Decimal Quantity { get; set; }

        public Decimal QuantityAbsolute { get; set; }

        public Decimal QuantityAbsoluteWithPrice { get; set; }

        public Decimal ShelfQuantity { get; set; }

        public Decimal OrderedQuantity { get; set; }

        public Decimal MissingQuantity { get; set; }

        public Int32 HandpiecesCount { get; set; }

        public Decimal? AverageFinalPrice { get; set; }

        public Decimal? TotalPrice { get; set; }

        public Decimal? TotalPriceAbsolute { get; set; }
    }
}
