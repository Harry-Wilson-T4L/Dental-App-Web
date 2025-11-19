var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (Object.prototype.hasOwnProperty.call(b, p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        if (typeof b !== "function" && b !== null)
            throw new TypeError("Class extends value " + String(b) + " is not a constructor or null");
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var DentalDrill;
(function (DentalDrill) {
    var CRM;
    (function (CRM) {
        var Pages;
        (function (Pages) {
            var InventoryMovements;
            (function (InventoryMovements) {
                var GroupOrder;
                (function (GroupOrder) {
                    var InventoryMovementBulkViewer = InventoryMovements.Shared.InventoryMovementBulkViewer;
                    var GroupOrderEditor = /** @class */ (function (_super) {
                        __extends(GroupOrderEditor, _super);
                        function GroupOrderEditor(root, movements) {
                            return _super.call(this, root, movements) || this;
                        }
                        GroupOrderEditor.prototype.init = function () {
                            _super.prototype.init.call(this);
                            var setPriceCheckbox = this.root.querySelector("input[type=checkbox][name=SetPrice]");
                            var priceTextBox = this.root.querySelector("input[name=Price]");
                            setPriceCheckbox.addEventListener("change", function (e) {
                                if (setPriceCheckbox.checked) {
                                    priceTextBox.disabled = false;
                                }
                                else {
                                    priceTextBox.disabled = true;
                                }
                            });
                        };
                        return GroupOrderEditor;
                    }(InventoryMovementBulkViewer));
                    GroupOrder.GroupOrderEditor = GroupOrderEditor;
                })(GroupOrder = InventoryMovements.GroupOrder || (InventoryMovements.GroupOrder = {}));
            })(InventoryMovements = Pages.InventoryMovements || (Pages.InventoryMovements = {}));
        })(Pages = CRM.Pages || (CRM.Pages = {}));
    })(CRM = DentalDrill.CRM || (DentalDrill.CRM = {}));
})(DentalDrill || (DentalDrill = {}));
//# sourceMappingURL=GroupOrderEditor.js.map