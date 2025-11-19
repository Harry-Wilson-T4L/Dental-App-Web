namespace DentalDrill.CRM.Pages.InventoryMovements.Preview {
    export class InventoryMovementsPreviewOptions {
        private _sku: string;
        private _workshop: string;
        private _tab: string;

        constructor() {
            this._sku = undefined;
            this._workshop = undefined;
            this._tab = undefined;
        }

        get sku(): string {
            return this._sku;
        }

        set sku(val: string) {
            this._sku = val;
        }

        get workshop(): string {
            return this._workshop;
        }

        set workshop(val: string) {
            this._workshop = val;
        }

        get tab(): string {
            return this._tab;
        }

        set tab(val: string) {
            this._tab = val;
        }
    }
}