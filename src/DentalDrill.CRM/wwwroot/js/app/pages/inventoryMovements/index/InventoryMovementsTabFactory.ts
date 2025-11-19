namespace DentalDrill.CRM.Pages.InventoryMovements.Index {
    export class InventoryMovementsTabFactory {
        private readonly _options: InventoryMovementsIndexOptions;
        private readonly _api: InventoryMovementApi;

        constructor(options: InventoryMovementsIndexOptions) {
            this._options = options;
            this._api = new InventoryMovementApi(options);
        }

        createTab(id: string, root: HTMLElement): InventoryMovementsTabBase {
            switch (id) {
            case "All":
                return new GridInventoryMovementsAllTab(id, root, this._options);
            case "Tray":
                return new GridInventoryMovementsTrayTab(id, root, this._options);
            case "Requested":
                return new GridInventoryMovementsRequestedTab(id, root, this._options, this._api);
            case "Approved":
                return new GridInventoryMovementsApprovedTab(id, root, this._options, this._api);
            case "Ordered":
                return new GridInventoryMovementsOrderedTab(id, root, this._options, this._api);
            case "Complete":
                return new GridInventoryMovementsCompleteTab(id, root, this._options);

            case "AllGroup":
                return new GridInventoryGroupMovementsAllTab(id, root, this._options);
            case "RequestedGroup":
                return new GridInventoryGroupMovementsRequestedTab(id, root, this._options, this._api);
            case "ApprovedGroup":
                return new GridInventoryGroupMovementsApprovedTab(id, root, this._options, this._api);
            case "OrderedGroup":
                return new GridInventoryGroupMovementsOrderedTab(id, root, this._options, this._api);
            case "CompleteGroup":
                return new GridInventoryGroupMovementsCompleteTab(id, root, this._options);

            case "StatsAvailableStock":
                return new StatsInventoryMovementsAvailableStockTab(id, root, this._options);
            default:
                throw new Error("Invalid tab id");
            }
        }
    }
}