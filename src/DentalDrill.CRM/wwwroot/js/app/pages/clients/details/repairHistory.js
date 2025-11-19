var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
var __generator = (this && this.__generator) || function (thisArg, body) {
    var _ = { label: 0, sent: function() { if (t[0] & 1) throw t[1]; return t[1]; }, trys: [], ops: [] }, f, y, t, g;
    return g = { next: verb(0), "throw": verb(1), "return": verb(2) }, typeof Symbol === "function" && (g[Symbol.iterator] = function() { return this; }), g;
    function verb(n) { return function (v) { return step([n, v]); }; }
    function step(op) {
        if (f) throw new TypeError("Generator is already executing.");
        while (_) try {
            if (f = 1, y && (t = op[0] & 2 ? y["return"] : op[0] ? y["throw"] || ((t = y["return"]) && t.call(y), 0) : y.next) && !(t = t.call(y, op[1])).done) return t;
            if (y = 0, t) op = [op[0] & 2, t.value];
            switch (op[0]) {
                case 0: case 1: t = op; break;
                case 4: _.label++; return { value: op[1], done: false };
                case 5: _.label++; y = op[1]; op = [0]; continue;
                case 7: op = _.ops.pop(); _.trys.pop(); continue;
                default:
                    if (!(t = _.trys, t = t.length > 0 && t[t.length - 1]) && (op[0] === 6 || op[0] === 2)) { _ = 0; continue; }
                    if (op[0] === 3 && (!t || (op[1] > t[0] && op[1] < t[3]))) { _.label = op[1]; break; }
                    if (op[0] === 6 && _.label < t[1]) { _.label = t[1]; t = op; break; }
                    if (t && _.label < t[2]) { _.label = t[2]; _.ops.push(op); break; }
                    if (t[2]) _.ops.pop();
                    _.trys.pop(); continue;
            }
            op = body.call(thisArg, _);
        } catch (e) { op = [6, e]; y = 0; } finally { f = t = 0; }
        if (op[0] & 5) throw op[1]; return { value: op[0] ? op[1] : void 0, done: true };
    }
};
var DentalDrill;
(function (DentalDrill) {
    var CRM;
    (function (CRM) {
        var Pages;
        (function (Pages) {
            var Clients;
            (function (Clients) {
                var Details;
                (function (Details) {
                    var HandpieceStatusIndicator = DentalDrill.CRM.Controls.HandpieceStatusIndicator;
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
                    })(HandpieceStatus || (HandpieceStatus = {}));
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
                                default:
                                    return "Unknown";
                            }
                        };
                        return HandpieceStatusHelper;
                    }());
                    var ClientRepairedItemStatus;
                    (function (ClientRepairedItemStatus) {
                        ClientRepairedItemStatus[ClientRepairedItemStatus["RequiresMaintenance"] = 0] = "RequiresMaintenance";
                        ClientRepairedItemStatus[ClientRepairedItemStatus["RemindedRecently"] = 1] = "RemindedRecently";
                        ClientRepairedItemStatus[ClientRepairedItemStatus["Complete"] = 2] = "Complete";
                        ClientRepairedItemStatus[ClientRepairedItemStatus["Active"] = 3] = "Active";
                        ClientRepairedItemStatus[ClientRepairedItemStatus["ReminderExpired"] = 4] = "ReminderExpired";
                        ClientRepairedItemStatus[ClientRepairedItemStatus["Cancelled"] = 5] = "Cancelled";
                        ClientRepairedItemStatus[ClientRepairedItemStatus["Disabled"] = 6] = "Disabled";
                    })(ClientRepairedItemStatus || (ClientRepairedItemStatus = {}));
                    var ClientRepairedItemStatusHelper = /** @class */ (function () {
                        function ClientRepairedItemStatusHelper() {
                        }
                        ClientRepairedItemStatusHelper.toDisplayString = function (value) {
                            switch (value) {
                                case ClientRepairedItemStatus.Active:
                                    return "Active";
                                case ClientRepairedItemStatus.Complete:
                                    return "Complete";
                                case ClientRepairedItemStatus.RequiresMaintenance:
                                    return "Requires Maintenance";
                                case ClientRepairedItemStatus.RemindedRecently:
                                    return "Reminded Recently";
                                case ClientRepairedItemStatus.ReminderExpired:
                                    return "Reminder Expired";
                                case ClientRepairedItemStatus.Cancelled:
                                    return "Cancelled";
                                case ClientRepairedItemStatus.Disabled:
                                    return "Disabled";
                                default:
                                    return "Unknown";
                            }
                        };
                        ClientRepairedItemStatusHelper.toIcon = function (value) {
                            switch (value) {
                                case ClientRepairedItemStatus.Active:
                                    return "<span class=\"fas fa-fw fa-wrench\"></span>";
                                case ClientRepairedItemStatus.Complete:
                                    return "<span class=\"fas fa-fw fa-check\"></span>";
                                case ClientRepairedItemStatus.RequiresMaintenance:
                                    return "<span class=\"fas fa-fw fa-bell\"></span>";
                                case ClientRepairedItemStatus.RemindedRecently:
                                    return "<span class=\"far fa-fw fa-bell-slash\"></span>";
                                case ClientRepairedItemStatus.ReminderExpired:
                                    return "<span class=\"fas fa-fw fa-bell-slash\"></span>";
                                case ClientRepairedItemStatus.Cancelled:
                                    return "<span class=\"fas fa-fw fa-ban\"></span>";
                                case ClientRepairedItemStatus.Disabled:
                                    return "<span class=\"fas fa-fw fa-power-off\"></span>";
                                default:
                                    return "";
                            }
                        };
                        return ClientRepairedItemStatusHelper;
                    }());
                    var RepairHistoryGrid = /** @class */ (function () {
                        function RepairHistoryGrid() {
                        }
                        Object.defineProperty(RepairHistoryGrid, "instance", {
                            get: function () {
                                return $("#RepairedItemsGrid").data("kendoGrid");
                            },
                            enumerable: false,
                            configurable: true
                        });
                        RepairHistoryGrid.formatLastRepairStatus = function (data) {
                            return "" + HandpieceStatusHelper.toDisplayString(data.LastRepairStatus);
                        };
                        RepairHistoryGrid.formatStatus = function (data) {
                            return ClientRepairedItemStatusHelper.toIcon(data.Status) + " " + ClientRepairedItemStatusHelper.toDisplayString(data.Status);
                        };
                        RepairHistoryGrid.renderStatusIndicator = function (data) {
                            var config = data.JobStatusConfig.split(";").map(function (x) { return parseInt(x); });
                            var indicator = new HandpieceStatusIndicator();
                            var indicatorValue = Math.abs(config[0]);
                            indicator.value = indicatorValue;
                            indicator.danger = config[0] < 0;
                            for (var i = 1; i <= 6; i++) {
                                indicator.setOverride(i, config[i] > 0 && i < indicatorValue);
                                indicator.setCount(i, i < indicatorValue ? config[i] : 0);
                            }
                            return indicator.render().outerHTML;
                        };
                        RepairHistoryGrid.handleToggle = function (e) {
                            return __awaiter(this, void 0, void 0, function () {
                                var dataItem;
                                return __generator(this, function (_a) {
                                    switch (_a.label) {
                                        case 0:
                                            e.preventDefault();
                                            dataItem = this.dataItem(e.currentTarget.closest("tr"));
                                            console.log(dataItem);
                                            return [4 /*yield*/, fetch("/ClientRepairHistory/Toggle?parentId=" + dataItem.ClientId, {
                                                    method: "POST",
                                                    credentials: "same-origin",
                                                    headers: {
                                                        "Content-Type": "application/json",
                                                        "X-Requested-With": "XMLHttpRequest"
                                                    },
                                                    body: JSON.stringify({
                                                        ClientHandpieceId: dataItem.Id,
                                                        Disable: dataItem.Status === ClientRepairedItemStatus.Disabled ? false : true,
                                                    })
                                                })];
                                        case 1:
                                            _a.sent();
                                            return [4 /*yield*/, this.dataSource.read()];
                                        case 2:
                                            _a.sent();
                                            return [2 /*return*/];
                                    }
                                });
                            });
                        };
                        RepairHistoryGrid.handleResetCount = function (e) {
                            return __awaiter(this, void 0, void 0, function () {
                                var grid, dataItem;
                                return __generator(this, function (_a) {
                                    switch (_a.label) {
                                        case 0:
                                            e.preventDefault();
                                            grid = $("#RepairedItemsGrid").data("kendoGrid");
                                            dataItem = grid.dataItem(e.currentTarget.closest("tr"));
                                            console.log(dataItem);
                                            return [4 /*yield*/, fetch("/ClientRepairHistory/ResetCount?parentId=" + dataItem.ClientId, {
                                                    method: "POST",
                                                    credentials: "same-origin",
                                                    headers: {
                                                        "Content-Type": "application/json",
                                                        "X-Requested-With": "XMLHttpRequest"
                                                    },
                                                    body: JSON.stringify({
                                                        ClientHandpieceId: dataItem.Id,
                                                    })
                                                })];
                                        case 1:
                                            _a.sent();
                                            return [4 /*yield*/, grid.dataSource.read()];
                                        case 2:
                                            _a.sent();
                                            return [2 /*return*/];
                                    }
                                });
                            });
                        };
                        return RepairHistoryGrid;
                    }());
                    Details.RepairHistoryGrid = RepairHistoryGrid;
                    $(function () {
                        $(document).on("click", ".client-repair-history-reset-count", RepairHistoryGrid.handleResetCount);
                    });
                })(Details = Clients.Details || (Clients.Details = {}));
            })(Clients = Pages.Clients || (Pages.Clients = {}));
        })(Pages = CRM.Pages || (CRM.Pages = {}));
    })(CRM = DentalDrill.CRM || (DentalDrill.CRM = {}));
})(DentalDrill || (DentalDrill = {}));
//# sourceMappingURL=repairHistory.js.map