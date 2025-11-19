using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DentalDrill.CRM.Models
{
    public class ClientFeedbackOptions
    {
        public Guid Id { get; set; }

        public Guid ClientId { get; set; }

        public Client Client { get; set; }

        public Int32 SkippedJobs { get; set; }
    }
}
