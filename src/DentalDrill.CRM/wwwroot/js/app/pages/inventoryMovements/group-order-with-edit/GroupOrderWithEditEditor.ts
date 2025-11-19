namespace DentalDrill.CRM.Pages.InventoryMovements.GroupOrderWithEdit {
    import InventoryMovementBulkEditor = Shared.InventoryMovementBulkEditor;

    export class GroupOrderWithEditEditor extends InventoryMovementBulkEditor {
        constructor(root: HTMLElement, movements: Shared.InventoryMovementBulkCollection) {
            super(root, movements);
        }
    }
}