using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DentalDrill.CRM.Emails.Models
{
    public class EmailContactDetails
    {
        public String SenderName { get; set; }

        public String Phone { get; set; }

        public String Address { get; set; }

        public String Email { get; set; }

        public String WebSite { get; set; }

        public String WebSiteUrl { get; set; }

        public String HubUrl { get; set; }

        public String BusinessNumber { get; set; }
    }
}
