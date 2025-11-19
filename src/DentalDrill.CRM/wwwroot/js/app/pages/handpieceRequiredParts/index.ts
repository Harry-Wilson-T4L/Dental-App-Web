namespace DentalDrill.CRM.Pages.HandpieceRequiredParts.Index {
    import GridHandlers = DevGuild.AspNet.Controls.Grids.Handlers.GridHandlers;
    import AjaxFormsManager = DevGuild.AspNet.Forms.AjaxForms.AjaxFormsManager;
    import EventHandler = DevGuild.Utilities.EventHandler;

    export enum HandpieceRequiredPartStatus {
        Unknown,
        Waiting,
        WaitingRequested,
        WaitingApproved,
        WaitingOrdered,
        Allocated,
        Completed,
        Cancelled,
    }

    export interface HandpieceRequiredPartReadModel {
        Id: string;
        Date: Date;
        Status: HandpieceRequiredPartStatus;
        SKUId: string;
        SKUName: string;
        RequiredQuantity: number;
        ShelfQuantity: number;
        Price: number;
    }

    export class HandpieceRequiredPartStatusHelper {
        static toDisplayName(value: HandpieceRequiredPartStatus): string {
            switch (value) {
                case HandpieceRequiredPartStatus.Unknown:
                    return "Unknown";
                case HandpieceRequiredPartStatus.Waiting:
                    return "Waiting";
                case HandpieceRequiredPartStatus.WaitingRequested:
                    return "Requested";
                case HandpieceRequiredPartStatus.WaitingApproved:
                    return "Approved";
                case HandpieceRequiredPartStatus.WaitingOrdered:
                    return "Ordered";
                case HandpieceRequiredPartStatus.Allocated:
                    return "Allocated";
                case HandpieceRequiredPartStatus.Completed:
                    return "Completed";
                case HandpieceRequiredPartStatus.Cancelled:
                    return "Cancelled";
                default:
                    return HandpieceRequiredPartStatus[value];
            }
        }

        static toDisplayColor(value: HandpieceRequiredPartStatus): string {
            switch (value) {
                case HandpieceRequiredPartStatus.Unknown:
                    return "background-color: #ced3db; color: #212529;"
                case HandpieceRequiredPartStatus.Waiting:
                    return "background-color: #ffc107; color: #212529;"
                case HandpieceRequiredPartStatus.WaitingRequested:
                    return "background-color: #ffc107; color: #212529;"
                case HandpieceRequiredPartStatus.WaitingApproved:
                    return "background-color: #ffc107; color: #212529;"
                case HandpieceRequiredPartStatus.WaitingOrdered:
                    return "background-color: #ffc107; color: #212529;"
                case HandpieceRequiredPartStatus.Allocated:
                    return "background-color: #28a745; color: white;"
                case HandpieceRequiredPartStatus.Completed:
                    return "background-color: #28a745; color: white;"
                case HandpieceRequiredPartStatus.Cancelled:
                    return "background-color: #ced3db; color: #212529;"
                default:
                    return "background-color: #ced3db; color: #212529;";
            }
        }
    }

    export class HandpieceRequiredPartIndexPage {
        private readonly _root: HTMLElement;
        private readonly _rootNode: JQuery<HTMLElement>;
        private readonly _handpieceId: string;

        private _grid: HandpieceRequiredPartGrid;

        constructor(root: HTMLElement, handpieceId: string) {
            this._root = root;
            this._rootNode = $(root);
            this._handpieceId = handpieceId;
        }

        init(): void {
            const gridWrapper = this._rootNode.find(".handpiece-required-parts__grid-wrapper");
            this._grid = new HandpieceRequiredPartGrid(this._handpieceId, gridWrapper);
            this._grid.init();

            AjaxFormsManager.htmlEvents.subscribe("HandpieceRequiredPartsCreate", async e => {
                if (e.Success === true) {
                    await this._grid.refresh();
                }
            });
        }
    }

    export class HandpieceRequiredPartGrid {
        private readonly _handpieceId: string;
        private readonly _rootNode: JQuery<HTMLElement>;
        private readonly _updated: EventHandler<HandpieceRequiredPartReadModel[]> = new EventHandler<HandpieceRequiredPartReadModel[]>();

        private _grid: kendo.ui.Grid;

        constructor(handpieceId: string, rootNode: JQuery<HTMLElement>) {
            this._handpieceId = handpieceId;
            this._rootNode = rootNode;
        }

        get updated(): EventHandler<HandpieceRequiredPartReadModel[]> {
            return this._updated;
        }

        init() {
            this._rootNode.addClass("k-grid--dense");
            this._rootNode.addClass("k-grid--small-text");
            this._rootNode.kendoGrid({
                dataSource: this.createDataSource(),
                columns: this.initializeColumns(),
            });

            this._grid = this._rootNode.data("kendoGrid");
        }

        async refresh(): Promise<any> {
            await this._grid.dataSource.read();
            const data = this._grid.dataSource.data<HandpieceRequiredPartReadModel>();
            this._updated.raise(this, data.map(x => x as HandpieceRequiredPartReadModel));
        }
        
        private createDataSource(): kendo.data.DataSource {
            const dataSource = new kendo.data.DataSource({
                type: "aspnetmvc-ajax",
                transport: {
                    read: {
                        url: `/HandpieceRequiredParts/Read?parentId=${this._handpieceId}`
                    },
                },
                schema: {
                    data: "Data",
                    total: "Total",
                    errors: "Errors",
                    model: {
                        id: "Id",
                        fields: {
                            "Date": { type: "Date" },
                            "Status": { type: "number" },
                            "SKUId": { type: "string" },
                            "SKUName": { type: "string" }, 
                            "RequiredQuantity": { type: "number" },
                            "ShelfQuantity": { type: "number" },
                            "Price": { type: "number" },
                        },
                    },
                },
                serverAggregates: true,
                serverFiltering: true,
                serverGrouping: true,
                serverPaging: true,
                serverSorting: true,
            });

            return dataSource;
        }

        private initializeColumns(): kendo.ui.GridColumn[] {
            const columns: kendo.ui.GridColumn[] = [];

            columns.push({
                title: "Date",
                field: "Date",
                width: "50px",
                format: "{0:d}",
            });
            columns.push({
                title: "Status",
                field: "Status",
                width: "50px",
                template: (data: HandpieceRequiredPartReadModel) => {
                    return `<span class="badge" style="${HandpieceRequiredPartStatusHelper.toDisplayColor(data.Status)}">${HandpieceRequiredPartStatusHelper.toDisplayName(data.Status)}</span>`;
                },
            });
            columns.push({
                title: "SKU",
                field: "SKUName",
                width: "150px",
            });
            columns.push({
                title: "QTY",
                field: "RequiredQuantity",
                width: "40px",
                template: (data: HandpieceRequiredPartReadModel) => {
                    return `${data.RequiredQuantity}/${data.ShelfQuantity}`;
                }
            });
            columns.push({
                title: "Price",
                field: "Price",
                width: "40px",
                template: (data: HandpieceRequiredPartReadModel) => {
                    if (data.Price !== undefined && data.Price !== null) {
                        return `$${data.Price}`;
                    } else {
                        return ``;
                    }
                }
            });
            columns.push({
                title: "Actions",
                width: "120px",
                command: [
                    {
                        name: "CustomOpenSKU",
                        iconClass: "fas fa-link",
                        text: "&nbsp; Movements",
                        click: e => {
                            e.preventDefault();
                            const dataItem = this._grid.dataItem<HandpieceRequiredPartReadModel>(e.currentTarget.closest("tr"));
                            const url = new DevGuild.AspNet.Routing.Uri(`/InventoryMovements?sku=${dataItem.SKUId}`);
                            url.open();
                        },
                    },
                    {
                        name: "CustomSwapAllocation",
                        iconClass: "fas fa-exchange-alt",
                        text: "&nbsp; Swap",
                        click: GridHandlers.createButtonClickPopupHandler<HandpieceRequiredPartReadModel>(
                            item => {
                                switch (item.Status) {
                                    case HandpieceRequiredPartStatus.Allocated:
                                        return new DevGuild.AspNet.Routing.Uri(`/HandpieceRequiredParts/DeallocateFrom?handpiece=${this._handpieceId}&sku=${item.SKUId}`);
                                    case HandpieceRequiredPartStatus.Waiting:
                                    case HandpieceRequiredPartStatus.WaitingRequested:
                                    case HandpieceRequiredPartStatus.WaitingApproved:
                                    case HandpieceRequiredPartStatus.WaitingOrdered:
                                        return new DevGuild.AspNet.Routing.Uri(`/HandpieceRequiredParts/AllocateTo?handpiece=${this._handpieceId}&sku=${item.SKUId}`);
                                    default:
                                        return undefined;
                                }
                            },
                            item => {
                                let formId: string = undefined;
                                switch (item.Status) {
                                    case HandpieceRequiredPartStatus.Allocated:
                                        formId = "HandpieceRequiredPartsDeallocateFrom";
                                        break;
                                    case HandpieceRequiredPartStatus.Waiting:
                                    case HandpieceRequiredPartStatus.WaitingRequested:
                                    case HandpieceRequiredPartStatus.WaitingApproved:
                                    case HandpieceRequiredPartStatus.WaitingOrdered:
                                        formId = "HandpieceRequiredPartsAllocateTo";
                                        break;
                                }
                                return {
                                    title: `Swap`,
                                    width: `900px`,
                                    height: `auto`,
                                    refresh: (e: kendo.ui.WindowEvent) => {
                                        e.sender.wrapper.on("click", ".editor__submit__cancel", clickEvent => {
                                            clickEvent.preventDefault();
                                            e.sender.close();
                                            e.sender.destroy();
                                        });
                                        e.sender.center();
                                    },
                                    open: async (e: kendo.ui.WindowEvent) => {
                                        await AjaxFormsManager.waitFor(formId);
                                        await this.refresh();
                                        e.sender.close();
                                        e.sender.destroy();
                                    },
                                };
                            }),
                    }
                ]
            })

            return columns;
        }
    }
}