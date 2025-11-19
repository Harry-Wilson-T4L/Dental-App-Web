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
            var HandpieceRequiredParts;
            (function (HandpieceRequiredParts) {
                var Index;
                (function (Index) {
                    var GridHandlers = DevGuild.AspNet.Controls.Grids.Handlers.GridHandlers;
                    var AjaxFormsManager = DevGuild.AspNet.Forms.AjaxForms.AjaxFormsManager;
                    var EventHandler = DevGuild.Utilities.EventHandler;
                    var HandpieceRequiredPartStatus;
                    (function (HandpieceRequiredPartStatus) {
                        HandpieceRequiredPartStatus[HandpieceRequiredPartStatus["Unknown"] = 0] = "Unknown";
                        HandpieceRequiredPartStatus[HandpieceRequiredPartStatus["Waiting"] = 1] = "Waiting";
                        HandpieceRequiredPartStatus[HandpieceRequiredPartStatus["WaitingRequested"] = 2] = "WaitingRequested";
                        HandpieceRequiredPartStatus[HandpieceRequiredPartStatus["WaitingApproved"] = 3] = "WaitingApproved";
                        HandpieceRequiredPartStatus[HandpieceRequiredPartStatus["WaitingOrdered"] = 4] = "WaitingOrdered";
                        HandpieceRequiredPartStatus[HandpieceRequiredPartStatus["Allocated"] = 5] = "Allocated";
                        HandpieceRequiredPartStatus[HandpieceRequiredPartStatus["Completed"] = 6] = "Completed";
                        HandpieceRequiredPartStatus[HandpieceRequiredPartStatus["Cancelled"] = 7] = "Cancelled";
                    })(HandpieceRequiredPartStatus = Index.HandpieceRequiredPartStatus || (Index.HandpieceRequiredPartStatus = {}));
                    var HandpieceRequiredPartStatusHelper = /** @class */ (function () {
                        function HandpieceRequiredPartStatusHelper() {
                        }
                        HandpieceRequiredPartStatusHelper.toDisplayName = function (value) {
                            switch (value) {
                                case HandpieceRequiredPartStatus.Unknown:
                                    return "Unknown";
                                case HandpieceRequiredPartStatus.Waiting:
                                    return "Waiting";
                                case HandpieceRequiredPartStatus.WaitingRequested:
                                    return "Requested";
                                case HandpieceRequiredPartStatus.WaitingApproved:
                                    return "Approved";
                                case HandpieceRequiredPartStatus.WaitingOrdered:
                                    return "Ordered";
                                case HandpieceRequiredPartStatus.Allocated:
                                    return "Allocated";
                                case HandpieceRequiredPartStatus.Completed:
                                    return "Completed";
                                case HandpieceRequiredPartStatus.Cancelled:
                                    return "Cancelled";
                                default:
                                    return HandpieceRequiredPartStatus[value];
                            }
                        };
                        HandpieceRequiredPartStatusHelper.toDisplayColor = function (value) {
                            switch (value) {
                                case HandpieceRequiredPartStatus.Unknown:
                                    return "background-color: #ced3db; color: #212529;";
                                case HandpieceRequiredPartStatus.Waiting:
                                    return "background-color: #ffc107; color: #212529;";
                                case HandpieceRequiredPartStatus.WaitingRequested:
                                    return "background-color: #ffc107; color: #212529;";
                                case HandpieceRequiredPartStatus.WaitingApproved:
                                    return "background-color: #ffc107; color: #212529;";
                                case HandpieceRequiredPartStatus.WaitingOrdered:
                                    return "background-color: #ffc107; color: #212529;";
                                case HandpieceRequiredPartStatus.Allocated:
                                    return "background-color: #28a745; color: white;";
                                case HandpieceRequiredPartStatus.Completed:
                                    return "background-color: #28a745; color: white;";
                                case HandpieceRequiredPartStatus.Cancelled:
                                    return "background-color: #ced3db; color: #212529;";
                                default:
                                    return "background-color: #ced3db; color: #212529;";
                            }
                        };
                        return HandpieceRequiredPartStatusHelper;
                    }());
                    Index.HandpieceRequiredPartStatusHelper = HandpieceRequiredPartStatusHelper;
                    var HandpieceRequiredPartIndexPage = /** @class */ (function () {
                        function HandpieceRequiredPartIndexPage(root, handpieceId) {
                            this._root = root;
                            this._rootNode = $(root);
                            this._handpieceId = handpieceId;
                        }
                        HandpieceRequiredPartIndexPage.prototype.init = function () {
                            var _this = this;
                            var gridWrapper = this._rootNode.find(".handpiece-required-parts__grid-wrapper");
                            this._grid = new HandpieceRequiredPartGrid(this._handpieceId, gridWrapper);
                            this._grid.init();
                            AjaxFormsManager.htmlEvents.subscribe("HandpieceRequiredPartsCreate", function (e) { return __awaiter(_this, void 0, void 0, function () {
                                return __generator(this, function (_a) {
                                    switch (_a.label) {
                                        case 0:
                                            if (!(e.Success === true)) return [3 /*break*/, 2];
                                            return [4 /*yield*/, this._grid.refresh()];
                                        case 1:
                                            _a.sent();
                                            _a.label = 2;
                                        case 2: return [2 /*return*/];
                                    }
                                });
                            }); });
                        };
                        return HandpieceRequiredPartIndexPage;
                    }());
                    Index.HandpieceRequiredPartIndexPage = HandpieceRequiredPartIndexPage;
                    var HandpieceRequiredPartGrid = /** @class */ (function () {
                        function HandpieceRequiredPartGrid(handpieceId, rootNode) {
                            this._updated = new EventHandler();
                            this._handpieceId = handpieceId;
                            this._rootNode = rootNode;
                        }
                        Object.defineProperty(HandpieceRequiredPartGrid.prototype, "updated", {
                            get: function () {
                                return this._updated;
                            },
                            enumerable: false,
                            configurable: true
                        });
                        HandpieceRequiredPartGrid.prototype.init = function () {
                            this._rootNode.addClass("k-grid--dense");
                            this._rootNode.addClass("k-grid--small-text");
                            this._rootNode.kendoGrid({
                                dataSource: this.createDataSource(),
                                columns: this.initializeColumns(),
                            });
                            this._grid = this._rootNode.data("kendoGrid");
                        };
                        HandpieceRequiredPartGrid.prototype.refresh = function () {
                            return __awaiter(this, void 0, void 0, function () {
                                var data;
                                return __generator(this, function (_a) {
                                    switch (_a.label) {
                                        case 0: return [4 /*yield*/, this._grid.dataSource.read()];
                                        case 1:
                                            _a.sent();
                                            data = this._grid.dataSource.data();
                                            this._updated.raise(this, data.map(function (x) { return x; }));
                                            return [2 /*return*/];
                                    }
                                });
                            });
                        };
                        HandpieceRequiredPartGrid.prototype.createDataSource = function () {
                            var dataSource = new kendo.data.DataSource({
                                type: "aspnetmvc-ajax",
                                transport: {
                                    read: {
                                        url: "/HandpieceRequiredParts/Read?parentId=" + this._handpieceId
                                    },
                                },
                                schema: {
                                    data: "Data",
                                    total: "Total",
                                    errors: "Errors",
                                    model: {
                                        id: "Id",
                                        fields: {
                                            "Date": { type: "Date" },
                                            "Status": { type: "number" },
                                            "SKUId": { type: "string" },
                                            "SKUName": { type: "string" },
                                            "RequiredQuantity": { type: "number" },
                                            "ShelfQuantity": { type: "number" },
                                            "Price": { type: "number" },
                                        },
                                    },
                                },
                                serverAggregates: true,
                                serverFiltering: true,
                                serverGrouping: true,
                                serverPaging: true,
                                serverSorting: true,
                            });
                            return dataSource;
                        };
                        HandpieceRequiredPartGrid.prototype.initializeColumns = function () {
                            var _this = this;
                            var columns = [];
                            columns.push({
                                title: "Date",
                                field: "Date",
                                width: "50px",
                                format: "{0:d}",
                            });
                            columns.push({
                                title: "Status",
                                field: "Status",
                                width: "50px",
                                template: function (data) {
                                    return "<span class=\"badge\" style=\"" + HandpieceRequiredPartStatusHelper.toDisplayColor(data.Status) + "\">" + HandpieceRequiredPartStatusHelper.toDisplayName(data.Status) + "</span>";
                                },
                            });
                            columns.push({
                                title: "SKU",
                                field: "SKUName",
                                width: "150px",
                            });
                            columns.push({
                                title: "QTY",
                                field: "RequiredQuantity",
                                width: "40px",
                                template: function (data) {
                                    return data.RequiredQuantity + "/" + data.ShelfQuantity;
                                }
                            });
                            columns.push({
                                title: "Price",
                                field: "Price",
                                width: "40px",
                                template: function (data) {
                                    if (data.Price !== undefined && data.Price !== null) {
                                        return "$" + data.Price;
                                    }
                                    else {
                                        return "";
                                    }
                                }
                            });
                            columns.push({
                                title: "Actions",
                                width: "120px",
                                command: [
                                    {
                                        name: "CustomOpenSKU",
                                        iconClass: "fas fa-link",
                                        text: "&nbsp; Movements",
                                        click: function (e) {
                                            e.preventDefault();
                                            var dataItem = _this._grid.dataItem(e.currentTarget.closest("tr"));
                                            var url = new DevGuild.AspNet.Routing.Uri("/InventoryMovements?sku=" + dataItem.SKUId);
                                            url.open();
                                        },
                                    },
                                    {
                                        name: "CustomSwapAllocation",
                                        iconClass: "fas fa-exchange-alt",
                                        text: "&nbsp; Swap",
                                        click: GridHandlers.createButtonClickPopupHandler(function (item) {
                                            switch (item.Status) {
                                                case HandpieceRequiredPartStatus.Allocated:
                                                    return new DevGuild.AspNet.Routing.Uri("/HandpieceRequiredParts/DeallocateFrom?handpiece=" + _this._handpieceId + "&sku=" + item.SKUId);
                                                case HandpieceRequiredPartStatus.Waiting:
                                                case HandpieceRequiredPartStatus.WaitingRequested:
                                                case HandpieceRequiredPartStatus.WaitingApproved:
                                                case HandpieceRequiredPartStatus.WaitingOrdered:
                                                    return new DevGuild.AspNet.Routing.Uri("/HandpieceRequiredParts/AllocateTo?handpiece=" + _this._handpieceId + "&sku=" + item.SKUId);
                                                default:
                                                    return undefined;
                                            }
                                        }, function (item) {
                                            var formId = undefined;
                                            switch (item.Status) {
                                                case HandpieceRequiredPartStatus.Allocated:
                                                    formId = "HandpieceRequiredPartsDeallocateFrom";
                                                    break;
                                                case HandpieceRequiredPartStatus.Waiting:
                                                case HandpieceRequiredPartStatus.WaitingRequested:
                                                case HandpieceRequiredPartStatus.WaitingApproved:
                                                case HandpieceRequiredPartStatus.WaitingOrdered:
                                                    formId = "HandpieceRequiredPartsAllocateTo";
                                                    break;
                                            }
                                            return {
                                                title: "Swap",
                                                width: "900px",
                                                height: "auto",
                                                refresh: function (e) {
                                                    e.sender.wrapper.on("click", ".editor__submit__cancel", function (clickEvent) {
                                                        clickEvent.preventDefault();
                                                        e.sender.close();
                                                        e.sender.destroy();
                                                    });
                                                    e.sender.center();
                                                },
                                                open: function (e) { return __awaiter(_this, void 0, void 0, function () {
                                                    return __generator(this, function (_a) {
                                                        switch (_a.label) {
                                                            case 0: return [4 /*yield*/, AjaxFormsManager.waitFor(formId)];
                                                            case 1:
                                                                _a.sent();
                                                                return [4 /*yield*/, this.refresh()];
                                                            case 2:
                                                                _a.sent();
                                                                e.sender.close();
                                                                e.sender.destroy();
                                                                return [2 /*return*/];
                                                        }
                                                    });
                                                }); },
                                            };
                                        }),
                                    }
                                ]
                            });
                            return columns;
                        };
                        return HandpieceRequiredPartGrid;
                    }());
                    Index.HandpieceRequiredPartGrid = HandpieceRequiredPartGrid;
                })(Index = HandpieceRequiredParts.Index || (HandpieceRequiredParts.Index = {}));
            })(HandpieceRequiredParts = Pages.HandpieceRequiredParts || (Pages.HandpieceRequiredParts = {}));
        })(Pages = CRM.Pages || (CRM.Pages = {}));
    })(CRM = DentalDrill.CRM || (DentalDrill.CRM = {}));
})(DentalDrill || (DentalDrill = {}));
//# sourceMappingURL=index.js.map