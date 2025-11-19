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
                    var HandpieceStatus;
                    (function (HandpieceStatus) {
                        HandpieceStatus[HandpieceStatus["None"] = 0] = "None";
                        HandpieceStatus[HandpieceStatus["Received"] = 10] = "Received";
                        HandpieceStatus[HandpieceStatus["BeingEstimated"] = 20] = "BeingEstimated";
                        HandpieceStatus[HandpieceStatus["WaitingForApproval"] = 30] = "WaitingForApproval";
                        HandpieceStatus[HandpieceStatus["TbcHoldOn"] = 31] = "TbcHoldOn";
                        HandpieceStatus[HandpieceStatus["NeedsReApproval"] = 32] = "NeedsReApproval";
                        HandpieceStatus[HandpieceStatus["EstimateSent"] = 35] = "EstimateSent";
                        HandpieceStatus[HandpieceStatus["BeingRepaired"] = 40] = "BeingRepaired";
                        HandpieceStatus[HandpieceStatus["WaitingForParts"] = 41] = "WaitingForParts";
                        HandpieceStatus[HandpieceStatus["TradeIn"] = 42] = "TradeIn";
                        HandpieceStatus[HandpieceStatus["ReadyToReturn"] = 50] = "ReadyToReturn";
                        HandpieceStatus[HandpieceStatus["ReturnUnrepaired"] = 51] = "ReturnUnrepaired";
                        HandpieceStatus[HandpieceStatus["SentComplete"] = 60] = "SentComplete";
                        HandpieceStatus[HandpieceStatus["Cancelled"] = 70] = "Cancelled";
                        HandpieceStatus[HandpieceStatus["Unrepairable"] = 71] = "Unrepairable";
                    })(HandpieceStatus = Shared.HandpieceStatus || (Shared.HandpieceStatus = {}));
                    var HandpieceStatusHelper = /** @class */ (function () {
                        function HandpieceStatusHelper() {
                        }
                        HandpieceStatusHelper.toDisplayString = function (value) {
                            switch (value) {
                                case HandpieceStatus.None:
                                    return "None";
                                case HandpieceStatus.Received:
                                    return "Received";
                                case HandpieceStatus.BeingEstimated:
                                    return "Being estimated";
                                case HandpieceStatus.WaitingForApproval:
                                    return "Estimate Complete";
                                case HandpieceStatus.TbcHoldOn:
                                    return "Tbc hold on";
                                case HandpieceStatus.NeedsReApproval:
                                    return "Needs approval";
                                case HandpieceStatus.EstimateSent:
                                    return "Estimate sent";
                                case HandpieceStatus.BeingRepaired:
                                    return "Being repaired";
                                case HandpieceStatus.WaitingForParts:
                                    return "Waiting for parts";
                                case HandpieceStatus.TradeIn:
                                    return "Trade-in";
                                case HandpieceStatus.ReadyToReturn:
                                    return "Ready to return";
                                case HandpieceStatus.ReturnUnrepaired:
                                    return "Return unrepaired";
                                case HandpieceStatus.SentComplete:
                                    return "Sent complete";
                                case HandpieceStatus.Cancelled:
                                    return "Cancelled";
                                case HandpieceStatus.Unrepairable:
                                    return "Unrepairable";
                            }
                        };
                        HandpieceStatusHelper.createDataSource = function () {
                            var items = [
                                HandpieceStatus.Received,
                                HandpieceStatus.BeingEstimated,
                                HandpieceStatus.WaitingForApproval,
                                HandpieceStatus.TbcHoldOn,
                                HandpieceStatus.NeedsReApproval,
                                HandpieceStatus.EstimateSent,
                                HandpieceStatus.BeingRepaired,
                                HandpieceStatus.WaitingForParts,
                                HandpieceStatus.TradeIn,
                                HandpieceStatus.ReadyToReturn,
                                HandpieceStatus.ReturnUnrepaired,
                                HandpieceStatus.SentComplete,
                                HandpieceStatus.Cancelled,
                                HandpieceStatus.Unrepairable,
                            ];
                            return new kendo.data.DataSource({
                                data: items.map(function (x) { return ({ name: HandpieceStatusHelper.toDisplayString(x), value: x }); })
                            });
                        };
                        return HandpieceStatusHelper;
                    }());
                    Shared.HandpieceStatusHelper = HandpieceStatusHelper;
                })(Shared = InventoryMovements.Shared || (InventoryMovements.Shared = {}));
            })(InventoryMovements = Pages.InventoryMovements || (Pages.InventoryMovements = {}));
        })(Pages = CRM.Pages || (CRM.Pages = {}));
    })(CRM = DentalDrill.CRM || (DentalDrill.CRM = {}));
})(DentalDrill || (DentalDrill = {}));
//# sourceMappingURL=HandpieceStatus.js.map