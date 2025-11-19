using System;

namespace DentalDrill.CRM.Models.Permissions
{
    [Flags]
    public enum WorkshopPermissions : Int64
    {
        None = 0,

        AccessWorkshop = (1 << 0),
        CreateJob = (1 << 1),

        All = (1 << 2) - 1,
    }
}
