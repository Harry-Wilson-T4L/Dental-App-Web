using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DentalDrill.CRM.Models
{
    public class HandpieceRequiredPart
    {
        public Guid Id { get; set; }

        public Guid HandpieceId { get; set; }

        public Handpiece Handpiece { get; set; }

        public Guid SKUId { get; set; }

        public InventorySKU SKU { get; set; }

        public Decimal Quantity { get; set; }

        public ICollection<HandpieceRequiredPartMovement> Movements { get; set; }
    }
}
