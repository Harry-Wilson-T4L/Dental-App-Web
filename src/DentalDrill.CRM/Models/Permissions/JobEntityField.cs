using System;

namespace DentalDrill.CRM.Models.Permissions
{
    [Flags]
    public enum JobEntityField : Int64
    {
        None = 0,

        Client = (1 << 0),
        Workshop = (1 << 1),
        Received = (1 << 2),
        Comment = (1 << 3),
        HasWarning = (1 << 4),
        ApprovedBy = (1 << 5),
        ApprovedOn = (1 << 6),
        RatePlan = (1 << 7),
        Creator = (1 << 8),

        All = (1 << 9) - 1,
    }
}
