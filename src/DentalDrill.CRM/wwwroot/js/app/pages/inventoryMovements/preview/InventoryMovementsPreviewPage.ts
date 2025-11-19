namespace DentalDrill.CRM.Pages.InventoryMovements.Preview {
    import GridHandlers = DevGuild.AspNet.Controls.Grids.Handlers.GridHandlers;
    import InventoryMovementReadModel = Index.InventoryMovementReadModel;
    import HandpieceStatus = Shared.HandpieceStatus;
    import HandpieceStatusHelper = Shared.HandpieceStatusHelper;

    export class InventoryMovementsPreviewPage {
        private readonly _root: HTMLElement;
        private readonly _options: InventoryMovementsPreviewOptions;

        private _dataSource: kendo.data.DataSource;
        private _grid: kendo.ui.Grid;

        constructor(root: HTMLElement, options: InventoryMovementsPreviewOptions) {
            this._root = root;
            this._options = options;
        }

        init() {
            this._dataSource = this.createDataSource(this.getUrlFromTab(), this._options.sku, this._options.workshop);
            this._grid = this.createGrid();
        }

        private createGrid(): kendo.ui.Grid {
            const container = this._root.querySelector(".grid-container") as HTMLElement;
            const grid = $(container)
                .css(`height`, `100%`)
                .addClass("k-grid--dense")
                .addClass("k-grid--small-text")
                .kendoGrid({
                    dataSource: this._dataSource,
                    columns: [
                        {
                            title: "Date",
                            field: "Date",
                            width: `50px`,
                            template: (data: InventoryMovementReadModel) => `${kendo.toString(data.Date, "d")}`,
                        },
                        {
                            title: "SKU",
                            field: "SKUName",
                            width: `150px`,
                        },
                        {
                            title: "QTY",
                            field: "QuantityAbsolute",
                            width: `50px`,
                        },
                        {
                            title: "Client",
                            field: "ClientId",
                            width: `100px`,
                            template: (data: InventoryMovementReadModel) => {
                                if (!data.ClientId || !data.ClientFullName) {
                                    return ``;
                                }

                                const link = document.createElement("a");
                                link.href = `/Clients/Details/${data.ClientId}`;
                                link.appendChild(document.createTextNode(data.ClientFullName));
                                return link.outerHTML;
                            }
                        },
                        {
                            title: "Job",
                            field: "HandpieceNumber",
                            width: `50px`,
                            template: (data: InventoryMovementReadModel) => {
                                if (!data.HandpieceId) {
                                    return ``;
                                }

                                return `<a style="color: #007bff;" href="/Handpieces/Edit/${data.HandpieceId}">${data.HandpieceNumber}</a> `;
                            },
                        },
                        {
                            title: "Job Status",
                            field: "HandpieceStatus",
                            width: `150px`,
                            template: (data: InventoryMovementReadModel) => {
                                if (!data.HandpieceId) {
                                    return ``;
                                }

                                return HandpieceStatusHelper.toDisplayString(data.HandpieceStatus);
                            },
                        },
                        {
                            title: "Price",
                            field: "FinalPrice",
                            width: `50px`,
                            template: (data: InventoryMovementReadModel) => {
                                if (data.FinalPrice === undefined || data.FinalPrice === null) {
                                    return ``;
                                }

                                if (data.MovementPrice === undefined || data.MovementPrice === null) {
                                    return `<i>$${kendo.toString(data.FinalPrice, "#,##0.##")}</i>`;
                                }

                                return `$${kendo.toString(data.FinalPrice, "#,##0.##")}`;
                            }
                        },
                        {
                            title: "Total",
                            field: "TotalPriceAbsolute",
                            width: `50px`,
                            template: (data: InventoryMovementReadModel) => {
                                if (data.TotalPriceAbsolute === undefined || data.TotalPriceAbsolute === null) {
                                    return ``;
                                }

                                if (data.MovementPrice === undefined || data.MovementPrice === null) {
                                    return `<i>$${kendo.toString(data.TotalPriceAbsolute, "#,##0.##")}</i>`;
                                }

                                return `$${kendo.toString(data.TotalPriceAbsolute, "#,##0.##")}`;
                            }
                        },
                        {
                            title: "Actions",
                            width: `50px`,
                            command: {
                                name: "CustomHistory",
                                iconClass: "fas fa-history",
                                text: "&nbsp;",
                                click: GridHandlers.createButtonClickPopupHandler<InventoryMovementReadModel>(
                                    item => new DevGuild.AspNet.Routing.Uri(`/InventoryMovements/History/${item.Id}`),
                                    item => ({
                                        title: `Move history`,
                                        width: `1000px`,
                                        height: `auto`,
                                        refresh: (e: kendo.ui.WindowEvent) => {
                                            e.sender.wrapper.on("click", ".editor__submit__cancel", clickEvent => {
                                                clickEvent.preventDefault();
                                                e.sender.close();
                                                e.sender.destroy();
                                            });
                                            e.sender.center();
                                        },
                                    })),
                            },
                        }
                    ]
                }).data("kendoGrid");
            return grid;
        }

        private getUrlFromTab(): string {
            switch (this._options.tab) {
                case "All":
                    return `/InventoryMovements/ReadAll`;
                case "Tray":
                    return `/InventoryMovements/ReadTray`;
                case "Ordered":
                    return `/InventoryMovements/ReadOrdered`;
                case "ApprovedAndRequested":
                    return `/InventoryMovements/ReadApprovedAndRequested`;
                case "Approved":
                    return `/InventoryMovements/ReadApproved`;
                case "Requested":
                    return `/InventoryMovements/ReadRequested`;
                case "Complete":
                    return `/InventoryMovements/ReadComplete`;
                default:
                    throw new Error(`Unknown tab ${this._options.tab}`);
            }
        }

        private createDataSource(url: string, sku: string, workshop: string): kendo.data.DataSource {
            const dataSource = new kendo.data.DataSource({
                type: "aspnetmvc-ajax",
                transport: {
                    read: {
                        url: url,
                        data: () => ({ sku: sku, workshop: workshop }),
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
                            "SKUId": { type: "string" },
                            "SKUName": { type: "string" },
                            "Quantity": { type: "number" },
                            "QuantityAbsolute": { type: "number" },
                            "Type": { type: "number" },
                            "Status": { type: "number" },
                            "HandpieceId": { type: "string" },
                            "HandpieceNumber": { type: "string" },
                            "HandpieceStatus": { type: "number" },
                            "ClientId": { type: "string" },
                            "ClientFullName": { type: "string" },
                            "AveragePrice": { type: "number" },
                            "MovementPrice": { type: "number" },
                            "FinalPrice": { type: "number" },
                            "TotalPrice": { type: "number" },
                            "TotalPriceAbsolute": { type: "number" },
                        },
                    },
                },
                sort: [
                    { field: "SKUTypeOrderNo", dir: "asc" },
                    { field: "SKUOrderNo", dir: "asc" },
                    { field: "Date", dir: "asc" },
                ],
                serverAggregates: true,
                serverFiltering: true,
                serverGrouping: true,
                serverPaging: true,
                serverSorting: true,
            });
            return dataSource;

        }
    }
}