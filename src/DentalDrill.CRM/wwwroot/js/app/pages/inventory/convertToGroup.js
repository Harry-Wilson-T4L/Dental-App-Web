var DentalDrill;
(function (DentalDrill) {
    var CRM;
    (function (CRM) {
        var Pages;
        (function (Pages) {
            var Inventory;
            (function (Inventory) {
                var ConvertToGroup;
                (function (ConvertToGroup) {
                    var InventoryConvertToGroupEditor = /** @class */ (function () {
                        function InventoryConvertToGroupEditor(root) {
                            this._root = root;
                            this._rootNode = $(root);
                            this._createNewGroup = this._rootNode.find("input[type=checkbox][name=CreateNewGroup]");
                            this._groupName = this._rootNode.find("input[name=GroupName]");
                            this._leafName = this._rootNode.find("input[name=LeafName]");
                            this._groupNameWrapper = this._groupName.closest(".row");
                            this._leafNameWrapper = this._leafName.closest(".row");
                        }
                        InventoryConvertToGroupEditor.prototype.init = function () {
                            var _this = this;
                            this._createNewGroup.on("change", function (e) {
                                _this.update();
                            });
                            this.update();
                        };
                        InventoryConvertToGroupEditor.prototype.update = function () {
                            if (this._createNewGroup.prop("checked")) {
                                this._groupName.prop("readonly", false);
                                this._leafName.prop("readonly", false);
                                this._groupNameWrapper.removeClass("k-state-disabled");
                                this._leafNameWrapper.removeClass("k-state-disabled");
                            }
                            else {
                                this._groupName.prop("readonly", true);
                                this._leafName.prop("readonly", true);
                                this._groupNameWrapper.addClass("k-state-disabled");
                                this._leafNameWrapper.addClass("k-state-disabled");
                            }
                        };
                        return InventoryConvertToGroupEditor;
                    }());
                    ConvertToGroup.InventoryConvertToGroupEditor = InventoryConvertToGroupEditor;
                })(ConvertToGroup = Inventory.ConvertToGroup || (Inventory.ConvertToGroup = {}));
            })(Inventory = Pages.Inventory || (Pages.Inventory = {}));
        })(Pages = CRM.Pages || (CRM.Pages = {}));
    })(CRM = DentalDrill.CRM || (DentalDrill.CRM = {}));
})(DentalDrill || (DentalDrill = {}));
//# sourceMappingURL=convertToGroup.js.map