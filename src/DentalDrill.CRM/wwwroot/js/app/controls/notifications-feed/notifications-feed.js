var DentalDrill;
(function (DentalDrill) {
    var CRM;
    (function (CRM) {
        var Controls;
        (function (Controls) {
            var NotificationsFeed;
            (function (NotificationsFeed) {
                var NotificationType;
                (function (NotificationType) {
                    NotificationType[NotificationType["Unknown"] = 0] = "Unknown";
                    NotificationType[NotificationType["JobCreated"] = 1] = "JobCreated";
                    NotificationType[NotificationType["JobEstimated"] = 2] = "JobEstimated";
                    NotificationType[NotificationType["JobApprovedForRepair"] = 3] = "JobApprovedForRepair";
                    NotificationType[NotificationType["JobRepairComplete"] = 4] = "JobRepairComplete";
                    NotificationType[NotificationType["HandpieceStoreOrderCreated"] = 5] = "HandpieceStoreOrderCreated";
                })(NotificationType = NotificationsFeed.NotificationType || (NotificationsFeed.NotificationType = {}));
                var NotificationsListViewTemplate = /** @class */ (function () {
                    function NotificationsListViewTemplate(type, template) {
                        this._type = type;
                        this._template = template;
                    }
                    Object.defineProperty(NotificationsListViewTemplate.prototype, "type", {
                        get: function () {
                            return this._type;
                        },
                        enumerable: false,
                        configurable: true
                    });
                    NotificationsListViewTemplate.prototype.render = function (data) {
                        return this._template(data);
                    };
                    return NotificationsListViewTemplate;
                }());
                NotificationsFeed.NotificationsListViewTemplate = NotificationsListViewTemplate;
                var NotificationsListViewTemplateCollection = /** @class */ (function () {
                    function NotificationsListViewTemplateCollection(list) {
                        this._list = [];
                        this._map = new Map();
                        for (var i = 0; i < list.length; i++) {
                            var item = list[i];
                            this._list.push(item);
                            this._map.set(item.type, item);
                        }
                    }
                    NotificationsListViewTemplateCollection.prototype.resolve = function (type) {
                        return this._map.get(type);
                    };
                    return NotificationsListViewTemplateCollection;
                }());
                NotificationsFeed.NotificationsListViewTemplateCollection = NotificationsListViewTemplateCollection;
                var NotificationsListView = /** @class */ (function () {
                    function NotificationsListView(root) {
                        var _this = this;
                        this._dataSource = this.createDataSource(root);
                        this._listView = this.createListView(root);
                        this._pager = this.createPager(root);
                        this._templates = this.loadTemplates(root);
                        NotificationsFeed.NotificationsConnection.instance.updated.subscribe(function (sender, args) {
                            _this._dataSource.read();
                        });
                    }
                    NotificationsListView.create = function (root) {
                        var instance = new NotificationsListView(root);
                        root.data("NotificationsListView", instance);
                        return instance;
                    };
                    NotificationsListView.prototype.createDataSource = function (root) {
                        var pageSizeVal = root.attr("data-page-size");
                        var pageSize = 5;
                        if (pageSizeVal) {
                            pageSize = parseInt(pageSizeVal);
                        }
                        var showRead = root.attr("data-show-read");
                        return new kendo.data.DataSource({
                            type: "aspnetmvc-ajax",
                            transport: {
                                read: { url: "/Notifications/Read" + (showRead === "True" ? "?showRead=true" : "") }
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
                                        CreatedOn: { type: "Date" },
                                        ReadOn: { type: "Date" },
                                        ResolvedOn: { type: "Date" },
                                        Type: { type: "number" },
                                        Payload: { type: "object" },
                                        Status: { type: "number" }
                                    }
                                }
                            },
                            filter: [],
                            sort: [
                                { field: "CreatedOn", dir: "asc" }
                            ]
                        });
                    };
                    NotificationsListView.prototype.createListView = function (root) {
                        var element = root.find(".notifications-feed__list-view");
                        element.kendoListView({
                            dataSource: this._dataSource,
                            template: this.renderTemplate.bind(this),
                            dataBound: function (e) {
                                if (e.sender.dataSource.data().length > 0) {
                                    e.sender.element.find(".no-data").remove();
                                }
                                else {
                                    e.sender.element.append($("<div class=\"no-data\">No Alerts</div>"));
                                }
                            }
                        });
                        return element.data("kendoListView");
                    };
                    NotificationsListView.prototype.createPager = function (root) {
                        var element = root.find(".notifications-feed__pager");
                        element.kendoPager({
                            dataSource: this._dataSource,
                            numeric: false,
                            info: false,
                            refresh: true,
                        });
                        var linkFullVersion = root.attr("data-link-full-version");
                        if (linkFullVersion && linkFullVersion.toLowerCase() === "true") {
                            var notificationsLink_1 = $("<a></a>")
                                .attr("href", "/Notifications")
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
                    NotificationsListView.prototype.loadTemplates = function (root) {
                        var element = root.find(".notifications-feed__templates");
                        var templates = element.find(".notifications-feed__templates__item");
                        var loaded = [];
                        templates.each(function (index, templateElement) {
                            var type = NotificationType[templateElement.getAttribute("data-type")];
                            var content = templateElement.innerHTML;
                            var template = kendo.template(content);
                            if (type && content && template) {
                                loaded.push(new NotificationsListViewTemplate(type, template));
                            }
                            else {
                                throw new Error("Failed to load template!");
                            }
                        });
                        return new NotificationsListViewTemplateCollection(loaded);
                    };
                    NotificationsListView.prototype.renderTemplate = function (data) {
                        var type = NotificationType[NotificationType[data.Type]];
                        var template = this._templates.resolve(type);
                        return template.render(data);
                    };
                    return NotificationsListView;
                }());
                NotificationsFeed.NotificationsListView = NotificationsListView;
                $(function () {
                    var feeds = $(".notifications-feed.notifications-feed--init");
                    feeds.each(function (index, element) {
                        NotificationsListView.create($(element));
                    });
                });
            })(NotificationsFeed = Controls.NotificationsFeed || (Controls.NotificationsFeed = {}));
        })(Controls = CRM.Controls || (CRM.Controls = {}));
    })(CRM = DentalDrill.CRM || (DentalDrill.CRM = {}));
})(DentalDrill || (DentalDrill = {}));
//# sourceMappingURL=notifications-feed.js.map