using System;

namespace DentalDrill.CRM.Models.Permissions
{
    [Flags]
    public enum InventoryPermissions : Int64
    {
        None = 0,

        SKURead = (1 << 0),
        SKUWrite = (1 << 1),
        SKUTypeWrite = (1 << 2),

        LegacyStockControl = (1 << 3),

        All = (1 << 4) - 1,
    }
}
