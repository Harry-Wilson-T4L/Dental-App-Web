namespace DentalDrill.CRM.Pages.InventoryMovements.GroupVerifyWithEdit {
    import InventoryMovementBulkEditor = Shared.InventoryMovementBulkEditor;

    export class GroupVerifyWithEditEditor extends InventoryMovementBulkEditor {
        constructor(root: HTMLElement, movements: Shared.InventoryMovementBulkCollection) {
            super(root, movements);
        }
    }
}