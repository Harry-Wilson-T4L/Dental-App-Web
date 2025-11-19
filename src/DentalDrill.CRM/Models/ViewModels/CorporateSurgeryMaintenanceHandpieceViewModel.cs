using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DentalDrill.CRM.Models.ViewModels
{
    public class CorporateSurgeryMaintenanceHandpieceViewModel
    {
        public String Id { get; set; }

        public String ClientId { get; set; }

        public String Brand { get; set; }

        public String MakeAndModel { get; set; }

        public String Serial { get; set; }

        public String ImageUrl { get; set; }

        public Boolean OverYearAgo { get; set; }
    }
}
