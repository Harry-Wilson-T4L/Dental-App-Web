using System;

namespace DentalDrill.CRM.Models
{
    public static class InventoryMovementTypeHelper
    {
        public static String ToDisplayName(this InventoryMovementType value)
        {
            return value switch
            {
                InventoryMovementType.Initial => "Initial",
                InventoryMovementType.Order => "Order",
                InventoryMovementType.Found => "Found",
                InventoryMovementType.Repair => "Repair",
                InventoryMovementType.Lost => "Lost",
                InventoryMovementType.MoveFromAnotherWorkshop => "Move from another workshop",
                InventoryMovementType.EphemeralMissingRequiredQuantity => "Missing required quantity",
                InventoryMovementType.RepairFragment => "Repair fragment",
                InventoryMovementType.MoveToAnotherWorkshop => "Move to another workshop",
                _ => throw new ArgumentOutOfRangeException(nameof(value), value, null),
            };
        }
    }
}
