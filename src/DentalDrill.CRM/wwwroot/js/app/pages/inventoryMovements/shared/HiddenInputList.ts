namespace DentalDrill.CRM.Pages.InventoryMovements.Shared {
    export class HiddenInputList {
        private readonly _root: HTMLElement;

        constructor(root: HTMLElement) {
            this._root = root;
        }

        clear() {
            this._root.innerHTML = ``;
        }

        add(name: string, value: string) {
            const input = document.createElement("input");
            input.type = "hidden";
            input.name = name;
            input.value = value;

            this._root.appendChild(input);
        }
    }
}