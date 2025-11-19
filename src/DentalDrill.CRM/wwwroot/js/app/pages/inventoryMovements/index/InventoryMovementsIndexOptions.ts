namespace DentalDrill.CRM.Pages.InventoryMovements.Index {
    export interface InventorySKUType {
        Id: string;
        Name: string;
    }

    export interface Workshop {
        Id: string;
        Name: string;
    }

    export class InventoryMovementsIndexOptions {
        private _workshop: Workshop;
        private _sku: string;
        private _showGrouped: boolean;
        private _tab: string;
        private _antiForgeryToken: string;
        private _statsTypes: InventorySKUType[];

        constructor() {
            this._sku = undefined;
            this._showGrouped = false;
            this._antiForgeryToken = undefined;
            this._statsTypes = [];
        }

        get workshop(): Workshop {
            return this._workshop;
        }

        set workshop(val: Workshop) {
            this._workshop = val;
        }

        get sku(): string {
            return this._sku;
        }

        set sku(val: string) {
            this._sku = val;
        }

        get showGrouped(): boolean {
            return this._showGrouped;
        }

        set showGrouped(val: boolean) {
            this._showGrouped = val;
        }

        get tab(): string {
            return this._tab;
        }

        set tab(val: string) {
            this._tab = val;
        }

        get antiForgeryToken(): string {
            return this._antiForgeryToken;
        }

        set antiForgeryToken(val: string) {
            this._antiForgeryToken = val;
        }

        get statsTypes(): InventorySKUType[] {
            return this._statsTypes;
        }
    }
}