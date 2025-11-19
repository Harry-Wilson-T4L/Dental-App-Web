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
                    var InventoryMovementDirection;
                    (function (InventoryMovementDirection) {
                        InventoryMovementDirection[InventoryMovementDirection["Increase"] = 0] = "Increase";
                        InventoryMovementDirection[InventoryMovementDirection["Decrease"] = 1000] = "Decrease";
                    })(InventoryMovementDirection = Shared.InventoryMovementDirection || (Shared.InventoryMovementDirection = {}));
                    var InventoryMovementDirectionHelper = /** @class */ (function () {
                        function InventoryMovementDirectionHelper() {
                        }
                        InventoryMovementDirectionHelper.toDisplayString = function (value) {
                            switch (value) {
                                case InventoryMovementDirection.Increase:
                                    return "Increase";
                                case InventoryMovementDirection.Decrease:
                                    return "Decrease";
                                default:
                                    throw new Error("Invalid movement direction");
                            }
                        };
                        return InventoryMovementDirectionHelper;
                    }());
                    Shared.InventoryMovementDirectionHelper = InventoryMovementDirectionHelper;
                })(Shared = InventoryMovements.Shared || (InventoryMovements.Shared = {}));
            })(InventoryMovements = Pages.InventoryMovements || (Pages.InventoryMovements = {}));
        })(Pages = CRM.Pages || (CRM.Pages = {}));
    })(CRM = DentalDrill.CRM || (DentalDrill.CRM = {}));
})(DentalDrill || (DentalDrill = {}));
//# sourceMappingURL=InventoryMovementDirection.js.map