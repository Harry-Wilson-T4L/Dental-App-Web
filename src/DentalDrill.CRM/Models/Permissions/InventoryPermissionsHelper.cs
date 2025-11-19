using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DentalDrill.CRM.Models.Permissions
{
    public static class InventoryPermissionsHelper
    {
        public static IReadOnlyList<InventoryPermissions> GetAllFlags()
        {
            return new InventoryPermissions[]
            {
                InventoryPermissions.SKURead,
                InventoryPermissions.SKUWrite,
                InventoryPermissions.SKUTypeWrite,
                InventoryPermissions.LegacyStockControl,
            };
        }

        public static IEnumerable<SelectListItem> MakeSelectList()
        {
            return new SelectList(InventoryPermissionsHelper.GetAllFlags().Select(x => Tuple.Create(x, x.ToFlagDisplayString())), "Item1", "Item2");
        }

        public static String ToDisplayString(this InventoryPermissions value)
        {
            return value switch
            {
                InventoryPermissions.All => "All",
                InventoryPermissions.None => "None",
                _ => String.Join(", ", value.SplitValue().Select(x => x.ToFlagDisplayString())),
            };
        }

        public static IReadOnlyList<InventoryPermissions> SplitValue(this InventoryPermissions value)
        {
            return InventoryPermissionsHelper.GetAllFlags().Where(flag => (value & flag) == flag).ToList();
        }

        public static InventoryPermissions CombineValue(this IReadOnlyList<InventoryPermissions> values)
        {
            return values.Aggregate(InventoryPermissions.None, (acc, x) => acc | x);
        }

        private static String ToFlagDisplayString(this InventoryPermissions value)
        {
            return value switch
            {
                InventoryPermissions.SKURead => "Read SKUs",
                InventoryPermissions.SKUWrite => "Write SKUs",
                InventoryPermissions.SKUTypeWrite => "Write SKU Types",
                InventoryPermissions.LegacyStockControl => "Stock Control (Legacy)",
                _ => value.ToString(),
            };
        }
    }
}
