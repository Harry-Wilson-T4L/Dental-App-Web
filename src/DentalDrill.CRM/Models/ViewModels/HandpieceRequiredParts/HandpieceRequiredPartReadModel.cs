using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DentalDrill.CRM.Models.ViewModels.HandpieceRequiredParts
{
    public class HandpieceRequiredPartReadModel
    {
        public Guid Id { get; set; }

        public DateTime Date { get; set; }

        public HandpieceRequiredPartStatus Status { get; set; }

        public Guid SKUId { get; set; }

        public String SKUName { get; set; }

        public Decimal RequiredQuantity { get; set; }

        public Decimal ShelfQuantity { get; set; }

        public Decimal? Price { get; set; }
    }
}
