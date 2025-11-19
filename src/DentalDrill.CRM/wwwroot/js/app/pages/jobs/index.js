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
            var Jobs;
            (function (Jobs) {
                var Index;
                (function (Index) {
                    var GridHandlers = DevGuild.AspNet.Controls.Grids.Handlers.GridHandlers;
                    var AjaxFormsManager = DevGuild.AspNet.Forms.AjaxForms.AjaxFormsManager;
                    var Collapsible = DentalDrill.CRM.Controls.Collapsible;
                    var HandpieceStatusIndicator = DentalDrill.CRM.Controls.HandpieceStatusIndicator;
                    var JobsFilters = /** @class */ (function () {
                        function JobsFilters() {
                        }
                        JobsFilters.clickSearch = function () {
                            JobsGridFilter.instance.apply();
                        };
                        JobsFilters.clickCancel = function () {
                            JobsGridFilter.instance.reset();
                        };
                        return JobsFilters;
                    }());
                    Index.JobsFilters = JobsFilters;
                    var JobsGridFilterFieldsCollection = /** @class */ (function (_super) {
                        __extends(JobsGridFilterFieldsCollection, _super);
                        function JobsGridFilterFieldsCollection(root) {
                            var _this = _super.call(this, root) || this;
                            _this._workshop = new DevGuild.Filters.Grids.DropDownListGridFilterField($("#WorkshopFilter"), "WorkshopId", { value: function (a) { return a.Value; }, defaultValue: "" });
                            _this._jobType = new DevGuild.Filters.Grids.DropDownListGridFilterField($("#JobTypeFilter"), "JobTypeId", { value: function (a) { return a.Value; }, defaultValue: "" });
                            _this._jobNumber = new DevGuild.Filters.Grids.StringInputGridFilterField($("#JobNumberFilter"), "JobNumberString");
                            _this._client = new DevGuild.Filters.Grids.DropDownListGridFilterField($("#ClientFilter"), "ClientId", { value: function (a) { return a.Id; }, defaultValue: "" });
                            _this._receivedFrom = new DevGuild.Filters.Grids.DatePickerFilterField($("#ReceivedFromFilter"), "Received", "gte");
                            _this._receivedTo = new DevGuild.Filters.Grids.DatePickerFilterField($("#ReceivedToFilter"), "Received", "lte");
                            _this._serial = new DevGuild.Filters.Grids.StringInputGridFilterField($("#SerialFilter"), "Serial");
                            _this._makeAndModel = new DevGuild.Filters.Grids.DropDownListGridFilterField($("#MakeAndModelFilter"), "MakeAndModel", { value: function (a) { return a.Value; }, defaultValue: "" });
                            _this._makeAndModel.applyValueDelegate = function (filters, value) {
                                var parts = value.split("||");
                                filters.push({ field: "Brand", operator: "eq", value: parts[0] });
                                filters.push({ field: "MakeAndModel", operator: "eq", value: parts[1] });
                            };
                            _this._type = new DevGuild.Filters.Grids.DropDownListGridFilterField($("#TypeFilter"), "SpeedType", { value: function (a) { return a.Value; }, defaultValue: "" });
                            _this._jobStatus = new DevGuild.Filters.Grids.DropDownListGridFilterField($("#JobStatusFilter"), "JobStatus", { value: function (a) { return a.Value; } });
                            _this._jobStatus.applyValueDelegate = function (filters, value) {
                                if (value === "All") {
                                    // Do nothing
                                }
                                else if (value === "Workshop") {
                                    var combined = [];
                                    combined.push({ field: "JobStatus", operator: "eq", value: "Received" });
                                    combined.push({ field: "JobStatus", operator: "eq", value: "BeingEstimated" });
                                    combined.push({ field: "JobStatus", operator: "eq", value: "BeingRepaired" });
                                    filters.push({ logic: "or", filters: combined });
                                }
                                else if (value === "Active") {
                                    filters.push({ field: "JobStatus", operator: "neq", value: "SentComplete" });
                                    filters.push({ field: "JobStatus", operator: "neq", value: "Cancelled" });
                                }
                                else {
                                    filters.push({ field: "JobStatus", operator: "eq", value: value });
                                }
                            };
                            _this._jobStatus.resetValueDelegate = function () { return "Active"; };
                            _this._handpieceStatus = new DevGuild.Filters.Grids.DropDownListGridFilterField($("#HandpieceStatusFilter"), "HandpieceStatus", { value: function (a) { return a.Value; } });
                            _this._handpieceStatus.applyValueDelegate = function (filters, value) {
                                if (value === "All") {
                                    // Do nothing
                                }
                                else if (value === "BeingRepairedEx") {
                                    var combined = [];
                                    combined.push({ field: "HandpieceStatus", operator: "eq", value: "BeingRepaired" });
                                    combined.push({ field: "HandpieceStatus", operator: "eq", value: "WaitingForParts" });
                                    filters.push({ logic: "or", filters: combined });
                                }
                                else if (value === "Workshop") {
                                    var combined = [];
                                    combined.push({ field: "HandpieceStatus", operator: "eq", value: "Received" });
                                    combined.push({ field: "HandpieceStatus", operator: "eq", value: "BeingEstimated" });
                                    combined.push({ field: "HandpieceStatus", operator: "eq", value: "BeingRepaired" });
                                    filters.push({ logic: "or", filters: combined });
                                }
                                else if (value === "Active") {
                                    filters.push({ field: "HandpieceStatus", operator: "neq", value: "SentComplete" });
                                    filters.push({ field: "HandpieceStatus", operator: "neq", value: "Cancelled" });
                                }
                                else {
                                    filters.push({ field: "HandpieceStatus", operator: "eq", value: value });
                                }
                            };
                            _this._handpieceStatus.resetValueDelegate = function () { return "All"; };
                            _this._parts = new DevGuild.Filters.Grids.DropDownListGridFilterField($("#PartsOutOfStockFilter"), "PartsOutOfStock", { value: function (a) { return a.Value; }, defaultValue: "" });
                            _this._parts.applyValueDelegate = function (filters, value) {
                                switch (value.toString()) {
                                    case "InStock":
                                        filters.push({ field: "PartsOutOfStock", operator: "eq", value: 0 });
                                        break;
                                    case "OutOfStock":
                                        filters.push({ field: "PartsOutOfStock", operator: "eq", value: 1 });
                                        break;
                                    case "PartialStock":
                                        filters.push({ field: "PartsOutOfStock", operator: "eq", value: 2 });
                                        break;
                                }
                            };
                            _this.subscribeEvents();
                            return _this;
                        }
                        JobsGridFilterFieldsCollection.prototype.applyAll = function (filters, exceptions) {
                            this._workshop.apply(filters, exceptions);
                            this._jobType.apply(filters, exceptions);
                            this._jobNumber.apply(filters, exceptions);
                            this._client.apply(filters, exceptions);
                            this._receivedFrom.apply(filters, exceptions);
                            this._receivedTo.apply(filters, exceptions);
                            this._serial.apply(filters, exceptions);
                            this._makeAndModel.apply(filters, exceptions);
                            this._type.apply(filters, exceptions);
                            this._jobStatus.apply(filters, exceptions);
                            this._handpieceStatus.apply(filters, exceptions);
                            this._parts.apply(filters, exceptions);
                        };
                        JobsGridFilterFieldsCollection.prototype.resetAll = function () {
                            this._workshop.reset();
                            this._jobType.reset();
                            this._jobNumber.reset();
                            this._client.reset();
                            this._receivedFrom.reset();
                            this._receivedTo.reset();
                            this._serial.reset();
                            this._makeAndModel.reset();
                            this._type.reset();
                            this._jobStatus.reset();
                            this._handpieceStatus.reset();
                            this._parts.reset();
                        };
                        JobsGridFilterFieldsCollection.prototype.subscribeEvents = function () {
                            var _this = this;
                            this._workshop.kendoControl.bind("change", function (e) {
                                JobsGridFilter.instance.apply();
                            });
                            this._jobType.kendoControl.bind("change", function (e) {
                                JobsGridFilter.instance.apply();
                            });
                            this._jobNumber.control.on("keypress", function (e) {
                                if (e.which === 13) {
                                    JobsGridFilter.instance.apply();
                                }
                            });
                            this._jobNumber.control.on("keyup", function (e) {
                                if (e.which === 27) {
                                    _this._jobNumber.control.val("");
                                    JobsGridFilter.instance.apply();
                                }
                            });
                            this._serial.control.on("keypress", function (e) {
                                if (e.which === 13) {
                                    JobsGridFilter.instance.apply();
                                }
                            });
                            this._serial.control.on("keyup", function (e) {
                                if (e.which === 27) {
                                    _this._serial.control.val("");
                                    JobsGridFilter.instance.apply();
                                }
                            });
                            this._jobStatus.kendoControl.bind("change", function (e) {
                                JobsGridFilter.instance.apply();
                            });
                            this._client.kendoControl.bind("change", function (e) {
                                JobsGridFilter.instance.apply();
                            });
                            this._makeAndModel.kendoControl.bind("change", function (e) {
                                JobsGridFilter.instance.apply();
                            });
                            this._handpieceStatus.kendoControl.bind("change", function (e) {
                                JobsGridFilter.instance.apply();
                            });
                            this._receivedFrom.kendoControl.bind("change", function (e) {
                                JobsGridFilter.instance.apply();
                            });
                            this._receivedFrom.control.on("keyup", function (e) {
                                if (e.which === 27) {
                                    _this._receivedFrom.kendoControl.value("");
                                    _this._receivedFrom.control.val("").trigger("change");
                                    JobsGridFilter.instance.apply();
                                }
                            });
                            this._receivedTo.kendoControl.bind("change", function (e) {
                                JobsGridFilter.instance.apply();
                            });
                            this._receivedTo.control.on("keyup", function (e) {
                                if (e.which === 27) {
                                    _this._receivedTo.kendoControl.value("");
                                    _this._receivedTo.control.val("").trigger("change");
                                    JobsGridFilter.instance.apply();
                                }
                            });
                            this._type.kendoControl.bind("change", function (e) {
                                JobsGridFilter.instance.apply();
                            });
                            this._parts.kendoControl.bind("change", function (e) {
                                JobsGridFilter.instance.apply();
                            });
                        };
                        return JobsGridFilterFieldsCollection;
                    }(DevGuild.Filters.Grids.GridFilterFieldsCollection));
                    Index.JobsGridFilterFieldsCollection = JobsGridFilterFieldsCollection;
                    var JobsGridFilter = /** @class */ (function (_super) {
                        __extends(JobsGridFilter, _super);
                        function JobsGridFilter(root) {
                            var _this = _super.call(this, root) || this;
                            _this.initialize();
                            return _this;
                        }
                        Object.defineProperty(JobsGridFilter, "instance", {
                            get: function () {
                                if (!JobsGridFilter._instance) {
                                    JobsGridFilter._instance = new JobsGridFilter($(".filters__jobs"));
                                }
                                return JobsGridFilter._instance;
                            },
                            enumerable: false,
                            configurable: true
                        });
                        JobsGridFilter.prototype.createFields = function (root) {
                            return new JobsGridFilterFieldsCollection(this.root);
                        };
                        JobsGridFilter.prototype.applyFilter = function (filters) {
                            var filteredFields = this.getFilteredFields(filters);
                            if (filteredFields.some(function (x) { return x === "JobNumberString"; }) ||
                                filteredFields.some(function (x) { return x === "Brand"; }) ||
                                filteredFields.some(function (x) { return x === "MakeAndModel"; }) ||
                                filteredFields.some(function (x) { return x === "Serial"; }) ||
                                filteredFields.some(function (x) { return x === "SpeedType"; }) ||
                                filteredFields.some(function (x) { return x === "HandpieceStatus"; }) ||
                                filteredFields.some(function (x) { return x === "PartsOutOfStock"; })) {
                                JobsHandpiecesGrid.instance.dataSource._query({ filter: filters, page: 1 }).then(function () {
                                    JobsGridsSwitcher.instance.switchToHandpieces();
                                });
                            }
                            else {
                                JobsGrid.instance.dataSource._query({ filter: filters, page: 1 }).then(function () {
                                    JobsGridsSwitcher.instance.switchToJobs();
                                });
                            }
                        };
                        JobsGridFilter.prototype.getFilteredFields = function (filter) {
                            if (filter.filters) {
                                var filtersList = filter;
                                var result = [];
                                for (var i = 0; i < filtersList.filters.length; i++) {
                                    var subResult = this.getFilteredFields(filtersList.filters[i]);
                                    for (var j = 0; j < subResult.length; j++) {
                                        if (result.indexOf(subResult[j]) < 0) {
                                            result.push(subResult[j]);
                                        }
                                    }
                                }
                                return result;
                            }
                            else if (filter.field) {
                                var filterItem = filter;
                                return [filterItem.field];
                            }
                            else {
                                return [];
                            }
                        };
                        return JobsGridFilter;
                    }(DevGuild.Filters.Grids.GridFilterCore));
                    Index.JobsGridFilter = JobsGridFilter;
                    var JobsGridsSwitcherMode;
                    (function (JobsGridsSwitcherMode) {
                        JobsGridsSwitcherMode[JobsGridsSwitcherMode["JobsGrid"] = 0] = "JobsGrid";
                        JobsGridsSwitcherMode[JobsGridsSwitcherMode["HandpiecesGrid"] = 1] = "HandpiecesGrid";
                    })(JobsGridsSwitcherMode = Index.JobsGridsSwitcherMode || (Index.JobsGridsSwitcherMode = {}));
                    var JobsGridsSwitcher = /** @class */ (function () {
                        function JobsGridsSwitcher(root) {
                            this._jobsGrid = new Collapsible(document.querySelector(".grid-switcher__jobs"));
                            this._handpiecesGrid = new Collapsible(document.querySelector(".grid-switcher__handpieces"));
                        }
                        Object.defineProperty(JobsGridsSwitcher, "instance", {
                            get: function () {
                                if (JobsGridsSwitcher._instance === undefined) {
                                    JobsGridsSwitcher._instance = new JobsGridsSwitcher(document.querySelector("body"));
                                }
                                return JobsGridsSwitcher._instance;
                            },
                            enumerable: false,
                            configurable: true
                        });
                        JobsGridsSwitcher.prototype.isJobsShown = function () {
                            return this._jobsGrid.isShown();
                        };
                        JobsGridsSwitcher.prototype.isHandpiecesShown = function () {
                            return this._handpiecesGrid.isShown();
                        };
                        JobsGridsSwitcher.prototype.switchToJobs = function () {
                            return __awaiter(this, void 0, void 0, function () {
                                return __generator(this, function (_a) {
                                    switch (_a.label) {
                                        case 0:
                                            if (this._jobsGrid.isShown()) {
                                                return [2 /*return*/];
                                            }
                                            return [4 /*yield*/, this._handpiecesGrid.hideAsync()];
                                        case 1:
                                            _a.sent();
                                            return [4 /*yield*/, this._jobsGrid.showAsync()];
                                        case 2:
                                            _a.sent();
                                            setTimeout(function () {
                                                GridResizer.resize(JobsGrid.instance);
                                            }, 0);
                                            return [2 /*return*/];
                                    }
                                });
                            });
                        };
                        JobsGridsSwitcher.prototype.switchToHandpieces = function () {
                            return __awaiter(this, void 0, void 0, function () {
                                return __generator(this, function (_a) {
                                    switch (_a.label) {
                                        case 0:
                                            if (this._handpiecesGrid.isShown()) {
                                                return [2 /*return*/];
                                            }
                                            return [4 /*yield*/, this._jobsGrid.hideAsync()];
                                        case 1:
                                            _a.sent();
                                            return [4 /*yield*/, this._handpiecesGrid.showAsync()];
                                        case 2:
                                            _a.sent();
                                            setTimeout(function () {
                                                GridResizer.resize(JobsHandpiecesGrid.instance);
                                            }, 0);
                                            return [2 /*return*/];
                                    }
                                });
                            });
                        };
                        return JobsGridsSwitcher;
                    }());
                    Index.JobsGridsSwitcher = JobsGridsSwitcher;
                    var JobsHandpiecesGrid = /** @class */ (function () {
                        function JobsHandpiecesGrid() {
                        }
                        Object.defineProperty(JobsHandpiecesGrid, "instance", {
                            get: function () {
                                return $("#JobsHandpiecesGrid").data("kendoGrid");
                            },
                            enumerable: false,
                            configurable: true
                        });
                        JobsHandpiecesGrid.handleDataBound = function (e) {
                            e.sender.element.find("[data-toggle='tooltip']").tooltip();
                        };
                        var _a;
                        _a = JobsHandpiecesGrid;
                        JobsHandpiecesGrid.handleDetails = GridHandlers.createButtonClickNavigationHandler(function (item) { return routes.handpieces.details(item.Id); });
                        JobsHandpiecesGrid.handleEdit = GridHandlers.createButtonClickNavigationHandler(function (item) { return routes.handpieces.edit(item.Id); });
                        JobsHandpiecesGrid.handleDelete = GridHandlers.createButtonClickPopupHandler(function (item) { return routes.handpieces.delete(item.Id); }, function (item) { return ({
                            title: "Delete Handpiece " + item.Serial,
                            width: "1000px",
                            height: "auto",
                            refresh: function (e) {
                                e.sender.wrapper.on("click", ".editor__submit__cancel", function (clickEvent) {
                                    clickEvent.preventDefault();
                                    e.sender.close();
                                    e.sender.destroy();
                                });
                                e.sender.center();
                            },
                            open: function (e) { return __awaiter(_a, void 0, void 0, function () {
                                return __generator(_a, function (_b) {
                                    switch (_b.label) {
                                        case 0: return [4 /*yield*/, AjaxFormsManager.waitFor("HandpiecesDelete")];
                                        case 1:
                                            _b.sent();
                                            return [4 /*yield*/, JobsHandpiecesGrid.instance.dataSource.read()];
                                        case 2:
                                            _b.sent();
                                            e.sender.close();
                                            e.sender.destroy();
                                            return [2 /*return*/];
                                    }
                                });
                            }); }
                        }); });
                        JobsHandpiecesGrid.handleCreateEstimate = GridHandlers.createGridButtonClickPopupHandler("#JobsHandpiecesGrid .k-grid-CustomCreateEstimate", function (target) { return routes.jobs.create("Estimate"); }, function (target) { return ({
                            title: "Create Estimate",
                            width: "1000px",
                            height: "auto",
                            refresh: function (e) {
                                e.sender.wrapper.on("click", ".editor__submit__cancel", function (clickEvent) {
                                    clickEvent.preventDefault();
                                    e.sender.close();
                                    e.sender.destroy();
                                });
                                e.sender.center();
                            },
                            open: function (e) { return __awaiter(_a, void 0, void 0, function () {
                                return __generator(_a, function (_b) {
                                    switch (_b.label) {
                                        case 0: return [4 /*yield*/, AjaxFormsManager.waitFor("JobsCreate")];
                                        case 1:
                                            _b.sent();
                                            return [4 /*yield*/, JobsGrid.instance.dataSource.read()];
                                        case 2:
                                            _b.sent();
                                            e.sender.close();
                                            e.sender.destroy();
                                            return [2 /*return*/];
                                    }
                                });
                            }); }
                        }); });
                        JobsHandpiecesGrid.handleCreateSale = GridHandlers.createGridButtonClickPopupHandler("#JobsHandpiecesGrid .k-grid-CustomCreateSale", function (target) { return routes.jobs.create("Sale"); }, function (target) { return ({
                            title: "Create Sale",
                            width: "1000px",
                            height: "auto",
                            refresh: function (e) {
                                e.sender.wrapper.on("click", ".editor__submit__cancel", function (clickEvent) {
                                    clickEvent.preventDefault();
                                    e.sender.close();
                                    e.sender.destroy();
                                });
                                e.sender.center();
                            },
                            open: function (e) { return __awaiter(_a, void 0, void 0, function () {
                                return __generator(_a, function (_b) {
                                    switch (_b.label) {
                                        case 0: return [4 /*yield*/, AjaxFormsManager.waitFor("JobsCreate")];
                                        case 1:
                                            _b.sent();
                                            return [4 /*yield*/, JobsGrid.instance.dataSource.read()];
                                        case 2:
                                            _b.sent();
                                            e.sender.close();
                                            e.sender.destroy();
                                            return [2 /*return*/];
                                    }
                                });
                            }); }
                        }); });
                        return JobsHandpiecesGrid;
                    }());
                    Index.JobsHandpiecesGrid = JobsHandpiecesGrid;
                    var JobsGrid = /** @class */ (function () {
                        function JobsGrid() {
                        }
                        Object.defineProperty(JobsGrid, "instance", {
                            get: function () {
                                return $("#JobsGrid").data("kendoGrid");
                            },
                            enumerable: false,
                            configurable: true
                        });
                        JobsGrid.renderStatusIndicator = function (data) {
                            var config = data.JobStatusConfig.split(";").map(function (x) { return parseInt(x); });
                            var indicator = new HandpieceStatusIndicator();
                            var indicatorValue = Math.abs(config[0]);
                            indicator.value = indicatorValue;
                            indicator.danger = config[0] < 0;
                            for (var i = 1; i <= 7; i++) {
                                indicator.setOverride(i, config[i] > 0 && i < indicatorValue);
                                indicator.setCount(i, i < indicatorValue ? config[i] : 0);
                            }
                            return indicator.render().outerHTML;
                        };
                        var _b;
                        _b = JobsGrid;
                        JobsGrid.handleEdit = GridHandlers.createButtonClickNavigationHandler(function (item) { return routes.jobs.edit(item.Id); });
                        JobsGrid.handleCreateEstimate = GridHandlers.createGridButtonClickPopupHandler("#JobsGrid .k-grid-CustomCreateEstimate", function (target) { return routes.jobs.create("Estimate"); }, function (target) { return ({
                            title: "Create Estimate",
                            width: "1000px",
                            height: "auto",
                            refresh: function (e) {
                                e.sender.wrapper.on("click", ".editor__submit__cancel", function (clickEvent) {
                                    clickEvent.preventDefault();
                                    e.sender.close();
                                    e.sender.destroy();
                                });
                                e.sender.center();
                            },
                            open: function (e) { return __awaiter(_b, void 0, void 0, function () {
                                return __generator(_b, function (_c) {
                                    switch (_c.label) {
                                        case 0: return [4 /*yield*/, AjaxFormsManager.waitFor("JobsCreate")];
                                        case 1:
                                            _c.sent();
                                            return [4 /*yield*/, JobsGrid.instance.dataSource.read()];
                                        case 2:
                                            _c.sent();
                                            e.sender.close();
                                            e.sender.destroy();
                                            return [2 /*return*/];
                                    }
                                });
                            }); }
                        }); });
                        JobsGrid.handleCreateSale = GridHandlers.createGridButtonClickPopupHandler("#JobsGrid .k-grid-CustomCreateSale", function (target) { return routes.jobs.create("Sale"); }, function (target) { return ({
                            title: "Create Sale",
                            width: "1000px",
                            height: "auto",
                            refresh: function (e) {
                                e.sender.wrapper.on("click", ".editor__submit__cancel", function (clickEvent) {
                                    clickEvent.preventDefault();
                                    e.sender.close();
                                    e.sender.destroy();
                                });
                                e.sender.center();
                            },
                            open: function (e) { return __awaiter(_b, void 0, void 0, function () {
                                return __generator(_b, function (_c) {
                                    switch (_c.label) {
                                        case 0: return [4 /*yield*/, AjaxFormsManager.waitFor("JobsCreate")];
                                        case 1:
                                            _c.sent();
                                            return [4 /*yield*/, JobsGrid.instance.dataSource.read()];
                                        case 2:
                                            _c.sent();
                                            e.sender.close();
                                            e.sender.destroy();
                                            return [2 /*return*/];
                                    }
                                });
                            }); }
                        }); });
                        return JobsGrid;
                    }());
                    Index.JobsGrid = JobsGrid;
                    var GridResizer = /** @class */ (function () {
                        function GridResizer() {
                        }
                        GridResizer.resize = function (grid) {
                            if (grid) {
                                grid.setOptions({ height: "100px" });
                                grid.resize();
                                grid.setOptions({ height: "100%" });
                                grid.resize();
                            }
                        };
                        return GridResizer;
                    }());
                    Index.GridResizer = GridResizer;
                    $(function () {
                        JobsGrid.instance.autoResizeWhen(function () { return JobsGridsSwitcher.instance.isJobsShown(); });
                        JobsHandpiecesGrid.instance.autoResizeWhen(function () { return JobsGridsSwitcher.instance.isHandpiecesShown(); });
                        var filters = JobsGridFilter.instance;
                        ////    $(window).on("resize", e => {
                        ////        if (JobsGridsSwitcher.instance.isJobsShown()) {
                        ////            const gridInstance = JobsGrid.instance;
                        ////            GridResizer.resize(gridInstance);
                        ////        }
                        ////        else if (JobsGridsSwitcher.instance.isHandpiecesShown()) {
                        ////            const gridInstance = $(document.querySelector("#JobsHandpiecesGrid")).data("kendoGrid");
                        ////            GridResizer.resize(gridInstance);
                        ////        }
                        ////    });
                    });
                })(Index = Jobs.Index || (Jobs.Index = {}));
            })(Jobs = Pages.Jobs || (Pages.Jobs = {}));
        })(Pages = CRM.Pages || (CRM.Pages = {}));
    })(CRM = DentalDrill.CRM || (DentalDrill.CRM = {}));
})(DentalDrill || (DentalDrill = {}));
//# sourceMappingURL=index.js.map