namespace DentalDrill.CRM.Pages.InventoryMovements.Index {
    import InventoryMovementType = Shared.InventoryMovementType;
    import InventoryMovementTypeHelper = Shared.InventoryMovementTypeHelper;
    import InventoryMovementStatus = Shared.InventoryMovementStatus;
    import InventoryMovementStatusHelper = Shared.InventoryMovementStatusHelper;

    export interface InventoryMovementGroupReadModel {
        Id: string;
        WorkshopId: string;
        SKUId: string;
        SKUName: string;
        Type: InventoryMovementType;
        Status: InventoryMovementStatus;
        MinDate: Date;
        MinDateTime: Date;
        MaxDate: Date;
        MaxDateTime: Date;
        Quantity: number;
        QuantityAbsolute: number;
        QuantityAbsoluteWithPrice: number;
        ShelfQuantity: number;
        OrderedQuantity: number;
        MissingQuantity: number;
        HandpiecesCount: number;
        AverageFinalPrice?: number;
        TotalPrice?: number;
        TotalPriceAbsolute?: number;
    }

    export interface GridInventoryGroupMovementsColumnsConfig {
        Date?: number;
        WorkshopName?: number;
        SKUName?: number;
        Quantity?: number;
        QuantityAbsolute?: number;
        ShelfQuantity?: number;
        OrderedQuantity?: number;
        MissingQuantity?: number;
        Type?: number;
        Status?: number;
        HandpiecesCount?: number;
        AveragePrice?: number;
        TotalPrice?: number;
        TotalPriceAbsolute?: number;
        Actions?: number;
    }

    export abstract class GridInventoryGroupMovementsTabBase extends InventoryMovementsTabBase {
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

