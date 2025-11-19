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
                    var InventoryMovementTypeHelper = InventoryMovements.Shared.InventoryMovementTypeHelper;
                    var InventoryMovementStatusHelper = InventoryMovements.Shared.InventoryMovementStatusHelper;
                    var GridInventoryGroupMovementsTabBase = /** @class */ (function (_super) {
                        __extends(GridInventoryGroupMovementsTabBase, _super);
                        function GridInventoryGroupMovementsTabBase(id, root, options) {
                            var _this = _super.call(this, id, root) || this;
                            _this._options = options;
                            return _this;
                        }
                        Object.defineProperty(GridInventoryGroupMovementsTabBase.prototype, "grid", {
                            get: function () {
                                return this._grid;
                            },
                            enumerable: false,
                            configurable: true
                        });
                        Object.defineProperty(GridInventoryGroupMovementsTabBase.prototype, "workshopId", {
                            get: function () {
                                return this._options.workshop ? this._options.workshop.Id : "";
                            },
                            enumerable: false,
                            configurable: true
                        });
                        GridInventoryGroupMovementsTabBase.prototype.initInternal = function () {
                            this._dataSource = this.createDataSource();
                            this._grid = this.createGrid();
                        };
                        GridInventoryGroupMovementsTabBase.prototype.activate = function () {
                            if (this._grid) {
                                this._grid.dataSource.read();
                            }
                        };
                        GridInventoryGroupMovementsTabBase.prototype.resize = function (visible) {
                            if (visible) {
                                this._grid.setOptions({ height: "100px" });
                                this._grid.resize(true);
                                this._grid.setOptions({ height: "100%" });
                                this._grid.resize(true);
                            }
                        };
                        GridInventoryGroupMovementsTabBase.prototype.createGrid = function () {
                            var _this = this;
                            var gridContainerElement = document.createElement("div");
                            gridContainerElement.classList.add("k-grid-commandicons2");
                            gridContainerElement.classList.add("k-grid--dense");
                            this.root.innerHTML = "";
                            this.root.appendChild(gridContainerElement);
                            var gridContainer = $(gridContainerElement);
                            return gridContainer.kendoGrid({
                                dataSource: this._dataSource,
                                columns: this.initializeColumns(),
                                sortable: {
                                    mode: "single",
                                },
                                filterable: {
                                    mode: "row",
                                },
                                dataBound: function (e) {
                                    _this.initializeGrid(e.sender);
                                    e.sender.tbody.find(".inventory-movements-link").each(function (index, element) {
                                        var existing = $(element).data("kendoTooltip");
                                        if (existing) {
                                            existing.destroy();
                                        }
                                        var sku = element.getAttribute("data-sku");
                                        var workshop = element.getAttribute("data-workshop");
                                        var tab = element.getAttribute("data-tab");
                                        $(element).kendoTooltip({
                                            position: "top",
                                            content: {
                                                url: "/InventoryMovements/Preview?sku=" + sku + "&workshop=" + workshop + "&tab=" + tab,
                                            },
                                            width: 950,
                                            height: 300,
                                        });
                                    });
                                }
                            }).data("kendoGrid");
                        };
                        GridInventoryGroupMovementsTabBase.prototype.initializeGrid = function (grid) {
                        };
                        GridInventoryGroupMovementsTabBase.prototype.initializeColumns = function () {
                            var _this = this;
                            var columns = [];
                            var columnsConfig = this.getColumnsConfig();
                            if (columnsConfig.Date && columnsConfig.Date > 0) {
                                columns.push({
                                    title: this.getColumnName("MinDate", "Date"),
                                    field: "MinDate",
                                    width: columnsConfig.Date,
                                    filterable: {
                                        cell: {
                                            operator: "eq",
                                            showOperators: false,
                                            template: function (args) {
                                                var datePicker = args.element.kendoDatePicker({
                                                    format: "d",
                                                }).data("kendoDatePicker");
                                                var defaultValueImpl = datePicker["value"];
                                                datePicker["value"] = function (value) {
                                                    if (value === undefined) {
                                                        var current = this["_value"];
                                                        var padDateFragment = function (s, l) {
                                                            while (s.length < l) {
                                                                s = "0" + s;
                                                            }
                                                            return s;
                                                        };
                                                        if (typeof current === "object" && current instanceof Date) {
                                                            return padDateFragment(current.getFullYear().toString(), 4) + "-" + padDateFragment((current.getMonth() + 1).toString(), 2) + "-" + padDateFragment(current.getDate().toString(), 2);
                                                        }
                                                        return;
                                                    }
                                                    return defaultValueImpl.apply(this, [value]);
                                                };
                                            },
                                        }
                                    },
                                    template: function (data) {
                                        if (data.MinDateTime !== undefined && data.MinDateTime !== null) {
                                            return kendo.toString(data.MinDateTime, "d") + " " + kendo.toString(data.MinDateTime, "HH:mm:ss");
                                        }
                                        return "Warning order";
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
                                    template: function (data) {
                                        var link = document.createElement("a");
                                        link.classList.add("inventory-movements-link");
                                        link.setAttribute("data-sku", data.SKUId);
                                        link.setAttribute("data-workshop", data.WorkshopId);
                                        link.setAttribute("data-tab", _this.getTabName());
                                        link.style.color = "#007bff";
                                        link.href = "/InventoryMovements?sku=" + data.SKUId + "&tab=" + _this.getTabName() + "&group=false";
                                        link.appendChild(document.createTextNode(kendo.toString(data.HandpiecesCount, "0")));
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
                                            template: function (args) {
                                                args.element.kendoDropDownList({
                                                    dataSource: InventoryMovementTypeHelper.createDataSource(),
                                                    dataValueField: "value",
                                                    dataTextField: "name",
                                                    valuePrimitive: true,
                                                });
                                            },
                                        },
                                    },
                                    template: function (data) { return "" + InventoryMovementTypeHelper.toDisplayString(data.Type); },
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
                                            template: function (args) {
                                                args.element.kendoDropDownList({
                                                    dataSource: InventoryMovementStatusHelper.createDataSource(),
                                                    dataValueField: "value",
                                                    dataTextField: "name",
                                                    valuePrimitive: true,
                                                });
                                            },
                                        },
                                    },
                                    template: function (data) { return "" + InventoryMovementStatusHelper.toDisplayString(data.Status); },
                                });
                            }
                            if (columnsConfig.AveragePrice && columnsConfig.AveragePrice > 0) {
                                columns.push({
                                    title: this.getColumnName("AveragePrice", "Price"),
                                    field: "AveragePrice",
                                    width: columnsConfig.AveragePrice,
                                    sortable: false,
                                    filterable: false,
                                    template: function (data) {
                                        if (data.AverageFinalPrice === undefined || data.AverageFinalPrice === null) {
                                            return "";
                                        }
                                        if (data.QuantityAbsolute !== data.QuantityAbsoluteWithPrice) {
                                            return "<i>$" + kendo.toString(data.AverageFinalPrice, "#,##0.##") + "</i>";
                                        }
                                        return "$" + kendo.toString(data.AverageFinalPrice, "#,##0.##");
                                    }
                                });
                            }
                            if (columnsConfig.TotalPrice && columnsConfig.TotalPrice > 0) {
                                columns.push({
                                    title: this.getColumnName("TotalPrice", "Total"),
                                    field: "TotalPrice",
                                    width: columnsConfig.TotalPrice,
                                    template: function (data) {
                                        if (data.TotalPrice === undefined || data.TotalPrice === null) {
                                            return "";
                                        }
                                        if (data.QuantityAbsolute !== data.QuantityAbsoluteWithPrice) {
                                            return "<i>$" + kendo.toString(data.TotalPrice, "#,##0.##") + "</i>";
                                        }
                                        return "$" + kendo.toString(data.TotalPrice, "#,##0.##");
                                    }
                                });
                            }
                            if (columnsConfig.TotalPriceAbsolute && columnsConfig.TotalPriceAbsolute > 0) {
                                columns.push({
                                    title: this.getColumnName("TotalPriceAbsolute", "Total"),
                                    field: "TotalPriceAbsolute",
                                    width: columnsConfig.TotalPriceAbsolute,
                                    template: function (data) {
                                        if (data.TotalPriceAbsolute === undefined || data.TotalPriceAbsolute === null) {
                                            return "";
                                        }
                                        if (data.QuantityAbsolute !== data.QuantityAbsoluteWithPrice) {
                                            return "<i>$" + kendo.toString(data.TotalPriceAbsolute, "#,##0.##") + "</i>";
                                        }
                                        return "$" + kendo.toString(data.TotalPriceAbsolute, "#,##0.##");
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
                        };
                        GridInventoryGroupMovementsTabBase.prototype.getColumnsConfig = function () {
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
                        };
                        GridInventoryGroupMovementsTabBase.prototype.getColumnName = function (field, defaultName) {
                            return defaultName;
                        };
                        GridInventoryGroupMovementsTabBase.prototype.createDataSource = function () {
                            var _this = this;
                            var dataSource = new kendo.data.DataSource({
                                type: "aspnetmvc-ajax",
                                transport: {
                                    read: {
                                        url: this.getEndpointUrl(),
                                        data: function () {
                                            var dataParams = {};
                                            if (_this._options.workshop) {
                                                dataParams["workshop"] = _this._options.workshop.Id;
                                            }
                                            if (_this._options.sku) {
                                                dataParams["sku"] = _this._options.sku;
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
                        };
                        return GridInventoryGroupMovementsTabBase;
                    }(Index.InventoryMovementsTabBase));
                    Index.GridInventoryGroupMovementsTabBase = GridInventoryGroupMovementsTabBase;
                })(Index = InventoryMovements.Index || (InventoryMovements.Index = {}));
            })(InventoryMovements = Pages.InventoryMovements || (Pages.InventoryMovements = {}));
        })(Pages = CRM.Pages || (CRM.Pages = {}));
    })(CRM = DentalDrill.CRM || (DentalDrill.CRM = {}));
})(DentalDrill || (DentalDrill = {}));
//# sourceMappingURL=GridInventoryGroupMovementsTabBase.js.map