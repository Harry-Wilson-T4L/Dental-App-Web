namespace DentalDrill.CRM.Pages.Inventory.Create {
    export class InventoryCreateEditor {
        private readonly _root: HTMLElement;
        private readonly _rootNode: JQuery<HTMLElement>;

        private readonly _name: JQuery<HTMLElement>;
        private readonly _initialShelfQuantity: JQuery<HTMLElement>;
        private readonly _initialOrderedQuantity: JQuery<HTMLElement>;
        private readonly _initialRequestedQuantity: JQuery<HTMLElement>;
        private readonly _warningQuantity: JQuery<HTMLElement>;
        private readonly _averagePrice: JQuery<HTMLElement>;
        private readonly _description: JQuery<HTMLElement>;
        private readonly _isGroupNode: JQuery<HTMLElement>;


        constructor(root: HTMLElement) {
            this._root = root;
            this._rootNode = $(root);

            this._name = this._rootNode.find("input[name=Name]");
            this._initialShelfQuantity = this._rootNode.find("input[name=InitialShelfQuantity]");
            this._initialOrderedQuantity = this._rootNode.find("input[name=InitialOrderedQuantity]");
            this._initialRequestedQuantity = this._rootNode.find("input[name=InitialRequestedQuantity]");
            this._warningQuantity = this._rootNode.find("input[name=WarningQuantity]");
            this._averagePrice = this._rootNode.find("input[name=AveragePrice]");
            this._description = this._rootNode.find("textarea[name=Description]");
            this._isGroupNode = this._rootNode.find("input[type=checkbox][name=IsGroupNode]");
        }

        init() {
            this._isGroupNode.on("change", (e: JQueryEventObject) => {
                if (this._isGroupNode.prop("checked")) {
                    this._initialShelfQuantity.prop("disabled", true);
                    this._initialOrderedQuantity.prop("disabled", true);
                    this._initialRequestedQuantity.prop("disabled", true);
                    this._averagePrice.prop("disabled", true);
                } else {
                    this._initialShelfQuantity.prop("disabled", false);
                    this._initialOrderedQuantity.prop("disabled", false);
                    this._initialRequestedQuantity.prop("disabled", false);
                    this._averagePrice.prop("disabled", false);
                }
            });
        }
    }
}