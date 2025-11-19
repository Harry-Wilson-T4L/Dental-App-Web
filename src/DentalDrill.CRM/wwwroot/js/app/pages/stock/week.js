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
                var Week;
                (function (Week) {
                    var _this = this;
                    var EventHandler = DevGuild.Utilities.EventHandler;
                    var WeekChangedEventArgs = /** @class */ (function () {
                        function WeekChangedEventArgs(weekId, previousWeekId) {
                            this._weekId = weekId;
                            this._previousWeekId = previousWeekId;
                        }
                        Object.defineProperty(WeekChangedEventArgs.prototype, "weekId", {
                            get: function () {
                                return this._weekId;
                            },
                            enumerable: false,
                            configurable: true
                        });
                        Object.defineProperty(WeekChangedEventArgs.prototype, "previousWeekId", {
                            get: function () {
                                return this._previousWeekId;
                            },
                            enumerable: false,
                            configurable: true
                        });
                        return WeekChangedEventArgs;
                    }());
                    Week.WeekChangedEventArgs = WeekChangedEventArgs;
                    var WeekSelector = /** @class */ (function () {
                        function WeekSelector(root) {
                            var _this = this;
                            this._weekChanged = new EventHandler();
                            this._root = root;
                            this._nameElement = this._root.querySelector(".week-selector__name");
                            this._previousElement = this._root.querySelector(".week-selector__previous");
                            this._nextElement = this._root.querySelector(".week-selector__next");
                            this._weekId = this._root.getAttribute("data-week-id");
                            this._previousElement.addEventListener("click", function (e) { return __awaiter(_this, void 0, void 0, function () {
                                return __generator(this, function (_a) {
                                    switch (_a.label) {
                                        case 0: return [4 /*yield*/, this.handlePrevious()];
                                        case 1:
                                            _a.sent();
                                            return [2 /*return*/];
                                    }
                                });
                            }); });
                            this._nextElement.addEventListener("click", function (e) { return __awaiter(_this, void 0, void 0, function () {
                                return __generator(this, function (_a) {
                                    switch (_a.label) {
                                        case 0: return [4 /*yield*/, this.handleNext()];
                                        case 1:
                                            _a.sent();
                                            return [2 /*return*/];
                                    }
                                });
                            }); });
                        }
                        WeekSelector.init = function (root) {
                            var item = new WeekSelector(root);
                            $(root).data("weekSelector", item);
                            return item;
                        };
                        Object.defineProperty(WeekSelector.prototype, "weekId", {
                            get: function () {
                                return this._weekId;
                            },
                            enumerable: false,
                            configurable: true
                        });
                        Object.defineProperty(WeekSelector.prototype, "weekChanged", {
                            get: function () {
                                return this._weekChanged;
                            },
                            enumerable: false,
                            configurable: true
                        });
                        WeekSelector.prototype.handlePrevious = function () {
                            return __awaiter(this, void 0, void 0, function () {
                                var originalWeek, previousWeek;
                                return __generator(this, function (_a) {
                                    switch (_a.label) {
                                        case 0:
                                            originalWeek = this.weekId;
                                            return [4 /*yield*/, this.fetchPreviousWeek()];
                                        case 1:
                                            previousWeek = _a.sent();
                                            if (previousWeek && originalWeek === this.weekId) {
                                                this._weekId = previousWeek.weekId;
                                                this._root.setAttribute("data-week-id", previousWeek.weekId);
                                                this._nameElement.innerText = previousWeek.weekName;
                                                this._root.classList.toggle("week-selector--has-previous", previousWeek.hasPrevious);
                                                this._root.classList.toggle("week-selector--has-next", previousWeek.hasNext);
                                                this._weekChanged.raise(this, new WeekChangedEventArgs(this.weekId, originalWeek));
                                            }
                                            return [2 /*return*/];
                                    }
                                });
                            });
                        };
                        WeekSelector.prototype.handleNext = function () {
                            return __awaiter(this, void 0, void 0, function () {
                                var originalWeek, nextWeek;
                                return __generator(this, function (_a) {
                                    switch (_a.label) {
                                        case 0:
                                            originalWeek = this.weekId;
                                            return [4 /*yield*/, this.fetchNextWeek()];
                                        case 1:
                                            nextWeek = _a.sent();
                                            if (nextWeek && originalWeek === this.weekId) {
                                                this._weekId = nextWeek.weekId;
                                                this._root.setAttribute("data-week-id", nextWeek.weekId);
                                                this._nameElement.innerText = nextWeek.weekName;
                                                this._root.classList.toggle("week-selector--has-previous", nextWeek.hasPrevious);
                                                this._root.classList.toggle("week-selector--has-next", nextWeek.hasNext);
                                                this._weekChanged.raise(this, new WeekChangedEventArgs(this.weekId, originalWeek));
                                            }
                                            return [2 /*return*/];
                                    }
                                });
                            });
                        };
                        WeekSelector.prototype.fetchPreviousWeek = function () {
                            return __awaiter(this, void 0, void 0, function () {
                                var response, data;
                                return __generator(this, function (_a) {
                                    switch (_a.label) {
                                        case 0: return [4 /*yield*/, fetch("/Calendar/PreviousWeek/" + this.weekId, {
                                                method: "POST",
                                                credentials: "same-origin",
                                                cache: "no-cache",
                                                body: ""
                                            })];
                                        case 1:
                                            response = _a.sent();
                                            if (!(response.status === 200)) return [3 /*break*/, 3];
                                            return [4 /*yield*/, response.json()];
                                        case 2:
                                            data = _a.sent();
                                            return [2 /*return*/, data];
                                        case 3: return [2 /*return*/, undefined];
                                    }
                                });
                            });
                        };
                        WeekSelector.prototype.fetchNextWeek = function () {
                            return __awaiter(this, void 0, void 0, function () {
                                var response, data;
                                return __generator(this, function (_a) {
                                    switch (_a.label) {
                                        case 0: return [4 /*yield*/, fetch("/Calendar/NextWeek/" + this.weekId, {
                                                method: "POST",
                                                credentials: "same-origin",
                                                cache: "no-cache",
                                                body: ""
                                            })];
                                        case 1:
                                            response = _a.sent();
                                            if (!(response.status === 200)) return [3 /*break*/, 3];
                                            return [4 /*yield*/, response.json()];
                                        case 2:
                                            data = _a.sent();
                                            return [2 /*return*/, data];
                                        case 3: return [2 /*return*/, undefined];
                                    }
                                });
                            });
                        };
                        return WeekSelector;
                    }());
                    Week.WeekSelector = WeekSelector;
                    var StockControlPartsOutGrid = /** @class */ (function () {
                        function StockControlPartsOutGrid() {
                        }
                        Object.defineProperty(StockControlPartsOutGrid, "instance", {
                            get: function () {
                                return $("#StockControlPartsOutGrid").data("kendoGrid");
                            },
                            enumerable: false,
                            configurable: true
                        });
                        StockControlPartsOutGrid.updateStatus = function (id, status, weekId) {
                            return __awaiter(this, void 0, void 0, function () {
                                var baseUrl, response;
                                return __generator(this, function (_a) {
                                    switch (_a.label) {
                                        case 0:
                                            baseUrl = "/Stock/UpdateStatus/" + id + "?status=" + status;
                                            if (weekId) {
                                                baseUrl = baseUrl + ("&weekId=" + weekId);
                                            }
                                            return [4 /*yield*/, fetch(baseUrl, {
                                                    method: "POST",
                                                    credentials: "same-origin",
                                                    cache: "no-cache",
                                                    body: ""
                                                })];
                                        case 1:
                                            response = _a.sent();
                                            if (response.status === 200) {
                                                return [2 /*return*/, true];
                                            }
                                            else {
                                                return [2 /*return*/, false];
                                            }
                                            return [2 /*return*/];
                                    }
                                });
                            });
                        };
                        StockControlPartsOutGrid.handleOpen = DevGuild.AspNet.Controls.Grids.Handlers.GridHandlers.createButtonClickHandler(function (item) {
                            routes.handpieces.edit(item.HandpieceId).open();
                        });
                        return StockControlPartsOutGrid;
                    }());
                    Week.StockControlPartsOutGrid = StockControlPartsOutGrid;
                    $(function () {
                        var weekSelector = WeekSelector.init(document.getElementById("StockControlPartsOutGrid_WeekSelector"));
                        weekSelector.weekChanged.subscribe(function (sender, e) { return __awaiter(_this, void 0, void 0, function () {
                            var grid, transport;
                            return __generator(this, function (_a) {
                                switch (_a.label) {
                                    case 0:
                                        grid = StockControlPartsOutGrid.instance;
                                        transport = grid.dataSource["transport"];
                                        transport.options.read.url = "/Stock/ReadHandpiecesWithPartsOut?weekId=" + e.weekId;
                                        return [4 /*yield*/, grid.dataSource.read()];
                                    case 1:
                                        _a.sent();
                                        return [2 /*return*/];
                                }
                            });
                        }); });
                        StockControlPartsOutGrid.instance.wrapper.on("click", ".stock__field__ordered", function (e) { return __awaiter(_this, void 0, void 0, function () {
                            var checkbox, item;
                            return __generator(this, function (_a) {
                                switch (_a.label) {
                                    case 0:
                                        checkbox = e.target;
                                        item = StockControlPartsOutGrid.instance.dataItem(e.target.closest("tr"));
                                        if (!checkbox.checked) return [3 /*break*/, 2];
                                        return [4 /*yield*/, StockControlPartsOutGrid.updateStatus(item.Id, "Ordered", weekSelector.weekId)];
                                    case 1:
                                        if (_a.sent()) {
                                            item.set("Ordered", true);
                                        }
                                        else {
                                            checkbox.checked = false;
                                        }
                                        return [3 /*break*/, 4];
                                    case 2: return [4 /*yield*/, StockControlPartsOutGrid.updateStatus(item.Id, "Active")];
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
                        StockControlPartsOutGrid.instance.wrapper.on("click", ".stock__field__ignored", function (e) { return __awaiter(_this, void 0, void 0, function () {
                            var checkbox, item;
                            return __generator(this, function (_a) {
                                switch (_a.label) {
                                    case 0:
                                        checkbox = e.target;
                                        item = StockControlPartsOutGrid.instance.dataItem(e.target.closest("tr"));
                                        if (!checkbox.checked) return [3 /*break*/, 2];
                                        return [4 /*yield*/, StockControlPartsOutGrid.updateStatus(item.Id, "Ignored", weekSelector.weekId)];
                                    case 1:
                                        if (_a.sent()) {
                                            item.set("Ignored", true);
                                        }
                                        else {
                                            checkbox.checked = false;
                                        }
                                        return [3 /*break*/, 4];
                                    case 2: return [4 /*yield*/, StockControlPartsOutGrid.updateStatus(item.Id, "Active")];
                                    case 3:
                                        if (_a.sent()) {
                                            item.set("Ignored", false);
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
                })(Week = Stock.Week || (Stock.Week = {}));
            })(Stock = Pages.Stock || (Pages.Stock = {}));
        })(Pages = CRM.Pages || (CRM.Pages = {}));
    })(CRM = DentalDrill.CRM || (DentalDrill.CRM = {}));
})(DentalDrill || (DentalDrill = {}));
//# sourceMappingURL=week.js.map