        protected abstract getTabName(): string;

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
                    e.sender.tbody.find(".inventory-movements-link").each((index, element) => {
                        const existing = $(element).data("kendoTooltip") as kendo.ui.Tooltip;
                        if (existing) {
                            existing.destroy();
                        }

                        const sku = element.getAttribute("data-sku");
                        const workshop = element.getAttribute("data-workshop");
                        const tab = element.getAttribute("data-tab");
                        $(element).kendoTooltip({
                            position: "top",
                            content: {
                                url: `/InventoryMovements/Preview?sku=${sku}&workshop=${workshop}&tab=${tab}`,
                            },
                            width: 950,
                            height: 300,
                        });
                    });
                }
            }).data("kendoGrid");
        }

        protected initializeGrid(grid: kendo.ui.Grid) {
        }

        protected initializeColumns(): kendo.ui.GridColumn[] {
            const columns: kendo.ui.GridColumn[] = [];
            const columnsConfig = this.getColumnsConfig();

            if (columnsConfig.Date && columnsConfig.Date > 0) {
                columns.push({
                    title: this.getColumnName("MinDate", "Date"),
                    field: "MinDate",
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
                    template: (data: InventoryMovementGroupReadModel) => {
                        if (data.MinDateTime !== undefined && data.MinDateTime !== null) {
                            return `${kendo.toString(data.MinDateTime, "d")} ${kendo.toString(data.MinDateTime, "HH:mm:ss")}`
                        }
                        
                        return `Warning order`;
                    }
                });
            }

            if (columnsConfig.WorkshopName && columnsConfig.WorkshopName > 0) {
                columns.push({
                    title: this.getColumnName("WorkshopName", "Workshop"),
                    field: "WorkshopName",
                    width: columnsConfig.WorkshopName,
                    filterable: {
                        cell: {
                            operator: "contains",
                            showOperators: false,
                        }
                    },
                });
            }

            if (columnsConfig.SKUName && columnsConfig.SKUName > 0) {
                columns.push({
                    title: this.getColumnName("SKUName", "SKU"),
                    field: "SKUName",
                    width: columnsConfig.SKUName,
                    filterable: {
                        cell: {
                            operator: "contains",
                            showOperators: false,
                        }
                    },
                });
            }

            if (columnsConfig.ShelfQuantity && columnsConfig.ShelfQuantity > 0) {
                columns.push({
                    title: this.getColumnName("ShelfQuantity", "Shelf"),
                    field: "ShelfQuantity",
                    width: columnsConfig.ShelfQuantity,
                });
            }

            if (columnsConfig.Quantity && columnsConfig.Quantity > 0) {
                columns.push({
                    title: this.getColumnName("Quantity", "Quantity"),
                    field: "Quantity",
                    width: columnsConfig.Quantity,
                });
            }

            if (columnsConfig.QuantityAbsolute && columnsConfig.QuantityAbsolute > 0) {
                columns.push({
                    title: this.getColumnName("QuantityAbsolute", "Quantity"),
                    field: "QuantityAbsolute",
                    width: columnsConfig.QuantityAbsolute,
                });
            }

            if (columnsConfig.HandpiecesCount && columnsConfig.HandpiecesCount > 0) {
                columns.push({
                    title: this.getColumnName("HandpiecesCount", "Jobs"),
                    field: "HandpiecesCount",
                    width: columnsConfig.HandpiecesCount,
                    template: (data: InventoryMovementGroupReadModel) => {
                        const link = document.createElement("a");
                        link.classList.add("inventory-movements-link");
                        link.setAttribute("data-sku", data.SKUId);
                        link.setAttribute("data-workshop", data.WorkshopId);
                        link.setAttribute("data-tab", this.getTabName());
                        link.style.color = `#007bff`;
                        link.href = `/InventoryMovements?sku=${data.SKUId}&tab=${this.getTabName()}&group=false`;
                        link.appendChild(document.createTextNode(kendo.toString(data.HandpiecesCount, `0`)));
                        return link.outerHTML;
                    },
                });
            }

            if (columnsConfig.OrderedQuantity && columnsConfig.OrderedQuantity > 0) {
                columns.push({
                    title: this.getColumnName("OrderedQuantity", "Coming"),
                    field: "OrderedQuantity",
                    width: columnsConfig.OrderedQuantity,
                });
            }

            if (columnsConfig.MissingQuantity && columnsConfig.MissingQuantity > 0) {
                columns.push({
                    title: this.getColumnName("MissingQuantity", "Workshop"),
                    field: "MissingQuantity",
                    width: columnsConfig.MissingQuantity,
                });
            }

            if (columnsConfig.Type && columnsConfig.Type > 0) {
                columns.push({
                    title: this.getColumnName("Type", "Type"),
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
                    template: (data: InventoryMovementGroupReadModel) => `${InventoryMovementTypeHelper.toDisplayString(data.Type)}`,
                });
            }
            
            if (columnsConfig.Status && columnsConfig.Status > 0) {
                columns.push({
                    title: this.getColumnName("Status", "Status"),
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
                    template: (data: InventoryMovementGroupReadModel) => `${InventoryMovementStatusHelper.toDisplayString(data.Status)}`,
                });
            }

            if (columnsConfig.AveragePrice && columnsConfig.AveragePrice > 0) {
                columns.push({
                    title: this.getColumnName("AveragePrice", "Price"),
                    field: "AveragePrice",
                    width: columnsConfig.AveragePrice,
                    sortable: false,
                    filterable: false,
                    template: (data: InventoryMovementGroupReadModel) => {
                        if (data.AverageFinalPrice === undefined || data.AverageFinalPrice === null) {
                            return ``;
                        }

                        if (data.QuantityAbsolute !== data.QuantityAbsoluteWithPrice) {
                            return `<i>$${kendo.toString(data.AverageFinalPrice, "#,##0.##")}</i>`;
                        }

                        return `$${kendo.toString(data.AverageFinalPrice, "#,##0.##")}`;
                    }
                });
            }

            if (columnsConfig.TotalPrice && columnsConfig.TotalPrice > 0) {
                columns.push({
                    title: this.getColumnName("TotalPrice", "Total"),
                    field: "TotalPrice",
                    width: columnsConfig.TotalPrice,
                    template: (data: InventoryMovementGroupReadModel) => {
                        if (data.TotalPrice === undefined || data.TotalPrice === null) {
                            return ``;
                        }

                        if (data.QuantityAbsolute !== data.QuantityAbsoluteWithPrice) {
                            return `<i>$${kendo.toString(data.TotalPrice, "#,##0.##")}</i>`;
                        }

                        return `$${kendo.toString(data.TotalPrice, "#,##0.##")}`;
                    }
                });
            }

            if (columnsConfig.TotalPriceAbsolute && columnsConfig.TotalPriceAbsolute > 0) {
                columns.push({
                    title: this.getColumnName("TotalPriceAbsolute", "Total"),
                    field: "TotalPriceAbsolute",
                    width: columnsConfig.TotalPriceAbsolute,
                    template: (data: InventoryMovementGroupReadModel) => {
                        if (data.TotalPriceAbsolute === undefined || data.TotalPriceAbsolute === null) {
                            return ``;
                        }

                        if (data.QuantityAbsolute !== data.QuantityAbsoluteWithPrice) {
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

        protected getColumnsConfig(): GridInventoryGroupMovementsColumnsConfig {
            return {
                Date: 100,
                WorkshopName: this.workshopId ? 0 : 100,
                SKUName: 250,
                Quantity: 50,
                QuantityAbsolute: 0,
                ShelfQuantity: 50,
                OrderedQuantity: 50,
                MissingQuantity: 50,
                Type: 100,
                Status: 100,
                HandpiecesCount: 75,
                AveragePrice: 65,
                TotalPrice: 0,
                TotalPriceAbsolute: 0,
                Actions: 300,
            };
        }

        protected getColumnName(field: string, defaultName: string) {
            return defaultName;
        }

        protected abstract initializeCommands(): kendo.ui.GridColumnCommandItem[];

        protected createDataSource(): kendo.data.DataSource {
            const dataSource = new kendo.data.DataSource({
                type: "aspnetmvc-ajax",
                transport: {
                    read: {
                        url: this.getEndpointUrl(),
                        data: () => {
                            var dataParams = { };

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
                            "SKUId": { type: "string" },
                            "SKUName": { type: "string" },
                            "Type": { type: "number" },
                            "Status": { type: "number" },
                            "MinDate": { type: "Date" },
                            "MinDateTime": { type: "Date" },
                            "MaxDate": { type: "Date" },
                            "MaxDateTime": { type: "Date" },
                            "Quantity": { type: "number" },
                            "QuantityAbsolute": { type: "number" },
                            "QuantityAbsoluteWithPrice": { type: "number" },
                            "ShelfQuantity": { type: "number" },
                            "OrderedQuantity": { type: "number" },
                            "MissingQuantity": { type: "number" },
                            "HandpiecesCount": { type: "number" },
                            "AverageFinalPrice": { type: "number" },
                            "TotalPrice": { type: "number" },
                            "TotalPriceAbsolute": { type: "number" },
                        },
                    },
                },
                sort: [
                    { field: "MinDateTime", dir: "asc" },
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