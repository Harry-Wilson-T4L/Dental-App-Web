namespace DentalDrill.CRM.Pages.InventoryMovements.Shared {
    export class InventoryMovementBulkViewer {
        private readonly _root: HTMLElement;
        private readonly _movements: InventoryMovementBulkCollection;

        private _items: InventoryMovementBulkModel[];
        private _dataSource: kendo.data.DataSource;
        private _grid: kendo.ui.Grid;

        constructor(root: HTMLElement, movements: InventoryMovementBulkCollection) {
            this._root = root;
            this._movements = movements;
        }

        protected get root(): HTMLElement {
            return this._root;
        }

        init() {
            this._items = this._movements.getBulkMovements();
            if (this._movements.extraInitialQuantity && this._movements.extraInitialQuantity > 0) {
                this._items.push({
                    Id: "00000000-0000-0000-0000-000000000000",
                    Direction: this._movements.direction,
                    Type: this._movements.type,
                    Status: this._movements.status,
                    AveragePrice: this._movements.averagePrice,
                    QuantityAbsolute: this._movements.extraInitialQuantity ? this._movements.extraInitialQuantity : 0,
                    Quantity: this._movements.extraInitialQuantity ? this._movements.extraInitialQuantity : 0,
                    CreatedOn: null,
                    CompletedOn: null,
                    HandpieceId: null,
                    HandpieceNumber: null,
                    HandpieceStatus: null,
                    RequiredQuantity: null,
                    Price: null,
                    PartsComment: null,
                    MovementComment: null,
                    BulkEditStatus: BulkEditStatus.Normal,
                });
            }
            
            this._dataSource = this.createDataSource();
            this._grid = this.createGrid();
        }

        private createGrid(): kendo.ui.Grid {
            const gridContainer = this._root.querySelector(".grid-container") as HTMLElement;
            const gridContainerElement = document.createElement("div");
            gridContainerElement.classList.add("k-grid-commandicons2");
            gridContainerElement.classList.add("k-grid--dense");
            gridContainerElement.classList.add("k-grid--small-text");
            gridContainer.appendChild(gridContainerElement);

            const grid = $(gridContainerElement).kendoGrid({
                columns: this.initializeColumns(),
                dataSource: this._dataSource,
                editable: false,
                scrollable: true,
            }).data("kendoGrid");

            return grid;
        }

        protected initializeColumns(): kendo.ui.GridColumn[] {
            const columns: kendo.ui.GridColumn[] = [];
            columns.push({
                title: "Date",
                field: "CreatedOn",
                width: 70,
                template: (data: InventoryMovementBulkModel) => {
                    if (data.CreatedOn !== undefined && data.CreatedOn !== null) {
                        return kendo.toString(data.CreatedOn, "d");
                    } else {
                        return ``;
                    }
                },
            });
            columns.push({
                title: "QTY",
                field: "QuantityAbsolute",
                width: 75,
                template: (data: InventoryMovementBulkModel) => {
                    if (data.QuantityAbsolute !== undefined && data.QuantityAbsolute !== null) {
                        if (data.RequiredQuantity !== undefined && data.RequiredQuantity !== null) {
                            return `${data.QuantityAbsolute} / ${data.RequiredQuantity}`;
                        } else {
                            return `${data.QuantityAbsolute}`;
                        }
                    }
                },
            });
            columns.push({
                title: "Job",
                width: 50,
                template: (data: InventoryMovementBulkModel) => {
                    if (data.HandpieceId !== undefined &&
                        data.HandpieceId !== null &&
                        data.HandpieceNumber !== undefined &&
                        data.HandpieceNumber !== null) {
                        return `<a href="/Handpieces/Edit/${data.HandpieceId}">${data.HandpieceNumber}</a>`;
                    } else {
                        return `Shelf`;
                    }
                },
            });
            columns.push({
                title: "Job Status",
                width: 100,
                template: (data: InventoryMovementBulkModel) => {
                    if (data.HandpieceStatus !== undefined && data.HandpieceStatus !== null) {
                        return HandpieceStatusHelper.toDisplayString(data.HandpieceStatus);
                    }

                    return ``;
                },
            });
            columns.push({
                title: "Parts Comment",
                field: "PartsComment",
                width: 65,
                template: (data: InventoryMovementBulkModel) => {
                    if (data.PartsComment === undefined || data.PartsComment === null) {
                        return ``;
                    }

                    const span = document.createElement("span");
                    span.classList.add("inventory-movements-comment");
                    span.title = data.PartsComment;
                    span.appendChild(document.createTextNode("See here"));

                    return span.outerHTML;
                }
            });
            columns.push({
                title: "Move Comment",
                field: "MovementComment",
                width: 65,
                template: (data: InventoryMovementBulkModel) => {
                    if (data.MovementComment === undefined || data.MovementComment === null) {
                        return ``;
                    }

                    const span = document.createElement("span");
                    span.classList.add("inventory-movements-comment");
                    span.title = data.MovementComment;
                    span.appendChild(document.createTextNode("See here"));

                    return span.outerHTML;
                }
            });
            columns.push({
                title: "Price",
                field: "Price",
                width: 75,
                template: (data: InventoryMovementBulkModel) => {
                    if (!(data.Price === undefined || data.Price === null)) {
                        return `$${kendo.toString(data.Price, "#,##0.##")}`;
                    }

                    if (!(data.AveragePrice === undefined || data.AveragePrice === null)) {
                        return `<i>$${kendo.toString(data.AveragePrice, "#,##0.##")}</i>`;
                    }

                    return ``;
                },
            });
            columns.push({
                title: "Total Price",
                width: 75,
                template: (data: InventoryMovementBulkModel) => {
                    if (data.QuantityAbsolute === undefined || data.QuantityAbsolute === null) {
                        return ``;
                    }

                    if (!(data.Price === undefined || data.Price === null)) {
                        return `$${kendo.toString(data.Price * data.QuantityAbsolute, "#,##0.##")}`;
                    }

                    if (!(data.AveragePrice === undefined || data.AveragePrice === null)) {
                        return `<i>$${kendo.toString(data.AveragePrice * data.QuantityAbsolute, "#,##0.##")}</i>`;
                    }

                    return ``;
                }
            });
            columns.push({
                title: "Actions",
                width: 240,
                command: []
            });

            return columns;
        }

        private createDataSource(): kendo.data.DataSource {
            const dataSource = new kendo.data.DataSource({
                data: this._items,
                schema: {
                    model: {
                        id: "Id",
                        fields: {
                            Id: { type: "string", editable: false },
                            Direction: { type: "number", editable: false },
                            Type: { type: "number", editable: false },
                            Status: { type: "number", editable: false },
                            HandpieceId: { type: "string", editable: false },
                            HandpieceNumber: { type: "string", editable: false },
                            HandpieceStatus: { type: "number", editable: false },
                            Quantity: { type: "number", editable: false },
                            QuantityAbsolute: { type: "number", editable: false },
                            Price: { type: "number", editable: false },
                            CreatedOn: { type: "Date", editable: false },
                            CompletedOn: { type: "Date", editable: false },
                            PartsComment: { type: "string", editable: false },
                            MovementComment: { type: "string", editable: false },
                            BulkEditStatus: { type: "number", editable: false },
                        }
                    }
                }
            });

            return dataSource;
        }
    }
}