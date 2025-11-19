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
            var HandpieceStore;
            (function (HandpieceStore) {
                var Index;
                (function (Index) {
                    var AjaxFormsManager = DevGuild.AspNet.Forms.AjaxForms.AjaxFormsManager;
                    var PriceSlider = /** @class */ (function () {
                        function PriceSlider(root) {
                            var _this = this;
                            this._root = root;
                            this._valueNode = root.find(".price-slider__value");
                            this._slider = root.find(".price-slider__slider .price-slider__slider__widget[data-role='rangeslider']").data("kendoRangeSlider");
                            this._slider.bind("change", function (e) {
                                var val = e.sender.value();
                                if (val !== undefined && typeof val === "object" && Array.isArray(val)) {
                                    var firstValue = val[0];
                                    var secondValue = val[1];
                                    _this._valueNode.text("$" + kendo.format("{0:#,##0.##}", firstValue) + " - $" + kendo.format("{0:#,##0.##}", secondValue));
                                }
                            });
                        }
                        Object.defineProperty(PriceSlider.prototype, "value", {
                            get: function () {
                                return this._slider.value();
                            },
                            set: function (val) {
                                this._slider.value(val);
                            },
                            enumerable: false,
                            configurable: true
                        });
                        PriceSlider.prototype.reset = function () {
                            this._slider.value([this._slider.options.min, this._slider.options.max]);
                        };
                        return PriceSlider;
                    }());
                    Index.PriceSlider = PriceSlider;
                    var HandpieceStoreFilters = /** @class */ (function () {
                        function HandpieceStoreFilters(page, root) {
                            var _this = this;
                            this._page = page;
                            this._root = root;
                            this._root.find(".handpiece-store-filters__apply-button").on("click", function (e) {
                                e.preventDefault();
                                _this.apply();
                            });
                            this._root.find(".handpiece-store-filters__reset-button").on("click", function (e) {
                                e.preventDefault();
                                _this.reset();
                            });
                            this._fieldSearch = this._root.find(".handpiece-store-filters__fields__search");
                            this._fieldPrice = new PriceSlider(this._root.find(".handpiece-store-filters__fields__price"));
                            this._fieldBrand = this._root.find(".handpiece-store-filters__fields__brand[data-role='multiselect']").data("kendoMultiSelect");
                            this._fieldModel = this._root.find(".handpiece-store-filters__fields__model[data-role='multiselect']").data("kendoMultiSelect");
                            this._fieldCoupling = this._root.find(".handpiece-store-filters__fields__coupling[data-role='multiselect']").data("kendoMultiSelect");
                            this._fieldType = this._root.find(".handpiece-store-filters__fields__type[data-role='multiselect']").data("kendoMultiSelect");
                        }
                        HandpieceStoreFilters.prototype.apply = function () {
                            var filterValues = {
                                Search: this._fieldSearch.val(),
                                Price: this._fieldPrice.value,
                                Brand: this._fieldBrand.value(),
                                Model: this._fieldModel.value(),
                                Coupling: this._fieldCoupling.value(),
                                Type: this._fieldType.value(),
                            };
                            this._page.listView.readData = filterValues;
                            this._page.listView.reload();
                        };
                        HandpieceStoreFilters.prototype.reset = function () {
                            this._fieldSearch.val("");
                            this._fieldPrice.reset();
                            this._fieldBrand.value([]);
                            this._fieldModel.value([]);
                            this._fieldCoupling.value([]);
                            this._fieldType.value([]);
                            this._page.listView.readData = {};
                            this._page.listView.reload();
                        };
                        return HandpieceStoreFilters;
                    }());
                    Index.HandpieceStoreFilters = HandpieceStoreFilters;
                    var HandpieceStoreListView = /** @class */ (function () {
                        function HandpieceStoreListView(page, root) {
                            var _this = this;
                            this._readData = {};
                            this._page = page;
                            this._root = root;
                            this._root.on("click", ".handpiece-store-list__item__actions__buy", function (e) {
                                e.preventDefault();
                                var itemRoot = $(e.target).closest(".handpiece-store-list__item");
                                if (itemRoot.length) {
                                    var itemId = itemRoot.attr("data-id");
                                    _this.handleItemBuy(itemId);
                                }
                            });
                        }
                        Object.defineProperty(HandpieceStoreListView.prototype, "readData", {
                            get: function () {
                                return this._readData;
                            },
                            set: function (data) {
                                this._readData = data;
                            },
                            enumerable: false,
                            configurable: true
                        });
                        HandpieceStoreListView.prototype.reload = function () {
                            var listView = this._root.find(".handpiece-store-list__view[data-role=listview]").data("kendoListView");
                            listView.dataSource.read();
                        };
                        HandpieceStoreListView.prototype.handleItemBuy = function (itemId) {
                            var _this = this;
                            var windowState = {
                                open: true,
                            };
                            var url = routes.handpieceStore.buy(itemId);
                            var dialogRoot = $("<div></div>");
                            var dialogOptions = {
                                title: "Buy Handpiece",
                                actions: ["close"],
                                content: url.value,
                                width: "800px",
                                height: "auto",
                                modal: true,
                                visible: false,
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
                                            case 0: return [4 /*yield*/, AjaxFormsManager.waitFor("HandpieceStoryBuy")];
                                            case 1:
                                                _a.sent();
                                                return [4 /*yield*/, this.reload()];
                                            case 2:
                                                _a.sent();
                                                e.sender.close();
                                                e.sender.destroy();
                                                windowState.open = false;
                                                return [2 /*return*/];
                                        }
                                    });
                                }); },
                                close: function () {
                                    dialogRoot.data("kendoWindow").destroy();
                                    windowState.open = false;
                                },
                            };
                            dialogRoot.kendoWindow(dialogOptions);
                            var dialog = dialogRoot.data("kendoWindow");
                            dialog.center();
                            dialog.open();
                            $(window).on("resize", function (e) {
                                if (windowState.open) {
                                    dialog.center();
                                }
                            });
                        };
                        return HandpieceStoreListView;
                    }());
                    Index.HandpieceStoreListView = HandpieceStoreListView;
                    var HandpieceStorePage = /** @class */ (function () {
                        function HandpieceStorePage(root) {
                            this._root = root;
                            this._filters = new HandpieceStoreFilters(this, root.find(".handpiece-store-filters"));
                            this._listView = new HandpieceStoreListView(this, root.find(".handpiece-store-list"));
                        }
                        Object.defineProperty(HandpieceStorePage.prototype, "filters", {
                            get: function () {
                                return this._filters;
                            },
                            enumerable: false,
                            configurable: true
                        });
                        Object.defineProperty(HandpieceStorePage.prototype, "listView", {
                            get: function () {
                                return this._listView;
                            },
                            enumerable: false,
                            configurable: true
                        });
                        return HandpieceStorePage;
                    }());
                    Index.HandpieceStorePage = HandpieceStorePage;
                    var Global = /** @class */ (function () {
                        function Global() {
                        }
                        Global.linkPage = function (page) {
                            Global._page = page;
                        };
                        Object.defineProperty(Global, "page", {
                            get: function () {
                                if (!Global._page) {
                                    throw "Page not linked";
                                }
                                return Global._page;
                            },
                            enumerable: false,
                            configurable: true
                        });
                        Global.getReadData = function () {
                            return Global.page.listView.readData;
                        };
                        return Global;
                    }());
                    Index.Global = Global;
                    $(function () {
                        var page = new HandpieceStorePage($(".handpiece-store"));
                        Global.linkPage(page);
                    });
                })(Index = HandpieceStore.Index || (HandpieceStore.Index = {}));
            })(HandpieceStore = Pages.HandpieceStore || (Pages.HandpieceStore = {}));
        })(Pages = CRM.Pages || (CRM.Pages = {}));
    })(CRM = DentalDrill.CRM || (DentalDrill.CRM = {}));
})(DentalDrill || (DentalDrill = {}));
//# sourceMappingURL=index.js.map