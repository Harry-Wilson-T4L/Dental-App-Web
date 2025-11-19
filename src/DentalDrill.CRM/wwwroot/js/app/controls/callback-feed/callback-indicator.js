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
        var Controls;
        (function (Controls) {
            var CallbackFeed;
            (function (CallbackFeed) {
                var Dropdown = DentalDrill.CRM.Controls.Dropdown;
                var NotificationsConnection = DentalDrill.CRM.Controls.NotificationsFeed.NotificationsConnection;
                var ClassToggler = /** @class */ (function () {
                    function ClassToggler(root, className) {
                        this._root = root;
                        this._className = className;
                    }
                    Object.defineProperty(ClassToggler.prototype, "value", {
                        get: function () {
                            return this._root.classList.contains(this._className);
                        },
                        set: function (val) {
                            if (val && !this._root.classList.contains(this._className)) {
                                this._root.classList.add(this._className);
                            }
                            else if (!val && this._root.classList.contains(this._className)) {
                                this._root.classList.remove(this._className);
                            }
                        },
                        enumerable: false,
                        configurable: true
                    });
                    return ClassToggler;
                }());
                CallbackFeed.ClassToggler = ClassToggler;
                var CallbackIndicatorNumber = /** @class */ (function () {
                    function CallbackIndicatorNumber(root) {
                        this._root = root;
                        this._value = parseInt(this._root.innerText);
                        if (isNaN(this._value)) {
                            this._value = undefined;
                        }
                    }
                    Object.defineProperty(CallbackIndicatorNumber.prototype, "value", {
                        get: function () {
                            return this._value;
                        },
                        set: function (val) {
                            this._value = val;
                            this._root.innerText = val.toString();
                        },
                        enumerable: false,
                        configurable: true
                    });
                    return CallbackIndicatorNumber;
                }());
                CallbackFeed.CallbackIndicatorNumber = CallbackIndicatorNumber;
                var CallbackIndicator = /** @class */ (function () {
                    function CallbackIndicator(root) {
                        var _this = this;
                        this._root = root;
                        this._hasUnresolved = new ClassToggler(this._root, "callback-indicator--has-unresolved");
                        this._hasUnread = new ClassToggler(this._root, "callback-indicator--has-unread");
                        this._disconnected = new ClassToggler(this._root, "callback-indicator--disconnected");
                        this._dropdown = new Dropdown(root);
                        this._numberContainer = new CallbackIndicatorNumber(this._root.querySelector(".callback-indicator__number"));
                        var connection = NotificationsConnection.instance;
                        connection.connected.subscribe(function (sender, args) {
                            _this.disconnected = false;
                        });
                        connection.disconnected.subscribe(function (sender, args) {
                            _this.disconnected = true;
                        });
                        connection.callbackUpdated.subscribe(function (sender, args) { return __awaiter(_this, void 0, void 0, function () {
                            var totals, previousUnread;
                            return __generator(this, function (_a) {
                                switch (_a.label) {
                                    case 0: return [4 /*yield*/, this.loadTotals()];
                                    case 1:
                                        totals = _a.sent();
                                        previousUnread = this._numberContainer.value;
                                        this._numberContainer.value = totals.new;
                                        if ((previousUnread === undefined || totals.new > previousUnread) && !this._dropdown.isShown()) {
                                            this.hasUnread = true;
                                        }
                                        return [2 /*return*/];
                                }
                            });
                        }); });
                        this._dropdown.preventContentClickHide();
                        this._dropdown.onShown.subscribe(function (sender, args) {
                            var component = _this._root.querySelector(".callback-feed");
                            if (component) {
                                var componentNode = $(component);
                                if (!componentNode.data("CallbackFeedListView")) {
                                    CallbackFeed.CallbackFeedListView.create(componentNode);
                                }
                            }
                            if (_this.hasUnread) {
                                _this.hasUnread = false;
                            }
                        });
                        this.disconnected = !connection.isConnected;
                    }
                    Object.defineProperty(CallbackIndicator.prototype, "hasUnresolved", {
                        get: function () {
                            return this._hasUnresolved.value;
                        },
                        set: function (val) {
                            this._hasUnresolved.value = val;
                        },
                        enumerable: false,
                        configurable: true
                    });
                    Object.defineProperty(CallbackIndicator.prototype, "hasUnread", {
                        get: function () {
                            return this._hasUnread.value;
                        },
                        set: function (val) {
                            this._hasUnread.value = val;
                        },
                        enumerable: false,
                        configurable: true
                    });
                    Object.defineProperty(CallbackIndicator.prototype, "disconnected", {
                        get: function () {
                            return this._disconnected.value;
                        },
                        set: function (val) {
                            this._disconnected.value = val;
                        },
                        enumerable: false,
                        configurable: true
                    });
                    CallbackIndicator.prototype.initialize = function () {
                        return __awaiter(this, void 0, void 0, function () {
                            var totals;
                            return __generator(this, function (_a) {
                                switch (_a.label) {
                                    case 0: return [4 /*yield*/, this.loadTotals()];
                                    case 1:
                                        totals = _a.sent();
                                        this._numberContainer.value = totals.new;
                                        return [2 /*return*/];
                                }
                            });
                        });
                    };
                    CallbackIndicator.prototype.loadTotals = function () {
                        return __awaiter(this, void 0, void 0, function () {
                            var response;
                            return __generator(this, function (_a) {
                                switch (_a.label) {
                                    case 0: return [4 /*yield*/, fetch("/Callback/Total", {
                                            method: "POST",
                                            credentials: "same-origin",
                                            cache: "no-cache",
                                            body: ""
                                        })];
                                    case 1:
                                        response = _a.sent();
                                        return [4 /*yield*/, response.json()];
                                    case 2: return [2 /*return*/, _a.sent()];
                                }
                            });
                        });
                    };
                    return CallbackIndicator;
                }());
                CallbackFeed.CallbackIndicator = CallbackIndicator;
                $(function () {
                    var indicatorElements = document.querySelectorAll(".callback-indicator");
                    for (var i = 0; i < indicatorElements.length; i++) {
                        var element = indicatorElements[i];
                        var indicator = new CallbackIndicator(element);
                        $(element).data("CallbackIndicator", element);
                        indicator.initialize();
                    }
                });
            })(CallbackFeed = Controls.CallbackFeed || (Controls.CallbackFeed = {}));
        })(Controls = CRM.Controls || (CRM.Controls = {}));
    })(CRM = DentalDrill.CRM || (DentalDrill.CRM = {}));
})(DentalDrill || (DentalDrill = {}));
//# sourceMappingURL=callback-indicator.js.map