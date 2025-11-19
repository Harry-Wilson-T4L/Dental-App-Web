namespace DentalDrill.CRM.Pages.InventoryMovements.GroupApproveWithEdit {
    import InventoryMovementBulkEditor = Shared.InventoryMovementBulkEditor;

    export class GroupApproveWithEditEditor extends InventoryMovementBulkEditor {
        constructor(root: HTMLElement, movements: Shared.InventoryMovementBulkCollection) {
            super(root, movements);
        }
    }
}