var DentalDrill;
(function (DentalDrill) {
    var CRM;
    (function (CRM) {
        var Pages;
        (function (Pages) {
            var InventoryMovements;
            (function (InventoryMovements) {
                var Index;
                (function (Index) {
                    var InventoryMovementsIndexOptions = /** @class */ (function () {
                        function InventoryMovementsIndexOptions() {
                            this._sku = undefined;
                            this._showGrouped = false;
                            this._antiForgeryToken = undefined;
                            this._statsTypes = [];
                        }
                        Object.defineProperty(InventoryMovementsIndexOptions.prototype, "workshop", {
                            get: function () {
                                return this._workshop;
                            },
                            set: function (val) {
                                this._workshop = val;
                            },
                            enumerable: false,
                            configurable: true
                        });
                        Object.defineProperty(InventoryMovementsIndexOptions.prototype, "sku", {
                            get: function () {
                                return this._sku;
                            },
                            set: function (val) {
                                this._sku = val;
                            },
                            enumerable: false,
                            configurable: true
                        });
                        Object.defineProperty(InventoryMovementsIndexOptions.prototype, "showGrouped", {
                            get: function () {
                                return this._showGrouped;
                            },
                            set: function (val) {
                                this._showGrouped = val;
                            },
                            enumerable: false,
                            configurable: true
                        });
                        Object.defineProperty(InventoryMovementsIndexOptions.prototype, "tab", {
                            get: function () {
                                return this._tab;
                            },
                            set: function (val) {
                                this._tab = val;
                            },
                            enumerable: false,
                            configurable: true
                        });
                        Object.defineProperty(InventoryMovementsIndexOptions.prototype, "antiForgeryToken", {
                            get: function () {
                                return this._antiForgeryToken;
                            },
                            set: function (val) {
                                this._antiForgeryToken = val;
                            },
                            enumerable: false,
                            configurable: true
                        });
                        Object.defineProperty(InventoryMovementsIndexOptions.prototype, "statsTypes", {
                            get: function () {
                                return this._statsTypes;
                            },
                            enumerable: false,
                            configurable: true
                        });
                        return InventoryMovementsIndexOptions;
                    }());
                    Index.InventoryMovementsIndexOptions = InventoryMovementsIndexOptions;
                })(Index = InventoryMovements.Index || (InventoryMovements.Index = {}));
            })(InventoryMovements = Pages.InventoryMovements || (Pages.InventoryMovements = {}));
        })(Pages = CRM.Pages || (CRM.Pages = {}));
    })(CRM = DentalDrill.CRM || (DentalDrill.CRM = {}));
})(DentalDrill || (DentalDrill = {}));
//# sourceMappingURL=InventoryMovementsIndexOptions.js.map