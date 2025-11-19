using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DentalDrill.CRM.Models.ViewModels
{
    public class HandpieceStoreFilterModel
    {
        public String Search { get; set; }

        public Decimal[] Price { get; set; }

        public Guid[] Brand { get; set; }

        public Guid[] Model { get; set; }

        public String[] Coupling { get; set; }

        public HandpieceSpeed[] Type { get; set; }
    }
}
