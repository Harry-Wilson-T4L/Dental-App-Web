using System;

namespace DentalDrill.CRM.Models.Permissions
{
    [Flags]
    public enum InventoryMovementPermissions : Int64
    {
        None = 0,

        MovementRead = (1 << 0),
        MovementCreateOrder = (1 << 1),
        MovementCreateLost = (1 << 2),
        MovementCreateFound = (1 << 3),
        MovementCreateMove = (1 << 4),

        MovementApprove = (1 << 5),
        MovementOrder = (1 << 6),
        MovementVerify = (1 << 7),
        MovementCancelRequested = (1 << 8),
        MovementCancelApproved = (1 << 9),
        MovementCancelVerified = (1 << 10),

        All = (1 << 11) - 1,
    }
}
