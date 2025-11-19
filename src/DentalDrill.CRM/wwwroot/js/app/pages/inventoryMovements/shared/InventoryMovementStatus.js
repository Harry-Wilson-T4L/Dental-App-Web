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
                    var InventoryMovementStatus;
                    (function (InventoryMovementStatus) {
                        InventoryMovementStatus[InventoryMovementStatus["Cancelled"] = 0] = "Cancelled";
                        InventoryMovementStatus[InventoryMovementStatus["Requested"] = 10] = "Requested";
                        InventoryMovementStatus[InventoryMovementStatus["Approved"] = 20] = "Approved";
                        InventoryMovementStatus[InventoryMovementStatus["Ordered"] = 30] = "Ordered";
                        InventoryMovementStatus[InventoryMovementStatus["Waiting"] = 40] = "Waiting";
                        InventoryMovementStatus[InventoryMovementStatus["Allocated"] = 90] = "Allocated";
                        InventoryMovementStatus[InventoryMovementStatus["Completed"] = 100] = "Completed";
                    })(InventoryMovementStatus = Shared.InventoryMovementStatus || (Shared.InventoryMovementStatus = {}));
                    var InventoryMovementStatusHelper = /** @class */ (function () {
                        function InventoryMovementStatusHelper() {
                        }
                        InventoryMovementStatusHelper.toDisplayString = function (value) {
                            switch (value) {
                                case InventoryMovementStatus.Cancelled:
                                    return "Cancelled";
                                case InventoryMovementStatus.Requested:
                                    return "To be approved";
                                case InventoryMovementStatus.Approved:
                                    return "To be ordered";
                                case InventoryMovementStatus.Ordered:
                                    return "Ordered";
                                case InventoryMovementStatus.Waiting:
                                    return "Waiting";
                                case InventoryMovementStatus.Allocated:
                                    return "Allocated";
                                case InventoryMovementStatus.Completed:
                                    return "Completed";
                                default:
                                    throw new Error("Invalid movement status");
                            }
                        };
                        InventoryMovementStatusHelper.createDataSource = function () {
                            var items = [
                                InventoryMovementStatus.Requested,
                                InventoryMovementStatus.Approved,
                                InventoryMovementStatus.Ordered,
                                InventoryMovementStatus.Waiting,
                                InventoryMovementStatus.Allocated,
                                InventoryMovementStatus.Completed,
                            ];
                            return new kendo.data.DataSource({
                                data: items.map(function (x) { return ({ name: InventoryMovementStatusHelper.toDisplayString(x), value: x }); })
                            });
                        };
                        return InventoryMovementStatusHelper;
                    }());
                    Shared.InventoryMovementStatusHelper = InventoryMovementStatusHelper;
                })(Shared = InventoryMovements.Shared || (InventoryMovements.Shared = {}));
            })(InventoryMovements = Pages.InventoryMovements || (Pages.InventoryMovements = {}));
        })(Pages = CRM.Pages || (CRM.Pages = {}));
    })(CRM = DentalDrill.CRM || (DentalDrill.CRM = {}));
})(DentalDrill || (DentalDrill = {}));
//# sourceMappingURL=InventoryMovementStatus.js.map