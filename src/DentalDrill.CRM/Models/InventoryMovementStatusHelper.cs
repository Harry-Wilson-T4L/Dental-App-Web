using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DentalDrill.CRM.Models
{
    public static class InventoryMovementStatusHelper
    {
        public static String ToDisplayName(this InventoryMovementStatus value)
        {
            return value switch
            {
                InventoryMovementStatus.Cancelled => "Cancelled",
                InventoryMovementStatus.Requested => "To be approved",
                InventoryMovementStatus.Approved => "To be ordered",
                InventoryMovementStatus.Ordered => "Ordered",
                InventoryMovementStatus.Waiting => "Waiting",
                InventoryMovementStatus.Allocated => "Allocated",
                InventoryMovementStatus.Completed => "Completed",
                _ => throw new ArgumentOutOfRangeException(nameof(value), value, null)
            };
        }
    }
}
