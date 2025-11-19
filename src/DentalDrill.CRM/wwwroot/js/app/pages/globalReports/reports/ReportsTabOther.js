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
                    var ReportsPageTabGridCollectionBase = CRM.Controls.Reporting.ReportsPageTabGridCollectionBase;
                    var ReportsTabOther = /** @class */ (function (_super) {
                        __extends(ReportsTabOther, _super);
                        function ReportsTabOther(pageIdentifier, tabRoot) {
                            var _this = _super.call(this, pageIdentifier, tabRoot) || this;
                            _this.registerGridDefinition("TechWarranty", _this.renderTechWarrantyGrid.bind(_this));
                            _this.registerGridDefinition("BatchReturns", _this.renderBatchReturnsGrid.bind(_this));
                            return _this;
                        }
                        ReportsTabOther.prototype.renderTechWarrantyGrid = function (wrapper) {
                            return __awaiter(this, void 0, void 0, function () {
                                var dataSource;
                                return __generator(this, function (_a) {
                                    wrapper.css("height", "500px");
                                    dataSource = new kendo.data.DataSource({
                                        type: "aspnetmvc-ajax",
                                        transport: {
                                            read: {
                                                url: "/Reports/ReadReportTechWarranty",
                                                data: {
                                                    From: this.globalFilters.from.toISOString(),
                                                    To: this.globalFilters.to.toISOString(),
                                                }
                                            }
                                        },
                                        schema: {
                                            data: "Data",
                                            total: "Total",
                                            errors: "Errors",
                                            model: {
                                                HandpieceId: { type: "string" },
                                                JobId: { type: "string" },
                                                JobNumber: { type: "number" },
                                                JobReceived: { type: "date" },
                                                RepairedById: { type: "string" },
                                                RepairedByName: { type: "string" },
                                                Brand: { type: "string" },
                                                Model: { type: "string" },
                                                Serial: { type: "string" },
                                                HandpieceCount: { type: "number" },
                                                Warranty: { type: "number" },
                                                DaysPassed: { type: "number" },
                                            }
                                        },
                                        group: [
                                            {
                                                field: "RepairedByName",
                                                aggregates: [
                                                    { field: "HandpieceCount", aggregate: "sum" },
                                                    { field: "Warranty", aggregate: "sum" },
                                                    { field: "DaysPassed", aggregate: "average" },
                                                ]
                                            }
                                        ],
                                        sort: [
                                            { field: "JobNumber", dir: "asc" },
                                        ],
                                    });
                                    wrapper.kendoGrid({
                                        dataSource: dataSource,
                                        columns: [
                                            {
                                                field: "RepairedByName",
                                                title: "Technician",
                                                width: "100px",
                                                hidden: true
                                            },
                                            {
                                                field: "JobNumber",
                                                title: "Technician / Job",
                                                width: "400px",
                                                template: "Estimate \\##:JobNumber# | #:Brand# #:Model# | #:Serial#",
                                            },
                                            {
                                                field: "HandpieceCount",
                                                title: "Repairs",
                                                width: "100px",
                                                groupHeaderColumnTemplate: "#:sum#",
                                            },
                                            {
                                                field: "Warranty",
                                                title: "Came back on warranty",
                                                width: "200px",
                                                groupHeaderColumnTemplate: "#:sum#",
                                            },
                                            {
                                                field: "DaysPassed",
                                                title: "Last seen",
                                                width: "100px",
                                                groupHeaderColumnTemplate: "#:average ? kendo.toString(average, \"n2\") : \"\"#",
                                            },
                                            {
                                                title: "",
                                                command: [
                                                    {
                                                        name: "CustomDetails",
                                                        click: function (e) {
                                                            e.preventDefault();
                                                            var row = $(e.target).closest("tr");
                                                            var item = this.dataItem(row);
                                                            routes.jobs.details(item.JobId).open();
                                                        },
                                                        text: "<span class=\"fas fa-fw fa-link\"></span> Estimate",
                                                    }
                                                ],
                                                width: "150px",
                                            }
                                        ],
                                        scrollable: true,
                                        sortable: {
                                            mode: "multiple",
                                            allowUnsort: true,
                                        },
                                        noRecords: {
                                            template: "No data available",
                                        },
                                        dataBound: function (e) {
                                            e.sender.wrapper.find(".k-grouping-row").each(function (index, row) {
                                                e.sender.collapseGroup(row);
                                            });
                                        }
                                    });
                                    return [2 /*return*/, wrapper.data("kendoGrid")];
                                });
                            });
                        };
                        ReportsTabOther.prototype.renderBatchReturnsGrid = function (wrapper) {
                            return __awaiter(this, void 0, void 0, function () {
                                var dataSource;
                                return __generator(this, function (_a) {
                                    wrapper.css("height", "500px");
                                    dataSource = new kendo.data.DataSource({
                                        type: "aspnetmvc-ajax",
                                        transport: {
                                            read: {
                                                url: "/Reports/ReadReportBatchReturns",
                                                data: {
                                                    From: this.globalFilters.from.toISOString(),
                                                    To: this.globalFilters.to.toISOString(),
                                                }
                                            }
                                        },
                                        schema: {
                                            data: "Data",
                                            total: "Total",
                                            errors: "Errors",
                                            model: {
                                                JobId: { type: "string" },
                                                JobNumber: { type: "number" },
                                                CompletedFirst: { type: "date" },
                                                CountOfHandpieces: { type: "number" },
                                                CountOfDistinctDates: { type: "number" },
                                                ListOfDates: { type: "string" },
                                            }
                                        },
                                        sort: [
                                            { field: "JobNumber", dir: "asc" },
                                        ],
                                    });
                                    wrapper.kendoGrid({
                                        dataSource: dataSource,
                                        columns: [
                                            {
                                                field: "JobNumber",
                                                title: "Job returned in batches",
                                                width: "200px",
                                                template: "Estimate \\##:JobNumber# | #:CountOfHandpieces# handpieces",
                                            },
                                            {
                                                field: "CompletedFirst",
                                                title: "Date of first batch",
                                                width: "100px",
                                                template: function (data) {
                                                    if (typeof data.CompletedFirst === "string") {
                                                        return kendo.toString(kendo.parseDate(data.CompletedFirst), "dd MMM yyyy");
                                                    }
                                                    else {
                                                        return kendo.toString(data.CompletedFirst, "dd MMM yyyy");
                                                    }
                                                }
                                            },
                                            {
                                                field: "CountOfDistinctDates",
                                                title: "Number of batches",
                                                width: "400px",
                                                template: function (data) {
                                                    var result = "" + data.CountOfDistinctDates.toString();
                                                    var dates = data.ListOfDates.split(",").filter(function (x, i, a) { return a.indexOf(x) === i; });
                                                    result += " (";
                                                    result += dates.map(function (x) { return kendo.toString(kendo.parseDate(x), "dd MMM yyyy"); }).join(", ");
                                                    result += ")";
                                                    return result;
                                                },
                                            },
                                            {
                                                title: "",
                                                command: [
                                                    {
                                                        name: "CustomDetails",
                                                        click: function (e) {
                                                            e.preventDefault();
                                                            var row = $(e.target).closest("tr");
                                                            var item = this.dataItem(row);
                                                            routes.jobs.details(item.JobId).open();
                                                        },
                                                        text: "<span class=\"fas fa-fw fa-link\"></span> Estimate",
                                                    }
                                                ],
                                                width: "150px",
                                            }
                                        ],
                                        scrollable: true,
                                        sortable: {
                                            mode: "multiple",
                                            allowUnsort: true,
                                        },
                                        noRecords: {
                                            template: "No data available",
                                        },
                                    });
                                    return [2 /*return*/, wrapper.data("kendoGrid")];
                                });
                            });
                        };
                        return ReportsTabOther;
                    }(ReportsPageTabGridCollectionBase));
                    Reports.ReportsTabOther = ReportsTabOther;
                })(Reports = GlobalReports.Reports || (GlobalReports.Reports = {}));
            })(GlobalReports = Pages.GlobalReports || (Pages.GlobalReports = {}));
        })(Pages = CRM.Pages || (CRM.Pages = {}));
    })(CRM = DentalDrill.CRM || (DentalDrill.CRM = {}));
})(DentalDrill || (DentalDrill = {}));
//# sourceMappingURL=ReportsTabOther.js.map