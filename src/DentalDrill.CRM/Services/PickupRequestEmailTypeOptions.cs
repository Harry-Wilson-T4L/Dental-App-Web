using System;

namespace DentalDrill.CRM.Services
{
    public class PickupRequestEmailTypeOptions
    {
        public Boolean? Enabled { get; set; }

        public String Recipient { get; set; }

        public String BaseUrl { get; set; }

        public String SubjectPrefix { get; set; }
    }
}
