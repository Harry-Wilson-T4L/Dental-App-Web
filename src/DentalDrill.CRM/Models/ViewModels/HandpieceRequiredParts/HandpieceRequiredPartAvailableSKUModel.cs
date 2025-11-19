using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DentalDrill.CRM.Models.ViewModels.HandpieceRequiredParts
{
    public class HandpieceRequiredPartAvailableSKUModel
    {
        public Guid Id { get; set; }

        public String Name { get; set; }

        public Decimal ShelfQuantity { get; set; }

        public Decimal? Price { get; set; }

        public Boolean IsDefaultChild { get; set; }
    }
}
