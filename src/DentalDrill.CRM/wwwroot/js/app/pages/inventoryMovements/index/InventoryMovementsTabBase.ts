namespace DentalDrill.CRM.Pages.InventoryMovements.Index {
    export abstract class InventoryMovementsTabBase {
        private readonly _id: string;
        private readonly _root: HTMLElement;
        private _initialized: boolean;

        constructor(id: string, root: HTMLElement) {
            this._id = id;
            this._root = root;
        }

        get id(): string {
            return this._id;
        }

        protected get root(): HTMLElement {
            return this._root;
        }

        get initialized(): boolean {
            return this._initialized;
        }

        init() {
            if (!this._initialized) {
                this.initInternal();
                this._initialized = true;
            }
        }

        protected abstract initInternal();

        activate() {
        }

        resize(visible: boolean) {
        }
    }
}