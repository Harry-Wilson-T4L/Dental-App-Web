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
                    var HandpieceStatusHelper = InventoryMovements.Shared.HandpieceStatusHelper;
                    var InventoryMovementTypeHelper = InventoryMovements.Shared.InventoryMovementTypeHelper;
                    var InventoryMovementStatusHelper = InventoryMovements.Shared.InventoryMovementStatusHelper;
                    var GridInventoryMovementsTabBase = /** @class */ (function (_super) {
                        __extends(GridInventoryMovementsTabBase, _super);
                        function GridInventoryMovementsTabBase(id, root, options) {
                            var _this = _super.call(this, id, root) || this;
                            _this._options = options;
                            return _this;
                        }
                        Object.defineProperty(GridInventoryMovementsTabBase.prototype, "grid", {
                            get: function () {
                                return this._grid;
                            },
                            enumerable: false,
                            configurable: true
                        });
                        Object.defineProperty(GridInventoryMovementsTabBase.prototype, "workshopId", {
                            get: function () {
                                return this._options.workshop ? this._options.workshop.Id : "";
                            },
                            enumerable: false,
                            configurable: true
                        });
                        GridInventoryMovementsTabBase.prototype.initInternal = function () {
                            this._dataSource = this.createDataSource();
                            this._grid = this.createGrid();
                        };
                        GridInventoryMovementsTabBase.prototype.activate = function () {
                            if (this._grid) {
                                this._grid.dataSource.read();
                            }
                        };
                        GridInventoryMovementsTabBase.prototype.resize = function (visible) {
                            if (visible) {
                                this._grid.setOptions({ height: "100px" });
                                this._grid.resize(true);
                                this._grid.setOptions({ height: "100%" });
                                this._grid.resize(true);
                            }
                        };
                        GridInventoryMovementsTabBase.prototype.createGrid = function () {
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
                                    e.sender.tbody.find("tr[role=row]").each(function (index, row) {
                                        var dataItem = e.sender.dataItem(row);
                                        _this.initializeRow(row, dataItem);
                                    });
                                }
                            }).data("kendoGrid");
                        };
                        GridInventoryMovementsTabBase.prototype.initializeGrid = function (grid) {
                        };
                        GridInventoryMovementsTabBase.prototype.initializeRow = function (row, dataItem) {
                            var openJobButton = row.querySelector("a.k-grid-CustomOpenJob");
                            if (openJobButton) {
                                var shouldDisable = !dataItem.HandpieceId;
                                if (shouldDisable) {
                                    openJobButton.classList.add("k-state-disabled");
                                }
                                else {
                                    openJobButton.classList.remove("k-state-disabled");
                                }
                            }
                            var tooltips = row.querySelectorAll(".k-grid-tooltip");
                            for (var i = 0; i < tooltips.length; i++) {
                                $(tooltips[i]).kendoTooltip();
                            }
                        };
                        GridInventoryMovementsTabBase.prototype.initializeColumns = function () {
                            var columns = [];
                            var columnsConfig = this.getColumnsConfig();
                            if (columnsConfig.Date && columnsConfig.Date > 0) {
                                columns.push({
                                    title: "Date",
                                    field: "Date",
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
                                        if (data.DateTime !== undefined && data.DateTime !== null) {
                                            return kendo.toString(data.DateTime, "d") + " " + kendo.toString(data.DateTime, "HH:mm:ss");
                                        }
                                        return "Warning order";
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
                                    title: "Status",
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
                            if (columnsConfig.HandpiecesNumbers && columnsConfig.HandpiecesNumbers > 0) {
                                columns.push({
                                    title: "Job",
                                    field: "HandpieceNumber",
                                    width: columnsConfig.HandpiecesNumbers,
                                    template: function (data) {
                                        if (!data.HandpieceId) {
                                            return "";
                                        }
                                        return "<a style=\"color: #007bff;\" href=\"/Handpieces/Edit/" + data.HandpieceId + "\">" + data.HandpieceNumber + "</a> ";
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
                                            template: function (args) {
                                                args.element.kendoDropDownList({
                                                    dataSource: HandpieceStatusHelper.createDataSource(),
                                                    dataValueField: "value",
                                                    dataTextField: "name",
                                                    valuePrimitive: true,
                                                });
                                            },
                                        },
                                    },
                                    template: function (data) {
                                        if (!data.HandpieceId) {
                                            return "";
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
                                    template: function (data) {
                                        if (!data.HandpieceId || !data.HandpiecePartsComment) {
                                            return "";
                                        }
                                        var labelText = data.HandpiecePartsComment;
                                        if (labelText.length > 40) {
                                            labelText = labelText.substr(0, 40) + "...";
                                        }
                                        var span = document.createElement("span");
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
                                    template: function (data) {
                                        if (data.MovementComment === undefined || data.MovementComment === null) {
                                            return "";
                                        }
                                        var labelText = data.MovementComment;
                                        if (labelText.length > 40) {
                                            labelText = labelText.substr(0, 40) + "...";
                                        }
                                        var span = document.createElement("span");
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
                                    template: function (data) {
                                        if (data.FinalPrice === undefined || data.FinalPrice === null) {
                                            return "";
                                        }
                                        if (data.MovementPrice === undefined || data.MovementPrice === null) {
                                            return "<i>$" + kendo.toString(data.FinalPrice, "#,##0.##") + "</i>";
                                        }
                                        return "$" + kendo.toString(data.FinalPrice, "#,##0.##");
                                    }
                                });
                            }
                            if (columnsConfig.TotalPrice && columnsConfig.TotalPrice > 0) {
                                columns.push({
                                    title: "Total",
                                    field: "TotalPrice",
                                    width: columnsConfig.TotalPrice,
                                    template: function (data) {
                                        if (data.TotalPrice === undefined || data.TotalPrice === null) {
                                            return "";
                                        }
                                        if (data.MovementPrice === undefined || data.MovementPrice === null) {
                                            return "<i>$" + kendo.toString(data.TotalPrice, "#,##0.##") + "</i>";
                                        }
                                        return "$" + kendo.toString(data.TotalPrice, "#,##0.##");
                                    }
                                });
                            }
                            if (columnsConfig.TotalPriceAbsolute && columnsConfig.TotalPriceAbsolute > 0) {
                                columns.push({
                                    title: "Total",
                                    field: "TotalPriceAbsolute",
                                    width: columnsConfig.TotalPriceAbsolute,
                                    template: function (data) {
                                        if (data.TotalPriceAbsolute === undefined || data.TotalPriceAbsolute === null) {
                                            return "";
                                        }
                                        if (data.MovementPrice === undefined || data.MovementPrice === null) {
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
                        GridInventoryMovementsTabBase.prototype.getColumnsConfig = function () {
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
                        };
                        GridInventoryMovementsTabBase.prototype.createDataSource = function () {
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
                        };
                        return GridInventoryMovementsTabBase;
                    }(Index.InventoryMovementsTabBase));
                    Index.GridInventoryMovementsTabBase = GridInventoryMovementsTabBase;
                })(Index = InventoryMovements.Index || (InventoryMovements.Index = {}));
            })(InventoryMovements = Pages.InventoryMovements || (Pages.InventoryMovements = {}));
        })(Pages = CRM.Pages || (CRM.Pages = {}));
    })(CRM = DentalDrill.CRM || (DentalDrill.CRM = {}));
})(DentalDrill || (DentalDrill = {}));
//# sourceMappingURL=GridInventoryMovementsTabBase.js.map