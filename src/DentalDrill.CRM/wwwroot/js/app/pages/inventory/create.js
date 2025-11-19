var DentalDrill;
(function (DentalDrill) {
    var CRM;
    (function (CRM) {
        var Pages;
        (function (Pages) {
            var Inventory;
            (function (Inventory) {
                var Create;
                (function (Create) {
                    var InventoryCreateEditor = /** @class */ (function () {
                        function InventoryCreateEditor(root) {
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
                        InventoryCreateEditor.prototype.init = function () {
                            var _this = this;
                            this._isGroupNode.on("change", function (e) {
                                if (_this._isGroupNode.prop("checked")) {
                                    _this._initialShelfQuantity.prop("disabled", true);
                                    _this._initialOrderedQuantity.prop("disabled", true);
                                    _this._initialRequestedQuantity.prop("disabled", true);
                                    _this._averagePrice.prop("disabled", true);
                                }
                                else {
                                    _this._initialShelfQuantity.prop("disabled", false);
                                    _this._initialOrderedQuantity.prop("disabled", false);
                                    _this._initialRequestedQuantity.prop("disabled", false);
                                    _this._averagePrice.prop("disabled", false);
                                }
                            });
                        };
                        return InventoryCreateEditor;
                    }());
                    Create.InventoryCreateEditor = InventoryCreateEditor;
                })(Create = Inventory.Create || (Inventory.Create = {}));
            })(Inventory = Pages.Inventory || (Pages.Inventory = {}));
        })(Pages = CRM.Pages || (CRM.Pages = {}));
    })(CRM = DentalDrill.CRM || (DentalDrill.CRM = {}));
})(DentalDrill || (DentalDrill = {}));
//# sourceMappingURL=create.js.map