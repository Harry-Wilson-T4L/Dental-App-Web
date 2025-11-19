namespace DentalDrill.CRM.Pages.InventoryMovements.Shared {
    export class InventoryMovementBulkEditor {
        private readonly _root: HTMLElement;
        private readonly _movements: InventoryMovementBulkCollection;

        private _items: InventoryMovementBulkModel[];
        private _dataSource: kendo.data.DataSource;
        private _grid: kendo.ui.Grid;

        private _form: HTMLFormElement;
        private _totalQuantity: HTMLInputElement;
        private _movementsResult: HiddenInputList;

        private _lastQuantity: number;

        constructor(root: HTMLElement, movements: InventoryMovementBulkCollection) {
            this._root = root;
            this._movements = movements;
        }

        init() {
            this._items = this._movements.getBulkMovements();
            if (!this._items.some(x => x.HandpieceId === undefined || x.HandpieceId === null)) {
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
            } else if (this._movements.extraInitialQuantity && this._movements.extraInitialQuantity > 0) {
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

            this._form = this._root.querySelector("form.inventory-group-editor__form");
            this._form.addEventListener("submit", e => this.onBeforeSubmit(e));
            this._totalQuantity = this._root.querySelector("input.inventory-group-editor__total-quantity");
            this._totalQuantity.addEventListener("change", e => this.updateQuantityFromTotal());
            this._movementsResult = new HiddenInputList(this._root.querySelector("div.inventory-group-editor__data"));

            this.updateTotalQuantityFromItems();
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
                editable: "incell",
                scrollable: true,
                dataBound: (e: kendo.ui.GridDataBoundEvent) => {
                    e.sender.tbody.find("tr[role='row']").each((index, element) => {
                        const item = e.sender.dataItem<InventoryMovementBulkModel>(element);
                        this.updateRowStatus(element as HTMLTableRowElement, item);

                        $(element).find(".inventory-movements-comment").kendoTooltip();
                    });
                },
                save: (e: kendo.ui.GridSaveEvent) => {
                    setTimeout(() => {
                        this.updateTotalQuantityFromItems();
                    });
                }
            }).data("kendoGrid");

            return grid;
        }

        private updateRowStatus(row: HTMLTableRowElement, item: InventoryMovementBulkModel): void {
            const postponeButton = row.querySelector(".k-grid-CustomPostpone");
            const cancelButton = row.querySelector(".k-grid-CustomCancel");
            switch (item.BulkEditStatus) {
            case BulkEditStatus.Normal:
                postponeButton.classList.remove("active");
                cancelButton.classList.remove("active");
                break;
            case BulkEditStatus.Postponed:
                postponeButton.classList.add("active");
                cancelButton.classList.remove("active");
                break;
            case BulkEditStatus.Cancelled:
                postponeButton.classList.remove("active");
                cancelButton.classList.add("active");
                break;
            }
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
                editor: (container: JQuery<HTMLElement>, options: kendo.ui.GridColumnEditorOptions) => {
                    const input = document.createElement("input");
                    input.type = "number";
                    input.name = options.field;
                    input.classList.add("k-textbox");

                    const item = options.model as any as InventoryMovementBulkModel;
                    if (item && item.RequiredQuantity !== undefined && item.RequiredQuantity !== null) {
                        input.addEventListener("change", e => {
                            const value = parseFloat(input.value);
                            if (value > item.RequiredQuantity) {
                                input.value = item.RequiredQuantity.toString();
                            }
                        });
                    }

                    container.append(input);
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
                editor: (container: JQuery<HTMLElement>, options: kendo.ui.GridColumnEditorOptions) => {
                    const input = document.createElement("input");
                    input.type = "number";
                    input.name = options.field;
                    input.classList.add("k-textbox");

                    container.append(input);
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
                command: [
                    {
                        name: "CustomOpenJob",
                        iconClass: "fas fa-link",
                        text: "&nbsp; Job",
                        click: (e: JQueryEventObject) => {
                            e.preventDefault();
                        }
                    },
                    {
                        name: "CustomPostpone",
                        iconClass: "fas fa-clock",
                        text: "&nbsp; Postpone",
                        click: (e: JQueryEventObject) => {
                            e.preventDefault();
                            const row = e.target.closest("tr");
                            const dataItem = new InventoryMovementBulkModelWrapper(this._grid.dataItem<InventoryMovementBulkModel>(row));
                            if (dataItem.BulkEditStatus !== BulkEditStatus.Postponed) {
                                dataItem.BulkEditStatus = BulkEditStatus.Postponed;
                            } else {
                                dataItem.BulkEditStatus = BulkEditStatus.Normal;
                            }

                            this._dataSource.sync();
                            this.updateRowStatus(row, dataItem);
                            this.updateTotalQuantityFromItems();
                        }
                    },
                    {
                        name: "CustomCancel",
                        iconClass: "fas fa-ban",
                        text: "&nbsp; Cancel",
                        click: (e: JQueryEventObject) => {
                            e.preventDefault();
                            const row = e.target.closest("tr");
                            const dataItem = new InventoryMovementBulkModelWrapper(this._grid.dataItem<InventoryMovementBulkModel>(row));
                            if (dataItem.BulkEditStatus !== BulkEditStatus.Cancelled) {
                                dataItem.BulkEditStatus = BulkEditStatus.Cancelled;
                            } else {
                                dataItem.BulkEditStatus = BulkEditStatus.Normal;
                            }

                            this._dataSource.sync();
                            this.updateRowStatus(row, dataItem);
                            this.updateTotalQuantityFromItems();
                        }
                    }
                ]
            });

            return columns;
        }

        private createDataSource(): kendo.data.DataSource {
            const dataSource = new kendo.data.DataSource({
                data: this._items,
                autoSync: true,
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
                            QuantityAbsolute: { type: "number", editable: true },
                            Price: { type: "number", editable: true },
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

        private updateTotalQuantityFromItems(): void {
            const items = this._dataSource.data<InventoryMovementBulkModel>();
            const totalQuantityValue = items
                .filter(x => x.BulkEditStatus === BulkEditStatus.Normal)
                .reduce((prev, curr) => prev + curr.QuantityAbsolute, 0);

            this._totalQuantity.value = totalQuantityValue.toString();
            this._lastQuantity = totalQuantityValue;
        }

        private updateQuantityFromTotal(): void {
            const totalQuantity = parseFloat(this._totalQuantity.value);
            const items = this._dataSource.data<InventoryMovementBulkModel>()
                .map(x => new InventoryMovementBulkModelWrapper(x as any))
                .filter(x => x.BulkEditStatus === BulkEditStatus.Normal);

            if (typeof this._lastQuantity === "number" && typeof totalQuantity === "number") {
                const delta = totalQuantity - this._lastQuantity;
                if (delta > 0) {
                    // Increasing total quantity by <delta>
                    let remaining = delta;

                    // Passing through movements with quantity below requested first
                    for (let i = 0; i < items.length; i++) {
                        if (items[i].RequiredQuantity !== undefined &&
                            items[i].RequiredQuantity !== null &&
                            items[i].RequiredQuantity > items[i].QuantityAbsolute) {
                            const missing = items[i].RequiredQuantity - items[i].QuantityAbsolute;
                            const increase = Math.min(remaining, missing);
                            remaining -= increase;
                            items[i].Quantity += increase;
                            items[i].QuantityAbsolute += increase;
                        }
                    }

                    // Trying to dump the rest into first Shelf movement if its present
                    const firstShelf = items.filter(x => x.HandpieceId === undefined || x.HandpieceId === null)[0];
                    if (firstShelf) {
                        firstShelf.Quantity += remaining;
                        firstShelf.QuantityAbsolute += remaining;
                    } else {
                        // Otherwise - dumping everything to first item
                        items[0].Quantity += remaining;
                        items[0].QuantityAbsolute += remaining;
                    }

                } else if (delta < 0) {
                    // Decreasing total quantity by <delta>
                    let remaining = -delta;

                    // Passing through all Shelf movements first in reverse order
                    for (let i = items.length - 1; i >= 0; i--) {
                        if (items[i].HandpieceId === undefined || items[i].HandpieceId === null) {
                            const reduction = Math.min(items[i].QuantityAbsolute, remaining);
                            remaining -= reduction;
                            items[i].Quantity -= reduction;
                            items[i].QuantityAbsolute -= reduction;
                        }
                    }

                    // Passing through remaining movements in reverse order
                    for (let i = items.length - 1; i >= 0; i--) {
                        const reduction = Math.min(items[i].QuantityAbsolute, remaining);
                        remaining -= reduction;
                        items[i].Quantity -= reduction;
                        items[i].QuantityAbsolute -= reduction;
                    }
                }

                this._lastQuantity = totalQuantity;
                this._grid.dataSource.sync();
            }
        }

        private onBeforeSubmit(e: Event) {
            e.preventDefault();

            this._movementsResult.clear();
            const items = this._dataSource.data<InventoryMovementBulkModel>();
            for (let i = 0; i < items.length; i++) {
                const item = items[i];

                this._movementsResult.add(`Result[${i}].Id`, item.Id);

                if (item.QuantityAbsolute !== undefined && item.QuantityAbsolute !== null) {
                    this._movementsResult.add(`Result[${i}].Quantity`, item.QuantityAbsolute.toString());
                }

                if (item.Price !== undefined && item.Price !== null) {
                    this._movementsResult.add(`Result[${i}].Price`, item.Price.toString());
                }

                this._movementsResult.add(`Result[${i}].BulkEditStatus`, item.BulkEditStatus.toString());
            }
        }
    }
}