using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DentalDrill.CRM.Models
{
    public enum JobStatus
    {
        Unknown = 0,
        Received = 10,
        BeingEstimated = 20,
        WaitingForApproval = 30,
        EstimateSent = 35,
        BeingRepaired = 40,
        ReadyToReturn = 50,
        SentComplete = 60,
        Cancelled = 70,
    }
}
