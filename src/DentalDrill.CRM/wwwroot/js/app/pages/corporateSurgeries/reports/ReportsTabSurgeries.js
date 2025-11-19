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
var __assign = (this && this.__assign) || function () {
    __assign = Object.assign || function(t) {
        for (var s, i = 1, n = arguments.length; i < n; i++) {
            s = arguments[i];
            for (var p in s) if (Object.prototype.hasOwnProperty.call(s, p))
                t[p] = s[p];
        }
        return t;
    };
    return __assign.apply(this, arguments);
};
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
var __spreadArray = (this && this.__spreadArray) || function (to, from, pack) {
    if (pack || arguments.length === 2) for (var i = 0, l = from.length, ar; i < l; i++) {
        if (ar || !(i in from)) {
            if (!ar) ar = Array.prototype.slice.call(from, 0, i);
            ar[i] = from[i];
        }
    }
    return to.concat(ar || Array.prototype.slice.call(from));
};
var DentalDrill;
(function (DentalDrill) {
    var CRM;
    (function (CRM) {
        var Pages;
        (function (Pages) {
            var CorporateSurgeries;
            (function (CorporateSurgeries) {
                var Reports;
                (function (Reports) {
                    var ToggleButton = DentalDrill.CRM.Controls.ToggleButton;
                    var ToggleList = DentalDrill.CRM.Controls.ToggleList;
                    var PdfReportBuilder = DentalDrill.CRM.Controls.PdfReports.PdfReportBuilder;
                    var ReportTable = DentalDrill.CRM.Controls.Reporting.ReportTable;
                    var SurgeryReportFlowWriter = DentalDrill.CRM.Controls.Reporting.SurgeryReportFlowWriter;
                    var ReportsTabSurgeries = /** @class */ (function (_super) {
                        __extends(ReportsTabSurgeries, _super);
                        function ReportsTabSurgeries(page, root) {
                            var _this = _super.call(this) || this;
                            _this._page = page;
                            _this._root = root;
                            _this._container = root.find(".reports__surgeries__container");
                            _this._insightsContainer = root.find(".reports__surgeries__insights-container");
                            _this._reportFields = new ToggleList(root.find(".reports__surgeries__fields"));
                            _this._reportFields.selectionChanged.subscribe(function (sender, e) { return __awaiter(_this, void 0, void 0, function () {
                                return __generator(this, function (_a) {
                                    switch (_a.label) {
                                        case 0: return [4 /*yield*/, this.initialize()];
                                        case 1:
                                            _a.sent();
                                            return [2 /*return*/];
                                    }
                                });
                            }); });
                            _this._dateAggregate = root.find("input.reports__surgeries__date-aggregate").data("kendoDropDownList");
                            _this._dateAggregate.bind("change", function (e) { return __awaiter(_this, void 0, void 0, function () {
                                return __generator(this, function (_a) {
                                    switch (_a.label) {
                                        case 0: return [4 /*yield*/, this.initialize()];
                                        case 1:
                                            _a.sent();
                                            return [2 /*return*/];
                                    }
                                });
                            }); });
                            _this._insightsToggle = new ToggleButton(root.find(".reports__surgeries__insights"));
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
                            root.find(".reports__surgeries__export__excel").click(function (e) { return __awaiter(_this, void 0, void 0, function () {
                                return __generator(this, function (_a) {
                                    switch (_a.label) {
                                        case 0: return [4 /*yield*/, this.exportExcel()];
                                        case 1:
                                            _a.sent();
                                            return [2 /*return*/];
                                    }
                                });
                            }); });
                            root.find(".reports__surgeries__export__pdf").click(function (e) { return __awaiter(_this, void 0, void 0, function () {
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
                        ReportsTabSurgeries.prototype.exportExcel = function () {
                            return __awaiter(this, void 0, void 0, function () {
                                return __generator(this, function (_a) {
                                    routes.corporateSurgeries.exportSurgeriesToExcel(this._page.corporateUrlPath, this._filterDateFrom, this._filterDateTo, this._dateAggregate.value(), this._reportFields.selected.join(","), "").open();
                                    return [2 /*return*/];
                                });
                            });
                        };
                        ReportsTabSurgeries.prototype.exportPdf = function () {
                            return __awaiter(this, void 0, void 0, function () {
                                var formatAsNumber, formatAsPercent, reportBuilder, reportWriter, children, table, dateRanges, _loop_1, _i, dateRanges_1, dateRange, _loop_2, _a, dateRanges_2, dateRange, _loop_3, _b, dateRanges_3, dateRange, _loop_4, _c, dateRanges_4, dateRange, _loop_5, _d, dateRanges_5, dateRange, detailRows, firstRow, tabStrip, index, dataSet, models, dataSet, brands, dataSet;
                                return __generator(this, function (_e) {
                                    formatAsNumber = function (x, decimalDigits) {
                                        if (typeof x === "number") {
                                            return x.toFixed(decimalDigits);
                                        }
                                        return "";
                                    };
                                    formatAsPercent = function (x, decimalDigits) {
                                        if (typeof x === "number") {
                                            return (x * 100).toFixed(decimalDigits) + "%";
                                        }
                                        return "";
                                    };
                                    reportBuilder = new PdfReportBuilder();
                                    reportBuilder.fileName = this._page.corporateUrlPath + "-BySurgery.pdf";
                                    reportBuilder.open();
                                    reportWriter = new SurgeryReportFlowWriter(reportBuilder);
                                    reportWriter.addHeading(1, "Handpieces by surgeries");
                                    if (this._insightsToggle.active) {
                                        children = this._insightsContainer.children();
                                        reportWriter.addInsightsTotals(undefined, $(children[0]));
                                        reportWriter.addCharts($(children[1]).find(".k-chart").toArray().map(function (element, index) { return $(element).data("kendoChart"); }));
                                    }
                                    table = new ReportTable();
                                    table.rowHeight = 23;
                                    table.addColumn("Surgery Name", 200, function (x) { return x.ClientName; });
                                    if (this._dateAggregate.value() === "EntirePeriod") {
                                        if (this._reportFields.isToggled("RatingAverage")) {
                                            table.addColumn("Avg. Rating", 50, function (x) { return formatAsNumber(x.RatingAverage, 2); });
                                        }
                                        if (this._reportFields.isToggled("UnrepairedPercent")) {
                                            table.addColumn("% Unrep.", 50, function (x) { return formatAsPercent(x.UnrepairedPercent, 2); });
                                        }
                                        if (this._reportFields.isToggled("CostSum")) {
                                            table.addColumn("Cost", 50, function (x) { return formatAsNumber(x.CostSum, 2); });
                                        }
                                        if (this._reportFields.isToggled("CostAverage")) {
                                            table.addColumn("Avg. Cost", 50, function (x) { return formatAsNumber(x.CostAverage, 2); });
                                        }
                                        if (this._reportFields.isToggled("HandpiecesCount")) {
                                            table.addColumn("# HPs", 50, function (x) { return formatAsNumber(x.HandpiecesCount, 0); });
                                        }
                                    }
                                    else {
                                        dateRanges = this.generateDateRange(this._filterDateFrom, this._filterDateTo, this._dateAggregate.value());
                                        if (this._reportFields.isToggled("RatingAverage")) {
                                            _loop_1 = function (dateRange) {
                                                table.addColumn("" + dateRange, 50, function (x) { return formatAsNumber(x.RatingAverage[dateRange], 2); });
                                            };
                                            for (_i = 0, dateRanges_1 = dateRanges; _i < dateRanges_1.length; _i++) {
                                                dateRange = dateRanges_1[_i];
                                                _loop_1(dateRange);
                                            }
                                            table.groupLastColumns("Avg. Rating", dateRanges.length);
                                        }
                                        if (this._reportFields.isToggled("UnrepairedPercent")) {
                                            _loop_2 = function (dateRange) {
                                                table.addColumn("" + dateRange, 50, function (x) { return formatAsPercent(x.UnrepairedPercent[dateRange], 2); });
                                            };
                                            for (_a = 0, dateRanges_2 = dateRanges; _a < dateRanges_2.length; _a++) {
                                                dateRange = dateRanges_2[_a];
                                                _loop_2(dateRange);
                                            }
                                            table.groupLastColumns("% Unrep.", dateRanges.length);
                                        }
                                        if (this._reportFields.isToggled("CostSum")) {
                                            _loop_3 = function (dateRange) {
                                                table.addColumn("" + dateRange, 50, function (x) { return formatAsNumber(x.CostSum[dateRange], 2); });
                                            };
                                            for (_b = 0, dateRanges_3 = dateRanges; _b < dateRanges_3.length; _b++) {
                                                dateRange = dateRanges_3[_b];
                                                _loop_3(dateRange);
                                            }
                                            table.groupLastColumns("Cost", dateRanges.length);
                                        }
                                        if (this._reportFields.isToggled("CostAverage")) {
                                            _loop_4 = function (dateRange) {
                                                table.addColumn("" + dateRange, 50, function (x) { return formatAsNumber(x.CostAverage[dateRange], 2); });
                                            };
                                            for (_c = 0, dateRanges_4 = dateRanges; _c < dateRanges_4.length; _c++) {
                                                dateRange = dateRanges_4[_c];
                                                _loop_4(dateRange);
                                            }
                                            table.groupLastColumns("Avg. Cost", dateRanges.length);
                                        }
                                        if (this._reportFields.isToggled("HandpiecesCount")) {
                                            _loop_5 = function (dateRange) {
                                                table.addColumn("" + dateRange, 50, function (x) { return formatAsNumber(x.HandpiecesCount[dateRange], 0); });
                                            };
                                            for (_d = 0, dateRanges_5 = dateRanges; _d < dateRanges_5.length; _d++) {
                                                dateRange = dateRanges_5[_d];
                                                _loop_5(dateRange);
                                            }
                                            table.groupLastColumns("# HPs", dateRanges.length);
                                        }
                                    }
                                    detailRows = this._mainGrid.wrapper.find(".k-grid-content .k-detail-row:visible");
                                    if (detailRows.length > 0) {
                                        firstRow = detailRows.eq(0);
                                        tabStrip = firstRow.find(".k-tabstrip").data("kendoTabStrip");
                                        index = tabStrip.select().index();
                                        if (index === 1) {
                                            dataSet = this._dataSource.data().map(function (x) { return x; });
                                            models = this._dataSourceModels.data().map(function (x) { return x; });
                                            table.columns[0].setFormatter(function (item) {
                                                if (item.Model) {
                                                    return item.Brand + " " + item.Model;
                                                }
                                                return item.ClientName;
                                            });
                                            table.setData(dataSet.concat(models), function (left, right) {
                                                if (left.ClientName < right.ClientName)
                                                    return -1;
                                                if (left.ClientName > right.ClientName)
                                                    return 1;
                                                if (left.ClientId < right.ClientId)
                                                    return -1;
                                                if (left.ClientId > right.ClientId)
                                                    return 1;
                                                var leftBrandModel = left && left.Brand && left.Model ? left.Brand + " " + left.Model : "";
                                                var rightBrandModel = right && right.Brand && right.Model ? right.Brand + " " + right.Model : "";
                                                if (leftBrandModel < rightBrandModel)
                                                    return -1;
                                                if (leftBrandModel > rightBrandModel)
                                                    return 1;
                                                return 0;
                                            });
                                            table.dataRowFormatted.subscribe(function (sender, args) {
                                                if (!args.item.Model) {
                                                    args.row.classList.add("report-table__row--parent");
                                                }
                                            });
                                        }
                                        else {
                                            dataSet = this._dataSource.data().map(function (x) { return x; });
                                            brands = this._dataSourceBrands.data().map(function (x) { return x; });
                                            table.columns[0].setFormatter(function (item) {
                                                if (item.Brand) {
                                                    return item.Brand;
                                                }
                                                return item.ClientName;
                                            });
                                            table.setData(dataSet.concat(brands), function (left, right) {
                                                if (left.ClientName < right.ClientName)
                                                    return -1;
                                                if (left.ClientName > right.ClientName)
                                                    return 1;
                                                if (left.ClientId < right.ClientId)
                                                    return -1;
                                                if (left.ClientId > right.ClientId)
                                                    return 1;
                                                var leftBrandModel = left && left.Brand ? "" + left.Brand : "";
                                                var rightBrandModel = right && right.Brand ? "" + right.Brand : "";
                                                if (leftBrandModel < rightBrandModel)
                                                    return -1;
                                                if (leftBrandModel > rightBrandModel)
                                                    return 1;
                                                return 0;
                                            });
                                            table.dataRowFormatted.subscribe(function (sender, args) {
                                                if (!args.item.Brand) {
                                                    args.row.classList.add("report-table__row--parent");
                                                }
                                            });
                                        }
                                    }
                                    else {
                                        dataSet = this._dataSource.data().map(function (x) { return x; });
                                        table.setData(dataSet, function (left, right) {
                                            if (left.ClientName < right.ClientName)
                                                return -1;
                                            if (left.ClientName > right.ClientName)
                                                return 1;
                                            if (left.ClientId < right.ClientId)
                                                return -1;
                                            if (left.ClientId > right.ClientId)
                                                return 1;
                                            return 0;
                                        });
                                    }
                                    reportWriter.addTable(table);
                                    return [2 /*return*/];
                                });
                            });
                        };
                        ReportsTabSurgeries.prototype.toggleInsights = function () {
                            return __awaiter(this, void 0, void 0, function () {
                                /* General rendering */
                                function addValue(key, value) {
                                    var valueColValue = $("<div></div>")
                                        .addClass("reports__insights__total__value")
                                        .text(value);
                                    var valueColKey = $("<div></div>")
                                        .addClass("reports__insights__total__key")
                                        .text(key);
                                    var valueCol = $("<div></div>")
                                        .addClass("col")
                                        .addClass("reports__insights__total")
                                        .addClass("text-center")
                                        .append(valueColValue)
                                        .append(valueColKey);
                                    totalsWrapper.append(valueCol);
                                }
                                function renderNoRecords() {
                                    var noRecordsWrapper = $("<div></div>").addClass("row");
                                    var noRecordValue = $("<div></div>")
                                        .addClass("reports__insights__total__value")
                                        .addClass("col-12")
                                        .text("No data available for selected date range and surgeries")
                                        .appendTo(noRecordsWrapper);
                                    self._insightsContainer.append(noRecordsWrapper);
                                }
                                /* Flat Values */
                                function renderFlatValueInsights(totalCount, totalRating, totalUnrepaired, totalCost) {
                                    if (self._reportFields.isToggled("RatingAverage")) {
                                        addValue("Avg. rating", "" + kendo.toString(totalRating / totalCount, "n2"));
                                    }
                                    if (self._reportFields.isToggled("UnrepairedPercent")) {
                                        addValue("Unrepaired", "" + kendo.toString(totalUnrepaired / totalCount, "p2"));
                                    }
                                    if (self._reportFields.isToggled("CostSum")) {
                                        addValue("Total spent", "$" + kendo.toString(totalCost, "n0"));
                                    }
                                    if (self._reportFields.isToggled("CostAverage")) {
                                        addValue("Avg. spent", "$" + kendo.toString(totalCost / totalCount, "n0"));
                                    }
                                    if (self._reportFields.isToggled("HandpiecesCount")) {
                                        addValue("Handpieces", "" + totalCount);
                                    }
                                    self._insightsContainer.append(totalsWrapper);
                                }
                                function renderAggregatedCharts() {
                                    self._insightsContainer.append(chartsWrapper);
                                    var aggregatedBarData = self.calculateAggregatedChartData();
                                    if (self._reportFields.isToggled("RatingAverage")) {
                                        var barChartData = self.getAggregatedBarData(aggregatedBarData, "AverageRating");
                                        charts.createLimitedBarChart()
                                            .initialize("Average rating of handpieces by period", barChartData, "{0:n2}", "#= value ? value.toFixed(2) : '' #", 10);
                                    }
                                    if (self._reportFields.isToggled("UnrepairedPercent")) {
                                        var barChartData = self.getAggregatedBarData(aggregatedBarData, "UnrepairedPercent");
                                        charts.createLimitedBarChart()
                                            .initialize("Average percent of unrepaired handpieces by period", barChartData, "{0:p2}", "#= value ? value.toLocaleString('en', {style: 'percent'}) : '' #", 0.5);
                                    }
                                    if (self._reportFields.isToggled("CostSum")) {
                                        var barChartData = self.getAggregatedBarData(aggregatedBarData, "CostSum");
                                        charts.createBarChart()
                                            .initialize("Sum of handpieces repair cost by period", barChartData, "{0:c0}", "#= value ? value.toLocaleString('en-AU', { style: 'currency', currency: 'AUD', minimumFractionDigits: 0, maximumFractionDigits: 0 }) : '' #", undefined);
                                    }
                                    if (self._reportFields.isToggled("CostAverage")) {
                                        var barChartData = self.getAggregatedBarData(aggregatedBarData, "CostAverage");
                                        charts.createBarChart()
                                            .initialize("Average of handpieces repair cost by period", barChartData, "{0:c0}", "#= value ? value.toLocaleString('en-AU', { style: 'currency', currency: 'AUD', minimumFractionDigits: 0, maximumFractionDigits: 0 }) : '' #", undefined);
                                    }
                                    if (self._reportFields.isToggled("HandpiecesCount")) {
                                        var barChartData = self.getAggregatedBarData(aggregatedBarData, "HandpiecesCount");
                                        charts.createBarChart()
                                            .initialize("Sum of handpieces repaired by period", (barChartData), "{0:n0}", "#= value ? value : '' #", undefined);
                                    }
                                }
                                function renderEntirePeriodCharts() {
                                    self._insightsContainer.append(chartsWrapper);
                                    {
                                        var pieChartData = self.getHandpieceNumberPieChartData();
                                        charts.createPieChart()
                                            .initialize("Brands by number of handpieces received", pieChartData, "{0:n0}", "#= category #: \n #= value #", undefined);
                                    }
                                    {
                                        var pieChartData = self.getHandpieceCostPieChartData();
                                        charts.createPieChart()
                                            .initialize("Brands by cost of handpieces repaired", pieChartData, "{0:c0}", "#= category #: \n #= value.toLocaleString('en-AU', { style: 'currency', currency: 'AUD', minimumFractionDigits: 0, maximumFractionDigits: 0 }) #", undefined);
                                    }
                                    {
                                        var mapChartData = self.calculateSurgeryChartData();
                                        charts.createTreeMap()
                                            .initialize("Surgeries map by cost of repairs", mapChartData, "{0:c0}", undefined, undefined);
                                    }
                                }
                                var totalsWrapper, chartsWrapper, charts, self, data, totalCount, totalRating, totalUnrepaired, totalCost, i, item, i, item, key;
                                return __generator(this, function (_a) {
                                    this._insightsContainer.empty();
                                    if (!this._insightsToggle.active) {
                                        return [2 /*return*/];
                                    }
                                    totalsWrapper = $("<div></div>").addClass("row");
                                    chartsWrapper = $("<div></div>").addClass("row");
                                    charts = new Reports.ChartsCollectionContainer(chartsWrapper);
                                    self = this;
                                    data = this._dataSource.data();
                                    if (data.length === 0) {
                                        renderNoRecords();
                                        return [2 /*return*/];
                                    }
                                    totalCount = 0;
                                    totalRating = 0;
                                    totalUnrepaired = 0;
                                    totalCost = 0;
                                    if (this._dateAggregate.value() === "EntirePeriod") {
                                        for (i = 0; i < data.length; i++) {
                                            item = data[i];
                                            if (item.HandpiecesCount !== undefined && typeof item.HandpiecesCount === "number") {
                                                totalCount += item.HandpiecesCount;
                                                if (item.CostSum !== undefined && typeof item.CostSum === "number") {
                                                    totalCost += item.CostSum;
                                                }
                                                if (item.RatingAverage !== undefined && typeof item.RatingAverage === "number") {
                                                    totalRating += item.RatingAverage * item.HandpiecesCount;
                                                }
                                                if (item.UnrepairedPercent !== undefined && typeof item.UnrepairedPercent === "number") {
                                                    totalUnrepaired += item.UnrepairedPercent * item.HandpiecesCount;
                                                }
                                            }
                                        }
                                        renderFlatValueInsights(totalCount, totalRating, totalUnrepaired, totalCost);
                                        renderEntirePeriodCharts();
                                    }
                                    else {
                                        for (i = 0; i < data.length; i++) {
                                            item = data[i];
                                            if (item.HandpiecesCount !== undefined && typeof item.HandpiecesCount === "object") {
                                                for (key in item.HandpiecesCount) {
                                                    if (item.HandpiecesCount.hasOwnProperty(key) && typeof item.HandpiecesCount[key] === "number") {
                                                        totalCount += item.HandpiecesCount[key];
                                                        if (item.CostSum !== undefined && item.CostSum.hasOwnProperty(key) && typeof item.CostSum[key] === "number") {
                                                            totalCost += item.CostSum[key];
                                                        }
                                                        if (item.RatingAverage !== undefined &&
                                                            item.RatingAverage.hasOwnProperty(key) &&
                                                            typeof item.RatingAverage[key] === "number") {
                                                            totalRating += item.RatingAverage[key] * item.HandpiecesCount[key];
                                                        }
                                                        if (item.UnrepairedPercent !== undefined &&
                                                            item.UnrepairedPercent.hasOwnProperty(key) &&
                                                            typeof item.UnrepairedPercent[key] === "number") {
                                                            totalUnrepaired += item.UnrepairedPercent[key] * item.HandpiecesCount[key];
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        renderFlatValueInsights(totalCount, totalRating, totalUnrepaired, totalCost);
                                        renderAggregatedCharts();
                                    }
                                    return [2 /*return*/];
                                });
                            });
                        };
                        ReportsTabSurgeries.prototype.initializeDataSourceFields = function (fields) {
                            var dateAggregate = this._dateAggregate.value();
                            var baseFields = ["RatingAverage", "CostSum", "CostAverage", "UnrepairedPercent", "HandpiecesCount"];
                            if (dateAggregate === "EntirePeriod") {
                                for (var _i = 0, baseFields_1 = baseFields; _i < baseFields_1.length; _i++) {
                                    var field = baseFields_1[_i];
                                    fields[field] = { type: "number", from: field };
                                }
                            }
                            else {
                                var dateRanges = this.generateDateRange(this._filterDateFrom, this._filterDateTo, dateAggregate);
                                for (var _a = 0, baseFields_2 = baseFields; _a < baseFields_2.length; _a++) {
                                    var field = baseFields_2[_a];
                                    for (var _b = 0, dateRanges_6 = dateRanges; _b < dateRanges_6.length; _b++) {
                                        var range = dateRanges_6[_b];
                                        fields[field + "_" + range.replace("-", "_")] = { type: "number", from: field + "[\"" + range + "\"]" };
                                    }
                                }
                            }
                            return fields;
                        };
                        ReportsTabSurgeries.prototype.initializeDataSources = function () {
                            return __awaiter(this, void 0, void 0, function () {
                                return __generator(this, function (_a) {
                                    switch (_a.label) {
                                        case 0:
                                            this._dataSource = new kendo.data.DataSource({
                                                type: "aspnetmvc-ajax",
                                                transport: {
                                                    read: {
                                                        url: "/CorporateSurgeries/" + this._page.corporateUrlPath + "/Reports/Handpieces",
                                                        data: {
                                                            From: this._filterDateFrom.toISOString(),
                                                            To: this._filterDateTo.toISOString(),
                                                            Clients: this._filterClients,
                                                            DateAggregate: this._dateAggregate.value()
                                                        }
                                                    }
                                                },
                                                schema: {
                                                    data: "Data",
                                                    total: "Total",
                                                    errors: "Errors",
                                                    model: {
                                                        fields: this.initializeDataSourceFields({
                                                            ClientId: { type: "string" },
                                                            ClientName: { type: "string" },
                                                        })
                                                    }
                                                }
                                            });
                                            return [4 /*yield*/, this._dataSource.read()];
                                        case 1:
                                            _a.sent();
                                            this._dataSourceModels = new kendo.data.DataSource({
                                                type: "aspnetmvc-ajax",
                                                transport: {
                                                    read: {
                                                        url: "/CorporateSurgeries/" + this._page.corporateUrlPath + "/Reports/Handpieces/ByModel",
                                                        data: {
                                                            From: this._filterDateFrom.toISOString(),
                                                            To: this._filterDateTo.toISOString(),
                                                            Clients: this._filterClients,
                                                            DateAggregate: this._dateAggregate.value()
                                                        }
                                                    }
                                                },
                                                schema: {
                                                    data: "Data",
                                                    total: "Total",
                                                    errors: "Errors",
                                                    model: {
                                                        fields: this.initializeDataSourceFields({
                                                            ClientId: { type: "string" },
                                                            ClientName: { type: "string" },
                                                            Brand: { type: "string" },
                                                            Model: { type: "string" }
                                                        })
                                                    }
                                                }
                                            });
                                            return [4 /*yield*/, this._dataSourceModels.read()];
                                        case 2:
                                            _a.sent();
                                            this._dataSourceBrands = new kendo.data.DataSource({
                                                type: "aspnetmvc-ajax",
                                                transport: {
                                                    read: {
                                                        url: "/CorporateSurgeries/" + this._page.corporateUrlPath + "/Reports/Handpieces/ByBrands",
                                                        data: {
                                                            From: this._filterDateFrom.toISOString(),
                                                            To: this._filterDateTo.toISOString(),
                                                            Clients: this._filterClients,
                                                            DateAggregate: this._dateAggregate.value()
                                                        }
                                                    }
                                                },
                                                schema: {
                                                    data: "Data",
                                                    total: "Total",
                                                    errors: "Errors",
                                                    model: {
                                                        fields: this.initializeDataSourceFields({
                                                            ClientId: { type: "string" },
                                                            ClientName: { type: "string" },
                                                            Brand: { type: "string" },
                                                            Model: { type: "string" }
                                                        })
                                                    }
                                                }
                                            });
                                            return [4 /*yield*/, this._dataSourceBrands.read()];
                                        case 3:
                                            _a.sent();
                                            return [2 /*return*/];
                                    }
                                });
                            });
                        };
                        ReportsTabSurgeries.prototype.getFieldsCandidates = function () {
                            return [
                                {
                                    field: "RatingAverage",
                                    title: "Avg. Rating",
                                    format: "{0:n}"
                                },
                                {
                                    field: "UnrepairedPercent",
                                    title: "% Unrep.",
                                    format: "{0:p2}"
                                },
                                {
                                    field: "CostSum",
                                    title: "Cost",
                                    format: "{0:c0}"
                                },
                                {
                                    field: "CostAverage",
                                    title: "Avg. Cost",
                                    format: "{0:c0}"
                                },
                                {
                                    field: "HandpiecesCount",
                                    title: "# HPs",
                                    format: "{0:n0}"
                                }
                            ];
                        };
                        ReportsTabSurgeries.prototype.createMainGrid = function () {
                            var _this = this;
                            var wrapper = $("<div></div>");
                            this._container.empty();
                            this._container.append(wrapper);
                            var columnsConfig = [];
                            columnsConfig.push({
                                field: "ClientId",
                                template: "#: data.ClientName #",
                                title: "Surgery Name",
                                width: "300px"
                            });
                            var selectedFields = this._reportFields.selected;
                            var fieldsCandidates = this.getFieldsCandidates();
                            var dateAggregate = this._dateAggregate.value();
                            for (var _i = 0, _a = fieldsCandidates.filter(function (x) { return selectedFields.indexOf(x.field) >= 0; }); _i < _a.length; _i++) {
                                var candidate = _a[_i];
                                if (dateAggregate === "EntirePeriod") {
                                    columnsConfig.push({
                                        field: candidate.field,
                                        title: candidate.title,
                                        width: "125px",
                                        format: candidate.format,
                                    });
                                }
                                else {
                                    var dateRanges = this.generateDateRange(this._filterDateFrom, this._filterDateTo, dateAggregate);
                                    var childColumns = [];
                                    for (var _b = 0, dateRanges_7 = dateRanges; _b < dateRanges_7.length; _b++) {
                                        var range = dateRanges_7[_b];
                                        childColumns.push({
                                            title: range,
                                            field: candidate.field + "_" + range.replace("-", "_"),
                                            width: "125px",
                                            format: candidate.format,
                                        });
                                    }
                                    columnsConfig.push({
                                        title: candidate.title,
                                        columns: childColumns
                                    });
                                }
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
                                detailTemplate: function (data) {
                                    var html = "";
                                    html += "<div class=\"reports__surgeries__item__details\">";
                                    html += "  <ul>";
                                    html += "    <li class=\"k-state-active\">By Brands</li>";
                                    html += "    <li>By Models</li>";
                                    html += "  </ul>";
                                    html += "  <div>";
                                    html += "    <div class=\"reports__surgeries__item__details__brands-grid\"></div>";
                                    html += "  </div>";
                                    html += "  <div>";
                                    html += "    <div class=\"reports__surgeries__item__details__models-grid\"></div>";
                                    html += "  </div>";
                                    html += "</div>";
                                    return html;
                                },
                                noRecords: {
                                    template: "No data available for selected date range and surgeries"
                                },
                                detailInit: function (e) {
                                    var tabStripHost = e.detailCell.find(".reports__surgeries__item__details");
                                    tabStripHost.kendoTabStrip();
                                    var brandsGridHost = e.detailCell.find(".reports__surgeries__item__details__brands-grid");
                                    var modelsGridHost = e.detailCell.find(".reports__surgeries__item__details__models-grid");
                                    var clientId = e.data["ClientId"];
                                    _this.createBrandsGrid(brandsGridHost, clientId);
                                    _this.createModelsGrid(modelsGridHost, clientId);
                                }
                            });
                            this._mainGrid = wrapper.data("kendoGrid");
                        };
                        ReportsTabSurgeries.prototype.createBrandsGrid = function (host, clientId) {
                            var subset = [];
                            var superset = this._dataSourceBrands.data();
                            for (var i = 0; i < superset.length; i++) {
                                var item = superset[i];
                                if (item["ClientId"] === clientId)
                                    subset.push({
                                        ClientId: item["ClientId"],
                                        ClientName: item["ClientName"],
                                        Brand: item["Brand"],
                                        RatingAverage: item["RatingAverage"],
                                        CostSum: item["CostSum"],
                                        CostAverage: item["CostAverage"],
                                        HandpiecesCount: item["HandpiecesCount"],
                                        UnrepairedPercent: item["UnrepairedPercent"]
                                    });
                            }
                            var brandsDataSource = new kendo.data.DataSource({
                                data: subset,
                                schema: {
                                    model: {
                                        fields: this.initializeDataSourceFields({
                                            ClientId: { type: "string" },
                                            ClientName: { type: "string" },
                                            Brand: { type: "string" },
                                        })
                                    }
                                }
                            });
                            var brandsColumnsConfig = [];
                            brandsColumnsConfig.push({
                                field: "Brand",
                                title: "Brand",
                                width: "200px",
                            });
                            var selectedFields = this._reportFields.selected;
                            var fieldsCandidates = this.getFieldsCandidates();
                            var dateAggregate = this._dateAggregate.value();
                            for (var _i = 0, _a = fieldsCandidates.filter(function (x) { return selectedFields.indexOf(x.field) >= 0; }); _i < _a.length; _i++) {
                                var candidate = _a[_i];
                                if (dateAggregate === "EntirePeriod") {
                                    brandsColumnsConfig.push({
                                        field: candidate.field,
                                        title: candidate.title,
                                        width: "125px",
                                        format: candidate.format,
                                    });
                                }
                                else {
                                    var dateRanges = this.generateDateRange(this._filterDateFrom, this._filterDateTo, dateAggregate);
                                    var childColumns = [];
                                    for (var _b = 0, dateRanges_8 = dateRanges; _b < dateRanges_8.length; _b++) {
                                        var range = dateRanges_8[_b];
                                        childColumns.push({
                                            title: range,
                                            field: candidate.field + "_" + range.replace("-", "_"),
                                            width: "125px",
                                            format: candidate.format,
                                        });
                                    }
                                    brandsColumnsConfig.push({
                                        title: candidate.title,
                                        columns: childColumns
                                    });
                                }
                            }
                            brandsColumnsConfig.push({
                                title: ""
                            });
                            host.kendoGrid({
                                dataSource: brandsDataSource,
                                columns: brandsColumnsConfig,
                                scrollable: true,
                                sortable: {
                                    mode: "multiple",
                                    allowUnsort: true,
                                },
                            });
                        };
                        ReportsTabSurgeries.prototype.createModelsGrid = function (host, clientId) {
                            var subset = [];
                            var superset = this._dataSourceModels.data();
                            for (var i = 0; i < superset.length; i++) {
                                var item = superset[i];
                                if (item["ClientId"] === clientId)
                                    subset.push({
                                        ClientId: item["ClientId"],
                                        ClientName: item["ClientName"],
                                        Brand: item["Brand"],
                                        Model: item["Model"],
                                        RatingAverage: item["RatingAverage"],
                                        CostSum: item["CostSum"],
                                        CostAverage: item["CostAverage"],
                                        HandpiecesCount: item["HandpiecesCount"],
                                        UnrepairedPercent: item["UnrepairedPercent"]
                                    });
                            }
                            var modelsDataSource = new kendo.data.DataSource({
                                data: subset,
                                schema: {
                                    model: {
                                        fields: this.initializeDataSourceFields({
                                            ClientId: { type: "string" },
                                            ClientName: { type: "string" },
                                            Model: { type: "string" }
                                        })
                                    }
                                }
                            });
                            var modelsColumnsConfig = [];
                            modelsColumnsConfig.push({
                                field: "Model",
                                template: "#: Brand # #: Model #",
                                title: "Model",
                                width: "200px"
                            });
                            var selectedFields = this._reportFields.selected;
                            var fieldsCandidates = this.getFieldsCandidates();
                            var dateAggregate = this._dateAggregate.value();
                            for (var _i = 0, _a = fieldsCandidates.filter(function (x) { return selectedFields.indexOf(x.field) >= 0; }); _i < _a.length; _i++) {
                                var candidate = _a[_i];
                                if (dateAggregate === "EntirePeriod") {
                                    modelsColumnsConfig.push({
                                        field: candidate.field,
                                        title: candidate.title,
                                        width: "125px",
                                        format: candidate.format,
                                    });
                                }
                                else {
                                    var dateRanges = this.generateDateRange(this._filterDateFrom, this._filterDateTo, dateAggregate);
                                    var childColumns = [];
                                    for (var _b = 0, dateRanges_9 = dateRanges; _b < dateRanges_9.length; _b++) {
                                        var range = dateRanges_9[_b];
                                        childColumns.push({
                                            title: range,
                                            field: candidate.field + "_" + range.replace("-", "_"),
                                            width: "125px",
                                            format: candidate.format,
                                        });
                                    }
                                    modelsColumnsConfig.push({
                                        title: candidate.title,
                                        columns: childColumns
                                    });
                                }
                            }
                            modelsColumnsConfig.push({
                                title: ""
                            });
                            host.kendoGrid({
                                dataSource: modelsDataSource,
                                columns: modelsColumnsConfig,
                                scrollable: true,
                                sortable: {
                                    mode: "multiple",
                                    allowUnsort: true,
                                },
                            });
                        };
                        ReportsTabSurgeries.prototype.getAggregatedBarData = function (data, reportFieldName) {
                            var response = new Array();
                            for (var i in data.series[reportFieldName].data) {
                                response.push({
                                    name: data.series[reportFieldName].data[i].name,
                                    value: data.series[reportFieldName].data[i].values,
                                });
                            }
                            return response;
                        };
                        ReportsTabSurgeries.prototype.getHandpieceCostPieChartData = function () {
                            var response = new Array();
                            var data = this.calculateHandpiecesChartData();
                            for (var i in data) {
                                response.push({
                                    category: data[i].Brand,
                                    value: data[i].TotalCost
                                });
                            }
                            return response;
                        };
                        ReportsTabSurgeries.prototype.getHandpieceNumberPieChartData = function () {
                            var response = new Array();
                            var data = this.calculateHandpiecesChartData();
                            for (var i in data) {
                                response.push({
                                    category: data[i].Brand,
                                    value: data[i].TotalNumber
                                });
                            }
                            return response;
                        };
                        ReportsTabSurgeries.prototype.calculateAggregatedChartData = function () {
                            var response = {
                                series: {
                                    AverageRating: {
                                        name: "Average Rating",
                                        data: []
                                    },
                                    UnrepairedPercent: {
                                        name: "Unrepaired Percent",
                                        data: []
                                    },
                                    CostSum: {
                                        name: "Cost Sum",
                                        data: []
                                    },
                                    CostAverage: {
                                        name: "Cost Average",
                                        data: []
                                    },
                                    HandpiecesCount: {
                                        name: "Handpieces Count",
                                        data: []
                                    }
                                },
                            };
                            var data = this._dataSourceModels.data();
                            var dateAggregate = this._dateAggregate.value();
                            var dateRanges = this.generateDateRange(this._filterDateFrom, this._filterDateTo, dateAggregate);
                            var _loop_6 = function (range) {
                                var handpiecesCount = data.reduce(function (x, hbs) { return x + hbs["HandpiecesCount_" + range.replace("-", "_")]; }, 0);
                                response.series['HandpiecesCount'].data.push({
                                    name: range,
                                    values: handpiecesCount
                                });
                                var averageRating = data.reduce(function (x, hbs) { return x + hbs["RatingAverage_" + range.replace("-", "_")] * hbs["HandpiecesCount_" + range.replace("-", "_")]; }, 0) /
                                    handpiecesCount;
                                response.series['AverageRating'].data.push({
                                    name: range,
                                    values: averageRating
                                });
                                var unrepairedPercent = data.reduce(function (x, hbs) { return x +
                                    hbs["UnrepairedPercent_" + range.replace("-", "_")] * hbs["HandpiecesCount_" + range.replace("-", "_")]; }, 0) /
                                    handpiecesCount;
                                response.series['UnrepairedPercent'].data.push({
                                    name: range,
                                    values: unrepairedPercent
                                });
                                var costSum = data.reduce(function (x, hbs) { return x + hbs["CostSum_" + range.replace("-", "_")]; }, 0);
                                response.series['CostSum'].data.push({
                                    name: range,
                                    values: costSum
                                });
                                var costAverage = costSum / handpiecesCount;
                                response.series['CostAverage'].data.push({
                                    name: range,
                                    values: costAverage
                                });
                            };
                            for (var _i = 0, dateRanges_10 = dateRanges; _i < dateRanges_10.length; _i++) {
                                var range = dateRanges_10[_i];
                                _loop_6(range);
                            }
                            return response;
                        };
                        ReportsTabSurgeries.prototype.calculateSurgeryChartData = function () {
                            var response = new Array();
                            var data = this._dataSourceModels.data();
                            var handpiecesBySurgery = data.reduce(function (hbs, d) {
                                var _a;
                                return (__assign(__assign({}, hbs), (_a = {}, _a[d.ClientName] = __spreadArray(__spreadArray([], (hbs[d.ClientName] || []), true), [d], false), _a)));
                            }, {});
                            var surgeries = Object.keys(handpiecesBySurgery);
                            for (var i in surgeries) {
                                var value = handpiecesBySurgery[surgeries[i]].reduce(function (x, hbs) { return x + hbs.CostSum; }, 0);
                                response.push({
                                    name: surgeries[i] + " &#10; " + value.toLocaleString("en-AU", { style: 'currency', currency: 'AUD', minimumFractionDigits: 0, maximumFractionDigits: 0 }),
                                    value: value
                                });
                            }
                            return response;
                        };
                        ReportsTabSurgeries.prototype.calculateHandpiecesChartData = function () {
                            var response = new Array();
                            var data = this._dataSourceModels.data();
                            var handpiecesByBrand = data.reduce(function (hbb, d) {
                                var _a;
                                return (__assign(__assign({}, hbb), (_a = {}, _a[d.Brand] = __spreadArray(__spreadArray([], (hbb[d.Brand] || []), true), [d], false), _a)));
                            }, {});
                            var brands = Object.keys(handpiecesByBrand);
                            for (var i in brands) {
                                response.push({
                                    Brand: brands[i],
                                    TotalNumber: handpiecesByBrand[brands[i]].reduce(function (x, hbb) { return x + hbb.HandpiecesCount; }, 0),
                                    TotalCost: handpiecesByBrand[brands[i]].reduce(function (x, hbb) { return x + hbb.CostSum; }, 0)
                                });
                            }
                            return response;
                        };
                        ReportsTabSurgeries.prototype.initialize = function () {
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
                        ReportsTabSurgeries.prototype.applyGlobalFilters = function (from, to, clients) {
                            return __awaiter(this, void 0, void 0, function () {
                                return __generator(this, function (_a) {
                                    this._filterDateFrom = from;
                                    this._filterDateTo = to;
                                    this._filterClients = clients;
                                    return [2 /*return*/];
                                });
                            });
                        };
                        return ReportsTabSurgeries;
                    }(Reports.ReportsTabBase));
                    Reports.ReportsTabSurgeries = ReportsTabSurgeries;
                })(Reports = CorporateSurgeries.Reports || (CorporateSurgeries.Reports = {}));
            })(CorporateSurgeries = Pages.CorporateSurgeries || (Pages.CorporateSurgeries = {}));
        })(Pages = CRM.Pages || (CRM.Pages = {}));
    })(CRM = DentalDrill.CRM || (DentalDrill.CRM = {}));
})(DentalDrill || (DentalDrill = {}));
//# sourceMappingURL=ReportsTabSurgeries.js.map