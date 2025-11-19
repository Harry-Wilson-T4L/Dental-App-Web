using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DentalDrill.CRM.Models.ViewModels
{
    public class HandpieceModelListingReadModel
    {
        public Guid Id { get; set; }

        public HandpieceStoreListingStatus Status { get; set; }

        public String SerialNumber { get; set; }

        public DateTime CreatedOn { get; set; }

        public Decimal Price { get; set; }

        public String Warranty { get; set; }

        public String Coupling { get; set; }

        public String Notes { get; set; }

        public String CosmeticCondition { get; set; }

        public String FiberOpticBrightness { get; set; }
    }
}
