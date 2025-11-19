using System;
using System.Collections.Generic;

namespace DentalDrill.CRM.Models.ViewModels.InventoryMovements
{
    public static class InventoryMovementCreateTypeHelper
    {
        public static IReadOnlyList<InventoryMovementCreateType> AvailableTypes { get; } = new[]
        {
            InventoryMovementCreateType.Order,
            InventoryMovementCreateType.Found,
            InventoryMovementCreateType.Lost,
            InventoryMovementCreateType.MoveBetweenWorkshops,
        };

        public static IReadOnlyList<InventoryMovementCreateType> AvailableTypesPreselectedWorkshop { get; } = new[]
        {
            InventoryMovementCreateType.Order,
            InventoryMovementCreateType.Found,
            InventoryMovementCreateType.Lost,
            InventoryMovementCreateType.MoveFromAnotherWorkshop,
            InventoryMovementCreateType.MoveToAnotherWorkshop,
        };

        public static String ToDisplayName(this InventoryMovementCreateType value)
        {
            return value switch
            {
                InventoryMovementCreateType.Order => "Order",
                InventoryMovementCreateType.Found => "Found",
                InventoryMovementCreateType.Lost => "Lost",
                InventoryMovementCreateType.MoveBetweenWorkshops => "Move between workshops",
                InventoryMovementCreateType.MoveFromAnotherWorkshop => "Move from another workshop",
                InventoryMovementCreateType.MoveToAnotherWorkshop => "Move to another workshop",
                _ => throw new ArgumentOutOfRangeException(nameof(value), value, null),
            };
        }
    }
}
