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
                var NotificationsConnection = DentalDrill.CRM.Controls.NotificationsFeed.NotificationsConnection;
                var CallbackFeedListView = /** @class */ (function () {
                    function CallbackFeedListView(root) {
                        var _this = this;
                        this._template = this.loadTemplate(root);
                        this._dataSource = this.createDataSource(root);
                        this._listView = this.createListView(root);
                        this._pager = this.createPager(root);
                        root.on("click", ".callback__actions__read", function (e) { return __awaiter(_this, void 0, void 0, function () {
                            var id, response;
                            return __generator(this, function (_a) {
                                switch (_a.label) {
                                    case 0:
                                        e.preventDefault();
                                        id = $(e.target).closest(".callback").attr("data-id");
                                        return [4 /*yield*/, fetch("/Callback/MarkAsRead/" + id, {
                                                method: "POST",
                                                credentials: "same-origin",
                                                cache: "no-cache",
                                                body: ""
                                            })];
                                    case 1:
                                        response = _a.sent();
                                        this._dataSource.read();
                                        return [2 /*return*/];
                                }
                            });
                        }); });
                        NotificationsConnection.instance.callbackUpdated.subscribe(function (sender, args) {
                            _this._dataSource.read();
                        });
                    }
                    CallbackFeedListView.create = function (root) {
                        var instance = new CallbackFeedListView(root);
                        root.data("CallbackFeedListView", instance);
                        return instance;
                    };
                    CallbackFeedListView.prototype.createDataSource = function (root) {
                        var pageSizeVal = root.attr("data-page-size");
                        var pageSize = 5;
                        if (pageSizeVal) {
                            pageSize = parseInt(pageSizeVal);
                        }
                        var showRead = root.attr("data-show-read");
                        return new kendo.data.DataSource({
                            type: "aspnetmvc-ajax",
                            transport: {
                                read: { url: "/Callback/Read" + (showRead === "True" ? "?showRead=true" : "") }
                            },
                            pageSize: pageSize,
                            page: 1,
                            serverPaging: true,
                            serverSorting: true,
                            serverFiltering: true,
                            serverGrouping: true,
                            serverAggregates: true,
                            schema: {
                                data: "Data",
                                total: "Total",
                                errors: "Errors",
                                model: {
                                    id: "Id",
                                    fields: {
                                        Id: { type: "string" },
                                        CallDateTime: { type: "Date" },
                                        ClientId: { type: "string" },
                                        ClientName: { type: "string" },
                                        Note: { type: "string" }
                                    }
                                }
                            },
                            filter: [],
                            sort: [
                                { field: "CallDateTime", dir: "asc" }
                            ]
                        });
                    };
                    CallbackFeedListView.prototype.createListView = function (root) {
                        var element = root.find(".callback-feed__list-view");
                        element.kendoListView({
                            dataSource: this._dataSource,
                            template: this._template,
                            dataBound: function (e) {
                                if (e.sender.dataSource.data().length > 0) {
                                    e.sender.element.find(".no-data").remove();
                                }
                                else {
                                    e.sender.element.append($("<div class=\"no-data\">No Callbacks</div>"));
                                }
                            }
                        });
                        return element.data("kendoListView");
                    };
                    CallbackFeedListView.prototype.createPager = function (root) {
                        var element = root.find(".callback-feed__pager");
                        element.kendoPager({
                            dataSource: this._dataSource,
                            numeric: false,
                            info: false,
                            refresh: true,
                        });
                        var linkFullVersion = root.attr("data-link-full-version");
                        if (linkFullVersion && linkFullVersion.toLowerCase() === "true") {
                            var notificationsLink_1 = $("<a></a>")
                                .attr("href", "/Callback")
                                .attr("class", "k-link")
                                .attr("style", "order: 100;")
                                .attr("title", "Open Feed")
                                .append($("<span></span>").addClass("fas fa-fw fa-external-link-alt"));
                            notificationsLink_1.on("click", function (e) {
                                window.location.href = notificationsLink_1.attr("href");
                            });
                            element.append(notificationsLink_1);
                        }
                        return element.data("kendoPager");
                    };
                    CallbackFeedListView.prototype.loadTemplate = function (root) {
                        var element = root.find(".callback-feed__template");
                        return kendo.template(element.html());
                    };
                    return CallbackFeedListView;
                }());
                CallbackFeed.CallbackFeedListView = CallbackFeedListView;
                $(function () {
                    var feeds = $(".callback-feed.callback-feed--init");
                    feeds.each(function (index, element) {
                        CallbackFeedListView.create($(element));
                    });
                });
            })(CallbackFeed = Controls.CallbackFeed || (Controls.CallbackFeed = {}));
        })(Controls = CRM.Controls || (CRM.Controls = {}));
    })(CRM = DentalDrill.CRM || (DentalDrill.CRM = {}));
})(DentalDrill || (DentalDrill = {}));
//# sourceMappingURL=callback-feed.js.map