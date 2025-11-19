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
                var Index;
                (function (Index) {
                    var ExternalHandpieceStatus;
                    (function (ExternalHandpieceStatus) {
                        ExternalHandpieceStatus[ExternalHandpieceStatus["None"] = 0] = "None";
                        ExternalHandpieceStatus[ExternalHandpieceStatus["Received"] = 1] = "Received";
                        ExternalHandpieceStatus[ExternalHandpieceStatus["BeingEstimated"] = 2] = "BeingEstimated";
                        ExternalHandpieceStatus[ExternalHandpieceStatus["WaitingForApproval"] = 3] = "WaitingForApproval";
                        ExternalHandpieceStatus[ExternalHandpieceStatus["EstimateSent"] = 4] = "EstimateSent";
                        ExternalHandpieceStatus[ExternalHandpieceStatus["BeingRepaired"] = 5] = "BeingRepaired";
                        ExternalHandpieceStatus[ExternalHandpieceStatus["ReadyForReturn"] = 6] = "ReadyForReturn";
                        ExternalHandpieceStatus[ExternalHandpieceStatus["SentComplete"] = 7] = "SentComplete";
                        ExternalHandpieceStatus[ExternalHandpieceStatus["Cancel"] = 8] = "Cancel";
                    })(ExternalHandpieceStatus = Index.ExternalHandpieceStatus || (Index.ExternalHandpieceStatus = {}));
                    var JobNumberGridFilterField = /** @class */ (function (_super) {
                        __extends(JobNumberGridFilterField, _super);
                        function JobNumberGridFilterField(root, fieldName) {
                            var _this = _super.call(this) || this;
                            _this._root = root;
                            _this._fieldName = fieldName;
                            return _this;
                        }
                        JobNumberGridFilterField.prototype.apply = function (filters, exceptions) {
                            var _this = this;
                            var value = this._root.val();
                            if (!value) {
                                return;
                            }
                            var regex = new RegExp("^(\\d+)($|[-A-Za-z][-A-Za-z0-9]*$)");
                            var match = regex.exec(value.toString());
                            if (!match || !match[1]) {
                                return;
                            }
                            var jobNumber = match[1];
                            if ((!exceptions || exceptions.every(function (x) { return x !== _this._fieldName; }))) {
                                filters.push({
                                    field: this._fieldName,
                                    operator: "eq",
                                    value: jobNumber
                                });
                            }
                        };
                        JobNumberGridFilterField.prototype.reset = function () {
                            this._root.val("").trigger("change");
                        };
                        Object.defineProperty(JobNumberGridFilterField.prototype, "control", {
                            get: function () {
                                return this._root;
                            },
                            enumerable: false,
                            configurable: true
                        });
                        return JobNumberGridFilterField;
                    }(DevGuild.Filters.Grids.GridFilterField));
                    Index.JobNumberGridFilterField = JobNumberGridFilterField;
                    var StatusRepairsGridFilterFieldsCollection = /** @class */ (function (_super) {
                        __extends(StatusRepairsGridFilterFieldsCollection, _super);
                        function StatusRepairsGridFilterFieldsCollection(root) {
                            var _this = _super.call(this, root) || this;
                            _this._jobNumber = new JobNumberGridFilterField($("#JobNumberFilter"), "JobNumber");
                            _this._makeAndModel = new DevGuild.Filters.Grids.DropDownListGridFilterField($("#MakeAndModelFilter"), "MakeAndModel", { value: function (a) { return a.Value; }, defaultValue: "" });
                            _this._makeAndModel.applyValueDelegate = function (filters, value) {
                                var split = value.split("||");
                                filters.push({
                                    field: "Brand",
                                    operator: "eq",
                                    value: split[0]
                                });
                                filters.push({
                                    field: "MakeAndModel",
                                    operator: "eq",
                                    value: split[1]
                                });
                            };
                            _this._serial = new DevGuild.Filters.Grids.StringInputGridFilterField($("#SerialFilter"), "Serial");
                            _this._speedType = new DevGuild.Filters.Grids.DropDownListGridFilterField($("#SpeedTypeFilter"), "SpeedType", { value: function (a) { return a.Value; }, defaultValue: 0 });
                            _this._speedType.resetValueDelegate = function () { return ""; };
                            _this._receivedFrom = new DevGuild.Filters.Grids.DatePickerFilterField($("#ReceivedFromFilter"), "Received", "gte");
                            _this._receivedTo = new DevGuild.Filters.Grids.DatePickerFilterField($("#ReceivedToFilter"), "Received", "lte");
                            return _this;
                        }
                        StatusRepairsGridFilterFieldsCollection.prototype.applyAll = function (filters, exceptions) {
                            this._jobNumber.apply(filters, exceptions);
                            this._makeAndModel.apply(filters, exceptions);
                            this._serial.apply(filters, exceptions);
                            this._speedType.apply(filters, exceptions);
                            this._receivedFrom.apply(filters, exceptions);
                            this._receivedTo.apply(filters, exceptions);
                        };
                        StatusRepairsGridFilterFieldsCollection.prototype.resetAll = function () {
                            this._jobNumber.reset();
                            this._makeAndModel.reset();
                            this._serial.reset();
                            this._speedType.reset();
                            this._receivedFrom.reset();
                            this._receivedTo.reset();
                        };
                        return StatusRepairsGridFilterFieldsCollection;
                    }(DevGuild.Filters.Grids.GridFilterFieldsCollection));
                    Index.StatusRepairsGridFilterFieldsCollection = StatusRepairsGridFilterFieldsCollection;
                    var RepairsGridFilter = /** @class */ (function (_super) {
                        __extends(RepairsGridFilter, _super);
                        function RepairsGridFilter(page) {
                            var _this = _super.call(this, page.root) || this;
                            _this._page = page;
                            return _this;
                        }
                        RepairsGridFilter.prototype.createFields = function (root) {
                            return new StatusRepairsGridFilterFieldsCollection(this.root);
                        };
                        RepairsGridFilter.prototype.applyFilter = function (filters) {
                            this._page.updateDataSource(filters);
                            var makeAndModel = $("#MakeAndModelFilter").data("kendoDropDownList").value() || "";
                            MaintenanceHandpiecesListView.setFiltersData({
                                JobNumber: $("#JobNumberFilter").data("kendoAutoComplete").value(),
                                Brand: makeAndModel.split("||")[0],
                                MakeAndModel: makeAndModel.split("||")[1],
                                Serial: $("#SerialFilter").data("kendoAutoComplete").value(),
                                SpeedType: $("#SpeedTypeFilter").data("kendoDropDownList").value(),
                                ReceivedFrom: $("#ReceivedFromFilter").data("kendoDatePicker").value(),
                                ReceivedTo: $("#ReceivedToFilter").data("kendoDatePicker").value()
                            });
                        };
                        return RepairsGridFilter;
                    }(DevGuild.Filters.Grids.GridFilterCore));
                    Index.RepairsGridFilter = RepairsGridFilter;
                    var TotalsRowItem = /** @class */ (function () {
                        function TotalsRowItem(root) {
                            this._root = root;
                            this._valueNode = root.find(".totals-row__value");
                            this._status = ExternalHandpieceStatus[root.attr("data-status")];
                        }
                        Object.defineProperty(TotalsRowItem.prototype, "value", {
                            get: function () {
                                return parseInt(this._valueNode.text());
                            },
                            set: function (val) {
                                this._valueNode.text(val.toString());
                            },
                            enumerable: false,
                            configurable: true
                        });
                        Object.defineProperty(TotalsRowItem.prototype, "status", {
                            get: function () {
                                return this._status;
                            },
                            enumerable: false,
                            configurable: true
                        });
                        return TotalsRowItem;
                    }());
                    Index.TotalsRowItem = TotalsRowItem;
                    var TotalsRow = /** @class */ (function () {
                        function TotalsRow(root) {
                            var _this = this;
                            this._items = [];
                            this._itemsMap = new Map();
                            this._root = root;
                            this._root.find(".totals-row__item").each(function (i, e) {
                                var item = new TotalsRowItem($(e));
                                _this._itemsMap.set(item.status, item);
                                _this._items.push(item);
                            });
                        }
                        TotalsRow.prototype.setValue = function (status, value) {
                            this._itemsMap.get(status).value = value;
                        };
                        TotalsRow.prototype.list = function () {
                            var result = [];
                            for (var i = 0; i < this._items.length; i++) {
                                result.push(this._items[i]);
                            }
                            return result;
                        };
                        return TotalsRow;
                    }());
                    Index.TotalsRow = TotalsRow;
                    var SurgeryStatusGrid = /** @class */ (function () {
                        function SurgeryStatusGrid(page, host) {
                            this._page = page;
                            this._host = host;
                            this._dataSource = this.createDataSource();
                            this._grid = this.createGrid();
                        }
                        SurgeryStatusGrid.prototype.setTotal = function (status, value) {
                            var nodes = this._host.find(".surgery-status-grid__status-counter[data-status=" + status + "]");
                            nodes.text(value.toString());
                        };
                        SurgeryStatusGrid.prototype.expandPresent = function () {
                            var _this = this;
                            var nodes = this._host.find(".surgery-status-grid__status-counter");
                            nodes.each(function (i, x) {
                                var node = $(x);
                                var nodeText = node.text();
                                if (nodeText && nodeText !== "0" && nodeText !== "-") {
                                    _this._grid.expandRow(node.closest("tr"));
                                }
                            });
                        };
                        SurgeryStatusGrid.prototype.createDataSource = function () {
                            return new kendo.data.DataSource({
                                data: [
                                    { Status: ExternalHandpieceStatus.Received, Name: "Received", StatusVisualisationNumber: 1 },
                                    { Status: ExternalHandpieceStatus.BeingEstimated, Name: "Being Estimated", StatusVisualisationNumber: 2 },
                                    { Status: ExternalHandpieceStatus.WaitingForApproval, Name: "Estimate Complete", StatusVisualisationNumber: 3 },
                                    { Status: ExternalHandpieceStatus.EstimateSent, Name: "Estimate sent", StatusVisualisationNumber: 4 },
                                    { Status: ExternalHandpieceStatus.BeingRepaired, Name: "Being Repaired", StatusVisualisationNumber: 5 },
                                    { Status: ExternalHandpieceStatus.ReadyForReturn, Name: "Ready for Return", StatusVisualisationNumber: 6 },
                                    { Status: ExternalHandpieceStatus.Cancel, Name: "Unrepaired", StatusVisualisationNumber: -7 },
                                ]
                            });
                        };
                        SurgeryStatusGrid.prototype.createGrid = function () {
                            var _this = this;
                            this._host.kendoGrid({
                                columns: [
                                    {
                                        title: "Name",
                                        field: "Name",
                                        template: "#:Name# <strong>(<span class=\"surgery-status-grid__status-counter\" data-status=\"#: Status#\">-</span>)</strong>"
                                    },
                                    {
                                        title: " ",
                                        field: "StatusVisualisationNumber",
                                        template: function (x) { return "<div class=\"handpiece-status-indicator\" style=\"width: 150px\" data-max=\"7\" data-value=\"" + Math.abs(x.StatusVisualisationNumber) + "\" data-danger=\"" + (x.StatusVisualisationNumber < 0 ? "True" : "False") + "#\">\n  <div class=\"progress handpiece-status-indicator__progress\">\n    <div class=\"progress-bar\"></div>\n  </div>\n  <div class=\"handpiece-status-indicator__points\">\n    <div class=\"handpiece-status-indicator__points__point\"></div>\n    <div class=\"handpiece-status-indicator__points__point\"></div>\n    <div class=\"handpiece-status-indicator__points__point\"></div>\n    <div class=\"handpiece-status-indicator__points__point\"></div>\n    <div class=\"handpiece-status-indicator__points__point\"></div>\n    <div class=\"handpiece-status-indicator__points__point\"></div>\n    <div class=\"handpiece-status-indicator__points__point\"></div>\n  </div>\n</div>"; }
                                    }
                                ],
                                columnMenu: false,
                                scrollable: false,
                                dataSource: this._dataSource,
                                detailTemplate: "<div class=\"surgery-status-grid__list-view\"></div> <div class=\"surgery-status-grid__list-view__pager\"></div>",
                                detailInit: function (e) {
                                    var status = e.data["Status"];
                                    var dataSource = _this._page.getStatusDataSource(status);
                                    e.detailRow.find(".surgery-status-grid__list-view").kendoListView({
                                        dataSource: dataSource,
                                        template: kendo.template($("#surgery-repair-template").html()),
                                        dataBound: function (dataBound) {
                                            e.detailRow.find(".surgery__repair__title[data-toggle=\"tooltip\"]").tooltip();
                                            e.detailRow.find(".surgery__repair__info[data-toggle=\"tooltip\"]").tooltip();
                                        }
                                    });
                                    e.detailRow.find(".surgery-status-grid__list-view__pager").kendoPager({
                                        pageSizes: [10, 25, 50, 100],
                                        buttonCount: 5,
                                        dataSource: dataSource
                                    });
                                },
                                detailExpand: function (e) {
                                    e.detailRow.find(".surgery-status-grid__list-view").data("kendoListView").dataSource.read();
                                }
                            });
                            return this._host.data("kendoGrid");
                        };
                        return SurgeryStatusGrid;
                    }());
                    Index.SurgeryStatusGrid = SurgeryStatusGrid;
                    var SurgeryPage = /** @class */ (function () {
                        function SurgeryPage(root) {
                            var _this = this;
                            this._statusSourceMap = new Map();
                            this._root = root;
                            this._id = root.attr("data-id");
                            this._path = root.attr("data-path");
                            this._handpiecesSource = this.createDataSource();
                            // Creating totals wrapper
                            this._totalsRow = new TotalsRow(root.find(".totals-row"));
                            // Creating filters wrapper
                            this._filters = new RepairsGridFilter(this);
                            this._root.find(".surgery-filters__search").on("click", function (e) {
                                _this._filters.apply();
                            });
                            this._root.find(".surgery-filters__clear").on("click", function (e) {
                                _this._filters.reset();
                            });
                            // Creating status grid
                            this._statusGrid = new SurgeryStatusGrid(this, this._root.find(".surgery-page__status-grid"));
                            this._root.data("SurgeryPage", this);
                        }
                        Object.defineProperty(SurgeryPage.prototype, "root", {
                            get: function () {
                                return this._root;
                            },
                            enumerable: false,
                            configurable: true
                        });
                        SurgeryPage.prototype.initialize = function () {
                            return __awaiter(this, void 0, void 0, function () {
                                return __generator(this, function (_a) {
                                    switch (_a.label) {
                                        case 0: return [4 /*yield*/, this.updateDataSource(undefined)];
                                        case 1:
                                            _a.sent();
                                            this._statusGrid.expandPresent();
                                            return [2 /*return*/];
                                    }
                                });
                            });
                        };
                        SurgeryPage.prototype.updateDataSource = function (filters) {
                            return __awaiter(this, void 0, void 0, function () {
                                var data, dataByStatus;
                                var _this = this;
                                return __generator(this, function (_a) {
                                    switch (_a.label) {
                                        case 0: return [4 /*yield*/, this._handpiecesSource.query({ filter: filters })];
                                        case 1:
                                            _a.sent();
                                            data = this._handpiecesSource.data();
                                            dataByStatus = data.map(function (x, i) { return x; })
                                                .reduce(function (previous, current) {
                                                if (previous[ExternalHandpieceStatus[current.Status]]) {
                                                    previous[ExternalHandpieceStatus[current.Status]].push(current);
                                                }
                                                else {
                                                    previous[ExternalHandpieceStatus[current.Status]] = [current];
                                                }
                                                return previous;
                                            }, {});
                                            this._totalsRow.list().forEach(function (item) {
                                                var group = dataByStatus[item.status];
                                                if (group) {
                                                    item.value = group.length;
                                                    _this._statusGrid.setTotal(item.status, group.length);
                                                    _this.setStatusData(item.status, group);
                                                }
                                                else {
                                                    item.value = 0;
                                                    _this._statusGrid.setTotal(item.status, 0);
                                                    _this.setStatusData(item.status, []);
                                                }
                                            });
                                            return [2 /*return*/];
                                    }
                                });
                            });
                        };
                        SurgeryPage.prototype.getStatusDataSource = function (status) {
                            return this._statusSourceMap.get(status);
                        };
                        SurgeryPage.prototype.setStatusData = function (status, data) {
                            if (this._statusSourceMap.has(status)) {
                                var dataSource = this._statusSourceMap.get(status);
                                var existing = dataSource["localData"];
                                existing.splice(0, existing.length);
                                for (var _i = 0, data_1 = data; _i < data_1.length; _i++) {
                                    var item = data_1[_i];
                                    existing.push(item);
                                }
                                dataSource.read();
                            }
                            else {
                                var dataSource = new kendo.data.DataSource({
                                    transport: {
                                        read: function (e) { return e.success(data); },
                                    },
                                    schema: {
                                        model: {
                                            id: "Id",
                                            fields: {
                                                Id: { type: "string" },
                                                JobNumber: { type: "string" },
                                                Brand: { type: "string" },
                                                MakeAndModel: { type: "string" },
                                                Serial: { type: "string" },
                                                Status: { type: "string" },
                                                Rating: { type: "number" },
                                                SpeedType: { type: "string" },
                                                Received: { type: "date" },
                                                ImageUrl: { type: "string" }
                                            }
                                        }
                                    },
                                    pageSize: 10
                                });
                                dataSource["localData"] = data;
                                this._statusSourceMap.set(status, dataSource);
                                dataSource.read();
                            }
                        };
                        SurgeryPage.prototype.createDataSource = function () {
                            return new kendo.data.DataSource({
                                type: "aspnetmvc-ajax",
                                transport: {
                                    read: {
                                        url: "/Surgeries/" + this._path + "/Handpieces"
                                    }
                                },
                                serverPaging: true,
                                serverSorting: true,
                                serverFiltering: true,
                                serverGrouping: true,
                                serverAggregates: true,
                                filter: [],
                                schema: {
                                    data: "Data",
                                    total: "Total",
                                    errors: "Errors",
                                    model: {
                                        id: "Id",
                                        fields: {
                                            Id: { type: "string" },
                                            JobNumber: { type: "string" },
                                            Brand: { type: "string" },
                                            MakeAndModel: { type: "string" },
                                            Serial: { type: "string" },
                                            Status: { type: "string" },
                                            Rating: { type: "number" },
                                            SpeedType: { type: "string" },
                                            Received: { type: "date" },
                                            ImageUrl: { type: "string" }
                                        }
                                    }
                                }
                            });
                            ;
                        };
                        return SurgeryPage;
                    }());
                    Index.SurgeryPage = SurgeryPage;
                    $(function () {
                        var page = new SurgeryPage($(".surgery-page"));
                        page.initialize();
                    });
                    var StatusRepairsGrid = /** @class */ (function () {
                        function StatusRepairsGrid() {
                        }
                        StatusRepairsGrid.handleDetailsClick = function (client, id) {
                            var url = routes.surgeries.handpiece(client, id);
                            var options = {
                                title: "Repair details"
                            };
                            var dialogRoot = $("<div></div>");
                            var dialogOptions = {
                                title: "",
                                actions: ["close"],
                                content: url.value,
                                width: "80%",
                                maxWidth: 1000,
                                //height: "500px",
                                height: "auto",
                                modal: true,
                                visible: false,
                                close: function () { return dialogRoot.data("kendoWindow").destroy(); },
                                refresh: function () {
                                    dialog.center();
                                }
                            };
                            $.extend(dialogOptions, options);
                            dialogRoot.kendoWindow(dialogOptions);
                            var dialog = dialogRoot.data("kendoWindow");
                            dialog.center();
                            dialog.open();
                        };
                        return StatusRepairsGrid;
                    }());
                    Index.StatusRepairsGrid = StatusRepairsGrid;
                    $(function () {
                        $(document).on("click", ".surgery__repair", function (e) {
                            var target = e.currentTarget;
                            var clientId = target.getAttribute("data-client-id");
                            var id = target.getAttribute("data-id");
                            if (clientId && id) {
                                StatusRepairsGrid.handleDetailsClick(clientId, id);
                            }
                        });
                    });
                    var MaintenanceHandpiecesListView = /** @class */ (function () {
                        function MaintenanceHandpiecesListView() {
                        }
                        Object.defineProperty(MaintenanceHandpiecesListView, "instance", {
                            get: function () {
                                return $("#MaintenanceHandpiecesListView").data("kendoListView");
                            },
                            enumerable: false,
                            configurable: true
                        });
                        MaintenanceHandpiecesListView.handleDataBound = function (e) {
                            return __awaiter(this, void 0, void 0, function () {
                                return __generator(this, function (_a) {
                                    $('.surgery-maintenance-handpiece__title[data-toggle="tooltip"]').tooltip();
                                    $('.surgery-maintenance-handpiece__info__serial[data-toggle="tooltip"]').tooltip();
                                    return [2 /*return*/];
                                });
                            });
                        };
                        MaintenanceHandpiecesListView.handleChange = function (e) {
                            return __awaiter(this, void 0, void 0, function () {
                                var selected;
                                return __generator(this, function (_a) {
                                    switch (_a.label) {
                                        case 0:
                                            selected = MaintenanceHandpiecesListView.selectedItem();
                                            if (selected === undefined) {
                                                return [2 /*return*/];
                                            }
                                            if (!selected) return [3 /*break*/, 2];
                                            MaintenanceHandpiecesListView._lastSelected = selected.Id;
                                            return [4 /*yield*/, MaintenanceHistory.loadHistory(selected)];
                                        case 1:
                                            _a.sent();
                                            return [3 /*break*/, 4];
                                        case 2:
                                            if (!(!selected && MaintenanceHandpiecesListView._lastSelected)) return [3 /*break*/, 4];
                                            MaintenanceHandpiecesListView._lastSelected = undefined;
                                            return [4 /*yield*/, MaintenanceHistory.clearHistory()];
                                        case 3:
                                            _a.sent();
                                            _a.label = 4;
                                        case 4: return [2 /*return*/];
                                    }
                                });
                            });
                        };
                        MaintenanceHandpiecesListView.setFiltersData = function (data) {
                            return __awaiter(this, void 0, void 0, function () {
                                var listView, data_2, i;
                                return __generator(this, function (_a) {
                                    switch (_a.label) {
                                        case 0:
                                            MaintenanceHandpiecesListView._filtersData = data;
                                            return [4 /*yield*/, MaintenanceHandpiecesListView.instance.dataSource.read()];
                                        case 1:
                                            _a.sent();
                                            if (MaintenanceHandpiecesListView._lastSelected) {
                                                listView = MaintenanceHandpiecesListView.instance;
                                                data_2 = listView.dataSource.data();
                                                for (i = 0; i < data_2.length; i++) {
                                                    if (data_2[i].Id === MaintenanceHandpiecesListView._lastSelected) {
                                                        listView.select(listView.element.find("[data-uid='" + data_2[i].uid + "']"));
                                                        return [2 /*return*/];
                                                    }
                                                }
                                                MaintenanceHandpiecesListView._lastSelected = undefined;
                                                MaintenanceHistory.clearHistory();
                                            }
                                            return [2 /*return*/];
                                    }
                                });
                            });
                        };
                        MaintenanceHandpiecesListView.filtersData = function () {
                            return MaintenanceHandpiecesListView._filtersData ? MaintenanceHandpiecesListView._filtersData : {};
                        };
                        MaintenanceHandpiecesListView.selectedItem = function () {
                            var listView = MaintenanceHandpiecesListView.instance;
                            var selectedNodes = listView.select();
                            for (var i = 0; i < selectedNodes.length; i++) {
                                var element = selectedNodes[i];
                                return listView.dataItem(element);
                            }
                            return undefined;
                        };
                        return MaintenanceHandpiecesListView;
                    }());
                    Index.MaintenanceHandpiecesListView = MaintenanceHandpiecesListView;
                    var MaintenanceHistory = /** @class */ (function () {
                        function MaintenanceHistory() {
                        }
                        Object.defineProperty(MaintenanceHistory, "container", {
                            get: function () {
                                return MaintenanceHistory._container;
                            },
                            enumerable: false,
                            configurable: true
                        });
                        Object.defineProperty(MaintenanceHistory, "template", {
                            get: function () {
                                if (!MaintenanceHistory._template) {
                                    MaintenanceHistory._template = kendo.template($("#surgery-maintenance__history__template").html());
                                }
                                return MaintenanceHistory._template;
                            },
                            enumerable: false,
                            configurable: true
                        });
                        MaintenanceHistory.loadHistory = function (handpiece) {
                            return __awaiter(this, void 0, void 0, function () {
                                var container, handpieceElement, targetRowLastElement;
                                return __generator(this, function (_a) {
                                    container = MaintenanceHistory.createContainer();
                                    container.html(MaintenanceHistory.template(handpiece));
                                    handpieceElement = $(".surgery-maintenance-handpiece[data-uid=" + handpiece["uid"] + "]");
                                    targetRowLastElement = MaintenanceHistory.getLastElementInRow(handpieceElement);
                                    container.insertAfter(targetRowLastElement);
                                    return [2 /*return*/];
                                });
                            });
                        };
                        MaintenanceHistory.clearHistory = function () {
                            return __awaiter(this, void 0, void 0, function () {
                                return __generator(this, function (_a) {
                                    MaintenanceHistory.removeContainer();
                                    return [2 /*return*/];
                                });
                            });
                        };
                        MaintenanceHistory.createContainer = function () {
                            MaintenanceHistory.removeContainer();
                            MaintenanceHistory._container = $("<summary></summary>");
                            MaintenanceHistory._container.addClass("surgery-maintenance-list-view__history-wrapper");
                            return MaintenanceHistory._container;
                        };
                        MaintenanceHistory.removeContainer = function () {
                            if (MaintenanceHistory._container) {
                                MaintenanceHistory._container.remove();
                                MaintenanceHistory._container = undefined;
                            }
                        };
                        MaintenanceHistory.findElementByCoordinates = function (top) {
                            return $(".surgery-maintenance-list-view").find(".surgery-maintenance-handpiece").filter(function (i, x) { return $(x).offset().top === top; });
                        };
                        MaintenanceHistory.getLastElementInRow = function (element) {
                            var top = $(element).offset().top;
                            var row = MaintenanceHistory.findElementByCoordinates(top);
                            return row[row.length - 1];
                        };
                        return MaintenanceHistory;
                    }());
                    Index.MaintenanceHistory = MaintenanceHistory;
                    var HandpieceImagesCarousel = /** @class */ (function () {
                        function HandpieceImagesCarousel(surgery, id, firstImage) {
                            this._surgery = surgery;
                            this._id = id;
                            this.updateViewPortSize();
                            this._windowWrapper = $("<div></div>");
                            this._windowWrapper.kendoWindow({
                                width: Math.round(this._viewPortSize.width * 0.9) + "px",
                                height: Math.round(this._viewPortSize.height * 0.9) + "px",
                                title: "Handpiece Images",
                                actions: ["close"],
                                modal: true,
                                visible: false,
                                content: "/Surgeries/" + this._surgery + "/Handpiece/" + id + "/Images" + (firstImage ? "?image=" + firstImage : ""),
                                refresh: function (e) {
                                    e.sender.center();
                                }
                            });
                            this._window = this._windowWrapper.data("kendoWindow");
                            this._firstTime = true;
                        }
                        HandpieceImagesCarousel.prototype.show = function (image) {
                            if (!this._firstTime && this.updateViewPortSize()) {
                                this._window.setOptions({
                                    width: Math.round(this._viewPortSize.width * 0.9) + "px",
                                    height: Math.round(this._viewPortSize.height * 0.9) + "px",
                                });
                            }
                            this._window.open();
                            this._window.center();
                            if (this._firstTime) {
                                this._firstTime = false;
                            }
                            else {
                                if (image) {
                                    this.selectPage(image);
                                }
                            }
                        };
                        HandpieceImagesCarousel.prototype.updateViewPortSize = function () {
                            var viewPortSize = {
                                width: $(window).width(),
                                height: $(window).height(),
                            };
                            if (!this._viewPortSize) {
                                this._viewPortSize = viewPortSize;
                                return true;
                            }
                            if (this._viewPortSize.width === viewPortSize.width && this._viewPortSize.height == viewPortSize.height) {
                                return false;
                            }
                            this._viewPortSize = viewPortSize;
                            return true;
                        };
                        HandpieceImagesCarousel.prototype.selectPage = function (id) {
                            var scrollViewWrapper = this._window.wrapper.find(".handpiece-image-carousel");
                            if (scrollViewWrapper.length === 0) {
                                return;
                            }
                            var page = scrollViewWrapper.find(".handpiece-image-carousel__page[data-id='" + id + "']");
                            if (page.length === 0) {
                                return;
                            }
                            try {
                                var pageNo = parseInt(page.attr("data-index"));
                                var scrollView = scrollViewWrapper.data("kendoScrollView");
                                scrollView.scrollTo(pageNo, false);
                            }
                            catch (exception) { }
                        };
                        return HandpieceImagesCarousel;
                    }());
                    Index.HandpieceImagesCarousel = HandpieceImagesCarousel;
                    var HandpieceImagesCarouselManager = /** @class */ (function () {
                        function HandpieceImagesCarouselManager(surgeryId) {
                            this._carousels = new Map();
                            this._surgeryId = surgeryId;
                        }
                        HandpieceImagesCarouselManager.prototype.showCarousel = function (id, image) {
                            var carousel;
                            if (this._carousels.has(id)) {
                                carousel = this._carousels.get(id);
                            }
                            else {
                                carousel = new HandpieceImagesCarousel(this._surgeryId, id, image);
                                this._carousels.set(id, carousel);
                            }
                            carousel.show(image);
                        };
                        return HandpieceImagesCarouselManager;
                    }());
                    Index.HandpieceImagesCarouselManager = HandpieceImagesCarouselManager;
                    $(function () {
                        var surgeryId = $(".surgery-page").attr("data-path");
                        var carouselManager = new HandpieceImagesCarouselManager(surgeryId);
                        $(document).on("click", ".handpiece-image-show-carousel", function (e) {
                            var target = $(e.target);
                            var id = target.attr("data-id");
                            var image = target.attr("data-image");
                            carouselManager.showCarousel(id, image);
                        });
                    });
                })(Index = Surgeries.Index || (Surgeries.Index = {}));
            })(Surgeries = Pages.Surgeries || (Pages.Surgeries = {}));
        })(Pages = CRM.Pages || (CRM.Pages = {}));
    })(CRM = DentalDrill.CRM || (DentalDrill.CRM = {}));
})(DentalDrill || (DentalDrill = {}));
//# sourceMappingURL=index.js.map