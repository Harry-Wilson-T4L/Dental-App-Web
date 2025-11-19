using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DentalDrill.CRM.Models
{
    public enum PickupRequestStatus
    {
        Created,
        EmailSent,
        ConsignmentCreated,
        Cancelled,
        Completed,
        Failed,
    }
}
