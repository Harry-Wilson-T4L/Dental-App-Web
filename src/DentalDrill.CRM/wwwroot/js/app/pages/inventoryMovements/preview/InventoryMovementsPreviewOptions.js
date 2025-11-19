var DentalDrill;
(function (DentalDrill) {
    var CRM;
    (function (CRM) {
        var Pages;
        (function (Pages) {
            var InventoryMovements;
            (function (InventoryMovements) {
                var Preview;
                (function (Preview) {
                    var InventoryMovementsPreviewOptions = /** @class */ (function () {
                        function InventoryMovementsPreviewOptions() {
                            this._sku = undefined;
                            this._workshop = undefined;
                            this._tab = undefined;
                        }
                        Object.defineProperty(InventoryMovementsPreviewOptions.prototype, "sku", {
                            get: function () {
                                return this._sku;
                            },
                            set: function (val) {
                                this._sku = val;
                            },
                            enumerable: false,
                            configurable: true
                        });
                        Object.defineProperty(InventoryMovementsPreviewOptions.prototype, "workshop", {
                            get: function () {
                                return this._workshop;
                            },
                            set: function (val) {
                                this._workshop = val;
                            },
                            enumerable: false,
                            configurable: true
                        });
                        Object.defineProperty(InventoryMovementsPreviewOptions.prototype, "tab", {
                            get: function () {
                                return this._tab;
                            },
                            set: function (val) {
                                this._tab = val;
                            },
                            enumerable: false,
                            configurable: true
                        });
                        return InventoryMovementsPreviewOptions;
                    }());
                    Preview.InventoryMovementsPreviewOptions = InventoryMovementsPreviewOptions;
                })(Preview = InventoryMovements.Preview || (InventoryMovements.Preview = {}));
            })(InventoryMovements = Pages.InventoryMovements || (Pages.InventoryMovements = {}));
        })(Pages = CRM.Pages || (CRM.Pages = {}));
    })(CRM = DentalDrill.CRM || (DentalDrill.CRM = {}));
})(DentalDrill || (DentalDrill = {}));
//# sourceMappingURL=InventoryMovementsPreviewOptions.js.map