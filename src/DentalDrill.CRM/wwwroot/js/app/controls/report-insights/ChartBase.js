var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (Object.prototype.hasOwnProperty.call(b, p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        if (typeof b !== "function" && b !== null)
            throw new TypeError("Class extends value " + String(b) + " is not a constructor or null");
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var DentalDrill;
(function (DentalDrill) {
    var CRM;
    (function (CRM) {
        var Controls;
        (function (Controls) {
            var Reporting;
            (function (Reporting) {
                var ChartBase = /** @class */ (function () {
                    function ChartBase(collection) {
                        this._collection = collection;
                        var _a = this.createHost(), host = _a.host, container = _a.container;
                        this._host = host;
                        this._container = container;
                    }
                    Object.defineProperty(ChartBase.prototype, "collection", {
                        get: function () {
                            return this._collection;
                        },
                        enumerable: false,
                        configurable: true
                    });
                    Object.defineProperty(ChartBase.prototype, "host", {
                        get: function () {
                            return this._host;
                        },
                        enumerable: false,
                        configurable: true
                    });
                    ChartBase.prototype.createHost = function () {
                        var host = $("<div></div>")
                            .addClass("chart__host");
                        var container = $("<div></div>")
                            .addClass("chart")
                            .addClass("chart__container")
                            .append(host);
                        this.afterHostCreated(host, container);
                        this._collection.container.append(container);
                        return { host: host, container: container };
                    };
                    ChartBase.prototype.afterHostCreated = function (host, container) {
                    };
                    return ChartBase;
                }());
                Reporting.ChartBase = ChartBase;
                var PieChart = /** @class */ (function (_super) {
                    __extends(PieChart, _super);
                    function PieChart(container) {
                        return _super.call(this, container) || this;
                    }
                    PieChart.prototype.override = function (method) {
                        this._optionsOverride = method;
                        return this;
                    };
                    PieChart.prototype.initialize = function (title, data, format, template, maxValue) {
                        var options = {
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
                        };
                        if (this._optionsOverride) {
                            this._optionsOverride(options);
                        }
                        this.host.kendoChart(options);
                    };
                    PieChart.prototype.afterHostCreated = function (host, container) {
                        container.addClass("col-12 col-md-6 mt-3");
                    };
                    return PieChart;
                }(ChartBase));
                Reporting.PieChart = PieChart;
                var BarChart = /** @class */ (function (_super) {
                    __extends(BarChart, _super);
                    function BarChart(container) {
                        return _super.call(this, container) || this;
                    }
                    BarChart.prototype.initialize = function (title, data, format, template, maxValue) {
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
                                    data: data.map(function (u) { return u.value; }),
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
                                    categories: data.map(function (u) { return u.name; }),
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
                    };
                    BarChart.prototype.afterHostCreated = function (host, container) {
                        container.addClass("col-12 col-md-6 mt-3");
                    };
                    return BarChart;
                }(ChartBase));
                Reporting.BarChart = BarChart;
                var LimitedBarChart = /** @class */ (function (_super) {
                    __extends(LimitedBarChart, _super);
                    function LimitedBarChart(container) {
                        return _super.call(this, container) || this;
                    }
                    LimitedBarChart.prototype.initialize = function (title, data, format, template, maxValue) {
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
                                    data: data.map(function (u) { return u.value; }),
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
                                    categories: data.map(function (u) { return u.name; }),
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
                    };
                    LimitedBarChart.prototype.afterHostCreated = function (host, container) {
                        container.addClass("col-12 col-md-6 mt-3");
                    };
                    return LimitedBarChart;
                }(ChartBase));
                Reporting.LimitedBarChart = LimitedBarChart;
                var TreeMapChart = /** @class */ (function (_super) {
                    __extends(TreeMapChart, _super);
                    function TreeMapChart(container) {
                        return _super.call(this, container) || this;
                    }
                    TreeMapChart.prototype.initialize = function (title, data, format, template, maxValue) {
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
                        var treeMap = this.host.data("kendoTreeMap");
                        this.host.kendoTooltip({
                            filter: ".k-leaf,.k-treemap-title",
                            position: "top",
                            content: function (e) {
                                var item = treeMap.dataItem(e.target.closest(".k-treemap-tile"));
                                return item.name;
                            }
                        });
                    };
                    TreeMapChart.prototype.afterHostCreated = function (host, container) {
                        container.addClass("col-12 mt-3");
                    };
                    return TreeMapChart;
                }(ChartBase));
                Reporting.TreeMapChart = TreeMapChart;
                var WaterfallChart = /** @class */ (function (_super) {
                    __extends(WaterfallChart, _super);
                    function WaterfallChart(container) {
                        return _super.call(this, container) || this;
                    }
                    WaterfallChart.prototype.initialize = function (title, data, format, template, maxValue) {
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
                    };
                    WaterfallChart.prototype.afterHostCreated = function (host, container) {
                        container.addClass("col-12 col-md-6 mt-3");
                    };
                    return WaterfallChart;
                }(ChartBase));
                Reporting.WaterfallChart = WaterfallChart;
                var ChartsCollectionContainer = /** @class */ (function () {
                    function ChartsCollectionContainer(container) {
                        this._charts = [];
                        this._container = container;
                    }
                    Object.defineProperty(ChartsCollectionContainer.prototype, "container", {
                        get: function () {
                            return this._container;
                        },
                        enumerable: false,
                        configurable: true
                    });
                    Object.defineProperty(ChartsCollectionContainer.prototype, "charts", {
                        get: function () {
                            return this._charts;
                        },
                        enumerable: false,
                        configurable: true
                    });
                    ChartsCollectionContainer.prototype.createPieChart = function () {
                        var chart = new PieChart(this);
                        this._charts.push(chart);
                        return chart;
                    };
                    ChartsCollectionContainer.prototype.createBarChart = function () {
                        var chart = new BarChart(this);
                        this._charts.push(chart);
                        return chart;
                    };
                    ChartsCollectionContainer.prototype.createLimitedBarChart = function () {
                        var chart = new LimitedBarChart(this);
                        this._charts.push(chart);
                        return chart;
                    };
                    ChartsCollectionContainer.prototype.createTreeMap = function () {
                        var chart = new TreeMapChart(this);
                        this._charts.push(chart);
                        return chart;
                    };
                    ChartsCollectionContainer.prototype.createWaterfallChart = function () {
                        var chart = new WaterfallChart(this);
                        this._charts.push(chart);
                        return chart;
                    };
                    return ChartsCollectionContainer;
                }());
                Reporting.ChartsCollectionContainer = ChartsCollectionContainer;
            })(Reporting = Controls.Reporting || (Controls.Reporting = {}));
        })(Controls = CRM.Controls || (CRM.Controls = {}));
    })(CRM = DentalDrill.CRM || (DentalDrill.CRM = {}));
})(DentalDrill || (DentalDrill = {}));
//# sourceMappingURL=ChartBase.js.map