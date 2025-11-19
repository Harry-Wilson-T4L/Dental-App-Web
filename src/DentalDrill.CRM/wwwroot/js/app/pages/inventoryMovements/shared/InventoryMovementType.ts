namespace DentalDrill.CRM.Pages.InventoryMovements.Shared {
    export enum InventoryMovementType {
        Initial = 0,
        Order = 1,
        Found = 2,
        MoveFromAnotherWorkshop = 100,
        EphemeralMissingRequiredQuantity = 500,
        Repair = 1000,
        Lost = 1001,
        RepairFragment = 1002,
        MoveToAnotherWorkshop = 1100,
    }

    export enum InventoryMovementCreateType {
        Order = 1,
        Found = 2,
        Lost = 1001,
        MoveBetweenWorkshops = 5000,
        MoveFromAnotherWorkshop = 100,
        MoveToAnotherWorkshop = 1100,
    }

    export class InventoryMovementTypeHelper {
        static toDisplayString(value: InventoryMovementType): string {
            switch (value) {
                case InventoryMovementType.Initial:
                    return "Initial";
                case InventoryMovementType.Order:
                    return "Order";
                case InventoryMovementType.Found:
                    return "Found";
                case InventoryMovementType.MoveFromAnotherWorkshop:
                    return "Move from another workshop";
                case InventoryMovementType.EphemeralMissingRequiredQuantity:
                    return "Pending Order";
                case InventoryMovementType.Repair:
                    return "Repair";
                case InventoryMovementType.Lost:
                    return "Lost";
                case InventoryMovementType.RepairFragment:
                    return "Repair (Partial)";
                case InventoryMovementType.MoveToAnotherWorkshop:
                    return "Move to another workshop";
                default:
                    throw new Error("Invalid movement type");
            }
        }

        static createDataSource(): kendo.data.DataSource {
            const items: InventoryMovementType[] = [
                InventoryMovementType.Initial,
                InventoryMovementType.Order,
                InventoryMovementType.Found,
                InventoryMovementType.MoveFromAnotherWorkshop,
                InventoryMovementType.EphemeralMissingRequiredQuantity,
                InventoryMovementType.Repair,
                InventoryMovementType.Lost,
                InventoryMovementType.RepairFragment,
                InventoryMovementType.MoveToAnotherWorkshop,
            ];

            return new kendo.data.DataSource({
                data: items.map(x => ({ name: InventoryMovementTypeHelper.toDisplayString(x), value: x }))
            });
        }
    }

    export class InventoryMovementCreateTypeHelper {
        static toDisplayString(value: InventoryMovementCreateType): string {
            switch (value) {
                case InventoryMovementCreateType.Order:
                    return "Order";
                case InventoryMovementCreateType.Found:
                    return "Found";
                case InventoryMovementCreateType.Lost:
                    return "Lost";
                case InventoryMovementCreateType.MoveBetweenWorkshops:
                    return "Move between workshops";
                case InventoryMovementCreateType.MoveFromAnotherWorkshop:
                    return "Move from another workshop";
                case InventoryMovementCreateType.MoveToAnotherWorkshop:
                    return "Move to another workshop";
                default:
                    throw new Error("Invalid movement type");
            }
        }
    }
}