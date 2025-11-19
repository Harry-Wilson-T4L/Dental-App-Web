using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DentalDrill.CRM.Models.ViewModels.HandpieceRequiredParts
{
    public class HandpieceRequiredPartApiAddModel
    {
        public Guid SKU { get; set; }

        public Decimal Quantity { get; set; }
    }
}
