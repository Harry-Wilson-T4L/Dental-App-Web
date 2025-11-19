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
                    var Collection = DevGuild.Utilities.Collection;
                    var ReportsPageTabGridWithInsightsBase = CRM.Controls.Reporting.ReportsPageTabGridWithInsightsBase;
                    var ReportsPageGridColumns = CRM.Controls.Reporting.ReportsPageGridColumns;
                    var ReportsTabServiceLevels = /** @class */ (function (_super) {
                        __extends(ReportsTabServiceLevels, _super);
                        function ReportsTabServiceLevels() {
                            return _super !== null && _super.apply(this, arguments) || this;
                        }
                        ReportsTabServiceLevels.prototype.initializeInsights = function () {
                            return __awaiter(this, void 0, void 0, function () {
                                var data, totalCount, totalCost, totalUnrepairable, totalReturnUnrepaired, totalTurnaround, totalWarranty, i, item, dateRanges, i, item, _i, dateRanges_1, dateRange;
                                var _this = this;
                                return __generator(this, function (_a) {
                                    switch (_a.label) {
                                        case 0:
                                            data = this.dataSource.data();
                                            if (!data || data.length === 0) {
                                                return [2 /*return*/];
                                            }
                                            totalCount = 0;
                                            totalCost = 0;
                                            totalUnrepairable = 0;
                                            totalReturnUnrepaired = 0;
                                            totalTurnaround = 0;
                                            totalWarranty = 0;
                                            if (this.dateAggregate === "EntirePeriod") {
                                                for (i = 0; i < data.length; i++) {
                                                    item = data[i];
                                                    if (item && item.HandpiecesCount !== undefined && item.HandpiecesCount !== null && typeof item.HandpiecesCount === "number") {
                                                        totalCount += item.HandpiecesCount;
                                                        if (item.CostSum && typeof item.CostSum === "number")
                                                            totalCost += item.CostSum;
                                                        if (item.UnrepairedPercent && typeof item.UnrepairedPercent === "number")
                                                            totalUnrepairable += item.UnrepairedPercent * item.HandpiecesCount;
                                                        if (item.ReturnUnrepairedPercent && typeof item.ReturnUnrepairedPercent === "number")
                                                            totalReturnUnrepaired += item.ReturnUnrepairedPercent * item.HandpiecesCount;
                                                        if (item.TurnaroundAverage && typeof item.TurnaroundAverage === "number")
                                                            totalTurnaround += item.TurnaroundAverage * item.HandpiecesCount;
                                                        if (item.WarrantyCount && typeof item.WarrantyCount === "number")
                                                            totalWarranty += item.WarrantyCount;
                                                    }
                                                }
                                            }
                                            else {
                                                dateRanges = this.generateDateRange(this.globalFilters.from, this.globalFilters.to, this.dateAggregate);
                                                for (i = 0; i < data.length; i++) {
                                                    item = data[i];
                                                    for (_i = 0, dateRanges_1 = dateRanges; _i < dateRanges_1.length; _i++) {
                                                        dateRange = dateRanges_1[_i];
                                                        if (item && item.HandpiecesCount[dateRange] !== undefined && item.HandpiecesCount[dateRange] !== null && typeof item.HandpiecesCount[dateRange] === "number") {
                                                            totalCount += item.HandpiecesCount[dateRange];
                                                            if (item.CostSum[dateRange] && typeof item.CostSum[dateRange] === "number")
                                                                totalCost += item.CostSum[dateRange];
                                                            if (item.UnrepairedPercent[dateRange] && typeof item.UnrepairedPercent[dateRange] === "number")
                                                                totalUnrepairable += item.UnrepairedPercent[dateRange] * item.HandpiecesCount[dateRange];
                                                            if (item.ReturnUnrepairedPercent[dateRange] && typeof item.ReturnUnrepairedPercent[dateRange] === "number")
                                                                totalReturnUnrepaired += item.ReturnUnrepairedPercent[dateRange] * item.HandpiecesCount[dateRange];
                                                            if (item.TurnaroundAverage[dateRange] && typeof item.TurnaroundAverage[dateRange] === "number")
                                                                totalTurnaround += item.TurnaroundAverage[dateRange] * item.HandpiecesCount[dateRange];
                                                            if (item.WarrantyCount[dateRange] && typeof item.WarrantyCount[dateRange] === "number")
                                                                totalWarranty += item.WarrantyCount[dateRange];
                                                        }
                                                    }
                                                }
                                            }
                                            if (totalCount === 0) {
                                                return [2 /*return*/];
                                            }
                                            return [4 /*yield*/, this.insightsContainer.addFlatValuesAsync(function (flat) { return __awaiter(_this, void 0, void 0, function () {
                                                    return __generator(this, function (_a) {
                                                        if (this.reportFields.isToggled("HandpiecesCount"))
                                                            flat.addValue("Repairs", "" + kendo.toString(totalCount, "n0"));
                                                        if (this.reportFields.isToggled("UnrepairedPercent"))
                                                            flat.addValue("Unrepairable", "" + kendo.toString(totalUnrepairable / totalCount, "p1"));
                                                        if (this.reportFields.isToggled("ReturnUnrepairedPercent"))
                                                            flat.addValue("Ret. unrep", "" + kendo.toString(totalReturnUnrepaired / totalCount, "p1"));
                                                        if (this.reportFields.isToggled("TurnaroundAverage"))
                                                            flat.addValue("Avg. turnaround", kendo.toString(totalTurnaround / totalCount, "n1") + "d");
                                                        if (this.reportFields.isToggled("WarrantyCount"))
                                                            flat.addValue("Warranty jobs", "" + kendo.toString(totalWarranty, "n0"));
                                                        if (this.reportFields.isToggled("CostSum"))
                                                            flat.addValue("Total cost", "$" + kendo.toString(totalCost, "n0"));
                                                        if (this.reportFields.isToggled("CostAverage"))
                                                            flat.addValue("Average cost", "$" + kendo.toString(totalCost / totalCount, "n0"));
                                                        return [2 /*return*/];
                                                    });
                                                }); })];
                                        case 1:
                                            _a.sent();
                                            return [4 /*yield*/, this.insightsContainer.addChartsAsync(function (charts) { return __awaiter(_this, void 0, void 0, function () {
                                                    var serviceLevelDataSource, collection, collection;
                                                    return __generator(this, function (_a) {
                                                        switch (_a.label) {
                                                            case 0: return [4 /*yield*/, this.createDataSource({ transport: { read: { data: { DateAggregate: "EntirePeriod", Fields: ["ServiceLevel"] } } } })];
                                                            case 1:
                                                                serviceLevelDataSource = _a.sent();
                                                                {
                                                                    collection = new Collection(serviceLevelDataSource.data().map(function (x) { return x; }));
                                                                    charts.createPieChart()
                                                                        .override(function (options) {
                                                                        options.seriesDefaults.labels.visible = false;
                                                                        options.tooltip.template = "#: category #: #: kendo.toString(value, 'n0') #";
                                                                    })
                                                                        .initialize("Repairs", collection.groupBy(function (x) { return x.ServiceLevelName; }).select(function (x) { return ({ category: x.key, value: x.items.count() }); }).toArray(), "{0:n0}", "#: category #: \n #: kendo.toString(value, 'n0') #", undefined);
                                                                }
                                                                {
                                                                    collection = new Collection(serviceLevelDataSource.data().map(function (x) { return x; }));
                                                                    charts.createPieChart()
                                                                        .override(function (options) {
                                                                        options.seriesDefaults.labels.visible = false;
                                                                        options.tooltip.template = "#: category #: $#: kendo.toString(value, 'n0') #";
                                                                    })
                                                                        .initialize("Cost", collection.groupBy(function (x) { return x.ServiceLevelName; }).select(function (x) { return ({ category: x.key, value: x.items.sum(function (y, i) { return y.CostSum; }) }); }).toArray(), "{0:n2}", "#: category #: \n $#: kendo.toString(value, 'n0') #", undefined);
                                                                }
                                                                return [2 /*return*/];
                                                        }
                                                    });
                                                }); })];
                                        case 2:
                                            _a.sent();
                                            return [2 /*return*/];
                                    }
                                });
                            });
                        };
                        ReportsTabServiceLevels.prototype.initializeDataSourceTransportRead = function () {
                            return __awaiter(this, void 0, void 0, function () {
                                return __generator(this, function (_a) {
                                    return [2 /*return*/, {
                                            url: "/Reports/ReadReportHandpieces",
                                            data: {
                                                From: this.globalFilters.from.toISOString(),
                                                To: this.globalFilters.to.toISOString(),
                                                DateAggregate: this.dateAggregate,
                                                Fields: ["ServiceLevel", "Brand"]
                                            }
                                        }];
                                });
                            });
                        };
                        ReportsTabServiceLevels.prototype.initializeDataSourceValueFields = function () {
                            return [
                                "HandpiecesCount",
                                "UnrepairedPercent",
                                "ReturnUnrepairedPercent",
                                "TurnaroundAverage",
                                "WarrantyCount",
                                "CostSum",
                                "CostAverage"
                            ];
                        };
                        ReportsTabServiceLevels.prototype.initializeDataSourceGroupFields = function () {
                            return {
                                Brand: { type: "string" },
                                ServiceLevelName: { type: "string" },
                            };
                        };
                        ReportsTabServiceLevels.prototype.initializeGridValueColumns = function () {
                            return [
                                ReportsPageGridColumns.handpieces.handpiecesCount(),
                                ReportsPageGridColumns.handpieces.unrepairablePercent(),
                                ReportsPageGridColumns.handpieces.returnUnrepairedPercent(),
                                ReportsPageGridColumns.handpieces.turnaroundAverage(),
                                ReportsPageGridColumns.handpieces.warrantyCount(),
                                ReportsPageGridColumns.handpieces.costSum(),
                                ReportsPageGridColumns.handpieces.costAverage()
                            ];
                        };
                        ReportsTabServiceLevels.prototype.initializeGridGroupColumns = function () {
                            var columnsConfig = [];
                            columnsConfig.push({
                                field: "ServiceLevelName",
                                title: "Service Level",
                                width: "200px",
                                hidden: true,
                            });
                            columnsConfig.push({
                                field: "Brand",
                                title: "Brand",
                                width: "300px"
                            });
                            return columnsConfig;
                        };
                        ReportsTabServiceLevels.prototype.alterDataSourceOptions = function (dataSourceOptions) {
                            dataSourceOptions.group = [
                                { field: "ServiceLevelName" }
                            ];
                        };
                        ReportsTabServiceLevels.prototype.alterGridOptions = function (gridOptions) {
                            gridOptions.dataBound = function (e) {
                                e.sender.wrapper.find(".k-grouping-row").each(function (index, row) {
                                    e.sender.collapseGroup(row);
                                });
                            };
                        };
                        return ReportsTabServiceLevels;
                    }(ReportsPageTabGridWithInsightsBase));
                    Reports.ReportsTabServiceLevels = ReportsTabServiceLevels;
                })(Reports = GlobalReports.Reports || (GlobalReports.Reports = {}));
            })(GlobalReports = Pages.GlobalReports || (Pages.GlobalReports = {}));
        })(Pages = CRM.Pages || (CRM.Pages = {}));
    })(CRM = DentalDrill.CRM || (DentalDrill.CRM = {}));
})(DentalDrill || (DentalDrill = {}));
//# sourceMappingURL=ReportsTabServiceLevels.js.map