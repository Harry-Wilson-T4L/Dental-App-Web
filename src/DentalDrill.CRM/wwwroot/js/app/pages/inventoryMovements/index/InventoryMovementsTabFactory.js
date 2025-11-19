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
                    var InventoryMovementsTabFactory = /** @class */ (function () {
                        function InventoryMovementsTabFactory(options) {
                            this._options = options;
                            this._api = new Index.InventoryMovementApi(options);
                        }
                        InventoryMovementsTabFactory.prototype.createTab = function (id, root) {
                            switch (id) {
                                case "All":
                                    return new Index.GridInventoryMovementsAllTab(id, root, this._options);
                                case "Tray":
                                    return new Index.GridInventoryMovementsTrayTab(id, root, this._options);
                                case "Requested":
                                    return new Index.GridInventoryMovementsRequestedTab(id, root, this._options, this._api);
                                case "Approved":
                                    return new Index.GridInventoryMovementsApprovedTab(id, root, this._options, this._api);
                                case "Ordered":
                                    return new Index.GridInventoryMovementsOrderedTab(id, root, this._options, this._api);
                                case "Complete":
                                    return new Index.GridInventoryMovementsCompleteTab(id, root, this._options);
                                case "AllGroup":
                                    return new Index.GridInventoryGroupMovementsAllTab(id, root, this._options);
                                case "RequestedGroup":
                                    return new Index.GridInventoryGroupMovementsRequestedTab(id, root, this._options, this._api);
                                case "ApprovedGroup":
                                    return new Index.GridInventoryGroupMovementsApprovedTab(id, root, this._options, this._api);
                                case "OrderedGroup":
                                    return new Index.GridInventoryGroupMovementsOrderedTab(id, root, this._options, this._api);
                                case "CompleteGroup":
                                    return new Index.GridInventoryGroupMovementsCompleteTab(id, root, this._options);
                                case "StatsAvailableStock":
                                    return new Index.StatsInventoryMovementsAvailableStockTab(id, root, this._options);
                                default:
                                    throw new Error("Invalid tab id");
                            }
                        };
                        return InventoryMovementsTabFactory;
                    }());
                    Index.InventoryMovementsTabFactory = InventoryMovementsTabFactory;
                })(Index = InventoryMovements.Index || (InventoryMovements.Index = {}));
            })(InventoryMovements = Pages.InventoryMovements || (Pages.InventoryMovements = {}));
        })(Pages = CRM.Pages || (CRM.Pages = {}));
    })(CRM = DentalDrill.CRM || (DentalDrill.CRM = {}));
})(DentalDrill || (DentalDrill = {}));
//# sourceMappingURL=InventoryMovementsTabFactory.js.map