using System;

namespace DentalDrill.CRM.Models.Permissions
{
    [Flags]
    public enum HandpieceEntityComponent : Int64
    {
        None = 0,

        MaintenanceHistory = (1 << 0),
        Image = (1 << 1),
        Note = (1 << 2),
        History = (1 << 3),

        All = (1 << 4) - 1,
    }
}
