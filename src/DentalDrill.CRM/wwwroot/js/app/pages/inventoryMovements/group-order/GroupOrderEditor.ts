namespace DentalDrill.CRM.Pages.InventoryMovements.GroupOrder {
    import InventoryMovementBulkViewer = Shared.InventoryMovementBulkViewer;

    export class GroupOrderEditor extends InventoryMovementBulkViewer {
        constructor(root: HTMLElement, movements: Shared.InventoryMovementBulkCollection) {
            super(root, movements);
        }

        init(): void {
            super.init();
            const setPriceCheckbox = this.root.querySelector("input[type=checkbox][name=SetPrice]") as HTMLInputElement;
            const priceTextBox = this.root.querySelector("input[name=Price]") as HTMLInputElement;
            setPriceCheckbox.addEventListener("change", e => {
                if (setPriceCheckbox.checked) {
                    priceTextBox.disabled = false;
                } else {
                    priceTextBox.disabled = true;
                }
            });
        }
    }
}