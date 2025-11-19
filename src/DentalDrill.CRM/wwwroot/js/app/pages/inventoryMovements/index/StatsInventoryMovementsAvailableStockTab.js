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
        var Pages;
        (function (Pages) {
            var InventoryMovements;
            (function (InventoryMovements) {
                var Index;
                (function (Index) {
                    var StatsAvailableStockChart = /** @class */ (function () {
                        function StatsAvailableStockChart(type, dataSource, chart) {
                            this._type = type;
                            this._chart = chart;
                        }
                        Object.defineProperty(StatsAvailableStockChart.prototype, "id", {
                            get: function () {
                                return this._type.Id;
                            },
                            enumerable: false,
                            configurable: true
                        });
                        Object.defineProperty(StatsAvailableStockChart.prototype, "name", {
                            get: function () {
                                return this._type.Name;
                            },
                            enumerable: false,
                            configurable: true
                        });
                        Object.defineProperty(StatsAvailableStockChart.prototype, "dataSource", {
                            get: function () {
                                return this._dataSource;
                            },
                            enumerable: false,
                            configurable: true
                        });
                        Object.defineProperty(StatsAvailableStockChart.prototype, "chart", {
                            get: function () {
                                return this._chart;
                            },
                            enumerable: false,
                            configurable: true
                        });
                        return StatsAvailableStockChart;
                    }());
                    Index.StatsAvailableStockChart = StatsAvailableStockChart;
                    var StatsInventoryMovementsAvailableStockTab = /** @class */ (function (_super) {
                        __extends(StatsInventoryMovementsAvailableStockTab, _super);
                        function StatsInventoryMovementsAvailableStockTab(id, root, options) {
                            var _this = _super.call(this, id, root) || this;
                            _this._charts = [];
                            _this._options = options;
                            return _this;
                        }
                        StatsInventoryMovementsAvailableStockTab.prototype.initInternal = function () {
                            this.root.innerHTML = "";
                            var chartsRoot = this.root.appendChild(document.createElement("div"));
                            chartsRoot.classList.add("inventory-charts");
                            var _loop_1 = function (type) {
                                var dataSource = this_1.createDataSource(type.Id);
                                var itemRoot = chartsRoot.appendChild(document.createElement("div"));
                                itemRoot.classList.add("inventory-charts__item");
                                itemRoot.setAttribute("data-sku-type", type.Id);
                                var itemTitle = itemRoot.appendChild(document.createElement("div"));
                                itemTitle.classList.add("inventory-charts__item__title");
                                itemTitle.appendChild(document.createTextNode(type.Name));
                                var itemFilter = itemRoot.appendChild(document.createElement("div"));
                                itemFilter.classList.add("inventory-charts__item__filter");
                                var itemFilterInput = itemFilter.appendChild(document.createElement("input"));
                                itemFilterInput.type = "text";
                                itemFilterInput.classList.add("k-textbox");
                                itemFilterInput.placeholder = "Filter SKUs";
                                var chartContainer = itemRoot.appendChild(document.createElement("div"));
                                chartContainer.classList.add("inventory-charts__item__chart");
                                var chartElement = chartContainer.appendChild(document.createElement("div"));
                                chartElement.classList.add("inventory-charts__item__chart__element");
                                var chart = $(chartElement).kendoChart({
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
                                            visual: function (e) {
                                                var labelVisual = e.createVisual();
                                                labelVisual.options.tooltip = {
                                                    content: e.value,
                                                };
                                                return labelVisual;
                                            },
                                            template: function (category) {
                                                if (category && category.value) {
                                                    return category.value.length > 47
                                                        ? category.value.substr(0, 44) + "..."
                                                        : category.value;
                                                }
                                                else {
                                                    return "";
                                                }
                                            },
                                        },
                                    },
                                    tooltip: {
                                        visible: true,
                                        template: "#: series.name #: #: value #",
                                    },
                                    dataBound: function (e) {
                                        var chart = e.sender;
                                        var distinctNames = chart.dataSource.data().map(function (x) { return x.Name; }).filter(function (x, i, a) { return a.indexOf(x) === i; }).length;
                                        var chartDistinctNames = parseInt(chartElement.getAttribute("data-count"));
                                        if (chartDistinctNames !== distinctNames) {
                                            chartElement.setAttribute("data-count", distinctNames.toString());
                                            chartElement.style.height = ((distinctNames * 30) + 30) + "px";
                                            chart.redraw();
                                        }
                                    },
                                }).data("kendoChart");
                                var item = new StatsAvailableStockChart(type, dataSource, chart);
                                this_1._charts.push(item);
                                itemFilterInput.addEventListener("input", function (e) {
                                    var newValue = itemFilterInput.value;
                                    if (newValue) {
                                        dataSource.filter([
                                            {
                                                field: "Name",
                                                operator: "contains",
                                                value: newValue,
                                            }
                                        ]);
                                    }
                                    else {
                                        dataSource.filter([]);
                                    }
                                });
                            };
                            var this_1 = this;
                            for (var _i = 0, _a = this._options.statsTypes; _i < _a.length; _i++) {
                                var type = _a[_i];
                                _loop_1(type);
                            }
                        };
                        StatsInventoryMovementsAvailableStockTab.prototype.activate = function () {
                        };
                        StatsInventoryMovementsAvailableStockTab.prototype.createDataSource = function (typeId) {
                            var dataSource = new kendo.data.DataSource({
                                type: "aspnetmvc-ajax",
                                transport: {
                                    read: {
                                        url: this._options.workshop
                                            ? "/Inventory/Statistics?type=" + typeId + "&workshop=" + this._options.workshop.Id
                                            : "/Inventory/Statistics?type=" + typeId,
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
                        };
                        return StatsInventoryMovementsAvailableStockTab;
                    }(Index.InventoryMovementsTabBase));
                    Index.StatsInventoryMovementsAvailableStockTab = StatsInventoryMovementsAvailableStockTab;
                })(Index = InventoryMovements.Index || (InventoryMovements.Index = {}));
            })(InventoryMovements = Pages.InventoryMovements || (Pages.InventoryMovements = {}));
        })(Pages = CRM.Pages || (CRM.Pages = {}));
    })(CRM = DentalDrill.CRM || (DentalDrill.CRM = {}));
})(DentalDrill || (DentalDrill = {}));
//# sourceMappingURL=StatsInventoryMovementsAvailableStockTab.js.map