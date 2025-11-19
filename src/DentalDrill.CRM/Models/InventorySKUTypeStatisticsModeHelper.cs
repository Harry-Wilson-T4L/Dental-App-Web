using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DentalDrill.CRM.Models
{
    public static class InventorySKUTypeStatisticsModeHelper
    {
        public static IEnumerable<SelectListItem> MakeSelectList()
        {
            return new SelectList(
                new[]
                {
                    Tuple.Create(InventorySKUTypeStatisticsMode.Hidden, InventorySKUTypeStatisticsMode.Hidden.ToDisplayString()),
                    Tuple.Create(InventorySKUTypeStatisticsMode.ShowTopLevel, InventorySKUTypeStatisticsMode.ShowTopLevel.ToDisplayString()),
                    Tuple.Create(InventorySKUTypeStatisticsMode.ShowSpecificSKUs, InventorySKUTypeStatisticsMode.ShowSpecificSKUs.ToDisplayString()),
                },
                "Item1",
                "Item2");
        }

        public static String ToDisplayString(this InventorySKUTypeStatisticsMode value)
        {
            return value switch
            {
                InventorySKUTypeStatisticsMode.Hidden => "Don't show",
                InventorySKUTypeStatisticsMode.ShowTopLevel => "Show top level SKUs",
                InventorySKUTypeStatisticsMode.ShowSpecificSKUs => "Show specific SKUs",
                _ => throw new ArgumentOutOfRangeException(nameof(value), value, null),
            };
        }
    }
}
