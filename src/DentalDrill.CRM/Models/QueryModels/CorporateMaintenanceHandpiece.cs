using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DentalDrill.CRM.Models.QueryModels
{
    public class CorporateMaintenanceHandpiece
    {
        public Guid ClientId { get; set; }

        public String ClientUrlPath { get; set; }

        public String Brand { get; set; }

        public String MakeAndModel { get; set; }

        public String Serial { get; set; }

        public DateTime LatestJobReceived { get; set; }

        public Guid? LatestImageId { get; set; }

        public String LatestImageContainer { get; set; }

        public String LatestImageContainerPrefix { get; set; }

        public String LatestImageVariations { get; set; }
    }
}
