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
            var Stock;
            (function (Stock) {
                var Status;
                (function (Status) {
                    var _this = this;
                    var StockControlBeingRepairedGrid = /** @class */ (function () {
                        function StockControlBeingRepairedGrid() {
                        }
                        Object.defineProperty(StockControlBeingRepairedGrid, "instance", {
                            get: function () {
                                return $("#StockControlBeingRepairedGrid").data("kendoGrid");
                            },
                            enumerable: false,
                            configurable: true
                        });
                        StockControlBeingRepairedGrid.handleOpen = DevGuild.AspNet.Controls.Grids.Handlers.GridHandlers.createButtonClickHandler(function (item) {
                            routes.handpieces.edit(item.Id).open();
                        });
                        return StockControlBeingRepairedGrid;
                    }());
                    Status.StockControlBeingRepairedGrid = StockControlBeingRepairedGrid;
                    var StockControlWaitingApprovalGrid = /** @class */ (function () {
                        function StockControlWaitingApprovalGrid() {
                        }
                        Object.defineProperty(StockControlWaitingApprovalGrid, "instance", {
                            get: function () {
                                return $("#StockControlWaitingApprovalGrid").data("kendoGrid");
                            },
                            enumerable: false,
                            configurable: true
                        });
                        StockControlWaitingApprovalGrid.handleOpen = DevGuild.AspNet.Controls.Grids.Handlers.GridHandlers.createButtonClickHandler(function (item) {
                            routes.handpieces.edit(item.Id).open();
                        });
                        return StockControlWaitingApprovalGrid;
                    }());
                    Status.StockControlWaitingApprovalGrid = StockControlWaitingApprovalGrid;
                    var CollapseContainer = /** @class */ (function () {
                        function CollapseContainer(root) {
                            var _this = this;
                            this._root = root;
                            this._toggle = $(this._root.querySelector(".collapse-toggle"));
                            this._content = $(this._root.querySelector(".collapse"));
                            this._chevron = $(this._root.querySelector(".collapse-toggle-chevron"));
                            this._chevronExpandedClass = this._chevron.attr("data-chevron-expanded");
                            this._chevronCollapsedClass = this._chevron.attr("data-chevron-collapsed");
                            this._content.on("show.bs.collapse", function (e) {
                                _this._chevron.removeClass(_this._chevronCollapsedClass);
                                _this._chevron.addClass(_this._chevronExpandedClass);
                            });
                            this._content.on("hide.bs.collapse", function (e) {
                                _this._chevron.addClass(_this._chevronCollapsedClass);
                                _this._chevron.removeClass(_this._chevronExpandedClass);
                            });
                            this._toggle.on("click", function (e) {
                                _this.toggle();
                            });
                            $(this._root).data("collapseContainer", this);
                        }
                        CollapseContainer.prototype.show = function () {
                            this._content.collapse("show");
                        };
                        CollapseContainer.prototype.hide = function () {
                            this._content.collapse("hide");
                        };
                        CollapseContainer.prototype.toggle = function () {
                            this._content.collapse("toggle");
                        };
                        return CollapseContainer;
                    }());
                    Status.CollapseContainer = CollapseContainer;
                    function updateOrderedStatus(id, ordered) {
                        return __awaiter(this, void 0, void 0, function () {
                            var baseUrl, response;
                            return __generator(this, function (_a) {
                                switch (_a.label) {
                                    case 0:
                                        baseUrl = "/Stock/UpdateOrderedStatus/" + id + "?ordered=" + ordered;
                                        return [4 /*yield*/, fetch(baseUrl, {
                                                method: "POST",
                                                credentials: "same-origin",
                                                cache: "no-cache",
                                                body: ""
                                            })];
                                    case 1:
                                        response = _a.sent();
                                        if (response.status === 200 || response.status === 204) {
                                            return [2 /*return*/, true];
                                        }
                                        else {
                                            return [2 /*return*/, false];
                                        }
                                        return [2 /*return*/];
                                }
                            });
                        });
                    }
                    $(function () {
                        var collapseContainers = document.querySelectorAll(".collapse-container");
                        for (var i = 0; i < collapseContainers.length; i++) {
                            var container = new CollapseContainer(collapseContainers[i]);
                        }
                        StockControlBeingRepairedGrid.instance.wrapper.on("click", ".stock__field__ordered", function (e) { return __awaiter(_this, void 0, void 0, function () {
                            var checkbox, item;
                            return __generator(this, function (_a) {
                                switch (_a.label) {
                                    case 0:
                                        checkbox = e.target;
                                        item = StockControlBeingRepairedGrid.instance.dataItem(e.target.closest("tr"));
                                        if (!checkbox.checked) return [3 /*break*/, 2];
                                        return [4 /*yield*/, updateOrderedStatus(item.Id, true)];
                                    case 1:
                                        if (_a.sent()) {
                                            item.set("Ordered", true);
                                        }
                                        else {
                                            checkbox.checked = false;
                                        }
                                        return [3 /*break*/, 4];
                                    case 2: return [4 /*yield*/, updateOrderedStatus(item.Id, false)];
                                    case 3:
                                        if (_a.sent()) {
                                            item.set("Ordered", false);
                                        }
                                        else {
                                            checkbox.checked = true;
                                        }
                                        _a.label = 4;
                                    case 4: return [2 /*return*/];
                                }
                            });
                        }); });
                        StockControlWaitingApprovalGrid.instance.wrapper.on("click", ".stock__field__ordered", function (e) { return __awaiter(_this, void 0, void 0, function () {
                            var checkbox, item;
                            return __generator(this, function (_a) {
                                switch (_a.label) {
                                    case 0:
                                        checkbox = e.target;
                                        item = StockControlWaitingApprovalGrid.instance.dataItem(e.target.closest("tr"));
                                        if (!checkbox.checked) return [3 /*break*/, 2];
                                        return [4 /*yield*/, updateOrderedStatus(item.Id, true)];
                                    case 1:
                                        if (_a.sent()) {
                                            item.set("Ordered", true);
                                        }
                                        else {
                                            checkbox.checked = false;
                                        }
                                        return [3 /*break*/, 4];
                                    case 2: return [4 /*yield*/, updateOrderedStatus(item.Id, false)];
                                    case 3:
                                        if (_a.sent()) {
                                            item.set("Ordered", false);
                                        }
                                        else {
                                            checkbox.checked = true;
                                        }
                                        _a.label = 4;
                                    case 4: return [2 /*return*/];
                                }
                            });
                        }); });
                    });
                })(Status = Stock.Status || (Stock.Status = {}));
            })(Stock = Pages.Stock || (Pages.Stock = {}));
        })(Pages = CRM.Pages || (CRM.Pages = {}));
    })(CRM = DentalDrill.CRM || (DentalDrill.CRM = {}));
})(DentalDrill || (DentalDrill = {}));
//# sourceMappingURL=status.js.map