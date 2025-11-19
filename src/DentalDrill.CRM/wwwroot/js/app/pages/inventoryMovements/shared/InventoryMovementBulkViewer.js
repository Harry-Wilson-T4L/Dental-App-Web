var DentalDrill;
(function (DentalDrill) {
    var CRM;
    (function (CRM) {
        var Pages;
        (function (Pages) {
            var InventoryMovements;
            (function (InventoryMovements) {
                var Shared;
                (function (Shared) {
                    var InventoryMovementBulkViewer = /** @class */ (function () {
                        function InventoryMovementBulkViewer(root, movements) {
                            this._root = root;
                            this._movements = movements;
                        }
                        Object.defineProperty(InventoryMovementBulkViewer.prototype, "root", {
                            get: function () {
                                return this._root;
                            },
                            enumerable: false,
                            configurable: true
                        });
                        InventoryMovementBulkViewer.prototype.init = function () {
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
                                    BulkEditStatus: Shared.BulkEditStatus.Normal,
                                });
                            }
                            this._dataSource = this.createDataSource();
                            this._grid = this.createGrid();
                        };
                        InventoryMovementBulkViewer.prototype.createGrid = function () {
                            var gridContainer = this._root.querySelector(".grid-container");
                            var gridContainerElement = document.createElement("div");
                            gridContainerElement.classList.add("k-grid-commandicons2");
                            gridContainerElement.classList.add("k-grid--dense");
                            gridContainerElement.classList.add("k-grid--small-text");
                            gridContainer.appendChild(gridContainerElement);
                            var grid = $(gridContainerElement).kendoGrid({
                                columns: this.initializeColumns(),
                                dataSource: this._dataSource,
                                editable: false,
                                scrollable: true,
                            }).data("kendoGrid");
                            return grid;
                        };
                        InventoryMovementBulkViewer.prototype.initializeColumns = function () {
                            var columns = [];
                            columns.push({
                                title: "Date",
                                field: "CreatedOn",
                                width: 70,
                                template: function (data) {
                                    if (data.CreatedOn !== undefined && data.CreatedOn !== null) {
                                        return kendo.toString(data.CreatedOn, "d");
                                    }
                                    else {
                                        return "";
                                    }
                                },
                            });
                            columns.push({
                                title: "QTY",
                                field: "QuantityAbsolute",
                                width: 75,
                                template: function (data) {
                                    if (data.QuantityAbsolute !== undefined && data.QuantityAbsolute !== null) {
                                        if (data.RequiredQuantity !== undefined && data.RequiredQuantity !== null) {
                                            return data.QuantityAbsolute + " / " + data.RequiredQuantity;
                                        }
                                        else {
                                            return "" + data.QuantityAbsolute;
                                        }
                                    }
                                },
                            });
                            columns.push({
                                title: "Job",
                                width: 50,
                                template: function (data) {
                                    if (data.HandpieceId !== undefined &&
                                        data.HandpieceId !== null &&
                                        data.HandpieceNumber !== undefined &&
                                        data.HandpieceNumber !== null) {
                                        return "<a href=\"/Handpieces/Edit/" + data.HandpieceId + "\">" + data.HandpieceNumber + "</a>";
                                    }
                                    else {
                                        return "Shelf";
                                    }
                                },
                            });
                            columns.push({
                                title: "Job Status",
                                width: 100,
                                template: function (data) {
                                    if (data.HandpieceStatus !== undefined && data.HandpieceStatus !== null) {
                                        return Shared.HandpieceStatusHelper.toDisplayString(data.HandpieceStatus);
                                    }
                                    return "";
                                },
                            });
                            columns.push({
                                title: "Parts Comment",
                                field: "PartsComment",
                                width: 65,
                                template: function (data) {
                                    if (data.PartsComment === undefined || data.PartsComment === null) {
                                        return "";
                                    }
                                    var span = document.createElement("span");
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
                                template: function (data) {
                                    if (data.MovementComment === undefined || data.MovementComment === null) {
                                        return "";
                                    }
                                    var span = document.createElement("span");
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
                                template: function (data) {
                                    if (!(data.Price === undefined || data.Price === null)) {
                                        return "$" + kendo.toString(data.Price, "#,##0.##");
                                    }
                                    if (!(data.AveragePrice === undefined || data.AveragePrice === null)) {
                                        return "<i>$" + kendo.toString(data.AveragePrice, "#,##0.##") + "</i>";
                                    }
                                    return "";
                                },
                            });
                            columns.push({
                                title: "Total Price",
                                width: 75,
                                template: function (data) {
                                    if (data.QuantityAbsolute === undefined || data.QuantityAbsolute === null) {
                                        return "";
                                    }
                                    if (!(data.Price === undefined || data.Price === null)) {
                                        return "$" + kendo.toString(data.Price * data.QuantityAbsolute, "#,##0.##");
                                    }
                                    if (!(data.AveragePrice === undefined || data.AveragePrice === null)) {
                                        return "<i>$" + kendo.toString(data.AveragePrice * data.QuantityAbsolute, "#,##0.##") + "</i>";
                                    }
                                    return "";
                                }
                            });
                            columns.push({
                                title: "Actions",
                                width: 240,
                                command: []
                            });
                            return columns;
                        };
                        InventoryMovementBulkViewer.prototype.createDataSource = function () {
                            var dataSource = new kendo.data.DataSource({
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
                        };
                        return InventoryMovementBulkViewer;
                    }());
                    Shared.InventoryMovementBulkViewer = InventoryMovementBulkViewer;
                })(Shared = InventoryMovements.Shared || (InventoryMovements.Shared = {}));
            })(InventoryMovements = Pages.InventoryMovements || (Pages.InventoryMovements = {}));
        })(Pages = CRM.Pages || (CRM.Pages = {}));
    })(CRM = DentalDrill.CRM || (DentalDrill.CRM = {}));
})(DentalDrill || (DentalDrill = {}));
//# sourceMappingURL=InventoryMovementBulkViewer.js.map