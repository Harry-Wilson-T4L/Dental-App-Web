using System;

namespace DentalDrill.CRM.Models.Permissions
{
    [Flags]
    public enum JobStatusFlags : Int64
    {
        None = 0,

        Received = (1 << 0),
        BeingEstimated = (1 << 1),
        WaitingForApproval = (1 << 2),
        EstimateSent = (1 << 3),
        BeingRepaired = (1 << 4),
        ReadyToReturn = (1 << 5),
        SentComplete = (1 << 6),
        Cancelled = (1 << 7),

        All = (1 << 8) - 1,
    }
}
