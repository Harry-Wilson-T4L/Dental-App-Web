namespace DentalDrill.CRM.Pages.InventoryMovements.Shared {
    export enum InventoryMovementStatus {
        Cancelled = 0,
        Requested = 10,
        Approved = 20,
        Ordered = 30,
        Waiting = 40,
        Allocated = 90,
        Completed = 100,
    }

    export class InventoryMovementStatusHelper {
        static toDisplayString(value: InventoryMovementStatus): string {
            switch (value) {
                case InventoryMovementStatus.Cancelled:
                    return "Cancelled";
                case InventoryMovementStatus.Requested:
                    return "To be approved";
                case InventoryMovementStatus.Approved:
                    return "To be ordered";
                case InventoryMovementStatus.Ordered:
                    return "Ordered";
                case InventoryMovementStatus.Waiting:
                    return "Waiting";
                case InventoryMovementStatus.Allocated:
                    return "Allocated";
                case InventoryMovementStatus.Completed:
                    return "Completed";
                default:
                    throw new Error("Invalid movement status");
            }
        }

        static createDataSource(): kendo.data.DataSource {
            const items: InventoryMovementStatus[] = [
                InventoryMovementStatus.Requested,
                InventoryMovementStatus.Approved,
                InventoryMovementStatus.Ordered,
                InventoryMovementStatus.Waiting,
                InventoryMovementStatus.Allocated,
                InventoryMovementStatus.Completed,
            ];

            return new kendo.data.DataSource({
                data: items.map(x => ({ name: InventoryMovementStatusHelper.toDisplayString(x), value: x }))
            });
        }
    }
}