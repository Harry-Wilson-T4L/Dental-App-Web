var DentalDrill;
(function (DentalDrill) {
    var CRM;
    (function (CRM) {
        var Pages;
        (function (Pages) {
            var InventoryMovements;
            (function (InventoryMovements) {
                var Shared;
                (function (Shared) {
                    var HiddenInputList = /** @class */ (function () {
                        function HiddenInputList(root) {
                            this._root = root;
                        }
                        HiddenInputList.prototype.clear = function () {
                            this._root.innerHTML = "";
                        };
                        HiddenInputList.prototype.add = function (name, value) {
                            var input = document.createElement("input");
                            input.type = "hidden";
                            input.name = name;
                            input.value = value;
                            this._root.appendChild(input);
                        };
                        return HiddenInputList;
                    }());
                    Shared.HiddenInputList = HiddenInputList;
                })(Shared = InventoryMovements.Shared || (InventoryMovements.Shared = {}));
            })(InventoryMovements = Pages.InventoryMovements || (Pages.InventoryMovements = {}));
        })(Pages = CRM.Pages || (CRM.Pages = {}));
    })(CRM = DentalDrill.CRM || (DentalDrill.CRM = {}));
})(DentalDrill || (DentalDrill = {}));
//# sourceMappingURL=HiddenInputList.js.map