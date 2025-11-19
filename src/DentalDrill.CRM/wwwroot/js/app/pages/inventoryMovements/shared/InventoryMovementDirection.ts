namespace DentalDrill.CRM.Pages.InventoryMovements.Shared {
    export enum InventoryMovementDirection {
        Increase = 0,
        Decrease = 1000,
    }

    export class InventoryMovementDirectionHelper {
        static toDisplayString(value: InventoryMovementDirection): string {
            switch (value) {
                case InventoryMovementDirection.Increase:
                    return "Increase";
                case InventoryMovementDirection.Decrease:
                    return "Decrease";
                default:
                    throw new Error("Invalid movement direction");
            }
        }
    }
}