namespace DentalDrill.CRM.Pages.InventoryMovements.GroupCancel {
    import InventoryMovementBulkViewer = Shared.InventoryMovementBulkViewer;

    export class GroupCancelEditor extends InventoryMovementBulkViewer {
        constructor(root: HTMLElement, movements: Shared.InventoryMovementBulkCollection) {
            super(root, movements);
        }
    }
}