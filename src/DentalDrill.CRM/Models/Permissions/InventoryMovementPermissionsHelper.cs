using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DentalDrill.CRM.Models.Permissions
{
    public static class InventoryMovementPermissionsHelper
    {
        public static IReadOnlyList<InventoryMovementPermissions> GetAllFlags()
        {
            return new InventoryMovementPermissions[]
            {
                InventoryMovementPermissions.MovementRead,
                InventoryMovementPermissions.MovementCreateOrder,
                InventoryMovementPermissions.MovementCreateLost,
                InventoryMovementPermissions.MovementCreateFound,
                InventoryMovementPermissions.MovementCreateMove,
                InventoryMovementPermissions.MovementApprove,
                InventoryMovementPermissions.MovementOrder,
                InventoryMovementPermissions.MovementVerify,
                InventoryMovementPermissions.MovementCancelRequested,
                InventoryMovementPermissions.MovementCancelApproved,
                InventoryMovementPermissions.MovementCancelVerified,
            };
        }

        public static IEnumerable<SelectListItem> MakeSelectList()
        {
            return new SelectList(InventoryMovementPermissionsHelper.GetAllFlags().Select(x => Tuple.Create(x, x.ToFlagDisplayString())), "Item1", "Item2");
        }

        public static String ToDisplayString(this InventoryMovementPermissions value)
        {
            return value switch
            {
                InventoryMovementPermissions.All => "All",
                InventoryMovementPermissions.None => "None",
                _ => String.Join(", ", value.SplitValue().Select(x => x.ToFlagDisplayString())),
            };
        }

        public static IReadOnlyList<InventoryMovementPermissions> SplitValue(this InventoryMovementPermissions value)
        {
            return InventoryMovementPermissionsHelper.GetAllFlags().Where(flag => (value & flag) == flag).ToList();
        }

        public static InventoryMovementPermissions CombineValue(this IReadOnlyList<InventoryMovementPermissions> values)
        {
            return values.Aggregate(InventoryMovementPermissions.None, (acc, x) => acc | x);
        }

        private static String ToFlagDisplayString(this InventoryMovementPermissions value)
        {
            return value switch
            {
                InventoryMovementPermissions.MovementRead => "Read movements",
                InventoryMovementPermissions.MovementCreateOrder => "Create Order movement",
                InventoryMovementPermissions.MovementCreateLost => "Create Lost movement",
                InventoryMovementPermissions.MovementCreateFound => "Create Found movement",
                InventoryMovementPermissions.MovementCreateMove => "Create Move movement",
                InventoryMovementPermissions.MovementApprove => "Approve requested Order movement",
                InventoryMovementPermissions.MovementOrder => "Order approved Order movement",
                InventoryMovementPermissions.MovementVerify => "Verify ordered Order movement",
                InventoryMovementPermissions.MovementCancelRequested => "Cancel requested Order movement",
                InventoryMovementPermissions.MovementCancelApproved => "Cancel approved Order movement",
                InventoryMovementPermissions.MovementCancelVerified => "Cancel verified Order movement",
                _ => throw new ArgumentOutOfRangeException(nameof(value), value, null),
            };
        }
    }
}
