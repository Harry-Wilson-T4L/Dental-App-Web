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
                    var InventoryMovementBulkEditor = /** @class */ (function () {
                        function InventoryMovementBulkEditor(root, movements) {
                            this._root = root;
                            this._movements = movements;
                        }
                        InventoryMovementBulkEditor.prototype.init = function () {
                            var _this = this;
                            this._items = this._movements.getBulkMovements();
                            if (!this._items.some(function (x) { return x.HandpieceId === undefined || x.HandpieceId === null; })) {
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
                            else if (this._movements.extraInitialQuantity && this._movements.extraInitialQuantity > 0) {
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
                            this._form = this._root.querySelector("form.inventory-group-editor__form");
                            this._form.addEventListener("submit", function (e) { return _this.onBeforeSubmit(e); });
                            this._totalQuantity = this._root.querySelector("input.inventory-group-editor__total-quantity");
                            this._totalQuantity.addEventListener("change", function (e) { return _this.updateQuantityFromTotal(); });
                            this._movementsResult = new Shared.HiddenInputList(this._root.querySelector("div.inventory-group-editor__data"));
                            this.updateTotalQuantityFromItems();
                        };
                        InventoryMovementBulkEditor.prototype.createGrid = function () {
                            var _this = this;
                            var gridContainer = this._root.querySelector(".grid-container");
                            var gridContainerElement = document.createElement("div");
                            gridContainerElement.classList.add("k-grid-commandicons2");
                            gridContainerElement.classList.add("k-grid--dense");
                            gridContainerElement.classList.add("k-grid--small-text");
                            gridContainer.appendChild(gridContainerElement);
                            var grid = $(gridContainerElement).kendoGrid({
                                columns: this.initializeColumns(),
                                dataSource: this._dataSource,
                                editable: "incell",
                                scrollable: true,
                                dataBound: function (e) {
                                    e.sender.tbody.find("tr[role='row']").each(function (index, element) {
                                        var item = e.sender.dataItem(element);
                                        _this.updateRowStatus(element, item);
                                        $(element).find(".inventory-movements-comment").kendoTooltip();
                                    });
                                },
                                save: function (e) {
                                    setTimeout(function () {
                                        _this.updateTotalQuantityFromItems();
                                    });
                                }
                            }).data("kendoGrid");
                            return grid;
                        };
                        InventoryMovementBulkEditor.prototype.updateRowStatus = function (row, item) {
                            var postponeButton = row.querySelector(".k-grid-CustomPostpone");
                            var cancelButton = row.querySelector(".k-grid-CustomCancel");
                            switch (item.BulkEditStatus) {
                                case Shared.BulkEditStatus.Normal:
                                    postponeButton.classList.remove("active");
                                    cancelButton.classList.remove("active");
                                    break;
                                case Shared.BulkEditStatus.Postponed:
                                    postponeButton.classList.add("active");
                                    cancelButton.classList.remove("active");
                                    break;
                                case Shared.BulkEditStatus.Cancelled:
                                    postponeButton.classList.remove("active");
                                    cancelButton.classList.add("active");
                                    break;
                            }
                        };
                        InventoryMovementBulkEditor.prototype.initializeColumns = function () {
                            var _this = this;
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
                                editor: function (container, options) {
                                    var input = document.createElement("input");
                                    input.type = "number";
                                    input.name = options.field;
                                    input.classList.add("k-textbox");
                                    var item = options.model;
                                    if (item && item.RequiredQuantity !== undefined && item.RequiredQuantity !== null) {
                                        input.addEventListener("change", function (e) {
                                            var value = parseFloat(input.value);
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
                                editor: function (container, options) {
                                    var input = document.createElement("input");
                                    input.type = "number";
                                    input.name = options.field;
                                    input.classList.add("k-textbox");
                                    container.append(input);
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
                                command: [
                                    {
                                        name: "CustomOpenJob",
                                        iconClass: "fas fa-link",
                                        text: "&nbsp; Job",
                                        click: function (e) {
                                            e.preventDefault();
                                        }
                                    },
                                    {
                                        name: "CustomPostpone",
                                        iconClass: "fas fa-clock",
                                        text: "&nbsp; Postpone",
                                        click: function (e) {
                                            e.preventDefault();
                                            var row = e.target.closest("tr");
                                            var dataItem = new Shared.InventoryMovementBulkModelWrapper(_this._grid.dataItem(row));
                                            if (dataItem.BulkEditStatus !== Shared.BulkEditStatus.Postponed) {
                                                dataItem.BulkEditStatus = Shared.BulkEditStatus.Postponed;
                                            }
                                            else {
                                                dataItem.BulkEditStatus = Shared.BulkEditStatus.Normal;
                                            }
                                            _this._dataSource.sync();
                                            _this.updateRowStatus(row, dataItem);
                                            _this.updateTotalQuantityFromItems();
                                        }
                                    },
                                    {
                                        name: "CustomCancel",
                                        iconClass: "fas fa-ban",
                                        text: "&nbsp; Cancel",
                                        click: function (e) {
                                            e.preventDefault();
                                            var row = e.target.closest("tr");
                                            var dataItem = new Shared.InventoryMovementBulkModelWrapper(_this._grid.dataItem(row));
                                            if (dataItem.BulkEditStatus !== Shared.BulkEditStatus.Cancelled) {
                                                dataItem.BulkEditStatus = Shared.BulkEditStatus.Cancelled;
                                            }
                                            else {
                                                dataItem.BulkEditStatus = Shared.BulkEditStatus.Normal;
                                            }
                                            _this._dataSource.sync();
                                            _this.updateRowStatus(row, dataItem);
                                            _this.updateTotalQuantityFromItems();
                                        }
                                    }
                                ]
                            });
                            return columns;
                        };
                        InventoryMovementBulkEditor.prototype.createDataSource = function () {
                            var dataSource = new kendo.data.DataSource({
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
                        };
                        InventoryMovementBulkEditor.prototype.updateTotalQuantityFromItems = function () {
                            var items = this._dataSource.data();
                            var totalQuantityValue = items
                                .filter(function (x) { return x.BulkEditStatus === Shared.BulkEditStatus.Normal; })
                                .reduce(function (prev, curr) { return prev + curr.QuantityAbsolute; }, 0);
                            this._totalQuantity.value = totalQuantityValue.toString();
                            this._lastQuantity = totalQuantityValue;
                        };
                        InventoryMovementBulkEditor.prototype.updateQuantityFromTotal = function () {
                            var totalQuantity = parseFloat(this._totalQuantity.value);
                            var items = this._dataSource.data()
                                .map(function (x) { return new Shared.InventoryMovementBulkModelWrapper(x); })
                                .filter(function (x) { return x.BulkEditStatus === Shared.BulkEditStatus.Normal; });
                            if (typeof this._lastQuantity === "number" && typeof totalQuantity === "number") {
                                var delta = totalQuantity - this._lastQuantity;
                                if (delta > 0) {
                                    // Increasing total quantity by <delta>
                                    var remaining = delta;
                                    // Passing through movements with quantity below requested first
                                    for (var i = 0; i < items.length; i++) {
                                        if (items[i].RequiredQuantity !== undefined &&
                                            items[i].RequiredQuantity !== null &&
                                            items[i].RequiredQuantity > items[i].QuantityAbsolute) {
                                            var missing = items[i].RequiredQuantity - items[i].QuantityAbsolute;
                                            var increase = Math.min(remaining, missing);
                                            remaining -= increase;
                                            items[i].Quantity += increase;
                                            items[i].QuantityAbsolute += increase;
                                        }
                                    }
                                    // Trying to dump the rest into first Shelf movement if its present
                                    var firstShelf = items.filter(function (x) { return x.HandpieceId === undefined || x.HandpieceId === null; })[0];
                                    if (firstShelf) {
                                        firstShelf.Quantity += remaining;
                                        firstShelf.QuantityAbsolute += remaining;
                                    }
                                    else {
                                        // Otherwise - dumping everything to first item
                                        items[0].Quantity += remaining;
                                        items[0].QuantityAbsolute += remaining;
                                    }
                                }
                                else if (delta < 0) {
                                    // Decreasing total quantity by <delta>
                                    var remaining = -delta;
                                    // Passing through all Shelf movements first in reverse order
                                    for (var i = items.length - 1; i >= 0; i--) {
                                        if (items[i].HandpieceId === undefined || items[i].HandpieceId === null) {
                                            var reduction = Math.min(items[i].QuantityAbsolute, remaining);
                                            remaining -= reduction;
                                            items[i].Quantity -= reduction;
                                            items[i].QuantityAbsolute -= reduction;
                                        }
                                    }
                                    // Passing through remaining movements in reverse order
                                    for (var i = items.length - 1; i >= 0; i--) {
                                        var reduction = Math.min(items[i].QuantityAbsolute, remaining);
                                        remaining -= reduction;
                                        items[i].Quantity -= reduction;
                                        items[i].QuantityAbsolute -= reduction;
                                    }
                                }
                                this._lastQuantity = totalQuantity;
                                this._grid.dataSource.sync();
                            }
                        };
                        InventoryMovementBulkEditor.prototype.onBeforeSubmit = function (e) {
                            e.preventDefault();
                            this._movementsResult.clear();
                            var items = this._dataSource.data();
                            for (var i = 0; i < items.length; i++) {
                                var item = items[i];
                                this._movementsResult.add("Result[" + i + "].Id", item.Id);
                                if (item.QuantityAbsolute !== undefined && item.QuantityAbsolute !== null) {
                                    this._movementsResult.add("Result[" + i + "].Quantity", item.QuantityAbsolute.toString());
                                }
                                if (item.Price !== undefined && item.Price !== null) {
                                    this._movementsResult.add("Result[" + i + "].Price", item.Price.toString());
                                }
                                this._movementsResult.add("Result[" + i + "].BulkEditStatus", item.BulkEditStatus.toString());
                            }
                        };
                        return InventoryMovementBulkEditor;
                    }());
                    Shared.InventoryMovementBulkEditor = InventoryMovementBulkEditor;
                })(Shared = InventoryMovements.Shared || (InventoryMovements.Shared = {}));
            })(InventoryMovements = Pages.InventoryMovements || (Pages.InventoryMovements = {}));
        })(Pages = CRM.Pages || (CRM.Pages = {}));
    })(CRM = DentalDrill.CRM || (DentalDrill.CRM = {}));
})(DentalDrill || (DentalDrill = {}));
//# sourceMappingURL=InventoryMovementBulkEditor.js.map