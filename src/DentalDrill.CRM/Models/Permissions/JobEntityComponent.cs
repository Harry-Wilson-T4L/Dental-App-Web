using System;

namespace DentalDrill.CRM.Models.Permissions
{
    [Flags]
    public enum JobEntityComponent : Int64
    {
        None = 0,

        Handpiece = (1 << 0),
        Invoice = (1 << 1),
        History = (1 << 2),

        All = (1 << 3) - 1,
    }
}
