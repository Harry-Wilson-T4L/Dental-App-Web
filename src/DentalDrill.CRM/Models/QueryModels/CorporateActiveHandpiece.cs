using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DentalDrill.CRM.Models.QueryModels
{
    public class CorporateActiveHandpiece
    {
        public Guid Id { get; set; }

        public Guid ClientId { get; set; }

        public String JobNumber { get; set; }

        public String Brand { get; set; }

        public String MakeAndModel { get; set; }

        public String Serial { get; set; }

        public HandpieceStatus Status { get; set; }

        public Int32 Rating { get; set; }

        public HandpieceSpeed SpeedType { get; set; }

        public DateTime Received { get; set; }

        public Guid? LatestImageId { get; set; }

        public String LatestImageContainer { get; set; }

        public String LatestImageContainerPrefix { get; set; }

        public String LatestImageVariations { get; set; }
    }
}
