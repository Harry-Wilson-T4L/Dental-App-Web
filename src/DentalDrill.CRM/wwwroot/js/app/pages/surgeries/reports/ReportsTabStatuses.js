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
            var Surgeries;
            (function (Surgeries) {
                var Reports;
                (function (Reports) {
                    var ToggleButton = DentalDrill.CRM.Controls.ToggleButton;
                    var PdfReportBuilder = DentalDrill.CRM.Controls.PdfReports.PdfReportBuilder;
                    var ReportTable = DentalDrill.CRM.Controls.Reporting.ReportTable;
                    var SurgeryReportFlowWriter = DentalDrill.CRM.Controls.Reporting.SurgeryReportFlowWriter;
                    var ReportsTabStatuses = /** @class */ (function (_super) {
                        __extends(ReportsTabStatuses, _super);
                        function ReportsTabStatuses(page, root) {
                            var _this = _super.call(this) || this;
                            _this._page = page;
                            _this._root = root;
                            _this._container = root.find(".reports__statuses__container");
                            _this._insightsContainer = root.find(".reports__statuses__insights-container");
                            _this._insightsToggle = new ToggleButton(root.find(".reports__statuses__insights"));
                            _this._insightsToggle.changed.subscribe(function (sender, e) { return __awaiter(_this, void 0, void 0, function () {
                                return __generator(this, function (_a) {
                                    switch (_a.label) {
                                        case 0: return [4 /*yield*/, this.toggleInsights()];
                                        case 1:
                                            _a.sent();
                                            return [2 /*return*/];
                                    }
                                });
                            }); });
                            root.find(".reports__statuses__export__excel").click(function (e) { return __awaiter(_this, void 0, void 0, function () {
                                return __generator(this, function (_a) {
                                    switch (_a.label) {
                                        case 0: return [4 /*yield*/, this.exportExcel()];
                                        case 1:
                                            _a.sent();
                                            return [2 /*return*/];
                                    }
                                });
                            }); });
                            root.find(".reports__statuses__export__pdf").click(function (e) { return __awaiter(_this, void 0, void 0, function () {
                                return __generator(this, function (_a) {
                                    switch (_a.label) {
                                        case 0: return [4 /*yield*/, this.exportPdf()];
                                        case 1:
                                            _a.sent();
                                            return [2 /*return*/];
                                    }
                                });
                            }); });
                            return _this;
                        }
                        ReportsTabStatuses.prototype.exportExcel = function () {
                            return __awaiter(this, void 0, void 0, function () {
                                return __generator(this, function (_a) {
                                    routes.surgeries.exportStatusesToExcel(this._page.clientUrlPath, this._filterDateFrom, this._filterDateTo).open();
                                    return [2 /*return*/];
                                });
                            });
                        };
                        ReportsTabStatuses.prototype.exportPdf = function () {
                            return __awaiter(this, void 0, void 0, function () {
                                var formatAsNumber, reportBuilder, reportWriter, children, table;
                                return __generator(this, function (_a) {
                                    formatAsNumber = function (x, decimalDigits) {
                                        if (typeof x === "number") {
                                            return x.toFixed(decimalDigits);
                                        }
                                        return "";
                                    };
                                    reportBuilder = new PdfReportBuilder();
                                    reportBuilder.fileName = this._page.clientUrlPath + "-ByStatus.pdf";
                                    reportBuilder.open();
                                    reportWriter = new SurgeryReportFlowWriter(reportBuilder);
                                    reportWriter.addHeading(1, "Handpieces by status");
                                    if (this._insightsToggle.active) {
                                        children = this._insightsContainer.children();
                                        reportWriter.addInsightsTotals("Total Handpieces by statuses", $(children[0]));
                                        reportWriter.addCharts($(children[2]).find(".k-chart").toArray().map(function (element, index) { return $(element).data("kendoChart"); }));
                                        reportWriter.addInsightsTotals("Average Handpieces by statuses", $(children[3]));
                                        reportWriter.nextPage();
                                    }
                                    table = new ReportTable();
                                    table.rowHeight = 23;
                                    table.addColumn("Brand", 120, function (x) { return x.Brand; });
                                    table.addColumn("Received", 50, function (x) { return formatAsNumber(x.StatusReceived, 0); });
                                    table.addColumn("Being Est.", 50, function (x) { return formatAsNumber(x.StatusBeingEstimated, 0); });
                                    table.addColumn("Waiting App.", 50, function (x) { return formatAsNumber(x.StatusWaitingForApproval, 0); });
                                    table.addColumn("Est. Sent", 50, function (x) { return formatAsNumber(x.StatusEstimateSent, 0); });
                                    table.addColumn("Being Rep.", 50, function (x) { return formatAsNumber(x.StatusBeingRepaired, 0); });
                                    table.addColumn("Rdy. For Ret.", 50, function (x) { return formatAsNumber(x.StatusReadyToReturn, 0); });
                                    table.addColumn("Unrepaired", 50, function (x) { return formatAsNumber(x.StatusCancelled, 0); });
                                    table.addColumn("Complete", 50, function (x) { return formatAsNumber(x.StatusSentComplete, 0); });
                                    table.setData(this._dataSource.data().map(function (x) { return x; }), function (left, right) {
                                        if (left.Brand < right.Brand)
                                            return -1;
                                        if (left.Brand > right.Brand)
                                            return 1;
                                        return 0;
                                    });
                                    reportWriter.addTable(table);
                                    return [2 /*return*/];
                                });
                            });
                        };
                        ReportsTabStatuses.prototype.initializeDataSourceFields = function (fields) {
                            var baseFields = [
                                "StatusReceived", "StatusBeingEstimated", "StatusWaitingForApproval",
                                "StatusEstimateSent", "StatusBeingRepaired", "StatusReadyToReturn",
                                "StatusSentComplete", "StatusCancelled", "StatusAny"
                            ];
                            for (var _i = 0, baseFields_1 = baseFields; _i < baseFields_1.length; _i++) {
                                var field = baseFields_1[_i];
                                fields[field] = { type: "number", from: field };
                            }
                            return fields;
                        };
                        ReportsTabStatuses.prototype.initializeDataSources = function () {
                            return __awaiter(this, void 0, void 0, function () {
                                return __generator(this, function (_a) {
                                    switch (_a.label) {
                                        case 0:
                                            this._dataSource = new kendo.data.DataSource({
                                                type: "aspnetmvc-ajax",
                                                transport: {
                                                    read: {
                                                        url: "/Surgeries/" + this._page.clientUrlPath + "/Reports/Statuses",
                                                        data: {
                                                            From: this._filterDateFrom.toISOString(),
                                                            To: this._filterDateTo.toISOString()
                                                        }
                                                    }
                                                },
                                                schema: {
                                                    data: "Data",
                                                    total: "Total",
                                                    errors: "Errors",
                                                    model: {
                                                        fields: this.initializeDataSourceFields({
                                                            Brand: { type: "string" }
                                                        })
                                                    }
                                                }
                                            });
                                            return [4 /*yield*/, this._dataSource.read()];
                                        case 1:
                                            _a.sent();
                                            return [2 /*return*/];
                                    }
                                });
                            });
                        };
                        ReportsTabStatuses.prototype.getFieldsCandidates = function () {
                            return [
                                {
                                    field: "StatusReceived",
                                    title: "Received",
                                    format: "{0:n0}"
                                },
                                {
                                    field: "StatusBeingEstimated",
                                    title: "Being Est.",
                                    format: "{0:n0}"
                                },
                                {
                                    field: "StatusWaitingForApproval",
                                    title: "Waiting App.",
                                    format: "{0:n0}"
                                },
                                {
                                    field: "StatusEstimateSent",
                                    title: "Est. Sent",
                                    format: "{0:n0}"
                                },
                                {
                                    field: "StatusBeingRepaired",
                                    title: "Being Rep.",
                                    format: "{0:n0}"
                                },
                                {
                                    field: "StatusReadyToReturn",
                                    title: "Rdy. For Ret.",
                                    format: "{0:n0}"
                                },
                                {
                                    field: "StatusCancelled",
                                    title: "Unrepaired",
                                    format: "{0:n0}"
                                },
                                {
                                    field: "StatusSentComplete",
                                    title: "Complete",
                                    format: "{0:n0}"
                                }
                            ];
                        };
                        ReportsTabStatuses.prototype.createMainGrid = function () {
                            var wrapper = $("<div></div>");
                            this._container.empty();
                            this._container.append(wrapper);
                            var columnsConfig = [];
                            columnsConfig.push({
                                field: "Brand",
                                title: "Brand",
                                width: "300px"
                            });
                            var fieldsCandidates = this.getFieldsCandidates();
                            for (var _i = 0, fieldsCandidates_1 = fieldsCandidates; _i < fieldsCandidates_1.length; _i++) {
                                var candidate = fieldsCandidates_1[_i];
                                columnsConfig.push({
                                    field: candidate.field,
                                    title: candidate.title,
                                    width: "90px",
                                    format: candidate.format,
                                });
                            }
                            columnsConfig.push({
                                title: ""
                            });
                            wrapper.kendoGrid({
                                dataSource: this._dataSource,
                                columns: columnsConfig,
                                scrollable: true,
                                sortable: {
                                    mode: "multiple",
                                    allowUnsort: true,
                                },
                                noRecords: {
                                    template: "No data available for selected date range and surgeries"
                                }
                            });
                        };
                        ReportsTabStatuses.prototype.initialize = function () {
                            return __awaiter(this, void 0, void 0, function () {
                                return __generator(this, function (_a) {
                                    switch (_a.label) {
                                        case 0: return [4 /*yield*/, this.initializeDataSources()];
                                        case 1:
                                            _a.sent();
                                            this.createMainGrid();
                                            return [4 /*yield*/, this.toggleInsights()];
                                        case 2:
                                            _a.sent();
                                            return [2 /*return*/];
                                    }
                                });
                            });
                        };
                        ReportsTabStatuses.prototype.applyGlobalFilters = function (from, to) {
                            return __awaiter(this, void 0, void 0, function () {
                                return __generator(this, function (_a) {
                                    this._filterDateFrom = from;
                                    this._filterDateTo = to;
                                    return [2 /*return*/];
                                });
                            });
                        };
                        ReportsTabStatuses.prototype.toggleInsights = function () {
                            return __awaiter(this, void 0, void 0, function () {
                                var data, charts, totalsSum, totalsAvg, self, aggregate, i, item, sumChartData, averageChartData;
                                return __generator(this, function (_a) {
                                    this._insightsContainer.empty();
                                    if (!this._insightsToggle.active) {
                                        return [2 /*return*/];
                                    }
                                    data = this._dataSource.data();
                                    if (data.length === 0) {
                                        this._insightsContainer.append($("<div></div>").addClass("row").append($("<div></div>")
                                            .addClass("reports__insights__total__value")
                                            .addClass("col-12")
                                            .text("No data available for selected date range and surgeries")));
                                        return [2 /*return*/];
                                    }
                                    charts = new Reports.ChartsCollectionContainer($("<div></div>").addClass("row"));
                                    totalsSum = new Reports.TotalsCollectionContainer($("<div></div>").addClass("row"));
                                    totalsAvg = new Reports.TotalsCollectionContainer($("<div></div>").addClass("row"));
                                    self = this;
                                    aggregate = {
                                        count: 0,
                                        sum: {
                                            StatusReceived: 0,
                                            StatusBeingEstimated: 0,
                                            StatusWaitingForApproval: 0,
                                            StatusEstimateSent: 0,
                                            StatusBeingRepaired: 0,
                                            StatusReadyToReturn: 0,
                                            StatusSentComplete: 0,
                                            StatusCancelled: 0,
                                            StatusAny: 0
                                        }
                                    };
                                    for (i = 0; i < data.length; i++) {
                                        item = data[i];
                                        aggregate.count++;
                                        aggregate.sum.StatusReceived += item.StatusReceived || 0;
                                        aggregate.sum.StatusBeingEstimated += item.StatusBeingEstimated || 0;
                                        aggregate.sum.StatusWaitingForApproval += item.StatusWaitingForApproval || 0;
                                        aggregate.sum.StatusEstimateSent += item.StatusEstimateSent || 0;
                                        aggregate.sum.StatusBeingRepaired += item.StatusBeingRepaired || 0;
                                        aggregate.sum.StatusReadyToReturn += item.StatusReadyToReturn || 0;
                                        aggregate.sum.StatusSentComplete += item.StatusSentComplete || 0;
                                        aggregate.sum.StatusCancelled += item.StatusCancelled || 0;
                                        aggregate.sum.StatusAny += item.StatusAny || 0;
                                    }
                                    totalsSum.addValue("Received", kendo.format("{0:n0}", aggregate.sum.StatusReceived));
                                    totalsSum.addValue("Being Estimated", kendo.format("{0:n0}", aggregate.sum.StatusBeingEstimated));
                                    totalsSum.addValue("Waiting For App.", kendo.format("{0:n0}", aggregate.sum.StatusWaitingForApproval));
                                    totalsSum.addValue("Estimate Sent", kendo.format("{0:n0}", aggregate.sum.StatusEstimateSent));
                                    totalsSum.addValue("Being Repaired", kendo.format("{0:n0}", aggregate.sum.StatusBeingRepaired));
                                    totalsSum.addValue("Ready to Return", kendo.format("{0:n0}", aggregate.sum.StatusReadyToReturn));
                                    totalsSum.addValue("Unrepaired", kendo.format("{0:n0}", aggregate.sum.StatusCancelled));
                                    totalsSum.addValue("Sent Complete", kendo.format("{0:n0}", aggregate.sum.StatusSentComplete));
                                    totalsAvg.addValue("Received", kendo.format("{0:0.##}", aggregate.sum.StatusReceived / aggregate.count));
                                    totalsAvg.addValue("Being Estimated", kendo.format("{0:0.##}", aggregate.sum.StatusBeingEstimated / aggregate.count));
                                    totalsAvg.addValue("Waiting For App.", kendo.format("{0:0.##}", aggregate.sum.StatusWaitingForApproval / aggregate.count));
                                    totalsAvg.addValue("Estimate Sent", kendo.format("{0:0.##}", aggregate.sum.StatusEstimateSent / aggregate.count));
                                    totalsAvg.addValue("Being Repaired", kendo.format("{0:0.##}", aggregate.sum.StatusBeingRepaired / aggregate.count));
                                    totalsAvg.addValue("Ready to Return", kendo.format("{0:0.##}", aggregate.sum.StatusReadyToReturn / aggregate.count));
                                    totalsAvg.addValue("Unrepaired", kendo.format("{0:0.##}", aggregate.sum.StatusCancelled / aggregate.count));
                                    totalsAvg.addValue("Sent Complete", kendo.format("{0:0.##}", aggregate.sum.StatusSentComplete / aggregate.count));
                                    this._insightsContainer.append(totalsSum.container);
                                    this._insightsContainer.append($("<div></div>").addClass("row").append($("<div></div>").addClass("col-12 text-center font-italic mt-2").text("Total Handpieces by statuses")));
                                    this._insightsContainer.append(charts.container);
                                    this._insightsContainer.append(totalsAvg.container);
                                    this._insightsContainer.append($("<div></div>").addClass("row").append($("<div></div>").addClass("col-12 text-center font-italic mt-2").text("Average Handpieces by statuses")));
                                    sumChartData = [
                                        { name: "Received", value: aggregate.sum.StatusReceived },
                                        { name: "Being Estimated", value: aggregate.sum.StatusBeingEstimated },
                                        { name: "Waiting For App.", value: aggregate.sum.StatusWaitingForApproval },
                                        { name: "Estimate Sent", value: aggregate.sum.StatusEstimateSent },
                                        { name: "Being Repaired", value: aggregate.sum.StatusBeingRepaired },
                                        { name: "Ready to Return", value: aggregate.sum.StatusReadyToReturn },
                                        { name: "Unrepaired", value: aggregate.sum.StatusCancelled },
                                        { name: "Sent Complete", value: aggregate.sum.StatusSentComplete }
                                    ];
                                    charts.createWaterfallChart().initialize("Total Handpieces", sumChartData, "{0:n0}", "#: value #", undefined);
                                    averageChartData = [
                                        { name: "Received", value: aggregate.sum.StatusReceived / aggregate.count },
                                        { name: "Being Estimated", value: aggregate.sum.StatusBeingEstimated / aggregate.count },
                                        { name: "Waiting For App.", value: aggregate.sum.StatusWaitingForApproval / aggregate.count },
                                        { name: "Estimate Sent", value: aggregate.sum.StatusEstimateSent / aggregate.count },
                                        { name: "Being Repaired", value: aggregate.sum.StatusBeingRepaired / aggregate.count },
                                        { name: "Ready to Return", value: aggregate.sum.StatusReadyToReturn / aggregate.count },
                                        { name: "Unrepaired", value: aggregate.sum.StatusCancelled / aggregate.count },
                                        { name: "Sent Complete", value: aggregate.sum.StatusSentComplete / aggregate.count }
                                    ];
                                    charts.createWaterfallChart().initialize("Average Handpieces", averageChartData, "{0:0.##}", "#: kendo.format('{0:0.\\#\\#}', value) #", undefined);
                                    return [2 /*return*/];
                                });
                            });
                        };
                        return ReportsTabStatuses;
                    }(Reports.ReportsTabBase));
                    Reports.ReportsTabStatuses = ReportsTabStatuses;
                })(Reports = Surgeries.Reports || (Surgeries.Reports = {}));
            })(Surgeries = Pages.Surgeries || (Pages.Surgeries = {}));
        })(Pages = CRM.Pages || (CRM.Pages = {}));
    })(CRM = DentalDrill.CRM || (DentalDrill.CRM = {}));
})(DentalDrill || (DentalDrill = {}));
//# sourceMappingURL=ReportsTabStatuses.js.map