using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DentalDrill.CRM.Models
{
    public static class HandpieceStoreOrderStatusHelper
    {
        public static String ToLocalizedString(this HandpieceStoreOrderStatus value)
        {
            return value switch
            {
                HandpieceStoreOrderStatus.Created => "Created",
                HandpieceStoreOrderStatus.Cancelled => "Cancelled",
                HandpieceStoreOrderStatus.Removed => "Removed",
                HandpieceStoreOrderStatus.Confirmed => "Confirmed",
                HandpieceStoreOrderStatus.Shipped => "Shipped",
                HandpieceStoreOrderStatus.Delivered => "Delivered",
                _ => throw new ArgumentOutOfRangeException(nameof(value), value, null),
            };
        }

        public static String ToActionString(this HandpieceStoreOrderStatus value)
        {
            return value switch
            {
                HandpieceStoreOrderStatus.Created => "Create",
                HandpieceStoreOrderStatus.Cancelled => "Cancel",
                HandpieceStoreOrderStatus.Removed => "Remove",
                HandpieceStoreOrderStatus.Confirmed => "Confirm",
                HandpieceStoreOrderStatus.Shipped => "Ship",
                HandpieceStoreOrderStatus.Delivered => "Deliver",
                _ => throw new ArgumentOutOfRangeException(nameof(value), value, null),
            };
        }
    }
}
