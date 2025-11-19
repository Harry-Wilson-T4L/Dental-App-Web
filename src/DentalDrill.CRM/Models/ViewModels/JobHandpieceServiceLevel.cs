using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DentalDrill.CRM.Models.ViewModels
{
    public class JobHandpieceServiceLevel
    {
        public Guid Id { get; set; }

        public String Name { get; set; }

        public Decimal? CostOfRepair { get; set; }
    }
}
