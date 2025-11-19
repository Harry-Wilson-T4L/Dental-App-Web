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
                var GroupVerifyWithEdit;
                (function (GroupVerifyWithEdit) {
                    var InventoryMovementBulkEditor = InventoryMovements.Shared.InventoryMovementBulkEditor;
                    var GroupVerifyWithEditEditor = /** @class */ (function (_super) {
                        __extends(GroupVerifyWithEditEditor, _super);
                        function GroupVerifyWithEditEditor(root, movements) {
                            return _super.call(this, root, movements) || this;
                        }
                        return GroupVerifyWithEditEditor;
                    }(InventoryMovementBulkEditor));
                    GroupVerifyWithEdit.GroupVerifyWithEditEditor = GroupVerifyWithEditEditor;
                })(GroupVerifyWithEdit = InventoryMovements.GroupVerifyWithEdit || (InventoryMovements.GroupVerifyWithEdit = {}));
            })(InventoryMovements = Pages.InventoryMovements || (Pages.InventoryMovements = {}));
        })(Pages = CRM.Pages || (CRM.Pages = {}));
    })(CRM = DentalDrill.CRM || (DentalDrill.CRM = {}));
})(DentalDrill || (DentalDrill = {}));
//# sourceMappingURL=GroupVerifyWithEditEditor.js.map