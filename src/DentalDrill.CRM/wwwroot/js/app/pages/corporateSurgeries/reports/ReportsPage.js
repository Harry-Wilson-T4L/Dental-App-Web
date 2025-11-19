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
            var CorporateSurgeries;
            (function (CorporateSurgeries) {
                var Reports;
                (function (Reports) {
                    var ReportsPage = /** @class */ (function () {
                        function ReportsPage(root) {
                            var _this = this;
                            this._root = root;
                            this._corporateId = root.attr("data-corporate-id");
                            this._corporateUrlPath = root.attr("data-corporate-urlpath");
                            this._mainControls = new Reports.ReportsPageMainControls(root);
                            this._surgeries = new Reports.ReportsTabSurgeries(this, root.find(".reports__surgeries"));
                            this._brands = new Reports.ReportsTabBrands(this, root.find(".reports__brands"));
                            this._statuses = new Reports.ReportsTabStatuses(this, root.find(".reports__statuses"));
                            this._mainControls.buttonClear.on("click", function (e) { return __awaiter(_this, void 0, void 0, function () {
                                var allClients;
                                return __generator(this, function (_a) {
                                    switch (_a.label) {
                                        case 0:
                                            this._mainControls.dateRangeSelector.reset();
                                            allClients = this._mainControls.clients.dataSource.data().map(function (x) { return x.Id; });
                                            this._mainControls.clients.value(allClients);
                                            this._mainControls.clients.trigger("change");
                                            return [4 /*yield*/, this._surgeries.applyGlobalFilters(this._mainControls.dateFrom.value(), this._mainControls.dateTo.value(), this._mainControls.clients.value())];
                                        case 1:
                                            _a.sent();
                                            return [4 /*yield*/, this._brands.applyGlobalFilters(this._mainControls.dateFrom.value(), this._mainControls.dateTo.value(), this._mainControls.clients.value())];
                                        case 2:
                                            _a.sent();
                                            return [4 /*yield*/, this._statuses.applyGlobalFilters(this._mainControls.dateFrom.value(), this._mainControls.dateTo.value(), this._mainControls.clients.value())];
                                        case 3:
                                            _a.sent();
                                            return [4 /*yield*/, this._surgeries.initialize()];
                                        case 4:
                                            _a.sent();
                                            return [4 /*yield*/, this._brands.initialize()];
                                        case 5:
                                            _a.sent();
                                            return [4 /*yield*/, this._statuses.initialize()];
                                        case 6:
                                            _a.sent();
                                            return [2 /*return*/];
                                    }
                                });
                            }); });
                            this._mainControls.buttonSearch.on("click", function (e) { return __awaiter(_this, void 0, void 0, function () {
                                return __generator(this, function (_a) {
                                    switch (_a.label) {
                                        case 0: return [4 /*yield*/, this._surgeries.applyGlobalFilters(this._mainControls.dateFrom.value(), this._mainControls.dateTo.value(), this._mainControls.clients.value())];
                                        case 1:
                                            _a.sent();
                                            return [4 /*yield*/, this._brands.applyGlobalFilters(this._mainControls.dateFrom.value(), this._mainControls.dateTo.value(), this._mainControls.clients.value())];
                                        case 2:
                                            _a.sent();
                                            return [4 /*yield*/, this._statuses.applyGlobalFilters(this._mainControls.dateFrom.value(), this._mainControls.dateTo.value(), this._mainControls.clients.value())];
                                        case 3:
                                            _a.sent();
                                            return [4 /*yield*/, this._surgeries.initialize()];
                                        case 4:
                                            _a.sent();
                                            return [4 /*yield*/, this._brands.initialize()];
                                        case 5:
                                            _a.sent();
                                            return [4 /*yield*/, this._statuses.initialize()];
                                        case 6:
                                            _a.sent();
                                            return [2 /*return*/];
                                    }
                                });
                            }); });
                            root.data("ReportsPage", this);
                            console.log("ReportsPage constructed");
                        }
                        Object.defineProperty(ReportsPage.prototype, "corporateId", {
                            get: function () {
                                return this._corporateId;
                            },
                            enumerable: false,
                            configurable: true
                        });
                        Object.defineProperty(ReportsPage.prototype, "corporateUrlPath", {
                            get: function () {
                                return this._corporateUrlPath;
                            },
                            enumerable: false,
                            configurable: true
                        });
                        ReportsPage.prototype.initialize = function () {
                            return __awaiter(this, void 0, void 0, function () {
                                return __generator(this, function (_a) {
                                    switch (_a.label) {
                                        case 0: return [4 /*yield*/, this._surgeries.applyGlobalFilters(this._mainControls.dateFrom.value(), this._mainControls.dateTo.value(), this._mainControls.clients.value())];
                                        case 1:
                                            _a.sent();
                                            return [4 /*yield*/, this._brands.applyGlobalFilters(this._mainControls.dateFrom.value(), this._mainControls.dateTo.value(), this._mainControls.clients.value())];
                                        case 2:
                                            _a.sent();
                                            return [4 /*yield*/, this._statuses.applyGlobalFilters(this._mainControls.dateFrom.value(), this._mainControls.dateTo.value(), this._mainControls.clients.value())];
                                        case 3:
                                            _a.sent();
                                            return [4 /*yield*/, this._surgeries.initialize()];
                                        case 4:
                                            _a.sent();
                                            return [4 /*yield*/, this._brands.initialize()];
                                        case 5:
                                            _a.sent();
                                            return [4 /*yield*/, this._statuses.initialize()];
                                        case 6:
                                            _a.sent();
                                            this.setValidation();
                                            console.log("ReportsPage initialized");
                                            return [2 /*return*/];
                                    }
                                });
                            });
                        };
                        ReportsPage.prototype.setValidation = function () {
                            var containerFrom = $("#FilterDateFrom");
                            containerFrom.kendoValidator({
                                rules: {
                                    isValidDate: function (input) {
                                        if (kendo.parseDate(input.val()) === null) {
                                            return false;
                                        }
                                        else {
                                            return true;
                                        }
                                    }
                                },
                                messages: {
                                    isValidDate: "Invalid date format"
                                }
                            });
                            var containerTo = $("#FilterDateTo");
                            containerTo.kendoValidator({
                                rules: {
                                    isValidDate: function (input) {
                                        if (kendo.parseDate(input.val()) === null) {
                                            return false;
                                        }
                                        else {
                                            return true;
                                        }
                                    }
                                },
                                messages: {
                                    isValidDate: "Invalid date format"
                                }
                            });
                        };
                        return ReportsPage;
                    }());
                    Reports.ReportsPage = ReportsPage;
                })(Reports = CorporateSurgeries.Reports || (CorporateSurgeries.Reports = {}));
            })(CorporateSurgeries = Pages.CorporateSurgeries || (Pages.CorporateSurgeries = {}));
        })(Pages = CRM.Pages || (CRM.Pages = {}));
    })(CRM = DentalDrill.CRM || (DentalDrill.CRM = {}));
})(DentalDrill || (DentalDrill = {}));
//# sourceMappingURL=ReportsPage.js.map