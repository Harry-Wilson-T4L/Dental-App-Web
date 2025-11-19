using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DentalDrill.CRM.Models.ViewModels
{
    public class HandpieceStoreOrderReadModel
    {
        public Guid Id { get; set; }

        public Int32 OrderNumber { get; set; }

        public DateTime CreatedOn { get; set; }

        public String BrandModel { get; set; }

        public String Status { get; set; }

        public String Coupling { get; set; }

        public String Notes { get; set; }

        public String CosmeticCondition { get; set; }

        public String FiberOpticBrightness { get; set; }

        public String Warranty { get; set; }

        public Decimal Price { get; set; }
    }
}
