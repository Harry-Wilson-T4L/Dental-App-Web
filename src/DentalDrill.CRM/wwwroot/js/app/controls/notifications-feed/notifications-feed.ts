namespace DentalDrill.CRM.Controls.NotificationsFeed {
    export interface NotificationReadModel {
        Id: number;
        CreatedOn: Date;
        ReadOn?: Date;
        ResolvedOn?: Date;
        Type: number;
        Payload: any;
        Status: number;
    }

    export enum NotificationType {
        Unknown = 0,
        JobCreated = 1,
        JobEstimated = 2,
        JobApprovedForRepair = 3,
        JobRepairComplete = 4,
        HandpieceStoreOrderCreated = 5,
    }

    export class NotificationsListViewTemplate {
        private readonly _type: NotificationType;
        private readonly _template: (data: any) => string;

        constructor(type: NotificationType, template: (data: any) => string) {
            this._type = type;
            this._template = template;
        }

        get type(): NotificationType {
            return this._type;
        }

        render(data: any): string {
            return this._template(data);
        }
    }

    export class NotificationsListViewTemplateCollection {
        private readonly _list: NotificationsListViewTemplate[];
        private readonly _map: Map<NotificationType, NotificationsListViewTemplate>;

        constructor(list: NotificationsListViewTemplate[]) {
            this._list = [];
            this._map = new Map<NotificationType, NotificationsListViewTemplate>();

            for (let i = 0; i < list.length; i++) {
                const item = list[i];

                this._list.push(item);
                this._map.set(item.type, item);
            }
        }

        resolve(type: NotificationType): NotificationsListViewTemplate {
            return this._map.get(type);
        }
    }

    export class NotificationsListView {
        private readonly _dataSource: kendo.data.DataSource;
        private readonly _listView: kendo.ui.ListView;
        private readonly _pager: kendo.ui.Pager;
        private readonly _templates: NotificationsListViewTemplateCollection;

        private constructor(root: JQuery) {
            this._dataSource = this.createDataSource(root);
            this._listView = this.createListView(root);
            this._pager = this.createPager(root);
            this._templates = this.loadTemplates(root);

            NotificationsConnection.instance.updated.subscribe((sender, args) => {
                this._dataSource.read();
            });
        }

        static create(root: JQuery): NotificationsListView {
            const instance = new NotificationsListView(root);
            root.data("NotificationsListView", instance);
            return instance;
        }

        private createDataSource(root: JQuery): kendo.data.DataSource {
            const pageSizeVal = root.attr("data-page-size");
            let pageSize = 5;
            if (pageSizeVal) {
                pageSize = parseInt(pageSizeVal);
            }

            const showRead = root.attr("data-show-read");

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
        }

        private createListView(root: JQuery): kendo.ui.ListView {
            const element = root.find(".notifications-feed__list-view");
            element.kendoListView({
                dataSource: this._dataSource,
                template: this.renderTemplate.bind(this),
                dataBound: (e: kendo.ui.ListViewEvent) => {
                    if (e.sender.dataSource.data().length > 0) {
                        e.sender.element.find(".no-data").remove();
                    } else {
                        e.sender.element.append($("<div class=\"no-data\">No Alerts</div>"));
                    }
                }
            });

            return element.data("kendoListView");
        }

        private createPager(root: JQuery): kendo.ui.Pager {
            const element = root.find(".notifications-feed__pager");
            element.kendoPager({
                dataSource: this._dataSource,
                numeric: false,
                info: false,
                refresh: true,
            });

            const linkFullVersion = root.attr("data-link-full-version");
            if (linkFullVersion && linkFullVersion.toLowerCase() === "true") {
                const notificationsLink = $("<a></a>")
                    .attr("href", "/Notifications")
                    .attr("class", "k-link")
                    .attr("style", "order: 100;")
                    .attr("title", "Open Feed")
                    .append($("<span></span>").addClass("fas fa-fw fa-external-link-alt"));

                notificationsLink.on("click", e => {
                    window.location.href = notificationsLink.attr("href");
                });

                element.append(notificationsLink);
            }

            return element.data("kendoPager");
        }

        private loadTemplates(root: JQuery): NotificationsListViewTemplateCollection {
            const element = root.find(".notifications-feed__templates");
            const templates = element.find(".notifications-feed__templates__item");
            const loaded: NotificationsListViewTemplate[] = [];

            templates.each((index, templateElement) => {
                const type = NotificationType[templateElement.getAttribute("data-type")];
                const content = templateElement.innerHTML;
                const template = kendo.template(content);
                if (type && content && template) {
                    loaded.push(new NotificationsListViewTemplate(type, template));
                } else {
                    throw new Error("Failed to load template!");
                }
            });

            return new NotificationsListViewTemplateCollection(loaded);
        }

        private renderTemplate(data: NotificationReadModel): string {
            const type = NotificationType[NotificationType[data.Type]];
            const template = this._templates.resolve(type);
            return template.render(data);
        }
    }

    $(() => {
        const feeds = $(".notifications-feed.notifications-feed--init");
        feeds.each((index, element) => {
            NotificationsListView.create($(element));
        });
    });
}