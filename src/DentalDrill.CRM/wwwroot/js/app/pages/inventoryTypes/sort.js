var DentalDrill;
(function (DentalDrill) {
    var CRM;
    (function (CRM) {
        var Pages;
        (function (Pages) {
            var InventoryTypes;
            (function (InventoryTypes) {
                var Sort;
                (function (Sort) {
                    var InventoryTypeSortEditor = /** @class */ (function () {
                        function InventoryTypeSortEditor(root) {
                            this._root = root;
                            this._scopeLabel = root.querySelector("label[for=Scope]");
                            this._scopeRadioGroup = $(root.querySelector(".k-radio-list[name=Scope]")).data("kendoRadioGroup");
                            this._specificSKULabel = root.querySelector("label[for=SpecificSKU]");
                            this._specificSKUDropDown = $(root.querySelector(".k-dropdown input[name=SpecificSKU]")).data("kendoDropDownList");
                            this._methodLabel = root.querySelector("label[for=Method]");
                            this._methodRadioGroup = $(root.querySelector(".k-radio-list[name=Method]")).data("kendoRadioGroup");
                        }
                        InventoryTypeSortEditor.prototype.init = function () {
                            var _this = this;
                            this._scopeRadioGroup.bind("change", function (e) {
                                var newValue = e.sender.value();
                                if (newValue == "Specific" || newValue == "SpecificRecursive") {
                                    _this._specificSKULabel.classList.remove("k-disabled");
                                    _this._specificSKUDropDown.enable(true);
                                }
                                else {
                                    _this._specificSKULabel.classList.add("k-disabled");
                                    _this._specificSKUDropDown.enable(false);
                                }
                            });
                        };
                        return InventoryTypeSortEditor;
                    }());
                    Sort.InventoryTypeSortEditor = InventoryTypeSortEditor;
                })(Sort = InventoryTypes.Sort || (InventoryTypes.Sort = {}));
            })(InventoryTypes = Pages.InventoryTypes || (Pages.InventoryTypes = {}));
        })(Pages = CRM.Pages || (CRM.Pages = {}));
    })(CRM = DentalDrill.CRM || (DentalDrill.CRM = {}));
})(DentalDrill || (DentalDrill = {}));
//# sourceMappingURL=sort.js.map