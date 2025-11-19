using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DentalDrill.CRM.Models.ViewModels.HandpieceRequiredParts
{
    public class HandpieceRequiredPartReallocateReadModel
    {
        public Guid Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public Decimal QuantityAbsolute { get; set; }

        public Guid? HandpieceId { get; set; }

        public String HandpieceNumber { get; set; }

        public HandpieceStatus? HandpieceStatus { get; set; }
    }
}
