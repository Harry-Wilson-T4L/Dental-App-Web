namespace DentalDrill.CRM.Pages.Clients.Details {
    import GridHandlers = DevGuild.AspNet.Controls.Grids.Handlers.GridHandlers;

    interface FeedbackFormQuestion {
        id: string;
        type: number;
        shortName: string;
    }

    enum FeedbackFormStatus {
        New,
        Completed,
        Expired,
        Cancelled,
    }

    interface ClientFeedbackFormReadModel {
        Id: string;
        Status: FeedbackFormStatus;
        CreatedOn: Date;
        SentOn: Date;
        TotalRating: number;
        Answers: object;
    }

    export class ClientFeedbackFormsGrid {
        private static _instance: ClientFeedbackFormsGrid;

        private readonly _root: HTMLElement;
        private readonly _clientId: string;
        private readonly _questions: FeedbackFormQuestion[];
        private _grid: kendo.ui.Grid;

        constructor(root: HTMLElement, clientId: string, questions: FeedbackFormQuestion[]) {
            this._root = root;
            this._clientId = clientId;
            this._questions = questions;
        }

        init(): void {
            this._grid = this.createGrid();
        }

        static initialize(root: HTMLElement, clientId: string, questions: FeedbackFormQuestion[]) : ClientFeedbackFormsGrid {
            const obj = new ClientFeedbackFormsGrid(root, clientId, questions);
            obj.init();
            ClientFeedbackFormsGrid._instance = obj;
            return obj;
        }

        static get instance(): ClientFeedbackFormsGrid {
            return ClientFeedbackFormsGrid._instance;
        }

        static handleSendButton = GridHandlers.createGridButtonClickPopupHandler(
            ".client-feedback-send-button",
            target => new DevGuild.AspNet.Routing.Uri(target.attr("href")),
            target => {
                return {
                    title: `Send new feedback form`,
                    width: "1000px",
                    height: "auto",
                    refresh: (e: kendo.ui.WindowEvent) => {
                        e.sender.wrapper.on("click", ".editor__submit__cancel", clickEvent => {
                            clickEvent.preventDefault();
                            e.sender.close();
                            e.sender.destroy();
                        });

                        e.sender.center();
                    },
                    open: async (e: kendo.ui.WindowEvent) => {
                        await DevGuild.AspNet.Forms.AjaxForms.AjaxFormsManager.waitFor(`ClientFeedbackFormsSendNewForm`);
                        await ClientFeedbackFormsGrid.instance._grid.dataSource.read();
                        e.sender.close();
                        e.sender.destroy();
                    }
                };
            });

        private createGrid(): kendo.ui.Grid {
            const dataSource = this.createDataSource();
            const gridContainer = $(this._root).find(".grid-container");
            gridContainer.kendoGrid({
                dataSource: dataSource,
                columns: this.initializeColumns(),
                pageable: true,
            });

            const grid = gridContainer.data("kendoGrid");
            return grid;
        }

        private initializeColumns(): kendo.ui.GridColumn[] {
            const columns: kendo.ui.GridColumn[] = [];
            columns.push({
                field: `CreatedOn`,
                title: `Created On`,
                template: `#if (data.CreatedOn) {# #:kendo.toString(data.CreatedOn, "d")#<br />#:kendo.toString(data.CreatedOn, "t")# #}#`,
            });
            columns.push({
                field: `SentOn`,
                title: `Sent On`,
                template: `#if (data.SentOn) {# #:kendo.toString(data.SentOn, "d")#<br />#:kendo.toString(data.SentOn, "t")# #}#`,
            });
            columns.push({
                field: `Status`,
                title: `Status`,
                template: function(data: ClientFeedbackFormReadModel): string {
                    if (data === undefined || data === null) {
                        return ``;
                    }

                    switch (data.Status) {
                        case FeedbackFormStatus.New:
                            return `New`;
                        case FeedbackFormStatus.Completed:
                            return `Completed`;
                        case FeedbackFormStatus.Expired:
                            return `Expired`;
                        case FeedbackFormStatus.Cancelled:
                            return `Cancelled`;
                    }
                }
            });
            columns.push({
                field: `TotalRating`,
                title: `Total Rating`,
            });

            for (let question of this._questions) {
                const fieldName = `Answers_${question.id.replace(/\-/g, "")}`;
                switch (question.type) {
                    case 0:
                        columns.push({
                            field: fieldName,
                            title: question.shortName
                        });
                        break;
                    case 1:
                        columns.push({
                            field: fieldName,
                            title: question.shortName
                        });
                        break;
                }
            }

            columns.push({
                command: [
                    {
                        name: "CustomDetails",
                        text: `<span class="fas fa-fw fa-external-link-alt"></span>`,
                        click: GridHandlers.createButtonClickPopupHandler<ClientFeedbackFormReadModel>(
                            item => routes.clientFeedback.details(item.Id),
                            item => ({
                                title: `Feedback form`,
                                width: "1000px",
                                height: "auto",
                                refresh: (e: kendo.ui.WindowEvent) => {
                                    e.sender.wrapper.on("click", ".editor__submit__cancel", clickEvent => {
                                        clickEvent.preventDefault();
                                        e.sender.close();
                                        e.sender.destroy();
                                    });

                                    e.sender.center();
                                }
                            }))
                    }
                ]
            });

            return columns;
        }

        private createDataSource(): kendo.data.DataSource {
            const dataSource = new kendo.data.DataSource({
                type: "aspnetmvc-ajax",
                transport: {
                    read: {
                        url: `/ClientFeedbackForms/Read?parentId=${this._clientId}`
                    }
                },
                pageSize: 20,
                schema: {
                    data: "Data",
                    total: "Total",
                    errors: "Errors",
                    model: {
                        id: "Id",
                        fields: this.initializeSchemaModel()
                    }
                },
                sort: [
                    { field: "CreatedOn", dir: "desc" }
                ]
            });

            return dataSource;
        }

        private initializeSchemaModel(): object {
            const fields = {
                CreatedOn: { type: "Date" },
                SentOn: { type: "Date" },
                Status: { type: "number" },
                TotalRating: { type: "number" },
            };

            for (let question of this._questions) {
                const fieldName = `Answers_${question.id.replace(/\-/g, "")}`;
                const fieldSource = `Answers["${question.id}"]`;
                switch (question.type) {
                    case 0:
                        fields[fieldName] = {
                            type: "number",
                            from: `${fieldSource}.IntegerValue`
                        };
                        break;
                    case 1:
                        fields[fieldName] = {
                            type: "string",
                            from: `${fieldSource}.StringValue`
                        };
                        break;
                }
            }

            return fields;
        }
    }
}