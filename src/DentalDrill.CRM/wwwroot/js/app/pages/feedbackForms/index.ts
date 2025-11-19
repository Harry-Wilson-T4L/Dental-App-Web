namespace DentalDrill.CRM.Pages.FeedbackForms.Index {
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

    interface FeedbackFormReadModel {
        Id: string;
        ClientId: string;
        ClientFullName: string;
        ClientName: string;
        ClientPrincipalDentist: string;
        ClientSuburb: string;
        Status: FeedbackFormStatus;
        CreatedOn: Date;
        SentOn: Date;
        TotalRating: number;
        Answers: object;
    }

    export class FeedbackFormsGrid {
        private static _instance: FeedbackFormsGrid;

        private readonly _root: HTMLElement;
        private readonly _questions: FeedbackFormQuestion[];
        private _grid: kendo.ui.Grid;

        constructor(root: HTMLElement, questions: FeedbackFormQuestion[]) {
            this._root = root;
            this._questions = questions;
        }

        init(): void {
            this._grid = this.createGrid();
        }

        static initialize(root: HTMLElement, questions: FeedbackFormQuestion[]) : FeedbackFormsGrid {
            const obj = new FeedbackFormsGrid(root, questions);
            obj.init();
            FeedbackFormsGrid._instance = obj;
            return obj;
        }

        static get instance(): FeedbackFormsGrid {
            return FeedbackFormsGrid._instance;
        }

        private createGrid(): kendo.ui.Grid {
            const dataSource = this.createDataSource();
            const gridContainer = $(this._root).find(".grid-container");
            gridContainer.kendoGrid({
                height: 630,
                dataSource: dataSource,
                columns: this.initializeColumns(),
                pageable: true,
                sortable: true,
                filterable: {
                    mode: "menu",
                    extra: false,
                    operators: {
                        date: {
                            gt: "After",
                            lt: "Before",
                        },
                        string: {
                            contains: "Contains",
                        },
                        number: {
                            gt: ">",
                            gte: ">=",
                            lt: "<",
                            lte: "<=",
                        },
                    },
                },
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
                filterable: {
                    extra: true,
                    operators: {
                        date: {
                            gt: "After",
                            lt: "Before",
                        },
                    },
                },
                width: 70,
            });
            columns.push({
                field: `SentOn`,
                title: `Sent On`,
                template: `#if (data.SentOn) {# #:kendo.toString(data.SentOn, "d")#<br />#:kendo.toString(data.SentOn, "t")# #}#`,
                filterable: {
                    extra: true,
                    operators: {
                        date: {
                            gt: "After",
                            lt: "Before",
                        },
                    },
                },
                width: 70,
            });
            columns.push({
                field: `ClientFullName`,
                title: `Client`,
                template: `#:data.ClientName#<br/>#:data.ClientPrincipalDentist#<br/>#:data.ClientSuburb#`,
                width: 150,
            });
            columns.push({
                field: `Status`,
                title: `Status`,
                template: function(data: FeedbackFormReadModel): string {
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
                },
                filterable: {
                    multi: true,
                    dataSource: [
                        { Status: 0, StatusName: "New" },
                        { Status: 1, StatusName: "Completed" },
                        { Status: 2, StatusName: "Expired" },
                        { Status: 3, StatusName: "Cancelled" },
                    ],
                    itemTemplate: e => `<label class="k-label"><input type="checkbox" class="" value="#:(data.all ? '' : data.Status)#"><span class="k-item-title">#:(data.all ? data.all : data.StatusName)#</span></label>`,
                },
                width: 80,
            });
            columns.push({
                field: `TotalRating`,
                title: `Total Rating`,
                width: 50,
            });

            for (let question of this._questions) {
                const fieldName = `Answers_${question.id.replace(/\-/g, "")}`;
                switch (question.type) {
                    case 0:
                        columns.push({
                            field: fieldName,
                            title: question.shortName,
                            sortable: false,
                            filterable: false,
                            width: 50,
                        });
                        break;
                    case 1:
                        columns.push({
                            field: fieldName,
                            title: question.shortName,
                            sortable: false,
                            filterable: false,
                            width: 100,
                        });
                        break;
                }
            }

            columns.push({
                command: [
                    {
                        name: "CustomDetails",
                        text: `<span class="fas fa-fw fa-external-link-alt"></span>`,
                        click: GridHandlers.createButtonClickPopupHandler<FeedbackFormReadModel>(
                            item => routes.feedbackForms.details(item.Id),
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
                ],
                width: 50,
            });

            return columns;
        }

        private createDataSource(): kendo.data.DataSource {
            const dataSource = new kendo.data.DataSource({
                type: "aspnetmvc-ajax",
                transport: {
                    read: {
                        url: routes.feedbackForms.read().value,
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
                serverAggregates: true,
                serverFiltering: true,
                serverGrouping: true,
                serverPaging: true,
                serverSorting: true,
                sort: [
                    { field: "CreatedOn", dir: "desc" }
                ]
            });

            return dataSource;
        }

        private initializeSchemaModel(): object {
            const fields = {
                CreatedOn: { type: "date" },
                SentOn: { type: "date" },
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