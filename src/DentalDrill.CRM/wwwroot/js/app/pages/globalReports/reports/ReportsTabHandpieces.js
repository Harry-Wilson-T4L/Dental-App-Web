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
            var GlobalReports;
            (function (GlobalReports) {
                var Reports;
                (function (Reports) {
                    var ReportsPageTabGridWithInsightsBase = CRM.Controls.Reporting.ReportsPageTabGridWithInsightsBase;
                    var ReportsPageDataSourceGroupHelper = CRM.Controls.Reporting.ReportsPageDataSourceGroupHelper;
                    var ReportsPageGridColumns = CRM.Controls.Reporting.ReportsPageGridColumns;
                    var ReportsTabHandpieces = /** @class */ (function (_super) {
                        __extends(ReportsTabHandpieces, _super);
                        function ReportsTabHandpieces() {
                            return _super !== null && _super.apply(this, arguments) || this;
                        }
                        ReportsTabHandpieces.prototype.initializeDataSourceTransportRead = function () {
                            return __awaiter(this, void 0, void 0, function () {
                                return __generator(this, function (_a) {
                                    return [2 /*return*/, {
                                            url: "/Reports/ReadReportHandpieces",
                                            data: {
                                                From: this.globalFilters.from.toISOString(),
                                                To: this.globalFilters.to.toISOString(),
                                                DateAggregate: this.dateAggregate,
                                                Fields: ["Client", "ServiceLevel", "Brand", "Model", "RepairedBy"]
                                            }
                                        }];
                                });
                            });
                        };
                        ReportsTabHandpieces.prototype.initializeDataSourceValueFields = function () {
                            return [
                                "HandpiecesCount",
                                "RatingAverage",
                                "UnrepairedPercent",
                                "ReturnUnrepairedPercent",
                                "TurnaroundAverage",
                                "WarrantyCount",
                                "CostSum",
                                "CostAverage"
                            ];
                        };
                        ReportsTabHandpieces.prototype.initializeDataSourceGroupFields = function () {
                            return {
                                ClientId: { type: "string" },
                                ClientName: { type: "string" },
                                ServiceLevelName: { type: "string" },
                                Brand: { type: "string" },
                                Model: { type: "string" },
                                RepairedByName: { type: "string" },
                            };
                        };
                        ReportsTabHandpieces.prototype.initializeGridValueColumns = function () {
                            return [
                                ReportsPageGridColumns.handpieces.handpiecesCount(),
                                ReportsPageGridColumns.handpieces.ratingAverage(),
                                ReportsPageGridColumns.handpieces.unrepairablePercent(),
                                ReportsPageGridColumns.handpieces.returnUnrepairedPercent(),
                                ReportsPageGridColumns.handpieces.turnaroundAverage(),
                                ReportsPageGridColumns.handpieces.warrantyCount(),
                                ReportsPageGridColumns.handpieces.costSum(),
                                ReportsPageGridColumns.handpieces.costAverage()
                            ];
                        };
                        ReportsTabHandpieces.prototype.initializeGridGroupColumns = function () {
                            var columnsConfig = [];
                            columnsConfig.push({
                                field: "ClientId",
                                template: "#: data.ClientName #",
                                groupHeaderTemplate: function (group) {
                                    var first = ReportsPageDataSourceGroupHelper.getFirstItemFromGroup(group, "ClientName");
                                    return first ? first.ClientName : "";
                                },
                                title: "Surgery Name",
                                width: "200px"
                            });
                            columnsConfig.push({
                                field: "ServiceLevelName",
                                title: "Service Level",
                                width: "100px",
                            });
                            columnsConfig.push({
                                field: "Brand",
                                title: "Brand",
                                width: "100px",
                            });
                            columnsConfig.push({
                                field: "Model",
                                title: "Model",
                                width: "100px",
                            });
                            columnsConfig.push({
                                field: "RepairedByName",
                                title: "Repaired By",
                                width: "100px",
                            });
                            return columnsConfig;
                        };
                        ReportsTabHandpieces.prototype.alterGridOptions = function (gridOptions) {
                            gridOptions.groupable = true;
                        };
                        return ReportsTabHandpieces;
                    }(ReportsPageTabGridWithInsightsBase));
                    Reports.ReportsTabHandpieces = ReportsTabHandpieces;
                })(Reports = GlobalReports.Reports || (GlobalReports.Reports = {}));
            })(GlobalReports = Pages.GlobalReports || (Pages.GlobalReports = {}));
        })(Pages = CRM.Pages || (CRM.Pages = {}));
    })(CRM = DentalDrill.CRM || (DentalDrill.CRM = {}));
})(DentalDrill || (DentalDrill = {}));
//# sourceMappingURL=ReportsTabHandpieces.js.map