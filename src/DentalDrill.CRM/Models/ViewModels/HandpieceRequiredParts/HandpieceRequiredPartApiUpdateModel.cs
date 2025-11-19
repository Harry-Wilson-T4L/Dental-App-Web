using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DentalDrill.CRM.Models.ViewModels.HandpieceRequiredParts
{
    public class HandpieceRequiredPartApiUpdateModel
    {
        public Decimal OldQuantity { get; set; }

        public Decimal NewQuantity { get; set; }
    }
}
