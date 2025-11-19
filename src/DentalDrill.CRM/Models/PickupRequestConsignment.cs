using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DentalDrill.CRM.Models
{
    public enum PickupRequestConsignmentStatus
    {
        Created,
        Succeeded,
        Failed,
    }

    public class PickupRequestConsignment
    {
        public Guid Id { get; set; }

        public Guid RequestId { get; set; }

        public PickupRequest Request { get; set; }

        public DateTime DateTime { get; set; }

        public PickupRequestConsignmentStatus Status { get; set; }

        public String Reference { get; set; }

        public String RequestXml { get; set; }

        public String ResponseXml { get; set; }

        public String ExceptionClass { get; set; }

        public String ExceptionMessage { get; set; }

        public String ExceptionDetails { get; set; }
    }
}
