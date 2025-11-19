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
            var InventoryMovements;
            (function (InventoryMovements) {
                var Index;
                (function (Index) {
                    var GridHandlers = DevGuild.AspNet.Controls.Grids.Handlers.GridHandlers;
                    var AjaxFormsManager = DevGuild.AspNet.Forms.AjaxForms.AjaxFormsManager;
                    var InventoryMovementType = InventoryMovements.Shared.InventoryMovementType;
                    var GridInventoryMovementsApprovedTab = /** @class */ (function (_super) {
                        __extends(GridInventoryMovementsApprovedTab, _super);
                        function GridInventoryMovementsApprovedTab(id, root, options, api) {
                            var _this = _super.call(this, id, root, options) || this;
                            _this._showWarnings = false;
                            _this._api = api;
                            return _this;
                        }
                        GridInventoryMovementsApprovedTab.prototype.getEndpointUrl = function () {
                            return this._showWarnings
                                ? "/InventoryMovements/ReadApprovedAndMissing"
                                : "/InventoryMovements/ReadApproved";
                        };
                        GridInventoryMovementsApprovedTab.prototype.getColumnsConfig = function () {
                            var config = _super.prototype.getColumnsConfig.call(this);
                            config.QuantityAbsolute = config.Quantity;
                            config.Quantity = 0;
                            config.TotalPriceAbsolute = config.TotalPrice;
                            config.TotalPrice = 0;
                            config.Actions += config.Type;
                            config.Actions += config.Status;
                            config.Type = 0;
                            config.Status = 0;
                            return config;
                        };
                        GridInventoryMovementsApprovedTab.prototype.initializeGrid = function (grid) {
                            var _this = this;
                            _super.prototype.initializeGrid.call(this, grid);
                            var nameHeader = grid.wrapper.find("th[data-field=SKUName]")[0];
                            var checkboxWrapper = nameHeader.querySelector(".inventory-movements-show-warnings");
                            if (!checkboxWrapper) {
                                checkboxWrapper = nameHeader.appendChild(document.createElement("div"));
                                checkboxWrapper.classList.add("inventory-movements-show-warnings");
                                var label = checkboxWrapper.appendChild(document.createElement("label"));
                                label.classList.add("inventory-movements-show-warnings__label");
                                label.addEventListener("click", function (e) { return e.stopPropagation(); });
                                var checkbox = label.appendChild(document.createElement("input"));
                                checkbox.type = "checkbox";
                                checkbox.classList.add("inventory-movements-show-warnings__checkbox", "k-checkbox");
                                ;
                                checkbox.addEventListener("click", function (e) { return e.stopPropagation(); });
                                checkbox.addEventListener("change", function (e) {
                                    e.stopPropagation();
                                    _this._showWarnings = !_this._showWarnings;
                                    _this.grid.dataSource["transport"].options.read.url = _this.getEndpointUrl();
                                    _this.grid.dataSource.read();
                                });
                                label.appendChild(document.createTextNode("Show Warnings"));
                            }
                        };
                        GridInventoryMovementsApprovedTab.prototype.initializeRow = function (row, dataItem) {
                            _super.prototype.initializeRow.call(this, row, dataItem);
                            var cancelButton = row.querySelector("a.k-grid-CustomCancel");
                            if (dataItem.Type === InventoryMovementType.EphemeralMissingRequiredQuantity) {
                                cancelButton.classList.add("k-state-disabled");
                            }
                            else {
                                cancelButton.classList.remove("k-state-disabled");
                            }
                        };
                        GridInventoryMovementsApprovedTab.prototype.initializeCommands = function () {
                            var _this = this;
                            var commands = [];
                            commands.push({
                                name: "CustomOpenJob",
                                iconClass: "fas fa-link",
                                text: "&nbsp; Job",
                                click: function (e) {
                                    e.preventDefault();
                                    var dataItem = _this.grid.dataItem(e.currentTarget.closest("tr"));
                                    if (!dataItem) {
                                        return;
                                    }
                                    if (dataItem.HandpieceId) {
                                        var url = routes.handpieces.edit(dataItem.HandpieceId);
                                        url.open();
                                    }
                                }
                            });
                            commands.push({
                                name: "CustomOrder",
                                className: this.workshopId === "" ? "k-state-disabled" : undefined,
                                iconClass: "far fa-check-circle",
                                text: "&nbsp; Order",
                                click: GridHandlers.createButtonClickPopupHandler(function (item) {
                                    if (item.Type === InventoryMovementType.EphemeralMissingRequiredQuantity) {
                                        return new DevGuild.AspNet.Routing.Uri("/InventoryMovements/OrderMissing?workshop=" + _this.workshopId + "&sku=" + item.Id);
                                    }
                                    else {
                                        return new DevGuild.AspNet.Routing.Uri("/InventoryMovements/Order/" + item.Id);
                                    }
                                }, function (item) { return ({
                                    title: "Order Movement",
                                    width: "800px",
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
                                                case 0:
                                                    if (!(item.Type === InventoryMovementType.EphemeralMissingRequiredQuantity)) return [3 /*break*/, 2];
                                                    return [4 /*yield*/, AjaxFormsManager.waitFor("InventoryMovementsOrderMissing")];
                                                case 1:
                                                    _a.sent();
                                                    return [3 /*break*/, 4];
                                                case 2: return [4 /*yield*/, AjaxFormsManager.waitFor("InventoryMovementsOrder")];
                                                case 3:
                                                    _a.sent();
                                                    _a.label = 4;
                                                case 4: return [4 /*yield*/, this.grid.dataSource.read()];
                                                case 5:
                                                    _a.sent();
                                                    e.sender.close();
                                                    e.sender.destroy();
                                                    return [2 /*return*/];
                                            }
                                        });
                                    }); },
                                }); }),
                            });
                            commands.push({
                                name: "CustomOrderWithEdit",
                                className: this.workshopId === "" ? "k-state-disabled" : undefined,
                                iconClass: "fas fa-check-circle",
                                text: "&nbsp; Order with Edit",
                                click: GridHandlers.createButtonClickPopupHandler(function (item) {
                                    if (item.Type === InventoryMovementType.EphemeralMissingRequiredQuantity) {
                                        return new DevGuild.AspNet.Routing.Uri("/InventoryMovements/OrderMissingWithEdit?workshop=" + _this.workshopId + "&sku=" + item.Id);
                                    }
                                    else {
                                        return new DevGuild.AspNet.Routing.Uri("/InventoryMovements/OrderWithEdit/" + item.Id);
                                    }
                                }, function (item) { return ({
                                    title: "Order Movement",
                                    width: "800px",
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
                                                case 0:
                                                    if (!(item.Type === InventoryMovementType.EphemeralMissingRequiredQuantity)) return [3 /*break*/, 2];
                                                    return [4 /*yield*/, AjaxFormsManager.waitFor("InventoryMovementsOrderMissingWithEdit")];
                                                case 1:
                                                    _a.sent();
                                                    return [3 /*break*/, 4];
                                                case 2: return [4 /*yield*/, AjaxFormsManager.waitFor("InventoryMovementsOrderWithEdit")];
                                                case 3:
                                                    _a.sent();
                                                    _a.label = 4;
                                                case 4: return [4 /*yield*/, this.grid.dataSource.read()];
                                                case 5:
                                                    _a.sent();
                                                    e.sender.close();
                                                    e.sender.destroy();
                                                    return [2 /*return*/];
                                            }
                                        });
                                    }); },
                                }); }),
                            });
                            commands.push({
                                name: "CustomCancel",
                                className: this.workshopId === "" ? "k-state-disabled" : undefined,
                                iconClass: "fas fa-ban",
                                text: "&nbsp; Cancel",
                                click: GridHandlers.createButtonClickPopupHandler(function (item) { return new DevGuild.AspNet.Routing.Uri("/InventoryMovements/Cancel/" + item.Id); }, function (item) { return ({
                                    title: "Cancel Movement",
                                    width: "800px",
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
                                                case 0: return [4 /*yield*/, AjaxFormsManager.waitFor("InventoryMovementsCancel")];
                                                case 1:
                                                    _a.sent();
                                                    return [4 /*yield*/, this.grid.dataSource.read()];
                                                case 2:
                                                    _a.sent();
                                                    e.sender.close();
                                                    e.sender.destroy();
                                                    return [2 /*return*/];
                                            }
                                        });
                                    }); },
                                }); }),
                            });
                            commands.push({
                                name: "CustomHistory",
                                iconClass: "fas fa-history",
                                text: "&nbsp;",
                                click: GridHandlers.createButtonClickPopupHandler(function (item) { return new DevGuild.AspNet.Routing.Uri("/InventoryMovements/History/" + item.Id); }, function (item) { return ({
                                    title: "Move history",
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
                                }); }),
                            });
                            return commands;
                        };
                        return GridInventoryMovementsApprovedTab;
                    }(Index.GridInventoryMovementsTabBase));
                    Index.GridInventoryMovementsApprovedTab = GridInventoryMovementsApprovedTab;
                })(Index = InventoryMovements.Index || (InventoryMovements.Index = {}));
            })(InventoryMovements = Pages.InventoryMovements || (Pages.InventoryMovements = {}));
        })(Pages = CRM.Pages || (CRM.Pages = {}));
    })(CRM = DentalDrill.CRM || (DentalDrill.CRM = {}));
})(DentalDrill || (DentalDrill = {}));
//# sourceMappingURL=GridInventoryMovementsApprovedTab.js.map