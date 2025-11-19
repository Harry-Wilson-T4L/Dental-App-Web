using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DentalDrill.CRM.Services
{
    public class PickupRequestEmailOptions
    {
        public Boolean Enabled { get; set; }

        public String Recipient { get; set; }

        public String BaseUrl { get; set; }

        public String SubjectPrefix { get; set; }

        public Dictionary<String, PickupRequestEmailTypeOptions> Types { get; set; }
    }
}
