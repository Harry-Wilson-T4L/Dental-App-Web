namespace DentalDrill.CRM.Pages.InventoryMovements.Index {
    import HandpieceStatus = Shared.HandpieceStatus;
    import HandpieceStatusHelper = Shared.HandpieceStatusHelper;
    import InventoryMovementType = Shared.InventoryMovementType;
    import InventoryMovementTypeHelper = Shared.InventoryMovementTypeHelper;
    import InventoryMovementStatus = Shared.InventoryMovementStatus;
    import InventoryMovementStatusHelper = Shared.InventoryMovementStatusHelper;

    export interface InventoryMovementReadModel {
        Id: string;
        Date: Date;
        DateTime: Date;
        SKUId: string;
        SKUName: string;
        Quantity: number;
        QuantityAbsolute: number;
        ShelfQuantity: number;
        Type: InventoryMovementType;
        Status: InventoryMovementStatus;
        HandpieceId: string;
        HandpieceNumber: string;
        HandpieceStatus: HandpieceStatus;
        HandpiecePartsComment: string;
        ClientId: string;
        ClientFullName: string;
        MovementComment: string;
        AveragePrice?: number;
        MovementPrice?: number;
        FinalPrice?: number;
        TotalPrice?: number;
        TotalPriceAbsolute?: number;
    }

    export interface GridInventoryMovementsColumnsConfig {
        Date?: number;
        WorkshopName?: number;
        SKUName?: number;
        Quantity?: number;
        QuantityAbsolute?: number;
        ShelfQuantity?: number;
        Type?: number;
        Status?: number;
        HandpiecesNumbers?: number;
        HandpiecesStatuses?: number;
        HandpiecesPartsComments?: number;
        MovementComment?: number;
        FinalPrice?: number;
        TotalPrice?: number;
        TotalPriceAbsolute?: number;
        Actions?: number;
    }

    export abstract class GridInventoryMovementsTabBase extends InventoryMovementsTabBase {
        private readonly _options: InventoryMovementsIndexOptions;

        private _dataSource: kendo.data.DataSource;
        private _grid: kendo.ui.Grid;

        constructor(id: string, root: HTMLElement, options: InventoryMovementsIndexOptions) {
            super(id, root);
            this._options = options;
        }

        get grid(): kendo.ui.Grid {
            return this._grid;
        }

        get workshopId(): string {
            return this._options.workshop ? this._options.workshop.Id : "";
        }

        initInternal() {
            this._dataSource = this.createDataSource();
            this._grid = this.createGrid();
        }

        activate(): void {
            if (this._grid) {
                this._grid.dataSource.read();
            }
        }

        resize(visible: boolean): void {
            if (visible) {
                this._grid.setOptions({ height: "100px" });
                this._grid.resize(true);

                this._grid.setOptions({ height: "100%" });
                this._grid.resize(true);
            }
        }

        protected createGrid(): kendo.ui.Grid {
            const gridContainerElement = document.createElement("div");
            gridContainerElement.classList.add("k-grid-commandicons2");
            gridContainerElement.classList.add("k-grid--dense");
            this.root.innerHTML = "";
            this.root.appendChild(gridContainerElement);
            const gridContainer = $(gridContainerElement);

            return gridContainer.kendoGrid({
                dataSource: this._dataSource,
                columns: this.initializeColumns(),
                sortable: {
                    mode: "single",
                },
                filterable: {
                    mode: "row",
                },
                dataBound: (e: kendo.ui.GridDataBoundEvent) => {
                    this.initializeGrid(e.sender);
                    e.sender.tbody.find("tr[role=row]").each((index: number, row: HTMLTableRowElement) => {
                        const dataItem = e.sender.dataItem<InventoryMovementReadModel>(row);
                        this.initializeRow(row, dataItem);
                    })
                }
            }).data("kendoGrid");
        }

        protected initializeGrid(grid: kendo.ui.Grid) {
        }

        protected initializeRow(row: HTMLTableRowElement, dataItem: InventoryMovementReadModel): void {
            const openJobButton = row.querySelector("a.k-grid-CustomOpenJob") as HTMLAnchorElement;
            if (openJobButton) {
                const shouldDisable = !dataItem.HandpieceId;
                if (shouldDisable) {
                    openJobButton.classList.add("k-state-disabled");
                } else {
                    openJobButton.classList.remove("k-state-disabled");
                }
            }

            const tooltips = row.querySelectorAll(".k-grid-tooltip");
            for (let i = 0; i < tooltips.length; i++) {
                $(tooltips[i]).kendoTooltip();
            }
        }

        protected initializeColumns(): kendo.ui.GridColumn[] {
            const columns: kendo.ui.GridColumn[] = [];
            const columnsConfig = this.getColumnsConfig();

            if (columnsConfig.Date && columnsConfig.Date > 0) {
                columns.push({
                    title: "Date",
                    field: "Date",
                    width: columnsConfig.Date,
                    filterable: {
                        cell: {
                            operator: "eq",
                            showOperators: false,
                            template: (args) => {
                                const datePicker = args.element.kendoDatePicker({
                                    format: "d",
                                }).data("kendoDatePicker");
                                const defaultValueImpl: Function = datePicker["value"];
                                datePicker["value"] = function(value) {
                                    if (value === undefined) {
                                        const current = this["_value"] as any;
                                        const padDateFragment: (s: string, l: number) => string = (s, l) => {
                                            while (s.length < l) {
                                                s = `0` + s;
                                            }

                                            return s;
                                        }
                                        
                                        if (typeof current === "object" && current instanceof Date) {
                                            return `${padDateFragment(current.getFullYear().toString(), 4)}-${padDateFragment((current.getMonth()+1).toString(), 2)}-${padDateFragment(current.getDate().toString(), 2)}`
                                        }

                                        return 
                                    }

                                    return defaultValueImpl.apply(this, [value]);
                                }
                            },
                        }
                    },
                    template: (data: InventoryMovementReadModel) => {
                        if (data.DateTime !== undefined && data.DateTime !== null) {
                            return `${kendo.toString(data.DateTime, "d")} ${kendo.toString(data.DateTime, "HH:mm:ss")}`
                        }

                        return `Warning order`;
                    }
                });
            }

            if (columnsConfig.WorkshopName && columnsConfig.WorkshopName > 0) {
                columns.push({
                    title: "Workshop",
                    field: "WorkshopName",
                    width: columnsConfig.WorkshopName,
                    filterable: {
                        cell: {
                            operator: "contains",
                            showOperators: false,
                        }
                    }
                });
            }

            if (columnsConfig.SKUName && columnsConfig.SKUName > 0) {
                columns.push({
                    title: "SKU",
                    field: "SKUName",
                    width: columnsConfig.SKUName,
                    filterable: {
                        cell: {
                            operator: "contains",
                            showOperators: false,
                        }
                    }
                });
            }

            if (columnsConfig.Quantity && columnsConfig.Quantity > 0) {
                columns.push({
                    title: "QTY",
                    field: "Quantity",
                    width: columnsConfig.Quantity,
                });
            }

            if (columnsConfig.QuantityAbsolute && columnsConfig.QuantityAbsolute > 0) {
                columns.push({
                    title: "QTY",
                    field: "QuantityAbsolute",
                    width: columnsConfig.QuantityAbsolute,
                });
            }

            if (columnsConfig.ShelfQuantity && columnsConfig.ShelfQuantity > 0) {
                columns.push({
                    title: "Shelf",
                    field: "ShelfQuantity",
                    width: columnsConfig.ShelfQuantity,
                });
            }

            if (columnsConfig.Type && columnsConfig.Type > 0) {
                columns.push({
                    title: "Type",
                    field: "Type",
                    width: columnsConfig.Type,
                    filterable: {
                        cell: {
                            operator: "eq",
                            showOperators: false,
                            template: (args) => {
                                args.element.kendoDropDownList({
                                    dataSource: InventoryMovementTypeHelper.createDataSource(),
                                    dataValueField: "value",
                                    dataTextField: "name",
                                    valuePrimitive: true,
                                });
                            },
                        },
                    },
                    template: (data: InventoryMovementReadModel) => `${InventoryMovementTypeHelper.toDisplayString(data.Type)}`,
                });
            }
            
            if (columnsConfig.Status && columnsConfig.Status > 0) {
                columns.push({
                    title: "Status",
                    field: "Status",
                    width: columnsConfig.Status,
                    filterable: {
                        cell: {
                            operator: "eq",
                            showOperators: false,
                            template: (args) => {
                                args.element.kendoDropDownList({
                                    dataSource: InventoryMovementStatusHelper.createDataSource(),
                                    dataValueField: "value",
                                    dataTextField: "name",
                                    valuePrimitive: true,
                                });
                            },
                        },
                    },
                    template: (data: InventoryMovementReadModel) => `${InventoryMovementStatusHelper.toDisplayString(data.Status)}`,
                });
            }
            
            if (columnsConfig.HandpiecesNumbers && columnsConfig.HandpiecesNumbers > 0) {
                columns.push({
                    title: "Job",
                    field: "HandpieceNumber",
                    width: columnsConfig.HandpiecesNumbers,
                    template: (data: InventoryMovementReadModel) => {
                        if (!data.HandpieceId) {
                            return ``;
                        }

                        return `<a style="color: #007bff;" href="/Handpieces/Edit/${data.HandpieceId}">${data.HandpieceNumber}</a> `;
                    },
                });
            }

            if (columnsConfig.HandpiecesStatuses && columnsConfig.HandpiecesStatuses > 0) {
                columns.push({
                    title: "Job Status",
                    field: "HandpieceStatus",
                    width: columnsConfig.HandpiecesStatuses,
                    filterable: {
                        cell: {
                            operator: "eq",
                            showOperators: false,
                            template: (args) => {
                                args.element.kendoDropDownList({
                                    dataSource: HandpieceStatusHelper.createDataSource(),
                                    dataValueField: "value",
                                    dataTextField: "name",
                                    valuePrimitive: true,
                                });
                            },
                        },
                    },
                    template: (data: InventoryMovementReadModel) => {
                        if (!data.HandpieceId) {
                            return ``;
                        }

                        return HandpieceStatusHelper.toDisplayString(data.HandpieceStatus);
                    },
                });
            }

            if (columnsConfig.HandpiecesPartsComments && columnsConfig.HandpiecesPartsComments > 0) {
                columns.push({
                    title: "Parts Comment",
                    field: "HandpiecePartsComment",
                    width: columnsConfig.HandpiecesPartsComments,
                    filterable: {
                        cell: {
                            operator: "contains",
                            showOperators: false,
                        }
                    },
                    template: (data: InventoryMovementReadModel) => {
                        if (!data.HandpieceId || !data.HandpiecePartsComment) {
                            return ``;
                        }

                        let labelText = data.HandpiecePartsComment;
                        if (labelText.length > 40) {
                            labelText = labelText.substr(0, 40) + "...";
                        }

                        const span = document.createElement("span");
                        span.classList.add("k-grid-tooltip");
                        span.setAttribute("title", data.HandpiecePartsComment);
                        span.textContent = labelText;

                        return span.outerHTML;
                    }
                });
            }

            if (columnsConfig.MovementComment && columnsConfig.MovementComment > 0) {
                columns.push({
                    title: "Move Comment",
                    field: "MovementComment",
                    width: columnsConfig.MovementComment,
                    filterable: {
                        cell: {
                            operator: "contains",
                            showOperators: false,
                        }
                    },
                    template: (data: InventoryMovementReadModel) => {
                        if (data.MovementComment === undefined || data.MovementComment === null) {
                            return ``;
                        }

                        let labelText = data.MovementComment;
                        if (labelText.length > 40) {
                            labelText = labelText.substr(0, 40) + "...";
                        }

                        const span = document.createElement("span");
                        span.classList.add("k-grid-tooltip");
                        span.setAttribute("title", data.MovementComment);
                        span.textContent = labelText;

                        return span.outerHTML;
                    }
                });
            }

            if (columnsConfig.FinalPrice && columnsConfig.FinalPrice > 0) {
                columns.push({
                    title: "Price",
                    field: "FinalPrice",
                    width: columnsConfig.FinalPrice,
                    template: (data: InventoryMovementReadModel) => {
                        if (data.FinalPrice === undefined || data.FinalPrice === null) {
                            return ``;
                        }

                        if (data.MovementPrice === undefined || data.MovementPrice === null) {
                            return `<i>$${kendo.toString(data.FinalPrice, "#,##0.##")}</i>`;
                        }

                        return `$${kendo.toString(data.FinalPrice, "#,##0.##")}`;
                    }
                });
            }

            if (columnsConfig.TotalPrice && columnsConfig.TotalPrice > 0) {
                columns.push({
                    title: "Total",
                    field: "TotalPrice",
                    width: columnsConfig.TotalPrice,
                    template: (data: InventoryMovementReadModel) => {
                        if (data.TotalPrice === undefined || data.TotalPrice === null) {
                            return ``;
                        }

                        if (data.MovementPrice === undefined || data.MovementPrice === null) {
                            return `<i>$${kendo.toString(data.TotalPrice, "#,##0.##")}</i>`;
                        }

                        return `$${kendo.toString(data.TotalPrice, "#,##0.##")}`;
                    }
                });
            }

            if (columnsConfig.TotalPriceAbsolute && columnsConfig.TotalPriceAbsolute > 0) {
                columns.push({
                    title: "Total",
                    field: "TotalPriceAbsolute",
                    width: columnsConfig.TotalPriceAbsolute,
                    template: (data: InventoryMovementReadModel) => {
                        if (data.TotalPriceAbsolute === undefined || data.TotalPriceAbsolute === null) {
                            return ``;
                        }

                        if (data.MovementPrice === undefined || data.MovementPrice === null) {
                            return `<i>$${kendo.toString(data.TotalPriceAbsolute, "#,##0.##")}</i>`;
                        }

                        return `$${kendo.toString(data.TotalPriceAbsolute, "#,##0.##")}`;
                    }
                });
            }

            if (columnsConfig.Actions && columnsConfig.Actions > 0) {
                columns.push({
                    title: "Actions",
                    width: columnsConfig.Actions,
                    command: this.initializeCommands(),
                });
            }

            return columns;
        }

        protected getColumnsConfig(): GridInventoryMovementsColumnsConfig {
            return {
                Date: 100,
                WorkshopName: this.workshopId ? 0 : 100,
                SKUName: 250,
                Quantity: 50,
                QuantityAbsolute: 0,
                ShelfQuantity: 50,
                Type: 100,
                Status: 100,
                HandpiecesNumbers: 75,
                HandpiecesStatuses: 100,
                HandpiecesPartsComments: 100,
                MovementComment: 100,
                FinalPrice: 65,
                TotalPrice: 0,
                TotalPriceAbsolute: 0,
                Actions: 200,
            };
        }

        protected abstract initializeCommands(): kendo.ui.GridColumnCommandItem[];

        protected createDataSource(): kendo.data.DataSource {
            const dataSource = new kendo.data.DataSource({
                type: "aspnetmvc-ajax",
                transport: {
                    read: {
                        url: this.getEndpointUrl(),
                        data: () => {
                            var dataParams = {};

                            if (this._options.workshop) {
                                dataParams["workshop"] = this._options.workshop.Id;
                            }

                            if (this._options.sku) {
                                dataParams["sku"] = this._options.sku;
                            }

                            return dataParams;
                        },
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
                            "DateTime": { type: "Date" },
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
                    { field: "DateTime", dir: "asc" },
                    { field: "SKUTypeOrderNo", dir: "asc" },
                    { field: "SKUOrderNo", dir: "asc" },
                ],
                serverAggregates: true,
                serverFiltering: true,
                serverGrouping: true,
                serverPaging: true,
                serverSorting: true,
            });

            return dataSource;
        }

        protected abstract getEndpointUrl(): string;
    }
}