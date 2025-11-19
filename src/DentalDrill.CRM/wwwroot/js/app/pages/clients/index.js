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
            var Clients;
            (function (Clients) {
                var Index;
                (function (Index) {
                    var GridHandlers = DevGuild.AspNet.Controls.Grids.Handlers.GridHandlers;
                    var ClientsGrid = /** @class */ (function () {
                        function ClientsGrid() {
                        }
                        Object.defineProperty(ClientsGrid, "instance", {
                            get: function () {
                                return $("#ClientsGrid").data("kendoGrid");
                            },
                            enumerable: false,
                            configurable: true
                        });
                        ClientsGrid.handleDataBound = function (e) {
                            e.sender.element.find("[data-toggle='tooltip']").tooltip();
                        };
                        var _a;
                        _a = ClientsGrid;
                        ClientsGrid.handleDetailsClick = GridHandlers.createButtonClickNavigationHandler(function (item) { return routes.clients.details(item.Id); });
                        ClientsGrid.handleEditClick = GridHandlers.createButtonClickPopupHandler(function (item) { return routes.clients.edit(item.Id); }, function (item) { return ({
                            title: "Edit Client " + item.Name,
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
                                        case 0: return [4 /*yield*/, DevGuild.AspNet.Forms.AjaxForms.AjaxFormsManager.waitFor("ClientEdit")];
                                        case 1:
                                            _b.sent();
                                            return [4 /*yield*/, ClientsGrid.instance.dataSource.read()];
                                        case 2:
                                            _b.sent();
                                            e.sender.close();
                                            e.sender.destroy();
                                            return [2 /*return*/];
                                    }
                                });
                            }); }
                        }); });
                        ClientsGrid.handleDeleteClick = GridHandlers.createButtonClickPopupHandler(function (item) { return routes.clients.delete(item.Id); }, function (item) { return ({
                            title: "Delete Client " + item.Name,
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
                                        case 0: return [4 /*yield*/, DevGuild.AspNet.Forms.AjaxForms.AjaxFormsManager.waitFor("ClientDelete")];
                                        case 1:
                                            _b.sent();
                                            return [4 /*yield*/, ClientsGrid.instance.dataSource.read()];
                                        case 2:
                                            _b.sent();
                                            e.sender.close();
                                            e.sender.destroy();
                                            return [2 /*return*/];
                                    }
                                });
                            }); }
                        }); });
                        ClientsGrid.handleCreate = GridHandlers.createGridButtonClickPopupHandler("#ClientsGrid .k-grid-CustomCreate", function (target) { return new DevGuild.AspNet.Routing.Uri(target.attr("href")); }, function (target) { return ({
                            title: "Create Client",
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
                            open: function () {
                                return __awaiter(this, void 0, void 0, function () {
                                    return __generator(this, function (_b) {
                                        switch (_b.label) {
                                            case 0: return [4 /*yield*/, DevGuild.AspNet.Forms.AjaxForms.AjaxFormsManager.waitFor("CreateClients")];
                                            case 1:
                                                _b.sent();
                                                return [4 /*yield*/, ClientsGrid.instance.dataSource.read()];
                                            case 2:
                                                _b.sent();
                                                this.close();
                                                this.destroy();
                                                return [2 /*return*/];
                                        }
                                    });
                                });
                            }
                        }); });
                        return ClientsGrid;
                    }());
                    Index.ClientsGrid = ClientsGrid;
                    var ClientsGridFilterFieldsCollection = /** @class */ (function (_super) {
                        __extends(ClientsGridFilterFieldsCollection, _super);
                        //private readonly _due: DevGuild.Filters.Grids.DatePickerFilterField;
                        function ClientsGridFilterFieldsCollection(root) {
                            var _this = _super.call(this, root) || this;
                            _this._serial = new DevGuild.Filters.Grids.StringInputGridFilterField($("#SerialFilter"), "serial");
                            _this._makeAndModel = new DevGuild.Filters.Grids.StringInputGridFilterField($("#ModelFilter"), "model");
                            _this._received = new DevGuild.Filters.Grids.DatePickerFilterField($("#ReceivedFilter"), "received");
                            return _this;
                            //this._due = new DevGuild.Filters.Grids.DatePickerFilterField($("#DueFilter"), "Due");
                        }
                        ClientsGridFilterFieldsCollection.prototype.applyAll = function (filters, exceptions) {
                            this._serial.apply(filters, exceptions);
                            this._makeAndModel.apply(filters, exceptions);
                            this._received.apply(filters, exceptions);
                            //this._due.apply(filters, exceptions);
                        };
                        ClientsGridFilterFieldsCollection.prototype.resetAll = function () {
                            this._serial.reset();
                            this._makeAndModel.reset();
                            this._received.reset();
                            //this._due.reset();
                        };
                        return ClientsGridFilterFieldsCollection;
                    }(DevGuild.Filters.Grids.GridFilterFieldsCollection));
                    Index.ClientsGridFilterFieldsCollection = ClientsGridFilterFieldsCollection;
                    var ClientsGridFilter = /** @class */ (function (_super) {
                        __extends(ClientsGridFilter, _super);
                        function ClientsGridFilter(root) {
                            var _this = _super.call(this, root) || this;
                            _this.initialize();
                            return _this;
                        }
                        Object.defineProperty(ClientsGridFilter, "instance", {
                            get: function () {
                                if (!ClientsGridFilter._instance) {
                                    ClientsGridFilter._instance = new ClientsGridFilter($("body"));
                                }
                                return ClientsGridFilter._instance;
                            },
                            enumerable: false,
                            configurable: true
                        });
                        ClientsGridFilter.prototype.createFields = function (root) {
                            return new ClientsGridFilterFieldsCollection(this.root);
                        };
                        ClientsGridFilter.prototype.applyFilter = function (filters) {
                            var gridDataSource = ClientsGrid.instance.dataSource;
                            gridDataSource.filter(filters);
                            gridDataSource.read();
                        };
                        return ClientsGridFilter;
                    }(DevGuild.Filters.Grids.GridFilterCore));
                    Index.ClientsGridFilter = ClientsGridFilter;
                    var ClientsFilters = /** @class */ (function () {
                        function ClientsFilters() {
                        }
                        ClientsFilters.clickSearch = function () {
                            //ClientsGridFilter.instance.apply();
                            ClientsGrid.instance.dataSource.read();
                        };
                        ClientsFilters.clickCancel = function () {
                            ClientsGridFilter.instance.reset();
                        };
                        return ClientsFilters;
                    }());
                    Index.ClientsFilters = ClientsFilters;
                })(Index = Clients.Index || (Clients.Index = {}));
            })(Clients = Pages.Clients || (Pages.Clients = {}));
        })(Pages = CRM.Pages || (CRM.Pages = {}));
    })(CRM = DentalDrill.CRM || (DentalDrill.CRM = {}));
})(DentalDrill || (DentalDrill = {}));
//# sourceMappingURL=index.js.map