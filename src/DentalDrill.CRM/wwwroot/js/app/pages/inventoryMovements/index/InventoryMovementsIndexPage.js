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
                    var AjaxFormsManager = DevGuild.AspNet.Forms.AjaxForms.AjaxFormsManager;
                    var InventoryMovementsTabsSetState;
                    (function (InventoryMovementsTabsSetState) {
                        InventoryMovementsTabsSetState[InventoryMovementsTabsSetState["Default"] = 0] = "Default";
                        InventoryMovementsTabsSetState[InventoryMovementsTabsSetState["Minimized"] = 1] = "Minimized";
                        InventoryMovementsTabsSetState[InventoryMovementsTabsSetState["Maximized"] = 2] = "Maximized";
                    })(InventoryMovementsTabsSetState = Index.InventoryMovementsTabsSetState || (Index.InventoryMovementsTabsSetState = {}));
                    var InventoryMovementsTabsSet = /** @class */ (function () {
                        function InventoryMovementsTabsSet(collection, container, options, descriptors) {
                            this._state = InventoryMovementsTabsSetState.Default;
                            collection.add(this);
                            this._collection = collection;
                            this._container = container;
                            this._descriptors = descriptors;
                            this._tabFactory = new Index.InventoryMovementsTabFactory(options);
                            this._tabs = new Map();
                            this._tabStrip = this.createTabStrip();
                            this.createToolbar();
                        }
                        Object.defineProperty(InventoryMovementsTabsSet.prototype, "state", {
                            get: function () {
                                return this._state;
                            },
                            enumerable: false,
                            configurable: true
                        });
                        Object.defineProperty(InventoryMovementsTabsSet.prototype, "activeTab", {
                            get: function () {
                                return this._activeTab;
                            },
                            enumerable: false,
                            configurable: true
                        });
                        InventoryMovementsTabsSet.prototype.select = function (tabIndex) {
                            this._tabStrip.select(tabIndex);
                        };
                        InventoryMovementsTabsSet.prototype.createTabStrip = function () {
                            var _this = this;
                            var element = this._container.appendChild(document.createElement("div"));
                            var dataSource = new kendo.data.DataSource({
                                data: this._descriptors,
                            });
                            return $(element).kendoTabStrip({
                                dataSource: dataSource,
                                dataTextField: "Title",
                                dataContentField: "Content",
                                activate: function (e) {
                                    var tabContent = e.contentElement.querySelector(".inventory-movements-tab");
                                    if (!tabContent) {
                                        throw new Error("Invalid tab content");
                                    }
                                    var tabId = tabContent.getAttribute("data-id");
                                    if (!tabId) {
                                        throw new Error("Invalid tab id");
                                    }
                                    if (_this._tabs.has(tabId)) {
                                        var tab = _this._tabs.get(tabId);
                                        _this._activeTab = tab;
                                        tab.activate();
                                    }
                                    else {
                                        var tab = _this._tabFactory.createTab(tabId, tabContent);
                                        _this._tabs.set(tabId, tab);
                                        _this._activeTab = tab;
                                        tab.init();
                                        tab.activate();
                                    }
                                }
                            }).data("kendoTabStrip");
                        };
                        InventoryMovementsTabsSet.prototype.createToolbar = function () {
                            var _this = this;
                            var element = this._container.appendChild(document.createElement("div"));
                            element.classList.add("inventory-movements__toolbar");
                            element.classList.add("btn-group");
                            var buttonMinimize = element.appendChild(document.createElement("button"));
                            buttonMinimize.type = "button";
                            buttonMinimize.classList.add("btn", "btn-sm", "btn-outline-secondary", "inventory-movements--when-not-minimized");
                            buttonMinimize.appendChild(document.createElement("span")).classList.add("far", "fa-fw", "fa-window-minimize");
                            buttonMinimize.addEventListener("click", function (e) {
                                _this._collection.minimize(_this);
                            });
                            var buttonRestore = element.appendChild(document.createElement("button"));
                            buttonRestore.type = "button";
                            buttonRestore.classList.add("btn", "btn-sm", "btn-outline-secondary", "inventory-movements--when-not-default");
                            buttonRestore.appendChild(document.createElement("span")).classList.add("far", "fa-fw", "fa-window-restore");
                            buttonRestore.addEventListener("click", function (e) {
                                _this._collection.restore(_this);
                            });
                            var buttonMaximize = element.appendChild(document.createElement("button"));
                            buttonMaximize.type = "button";
                            buttonMaximize.classList.add("btn", "btn-sm", "btn-outline-secondary", "inventory-movements--when-not-maximized");
                            buttonMaximize.appendChild(document.createElement("span")).classList.add("far", "fa-fw", "fa-window-maximize");
                            buttonMaximize.addEventListener("click", function (e) {
                                _this._collection.maximize(_this);
                            });
                        };
                        InventoryMovementsTabsSet.prototype.minimize = function () {
                            this._state = InventoryMovementsTabsSetState.Minimized;
                            this._container.classList.add("inventory-movements--minimized");
                            this._container.classList.remove("inventory-movements--maximized");
                            this._container.classList.remove("inventory-movements--default");
                            this.resize(false);
                        };
                        InventoryMovementsTabsSet.prototype.maximize = function () {
                            this._state = InventoryMovementsTabsSetState.Maximized;
                            this._container.classList.remove("inventory-movements--minimized");
                            this._container.classList.add("inventory-movements--maximized");
                            this._container.classList.remove("inventory-movements--default");
                            this.resize(true);
                        };
                        InventoryMovementsTabsSet.prototype.restore = function () {
                            this._state = InventoryMovementsTabsSetState.Default;
                            this._container.classList.remove("inventory-movements--minimized");
                            this._container.classList.remove("inventory-movements--maximized");
                            this._container.classList.add("inventory-movements--default");
                            this.resize(true);
                        };
                        InventoryMovementsTabsSet.prototype.resize = function (visible) {
                            var _this = this;
                            setTimeout(function () {
                                _this._tabs.forEach(function (tab) {
                                    tab.resize(visible);
                                });
                            });
                        };
                        return InventoryMovementsTabsSet;
                    }());
                    Index.InventoryMovementsTabsSet = InventoryMovementsTabsSet;
                    var InventoryMovementsTabsSetCollection = /** @class */ (function () {
                        function InventoryMovementsTabsSetCollection() {
                            this._tabSets = [];
                        }
                        InventoryMovementsTabsSetCollection.prototype.add = function (tabSet) {
                            this._tabSets.push(tabSet);
                        };
                        InventoryMovementsTabsSetCollection.prototype.minimize = function (tabSet) {
                            tabSet.minimize();
                            var others = this.getOthers(tabSet);
                            if (others.length === 1) {
                                others[0].maximize();
                                return;
                            }
                        };
                        InventoryMovementsTabsSetCollection.prototype.maximize = function (tabSet) {
                            tabSet.maximize();
                            var others = this.getOthers(tabSet);
                            for (var _i = 0, _a = others.filter(function (x) { return x.state !== InventoryMovementsTabsSetState.Minimized; }); _i < _a.length; _i++) {
                                var other = _a[_i];
                                other.minimize();
                            }
                        };
                        InventoryMovementsTabsSetCollection.prototype.restore = function (tabSet) {
                            tabSet.restore();
                            var others = this.getOthers(tabSet);
                            for (var _i = 0, others_1 = others; _i < others_1.length; _i++) {
                                var other = others_1[_i];
                                other.restore();
                            }
                        };
                        InventoryMovementsTabsSetCollection.prototype.getOthers = function (except) {
                            return this._tabSets.filter(function (x) { return x !== except; });
                        };
                        return InventoryMovementsTabsSetCollection;
                    }());
                    Index.InventoryMovementsTabsSetCollection = InventoryMovementsTabsSetCollection;
                    var InventoryMovementsIndexPage = /** @class */ (function () {
                        function InventoryMovementsIndexPage(root, options) {
                            this._root = root;
                            this._options = options;
                        }
                        InventoryMovementsIndexPage.prototype.init = function () {
                            this._collection = new InventoryMovementsTabsSetCollection();
                            this._movements = new InventoryMovementsTabsSet(this._collection, this._root.querySelector(".inventory-movements__movements"), this._options, this.createMovementsTabStripDataSource());
                            this._charts = new InventoryMovementsTabsSet(this._collection, this._root.querySelector(".inventory-movements__charts"), this._options, this.createChartsTabStripDataSource());
                            this._movements.select(this.getInitialMovementsTab());
                            this._charts.select(0);
                            this.initActions();
                        };
                        InventoryMovementsIndexPage.prototype.getInitialMovementsTab = function () {
                            if (this._options.showGrouped) {
                                switch (this._options.tab) {
                                    case "Approved":
                                        return 0;
                                    case "Requested":
                                        return 1;
                                    case "Ordered":
                                        return 2;
                                    case "Complete":
                                        return 3;
                                    case "All":
                                        return 4;
                                    default:
                                        return 0;
                                }
                            }
                            else {
                                switch (this._options.tab) {
                                    case "Tray":
                                        return 0;
                                    case "Approved":
                                        return 0;
                                    case "Requested":
                                        return 1;
                                    case "Ordered":
                                        return 2;
                                    case "Complete":
                                        return 3;
                                    case "All":
                                        return 4;
                                    default:
                                        return 0;
                                }
                            }
                            return 0;
                        };
                        InventoryMovementsIndexPage.prototype.createMovementsTabStripDataSource = function () {
                            var data = [];
                            if (this._options.showGrouped) {
                                data.push({
                                    Title: "Order",
                                    Content: this.getMovementTabContent("ApprovedGroup"),
                                });
                                data.push({
                                    Title: "Not Approved",
                                    Content: this.getMovementTabContent("RequestedGroup"),
                                });
                                data.push({
                                    Title: "Verify",
                                    Content: this.getMovementTabContent("OrderedGroup"),
                                });
                                data.push({
                                    Title: "Complete",
                                    Content: this.getMovementTabContent("CompleteGroup"),
                                });
                                data.push({
                                    Title: "All",
                                    Content: this.getMovementTabContent("AllGroup"),
                                });
                            }
                            else {
                                if (this._options.tab === "Tray") {
                                    data.push({
                                        Title: "Tray",
                                        Content: this.getMovementTabContent("Tray"),
                                    });
                                }
                                data.push({
                                    Title: "Order",
                                    Content: this.getMovementTabContent("Approved"),
                                });
                                data.push({
                                    Title: "Not Approved",
                                    Content: this.getMovementTabContent("Requested"),
                                });
                                data.push({
                                    Title: "Verify",
                                    Content: this.getMovementTabContent("Ordered"),
                                });
                                data.push({
                                    Title: "Complete",
                                    Content: this.getMovementTabContent("Complete"),
                                });
                                data.push({
                                    Title: "All",
                                    Content: this.getMovementTabContent("All"),
                                });
                            }
                            return data;
                        };
                        InventoryMovementsIndexPage.prototype.createChartsTabStripDataSource = function () {
                            var data = [];
                            data.push({
                                Title: "Available Stock",
                                Content: this.getMovementTabContent("StatsAvailableStock"),
                            });
                            return data;
                        };
                        InventoryMovementsIndexPage.prototype.getMovementTabContent = function (id) {
                            return "<div class=\"inventory-movements-tab\" data-id=\"" + id + "\">Loading...</div>";
                        };
                        InventoryMovementsIndexPage.prototype.initActions = function () {
                            var _this = this;
                            var createButton = this._root.querySelector(".inventory-movements__actions__create");
                            createButton.addEventListener("click", function (e) {
                                e.preventDefault();
                                var url = new DevGuild.AspNet.Routing.Uri(createButton.getAttribute("href"));
                                if (e.ctrlKey) {
                                    url.open();
                                }
                                else {
                                    var dialogRoot_1 = $("<div></div>");
                                    var dialogOptions = {
                                        title: "Move SKU",
                                        actions: ["close"],
                                        content: url.value,
                                        width: "800px",
                                        height: "auto",
                                        modal: true,
                                        visible: false,
                                        close: function () { return dialogRoot_1.data("kendoWindow").destroy(); },
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
                                                    case 0: return [4 /*yield*/, AjaxFormsManager.waitFor("InventoryMovementsCreate")];
                                                    case 1:
                                                        _a.sent();
                                                        if (this._movements && this._movements.activeTab) {
                                                            this._movements.activeTab.activate();
                                                        }
                                                        e.sender.close();
                                                        e.sender.destroy();
                                                        return [2 /*return*/];
                                                }
                                            });
                                        }); },
                                    };
                                    var dialog = dialogRoot_1.kendoWindow(dialogOptions).data("kendoWindow");
                                    dialog.center();
                                    dialog.open();
                                }
                            });
                        };
                        return InventoryMovementsIndexPage;
                    }());
                    Index.InventoryMovementsIndexPage = InventoryMovementsIndexPage;
                })(Index = InventoryMovements.Index || (InventoryMovements.Index = {}));
            })(InventoryMovements = Pages.InventoryMovements || (Pages.InventoryMovements = {}));
        })(Pages = CRM.Pages || (CRM.Pages = {}));
    })(CRM = DentalDrill.CRM || (DentalDrill.CRM = {}));
})(DentalDrill || (DentalDrill = {}));
//# sourceMappingURL=InventoryMovementsIndexPage.js.map