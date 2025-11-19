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
        var Controls;
        (function (Controls) {
            var Reporting;
            (function (Reporting) {
                var ToggleButton = DentalDrill.CRM.Controls.ToggleButton;
                var ToggleList = DentalDrill.CRM.Controls.ToggleList;
                var ReportsPageTabGridWithInsightsBase = /** @class */ (function (_super) {
                    __extends(ReportsPageTabGridWithInsightsBase, _super);
                    function ReportsPageTabGridWithInsightsBase(pageIdentifier, tabRoot) {
                        var _this = _super.call(this) || this;
                        _this._pageIdentifier = pageIdentifier;
                        _this._tabRoot = tabRoot;
                        // Initializing Controls
                        _this._reportFields = new ToggleList(tabRoot.find(".reports__tab__fields"));
                        _this._reportFields.selectionChanged.subscribe(function (sender, args) { return __awaiter(_this, void 0, void 0, function () {
                            return __generator(this, function (_a) {
                                switch (_a.label) {
                                    case 0: return [4 /*yield*/, this.initialize()];
                                    case 1:
                                        _a.sent();
                                        return [2 /*return*/];
                                }
                            });
                        }); });
                        _this._dateAggregate = tabRoot.find("input.reports__tab__date-aggregate").data("kendoDropDownList");
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
                        // Initializing Data
                        _this._globalFilters = undefined;
                        _this._dataSource = undefined;
                        // Initializing Grid
                        _this._gridContainer = tabRoot.find(".reports__tab__grid-container");
                        _this._grid = undefined;
                        // Initializing Insights
                        _this._insightsToggle = new ToggleButton(tabRoot.find(".reports__tab__insights"));
                        _this._insightsToggle.changed.subscribe(function (sender, args) { return __awaiter(_this, void 0, void 0, function () {
                            return __generator(this, function (_a) {
                                switch (_a.label) {
                                    case 0: return [4 /*yield*/, this.toggleInsights()];
                                    case 1:
                                        _a.sent();
                                        return [2 /*return*/];
                                }
                            });
                        }); });
                        _this._insightsContainer = new Reporting.ReportInsightsContainer(tabRoot.find(".reports__tab__insights-container"));
                        // Initializing Export
                        tabRoot.find(".reports__tab__export__excel").on("click", function (e) { return __awaiter(_this, void 0, void 0, function () {
                            return __generator(this, function (_a) {
                                switch (_a.label) {
                                    case 0: return [4 /*yield*/, this.exportExcel()];
                                    case 1:
                                        _a.sent();
                                        return [2 /*return*/];
                                }
                            });
                        }); });
                        tabRoot.find(".reports__tab__export__pdf").on("click", function (e) { return __awaiter(_this, void 0, void 0, function () {
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
                    Object.defineProperty(ReportsPageTabGridWithInsightsBase.prototype, "reportFields", {
                        get: function () {
                            return this._reportFields;
                        },
                        enumerable: false,
                        configurable: true
                    });
                    Object.defineProperty(ReportsPageTabGridWithInsightsBase.prototype, "globalFilters", {
                        get: function () {
                            return this._globalFilters;
                        },
                        enumerable: false,
                        configurable: true
                    });
                    Object.defineProperty(ReportsPageTabGridWithInsightsBase.prototype, "dataSource", {
                        get: function () {
                            return this._dataSource;
                        },
                        enumerable: false,
                        configurable: true
                    });
                    Object.defineProperty(ReportsPageTabGridWithInsightsBase.prototype, "dateAggregate", {
                        get: function () {
                            return this._dateAggregate.value();
                        },
                        enumerable: false,
                        configurable: true
                    });
                    Object.defineProperty(ReportsPageTabGridWithInsightsBase.prototype, "insightsToggle", {
                        get: function () {
                            return this._insightsToggle;
                        },
                        enumerable: false,
                        configurable: true
                    });
                    Object.defineProperty(ReportsPageTabGridWithInsightsBase.prototype, "insightsContainer", {
                        get: function () {
                            return this._insightsContainer;
                        },
                        enumerable: false,
                        configurable: true
                    });
                    ReportsPageTabGridWithInsightsBase.prototype.initialize = function () {
                        return __awaiter(this, void 0, void 0, function () {
                            var _a, _b;
                            return __generator(this, function (_c) {
                                switch (_c.label) {
                                    case 0:
                                        _a = this;
                                        return [4 /*yield*/, this.initializeDataSource()];
                                    case 1:
                                        _a._dataSource = _c.sent();
                                        _b = this;
                                        return [4 /*yield*/, this.initializeGrid()];
                                    case 2:
                                        _b._grid = _c.sent();
                                        return [4 /*yield*/, this.toggleInsights()];
                                    case 3:
                                        _c.sent();
                                        return [2 /*return*/];
                                }
                            });
                        });
                    };
                    ReportsPageTabGridWithInsightsBase.prototype.exportExcel = function () {
                        return __awaiter(this, void 0, void 0, function () {
                            return __generator(this, function (_a) {
                                throw new Error("Excel export is not implemented");
                            });
                        });
                    };
                    ReportsPageTabGridWithInsightsBase.prototype.exportPdf = function () {
                        return __awaiter(this, void 0, void 0, function () {
                            return __generator(this, function (_a) {
                                throw new Error("Pdf export is not implemented");
                            });
                        });
                    };
                    ReportsPageTabGridWithInsightsBase.prototype.toggleInsights = function () {
                        return __awaiter(this, void 0, void 0, function () {
                            return __generator(this, function (_a) {
                                switch (_a.label) {
                                    case 0:
                                        this._insightsContainer.clear();
                                        if (!this._insightsToggle.active) {
                                            return [2 /*return*/];
                                        }
                                        return [4 /*yield*/, this.initializeInsights()];
                                    case 1:
                                        _a.sent();
                                        return [2 /*return*/];
                                }
                            });
                        });
                    };
                    ReportsPageTabGridWithInsightsBase.prototype.initializeDataSource = function () {
                        return __awaiter(this, void 0, void 0, function () {
                            var _a;
                            return __generator(this, function (_b) {
                                switch (_b.label) {
                                    case 0:
                                        this._dataSource = undefined;
                                        _a = this;
                                        return [4 /*yield*/, this.createDataSource()];
                                    case 1:
                                        _a._dataSource = _b.sent();
                                        return [2 /*return*/, this._dataSource];
                                }
                            });
                        });
                    };
                    ReportsPageTabGridWithInsightsBase.prototype.createDataSource = function (customOptions) {
                        return __awaiter(this, void 0, void 0, function () {
                            var dataSourceOptions, dataSource;
                            var _a, _b, _c, _d;
                            return __generator(this, function (_e) {
                                switch (_e.label) {
                                    case 0:
                                        _a = {
                                            type: "aspnetmvc-ajax"
                                        };
                                        _b = {};
                                        return [4 /*yield*/, this.initializeDataSourceTransportRead()];
                                    case 1:
                                        _a.transport = (_b.read = _e.sent(),
                                            _b);
                                        _c = {
                                            data: "Data",
                                            total: "Total",
                                            errors: "Errors"
                                        };
                                        _d = {};
                                        return [4 /*yield*/, this.initializeDataSourceFields(this.initializeDataSourceGroupFields())];
                                    case 2:
                                        dataSourceOptions = (_a.schema = (_c.model = (_d.fields = _e.sent(),
                                            _d),
                                            _c),
                                            _a);
                                        this.alterDataSourceOptions(dataSourceOptions);
                                        if (customOptions) {
                                            $.extend(true, dataSourceOptions, customOptions);
                                        }
                                        dataSource = new kendo.data.DataSource(dataSourceOptions);
                                        return [4 /*yield*/, dataSource.read()];
                                    case 3:
                                        _e.sent();
                                        return [2 /*return*/, dataSource];
                                }
                            });
                        });
                    };
                    ReportsPageTabGridWithInsightsBase.prototype.initializeDataSourceFields = function (fields) {
                        return __awaiter(this, void 0, void 0, function () {
                            var dateAggregate, baseFields, _i, baseFields_1, field, dateRanges, _a, baseFields_2, field, _b, dateRanges_1, range;
                            return __generator(this, function (_c) {
                                dateAggregate = this._dateAggregate.value();
                                baseFields = this.initializeDataSourceValueFields();
                                if (dateAggregate === "EntirePeriod") {
                                    for (_i = 0, baseFields_1 = baseFields; _i < baseFields_1.length; _i++) {
                                        field = baseFields_1[_i];
                                        fields[field] = { type: "number", from: field };
                                    }
                                }
                                else {
                                    dateRanges = this.generateDateRange(this._globalFilters.from, this._globalFilters.to, dateAggregate);
                                    for (_a = 0, baseFields_2 = baseFields; _a < baseFields_2.length; _a++) {
                                        field = baseFields_2[_a];
                                        for (_b = 0, dateRanges_1 = dateRanges; _b < dateRanges_1.length; _b++) {
                                            range = dateRanges_1[_b];
                                            fields[field + "_" + range.replace(/-/g, "_")] = { type: "number", from: field + "[\"" + range + "\"]" };
                                        }
                                    }
                                }
                                return [2 /*return*/, fields];
                            });
                        });
                    };
                    ReportsPageTabGridWithInsightsBase.prototype.alterDataSourceOptions = function (dataSourceOptions) {
                    };
                    ReportsPageTabGridWithInsightsBase.prototype.initializeGrid = function () {
                        return __awaiter(this, void 0, void 0, function () {
                            var wrapper, columnsConfig, selectedFields, fieldsCandidates, dateAggregate, _i, _a, candidate, dateRanges, _b, _c, candidate, childColumns, _d, dateRanges_2, range, gridOptions;
                            return __generator(this, function (_e) {
                                wrapper = $("<div></div>");
                                this._gridContainer.empty();
                                this._gridContainer.append(wrapper);
                                columnsConfig = this.initializeGridGroupColumns();
                                selectedFields = this._reportFields.selected;
                                fieldsCandidates = this.initializeGridValueColumns();
                                dateAggregate = this._dateAggregate.value();
                                if (dateAggregate === "EntirePeriod") {
                                    for (_i = 0, _a = fieldsCandidates.filter(function (x) { return selectedFields.indexOf(x.field) >= 0; }); _i < _a.length; _i++) {
                                        candidate = _a[_i];
                                        columnsConfig.push({
                                            field: candidate.field,
                                            title: candidate.title,
                                            width: "125px",
                                            format: candidate.format,
                                            groupHeaderColumnTemplate: candidate.groupHeaderColumnTemplate ? candidate.groupHeaderColumnTemplate("") : undefined
                                        });
                                    }
                                }
                                else {
                                    dateRanges = this.generateDateRange(this._globalFilters.from, this._globalFilters.to, dateAggregate);
                                    for (_b = 0, _c = fieldsCandidates.filter(function (x) { return selectedFields.indexOf(x.field) >= 0; }); _b < _c.length; _b++) {
                                        candidate = _c[_b];
                                        childColumns = [];
                                        for (_d = 0, dateRanges_2 = dateRanges; _d < dateRanges_2.length; _d++) {
                                            range = dateRanges_2[_d];
                                            childColumns.push({
                                                title: range,
                                                field: candidate.field + "_" + range.replace(/-/g, "_"),
                                                width: "125px",
                                                format: candidate.format,
                                                groupHeaderColumnTemplate: candidate.groupHeaderColumnTemplate ? candidate.groupHeaderColumnTemplate("_" + range.replace(/-/g, "_")) : undefined
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
                                gridOptions = {
                                    dataSource: this._dataSource,
                                    columns: columnsConfig,
                                    scrollable: true,
                                    sortable: {
                                        mode: "multiple",
                                        allowUnsort: true,
                                    },
                                    noRecords: {
                                        template: "No data available"
                                    }
                                };
                                this.alterGridOptions(gridOptions);
                                wrapper.kendoGrid(gridOptions);
                                return [2 /*return*/, wrapper.data("kendoGrid")];
                            });
                        });
                    };
                    ReportsPageTabGridWithInsightsBase.prototype.alterGridOptions = function (gridOptions) {
                    };
                    ReportsPageTabGridWithInsightsBase.prototype.initializeInsights = function () {
                        return __awaiter(this, void 0, void 0, function () {
                            return __generator(this, function (_a) {
                                return [2 /*return*/];
                            });
                        });
                    };
                    ReportsPageTabGridWithInsightsBase.prototype.applyGlobalFilters = function (globalFilters) {
                        return __awaiter(this, void 0, void 0, function () {
                            return __generator(this, function (_a) {
                                this._globalFilters = globalFilters;
                                return [2 /*return*/];
                            });
                        });
                    };
                    return ReportsPageTabGridWithInsightsBase;
                }(Reporting.ReportsPageTabDateRangeBase));
                Reporting.ReportsPageTabGridWithInsightsBase = ReportsPageTabGridWithInsightsBase;
            })(Reporting = Controls.Reporting || (Controls.Reporting = {}));
        })(Controls = CRM.Controls || (CRM.Controls = {}));
    })(CRM = DentalDrill.CRM || (DentalDrill.CRM = {}));
})(DentalDrill || (DentalDrill = {}));
//# sourceMappingURL=ReportsPageTabGridWithInsightsBase.js.map