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
                    var InventoryMovementType;
                    (function (InventoryMovementType) {
                        InventoryMovementType[InventoryMovementType["Initial"] = 0] = "Initial";
                        InventoryMovementType[InventoryMovementType["Order"] = 1] = "Order";
                        InventoryMovementType[InventoryMovementType["Found"] = 2] = "Found";
                        InventoryMovementType[InventoryMovementType["MoveFromAnotherWorkshop"] = 100] = "MoveFromAnotherWorkshop";
                        InventoryMovementType[InventoryMovementType["EphemeralMissingRequiredQuantity"] = 500] = "EphemeralMissingRequiredQuantity";
                        InventoryMovementType[InventoryMovementType["Repair"] = 1000] = "Repair";
                        InventoryMovementType[InventoryMovementType["Lost"] = 1001] = "Lost";
                        InventoryMovementType[InventoryMovementType["RepairFragment"] = 1002] = "RepairFragment";
                        InventoryMovementType[InventoryMovementType["MoveToAnotherWorkshop"] = 1100] = "MoveToAnotherWorkshop";
                    })(InventoryMovementType = Shared.InventoryMovementType || (Shared.InventoryMovementType = {}));
                    var InventoryMovementCreateType;
                    (function (InventoryMovementCreateType) {
                        InventoryMovementCreateType[InventoryMovementCreateType["Order"] = 1] = "Order";
                        InventoryMovementCreateType[InventoryMovementCreateType["Found"] = 2] = "Found";
                        InventoryMovementCreateType[InventoryMovementCreateType["Lost"] = 1001] = "Lost";
                        InventoryMovementCreateType[InventoryMovementCreateType["MoveBetweenWorkshops"] = 5000] = "MoveBetweenWorkshops";
                        InventoryMovementCreateType[InventoryMovementCreateType["MoveFromAnotherWorkshop"] = 100] = "MoveFromAnotherWorkshop";
                        InventoryMovementCreateType[InventoryMovementCreateType["MoveToAnotherWorkshop"] = 1100] = "MoveToAnotherWorkshop";
                    })(InventoryMovementCreateType = Shared.InventoryMovementCreateType || (Shared.InventoryMovementCreateType = {}));
                    var InventoryMovementTypeHelper = /** @class */ (function () {
                        function InventoryMovementTypeHelper() {
                        }
                        InventoryMovementTypeHelper.toDisplayString = function (value) {
                            switch (value) {
                                case InventoryMovementType.Initial:
                                    return "Initial";
                                case InventoryMovementType.Order:
                                    return "Order";
                                case InventoryMovementType.Found:
                                    return "Found";
                                case InventoryMovementType.MoveFromAnotherWorkshop:
                                    return "Move from another workshop";
                                case InventoryMovementType.EphemeralMissingRequiredQuantity:
                                    return "Pending Order";
                                case InventoryMovementType.Repair:
                                    return "Repair";
                                case InventoryMovementType.Lost:
                                    return "Lost";
                                case InventoryMovementType.RepairFragment:
                                    return "Repair (Partial)";
                                case InventoryMovementType.MoveToAnotherWorkshop:
                                    return "Move to another workshop";
                                default:
                                    throw new Error("Invalid movement type");
                            }
                        };
                        InventoryMovementTypeHelper.createDataSource = function () {
                            var items = [
                                InventoryMovementType.Initial,
                                InventoryMovementType.Order,
                                InventoryMovementType.Found,
                                InventoryMovementType.MoveFromAnotherWorkshop,
                                InventoryMovementType.EphemeralMissingRequiredQuantity,
                                InventoryMovementType.Repair,
                                InventoryMovementType.Lost,
                                InventoryMovementType.RepairFragment,
                                InventoryMovementType.MoveToAnotherWorkshop,
                            ];
                            return new kendo.data.DataSource({
                                data: items.map(function (x) { return ({ name: InventoryMovementTypeHelper.toDisplayString(x), value: x }); })
                            });
                        };
                        return InventoryMovementTypeHelper;
                    }());
                    Shared.InventoryMovementTypeHelper = InventoryMovementTypeHelper;
                    var InventoryMovementCreateTypeHelper = /** @class */ (function () {
                        function InventoryMovementCreateTypeHelper() {
                        }
                        InventoryMovementCreateTypeHelper.toDisplayString = function (value) {
                            switch (value) {
                                case InventoryMovementCreateType.Order:
                                    return "Order";
                                case InventoryMovementCreateType.Found:
                                    return "Found";
                                case InventoryMovementCreateType.Lost:
                                    return "Lost";
                                case InventoryMovementCreateType.MoveBetweenWorkshops:
                                    return "Move between workshops";
                                case InventoryMovementCreateType.MoveFromAnotherWorkshop:
                                    return "Move from another workshop";
                                case InventoryMovementCreateType.MoveToAnotherWorkshop:
                                    return "Move to another workshop";
                                default:
                                    throw new Error("Invalid movement type");
                            }
                        };
                        return InventoryMovementCreateTypeHelper;
                    }());
                    Shared.InventoryMovementCreateTypeHelper = InventoryMovementCreateTypeHelper;
                })(Shared = InventoryMovements.Shared || (InventoryMovements.Shared = {}));
            })(InventoryMovements = Pages.InventoryMovements || (Pages.InventoryMovements = {}));
        })(Pages = CRM.Pages || (CRM.Pages = {}));
    })(CRM = DentalDrill.CRM || (DentalDrill.CRM = {}));
})(DentalDrill || (DentalDrill = {}));
//# sourceMappingURL=InventoryMovementType.js.map