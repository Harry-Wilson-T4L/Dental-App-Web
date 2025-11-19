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
                    var InventoryMovementsTabBase = /** @class */ (function () {
                        function InventoryMovementsTabBase(id, root) {
                            this._id = id;
                            this._root = root;
                        }
                        Object.defineProperty(InventoryMovementsTabBase.prototype, "id", {
                            get: function () {
                                return this._id;
                            },
                            enumerable: false,
                            configurable: true
                        });
                        Object.defineProperty(InventoryMovementsTabBase.prototype, "root", {
                            get: function () {
                                return this._root;
                            },
                            enumerable: false,
                            configurable: true
                        });
                        Object.defineProperty(InventoryMovementsTabBase.prototype, "initialized", {
                            get: function () {
                                return this._initialized;
                            },
                            enumerable: false,
                            configurable: true
                        });
                        InventoryMovementsTabBase.prototype.init = function () {
                            if (!this._initialized) {
                                this.initInternal();
                                this._initialized = true;
                            }
                        };
                        InventoryMovementsTabBase.prototype.activate = function () {
                        };
                        InventoryMovementsTabBase.prototype.resize = function (visible) {
                        };
                        return InventoryMovementsTabBase;
                    }());
                    Index.InventoryMovementsTabBase = InventoryMovementsTabBase;
                })(Index = InventoryMovements.Index || (InventoryMovements.Index = {}));
            })(InventoryMovements = Pages.InventoryMovements || (Pages.InventoryMovements = {}));
        })(Pages = CRM.Pages || (CRM.Pages = {}));
    })(CRM = DentalDrill.CRM || (DentalDrill.CRM = {}));
})(DentalDrill || (DentalDrill = {}));
//# sourceMappingURL=InventoryMovementsTabBase.js.map