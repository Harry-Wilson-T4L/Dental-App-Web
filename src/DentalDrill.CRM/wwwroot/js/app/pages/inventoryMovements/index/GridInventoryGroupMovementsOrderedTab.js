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
                    var GridInventoryGroupMovementsOrderedTab = /** @class */ (function (_super) {
                        __extends(GridInventoryGroupMovementsOrderedTab, _super);
                        function GridInventoryGroupMovementsOrderedTab(id, root, options, api) {
                            var _this = _super.call(this, id, root, options) || this;
                            _this._api = api;
                            return _this;
                        }
                        GridInventoryGroupMovementsOrderedTab.prototype.getTabName = function () {
                            return "Ordered";
                        };
                        GridInventoryGroupMovementsOrderedTab.prototype.getEndpointUrl = function () {
                            return "/InventoryMovements/ReadGroupOrdered";
                        };
                        GridInventoryGroupMovementsOrderedTab.prototype.getColumnsConfig = function () {
                            var config = _super.prototype.getColumnsConfig.call(this);
                            config.QuantityAbsolute = config.Quantity;
                            config.Quantity = 0;
                            config.TotalPriceAbsolute = config.TotalPrice;
                            config.TotalPrice = 0;
                            config.Actions += config.Type;
                            config.Actions += config.Status;
                            config.Type = 0;
                            config.Status = 0;
                            config.OrderedQuantity = 0;
                            return config;
                        };
                        GridInventoryGroupMovementsOrderedTab.prototype.initializeCommands = function () {
                            var _this = this;
                            var commands = [];
                            commands.push({
                                name: "CustomOpenJob",
                                iconClass: "fas fa-link",
                                text: "&nbsp; Jobs",
                                click: function (e) {
                                    e.preventDefault();
                                    var data = _this.grid.dataItem(e.target.closest("tr"));
                                    var url = new DevGuild.AspNet.Routing.Uri("/InventoryMovements?workshop=" + _this.workshopId + "&sku=" + data.SKUId + "&tab=" + _this.getTabName() + "&group=false");
                                    url.navigate();
                                }
                            });
                            commands.push({
                                name: "CustomVerify",
                                className: this.workshopId === "" ? "k-state-disabled" : undefined,
                                iconClass: "far fa-check-circle",
                                text: "&nbsp; Verify",
                                click: GridHandlers.createButtonClickPopupHandler(function (item) { return new DevGuild.AspNet.Routing.Uri("/InventoryMovements/GroupVerify?workshop=" + _this.workshopId + "&sku=" + item.SKUId); }, function (item) { return ({
                                    title: "Verify movements",
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
                                    open: function (e) { return __awaiter(_this, void 0, void 0, function () {
                                        return __generator(this, function (_a) {
                                            switch (_a.label) {
                                                case 0: return [4 /*yield*/, AjaxFormsManager.waitFor("InventoryMovementsGroupVerify")];
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
                                name: "CustomVerifyWithEdit",
                                className: this.workshopId === "" ? "k-state-disabled" : undefined,
                                iconClass: "fas fa-check-circle",
                                text: "&nbsp; Verify with Edit",
                                click: GridHandlers.createButtonClickPopupHandler(function (item) { return new DevGuild.AspNet.Routing.Uri("/InventoryMovements/GroupVerifyWithEdit?workshop=" + _this.workshopId + "&sku=" + item.SKUId); }, function (item) { return ({
                                    title: "Verify movements with edit",
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
                                    open: function (e) { return __awaiter(_this, void 0, void 0, function () {
                                        return __generator(this, function (_a) {
                                            switch (_a.label) {
                                                case 0: return [4 /*yield*/, AjaxFormsManager.waitFor("InventoryMovementsGroupVerifyWithEdit")];
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
                                name: "CustomCancel",
                                className: this.workshopId === "" ? "k-state-disabled" : undefined,
                                iconClass: "fas fa-ban",
                                text: "&nbsp; Cancel",
                                click: GridHandlers.createButtonClickPopupHandler(function (item) { return new DevGuild.AspNet.Routing.Uri("/InventoryMovements/GroupCancel?workshop=" + _this.workshopId + "&sku=" + item.SKUId + "&status=Ordered"); }, function (item) { return ({
                                    title: "Cancel movements",
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
                                    open: function (e) { return __awaiter(_this, void 0, void 0, function () {
                                        return __generator(this, function (_a) {
                                            switch (_a.label) {
                                                case 0: return [4 /*yield*/, AjaxFormsManager.waitFor("InventoryMovementsGroupCancel")];
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
                            return commands;
                        };
                        return GridInventoryGroupMovementsOrderedTab;
                    }(Index.GridInventoryGroupMovementsTabBase));
                    Index.GridInventoryGroupMovementsOrderedTab = GridInventoryGroupMovementsOrderedTab;
                })(Index = InventoryMovements.Index || (InventoryMovements.Index = {}));
            })(InventoryMovements = Pages.InventoryMovements || (Pages.InventoryMovements = {}));
        })(Pages = CRM.Pages || (CRM.Pages = {}));
    })(CRM = DentalDrill.CRM || (DentalDrill.CRM = {}));
})(DentalDrill || (DentalDrill = {}));
//# sourceMappingURL=GridInventoryGroupMovementsOrderedTab.js.map