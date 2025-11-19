namespace DentalDrill.CRM.Controls.CallbackFeed {
    import NotificationsConnection = DentalDrill.CRM.Controls.NotificationsFeed.NotificationsConnection;

    export interface CallbackReadModel {
        Id: string;
        CallDateTime?: Date;
        ClientId: string;
        ClientName: string;
        Note: string;
    }

    export class CallbackFeedListView {
        private readonly _dataSource: kendo.data.DataSource;
        private readonly _listView: kendo.ui.ListView;
        private readonly _pager: kendo.ui.Pager;
        private readonly _template: (data: any) => string;

        private constructor(root: JQuery) {
            this._template = this.loadTemplate(root);
            this._dataSource = this.createDataSource(root);
            this._listView = this.createListView(root);
            this._pager = this.createPager(root);

            root.on("click", ".callback__actions__read", async (e: JQueryEventObject) => {
                e.preventDefault();
                const id = $(e.target).closest(".callback").attr("data-id");
                const response = await fetch(`/Callback/MarkAsRead/${id}`, {
                    method: "POST",
                    credentials: "same-origin",
                    cache: "no-cache",
                    body: ""
                });

                this._dataSource.read();
            });

            NotificationsConnection.instance.callbackUpdated.subscribe((sender, args) => {
                this._dataSource.read();
            });
        }

        static create(root: JQuery): CallbackFeedListView {
            const instance = new CallbackFeedListView(root);
            root.data("CallbackFeedListView", instance);
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
        }

        private createListView(root: JQuery): kendo.ui.ListView {
            const element = root.find(".callback-feed__list-view");
            element.kendoListView({
                dataSource: this._dataSource,
                template: this._template,
                dataBound: (e: kendo.ui.ListViewEvent) => {
                    if (e.sender.dataSource.data().length > 0) {
                        e.sender.element.find(".no-data").remove();
                    } else {
                        e.sender.element.append($("<div class=\"no-data\">No Callbacks</div>"));
                    }
                }
            });

            return element.data("kendoListView");
        }

        private createPager(root: JQuery): kendo.ui.Pager {
            const element = root.find(".callback-feed__pager");
            element.kendoPager({
                dataSource: this._dataSource,
                numeric: false,
                info: false,
                refresh: true,
            });

            const linkFullVersion = root.attr("data-link-full-version");
            if (linkFullVersion && linkFullVersion.toLowerCase() === "true") {
                const notificationsLink = $("<a></a>")
                    .attr("href", "/Callback")
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

        private loadTemplate(root: JQuery): (data: any) => string {
            const element = root.find(".callback-feed__template");
            return kendo.template(element.html());
        }
    }

    $(() => {
        const feeds = $(".callback-feed.callback-feed--init");
        feeds.each((index, element) => {
            CallbackFeedListView.create($(element));
        });
    });
}