namespace DentalDrill.CRM.Pages.InventoryMovements.Index {
    export class StatsAvailableStockChart {
        private readonly _type: InventorySKUType;
        private readonly _dataSource: kendo.data.DataSource;
        private readonly _chart: kendo.dataviz.ui.Chart;

        constructor(type: InventorySKUType, dataSource: kendo.data.DataSource, chart: kendo.dataviz.ui.Chart) {
            this._type = type;
            this._chart = chart;
        }

        get id(): string {
            return this._type.Id;
        }

        get name(): string {
            return this._type.Name;
        }

        get dataSource(): kendo.data.DataSource {
            return this._dataSource;
        }

        get chart(): kendo.dataviz.ui.Chart {
            return this._chart;
        }
    }

    export class StatsInventoryMovementsAvailableStockTab extends InventoryMovementsTabBase {
        private readonly _options: InventoryMovementsIndexOptions;
        private readonly _charts: StatsAvailableStockChart[] = [];

        constructor(id: string, root: HTMLElement, options: InventoryMovementsIndexOptions) {
            super(id, root);
            this._options = options;
        }

        initInternal() {
            this.root.innerHTML = ``;
            const chartsRoot = this.root.appendChild(document.createElement("div"));
            chartsRoot.classList.add("inventory-charts");
            
            for (let type of this._options.statsTypes) {
                const dataSource = this.createDataSource(type.Id);

                const itemRoot = chartsRoot.appendChild(document.createElement("div"));
                itemRoot.classList.add("inventory-charts__item");
                itemRoot.setAttribute("data-sku-type", type.Id);

                const itemTitle = itemRoot.appendChild(document.createElement("div"));
                itemTitle.classList.add("inventory-charts__item__title");
                itemTitle.appendChild(document.createTextNode(type.Name));

                const itemFilter = itemRoot.appendChild(document.createElement("div"));
                itemFilter.classList.add("inventory-charts__item__filter");

                const itemFilterInput = itemFilter.appendChild(document.createElement("input"));
                itemFilterInput.type = "text";
                itemFilterInput.classList.add("k-textbox");
                itemFilterInput.placeholder = `Filter SKUs`;

                const chartContainer = itemRoot.appendChild(document.createElement("div"));
                chartContainer.classList.add("inventory-charts__item__chart");

                const chartElement = chartContainer.appendChild(document.createElement("div"));
                chartElement.classList.add("inventory-charts__item__chart__element");

                const chart = $(chartElement).kendoChart({
                    dataSource: dataSource,
                    title: {
                        visible: false,
                    },
                    legend: {
                        visible: false,
                    },
                    seriesDefaults: {
                        type: "bar",
                        stack: true,
                    },
                    series: [
                        {
                            field: "ShelfQuantity",
                            categoryField: "Name",
                            name: "Shelf",
                            color: "green",
                        },
                        {
                            field: "TrayQuantity",
                            categoryField: "Name",
                            name: "Tray",
                            color: "orange",
                        },
                        {
                            field: "RequestedQuantity",
                            categoryField: "Name",
                            name: "Requested",
                            color: "lightblue",
                        },
                        {
                            field: "OrderedQuantity",
                            categoryField: "Name",
                            name: "Ordered",
                            color: "blue",
                        },
                    ],
                    valueAxis: {
                        reverse: true,
                        labels: {
                            format: "#,##0.##",
                        },
                        majorUnit: 10,
                        minorUnit: 1,
                    },
                    categoryAxis: {
                        labels: {
                            visual: e => {
                                const labelVisual = e.createVisual();
                                labelVisual.options.tooltip = {
                                    content: e.value,
                                };

                                return labelVisual;
                            },

                            template: (category: { value: string }) => {
                                if (category && category.value) {
                                    return category.value.length > 47
                                        ? category.value.substr(0, 44) + `...`
                                        : category.value;
                                } else {
                                    return ``;
                                }
                            },
                        },
                    },
                    tooltip: {
                        visible: true,
                        template: `#: series.name #: #: value #`,
                    },
                    dataBound: (e: kendo.dataviz.ui.ChartDataBoundEvent) => {
                        const chart = e.sender;
                        const distinctNames = chart.dataSource.data<{ Name: string }>().map(x => x.Name).filter((x, i, a) => a.indexOf(x) === i).length;

                        const chartDistinctNames = parseInt(chartElement.getAttribute("data-count"));
                        if (chartDistinctNames !== distinctNames) {
                            chartElement.setAttribute("data-count", distinctNames.toString());
                            chartElement.style.height = `${((distinctNames * 30) + 30)}px`;
                            chart.redraw();
                        }
                    },
                }).data("kendoChart");

                const item = new StatsAvailableStockChart(type, dataSource, chart);
                this._charts.push(item);

                itemFilterInput.addEventListener("input", e => {
                    const newValue = itemFilterInput.value;
                    if (newValue) {
                        dataSource.filter([
                            {
                                field: "Name",
                                operator: "contains",
                                value: newValue,
                            }
                        ]);
                    } else {
                        dataSource.filter([]);
                    }
                });
            }
        }

        activate(): void {
        }

        protected createDataSource(typeId: string): kendo.data.DataSource {
            const dataSource = new kendo.data.DataSource({
                type: "aspnetmvc-ajax",
                transport: {
                    read: {
                        url: this._options.workshop
                            ? `/Inventory/Statistics?type=${typeId}&workshop=${this._options.workshop.Id}`
                            : `/Inventory/Statistics?type=${typeId}`,
                    },
                },
                schema: {
                    data: "Data",
                    total: "Total",
                    errors: "Errors",
                    model: {
                        id: "Id",
                        fields: {
                            "Id": { type: "string" },
                            "TypeId": { type: "string" },
                            "TypeName": { type: "string" }, 
                            "Name": { type: "string" },
                            "TotalQuantity": { type: "number" },
                            "ShelfQuantity": { type: "number" },
                            "TrayQuantity": { type: "number" },
                            "OrderedQuantity": { type: "number" },
                            "RequestedQuantity": { type: "number" },
                        },
                    },
                },
                sort: [
                    { field: "OrderNo", dir: "asc" },
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