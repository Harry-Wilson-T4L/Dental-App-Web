namespace DentalDrill.CRM.Pages.CorporateSurgeries.Reports {
    export abstract class ChartBase {
        private readonly _collection: ChartsCollectionContainer;
        private readonly _container: JQuery;
        private readonly _host: JQuery;

        protected constructor(collection: ChartsCollectionContainer) {
            this._collection = collection;
            const { host, container } = this.createHost();
            this._host = host;
            this._container = container;
        }

        protected get collection(): ChartsCollectionContainer {
            return this._collection;
        }

        protected get host(): JQuery {
            return this._host;
        }

        abstract initialize(title: string, data: any[], format: string, template: string, maxValue: number): void;

        private createHost(): { host: JQuery, container: JQuery } {
            const host = $("<div></div>")
                .addClass("chart__host");

            const container = $("<div></div>")
                .addClass("chart")
                .addClass("chart__container")
                .append(host);

            this.afterHostCreated(host, container);
            this._collection.container.append(container);

            return { host, container };
        }

        protected afterHostCreated(host: JQuery, container: JQuery): void {
        }
    }

    export class PieChart extends ChartBase {
        constructor(container: ChartsCollectionContainer) {
            super(container);
        }

        initialize(title: string, data: any[], format: string, template: string, maxValue: number): void {
            this.host.kendoChart({
                title: {
                    position: "bottom",
                    text: title,
                    font: "italic 1em sans-serif",
                    margin: {
                        top: -30
                    }
                },
                legend: {
                    visible: false
                },
                chartArea: {
                    background: ""
                },
                seriesDefaults: {
                    labels: {
                        visible: true,
                        background: "transparent",
                        template: template,
                        format: format
                    }
                },
                seriesColors: [
                    "#0c81c5", "#c5dceb", "#3aa2de", "#d8ecf8", "#1f6aa3", "#054158"
                ],
                series: [
                    {
                        type: "pie",
                        startAngle: 150,
                        data: data
                    }
                ],
                tooltip: {
                    visible: true,
                    format: format
                }
            });
        }

        protected afterHostCreated(host: JQuery, container: JQuery): void {
            container.addClass("col-12 col-md-6 mt-3");
        }
    }

    export class BarChart extends ChartBase {
        constructor(container: ChartsCollectionContainer) {
            super(container);
        }

        initialize(title: string, data: any[], format: string, template: string, maxValue: number): void {
            this.host.kendoChart({
                renderAs: "canvas",
                title: {
                    position: "bottom",
                    text: title,
                    font: "italic 1em sans-serif"
                },
                axisDefaults: {
                    labels: {
                        font: "0.75em Aria, Helvetica, sans-serif"
                    }
                },
                legend: {
                    visible: false
                },
                seriesDefaults: {
                    type: "column",
                    labels: {
                        visible: true,
                        background: "transparent",
                        template: template,
                        format: format,
                        font: "0.75em Aria, Helvetica, sans-serif"
                    }
                },
                series: [
                    {
                        name: "Average Rating",
                        data: data.map(u => u.value),
                        color: "#4682b4"
                    }
                ],
                pannable: {
                    lock: "y"
                },
                zoomable: {
                    mousewheel: {
                        lock: "y"
                    },
                    selection: {
                        lock: "y"
                    }
                },
                valueAxis: [
                    {
                        labels: {
                            format: format
                        },
                        line: {
                            visible: false
                        }
                    }
                ],
                categoryAxis: [
                    {
                        categories: data.map(u => u.name),
                        labels: {
                            rotation: "auto"
                        },
                        line: {
                            visible: false
                        }
                    }
                ],
                tooltip: {
                    visible: true,
                    format: format
                }
            });
        }

        afterHostCreated(host: JQuery, container: JQuery): void {
            container.addClass("col-12 col-md-6 mt-3");
        }
    }

    export class LimitedBarChart extends ChartBase {
        constructor(container: ChartsCollectionContainer) {
            super(container);
        }

        initialize(title: string, data: any[], format: string, template: string, maxValue: number): void {
            this.host.kendoChart({
                renderAs: "canvas",
                title: {
                    position: "bottom",
                    text: title,
                    font: "italic 1em sans-serif"
                },
                legend: {
                    visible: false
                },
                seriesDefaults: {
                    type: "column",
                    labels: {
                        visible: true,
                        background: "transparent",
                        template: template,
                        format: format
                    }
                },
                series: [
                    {
                        name: "Average Rating",
                        data: data.map(u => u.value),
                        color: "#4682b4"
                    }
                ],
                pannable: {
                    lock: "y"
                },
                zoomable: {
                    mousewheel: {
                        lock: "y"
                    },
                    selection: {
                        lock: "y"
                    }
                },
                valueAxis: [
                    {
                        min: 0,
                        max: maxValue,
                        labels: {
                            format: format
                        },
                        line: {
                            visible: false
                        }
                    }
                ],
                categoryAxis: [
                    {
                        categories: data.map(u => u.name),
                        labels: {
                            rotation: "auto"
                        },
                        line: {
                            visible: false
                        }
                    }
                ],
                tooltip: {
                    visible: true,
                    format: format
                }
            });
        }

        afterHostCreated(host: JQuery, container: JQuery): void {
            container.addClass("col-12 col-md-6 mt-3");
        }
    }

    export class TreeMapChart extends ChartBase {
        constructor(container: ChartsCollectionContainer) {
            super(container);
        }

        initialize(title: string, data: any[], format: string, template: string, maxValue: number): void {
            this.host.kendoTreeMap({
                dataSource: {
                    data: [
                        {
                            name: title,
                            items: data
                        }
                    ],
                    schema: {
                        model: {
                            children: "items"
                        }
                    }
                },
                valueField: "value",
                textField: "name",
                colors: [
                    ["#0c81c5", "#c5dceb"], ["#3aa2de", "#d8ecf8"],
                    ["#449000", "#dae9cc"], ["#76b800", "#dae7c3"],
                    ["#ffae00", "#f5e5c3"], ["#ef4c00", "#f1b092"],
                    ["#9e0a61", "#eccedf"]
                ]
            });

            const treeMap = this.host.data("kendoTreeMap");
            
            this.host.kendoTooltip({
                filter: ".k-leaf,.k-treemap-title",
                position: "top",
                content: e => {
                    const item = treeMap.dataItem(e.target.closest(".k-treemap-tile")) as any;
                    return item.name;
                }
            });
        }

        afterHostCreated(host: JQuery, container: JQuery): void {
            container.addClass("col-12 mt-3");
        }
    }

    export class WaterfallChart extends ChartBase {
        constructor(container: ChartsCollectionContainer) {
            super(container);
        }

        initialize(title: string, data: any[], format: string, template: string, maxValue: number): void {
            this.host.kendoChart({
                renderAs: "canvas",
                title: {
                    position: "bottom",
                    text: title,
                    font: "italic 1em sans-serif"
                },
                axisDefaults: {
                    labels: {
                        font: "0.75em Aria, Helvetica, sans-serif"
                    },
                    majorGridLines: {
                        visible: false,
                    },
                    title: {
                        font: "1em Aria, Helvetica, sans-serif"
                    }
                },
                dataSource: {
                    data: data
                },
                legend: {
                    visible: false
                },
                seriesDefaults: {
                    type: "column",
                    labels: {
                        visible: true,
                        background: "transparent",
                        template: template,
                        format: format,
                        font: "0.75em Aria, Helvetica, sans-serif"
                    }
                },
                series: [
                    {
                        type: "horizontalWaterfall",
                        field: "value",
                        categoryField: "name",
                        color: "#4682b4"
                    }
                ],
                pannable: {
                    lock: "y"
                },
                zoomable: {
                    mousewheel: {
                        lock: "y"
                    },
                    selection: {
                        lock: "y"
                    }
                },
            });
        }

        afterHostCreated(host: JQuery, container: JQuery): void {
            container.addClass("col-12 col-md-6 mt-3");
        }
    }

    export class ChartsCollectionContainer {
        private readonly _container: JQuery;
        private readonly _charts: ChartBase[] = [];

        constructor(container: JQuery) {
            this._container = container;
        }

        get container(): JQuery {
            return this._container;
        }

        get charts(): ChartBase[] {
            return this._charts;
        }

        createPieChart(): PieChart {
            const chart = new PieChart(this);
            this._charts.push(chart);

            return chart;
        }

        createBarChart(): BarChart {
            const chart = new BarChart(this);
            this._charts.push(chart);

            return chart;
        }

        createLimitedBarChart(): LimitedBarChart {
            const chart = new LimitedBarChart(this);
            this._charts.push(chart);

            return chart;
        }

        createTreeMap(): TreeMapChart {
            const chart = new TreeMapChart(this);
            this._charts.push(chart);

            return chart;
        }

        createWaterfallChart(): WaterfallChart {
            const chart = new WaterfallChart(this);
            this._charts.push(chart);

            return chart;
        }
    }

    export class TotalsCollectionContainer {
        private readonly _container: JQuery;

        constructor(container) {
            this._container = container;
        }

        get container(): JQuery {
            return this._container;
        }

        addValue(key: string, value: string) {
            const valueColValue = $("<div></div>")
                .addClass("reports__insights__total__value")
                .text(value);

            const valueColKey = $("<div></div>")
                .addClass("reports__insights__total__key")
                .text(key);

            const valueCol = $("<div></div>")
                .addClass("col")
                .addClass("reports__insights__total")
                .addClass("text-center")
                .append(valueColValue)
                .append(valueColKey);

            this._container.append(valueCol);
        }
    }
}