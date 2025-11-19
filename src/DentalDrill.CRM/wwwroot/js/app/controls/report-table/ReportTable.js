var DentalDrill;
(function (DentalDrill) {
    var CRM;
    (function (CRM) {
        var Controls;
        (function (Controls) {
            var Reporting;
            (function (Reporting) {
                var EventHandler = DevGuild.Utilities.EventHandler;
                var ReportTable = /** @class */ (function () {
                    function ReportTable() {
                        this._columnGroups = [];
                        this._columns = [];
                        this._headerRowFormatted = new EventHandler();
                        this._headerCellFormatted = new EventHandler();
                        this._dataRowFormatted = new EventHandler();
                        this._dataCellFormatted = new EventHandler();
                    }
                    Object.defineProperty(ReportTable.prototype, "columns", {
                        get: function () {
                            return this._columns;
                        },
                        enumerable: false,
                        configurable: true
                    });
                    Object.defineProperty(ReportTable.prototype, "items", {
                        get: function () {
                            return this._items;
                        },
                        enumerable: false,
                        configurable: true
                    });
                    Object.defineProperty(ReportTable.prototype, "rowHeight", {
                        get: function () {
                            return this._rowHeight;
                        },
                        set: function (val) {
                            this._rowHeight = val;
                        },
                        enumerable: false,
                        configurable: true
                    });
                    Object.defineProperty(ReportTable.prototype, "headerRowFormatted", {
                        get: function () {
                            return this._headerRowFormatted;
                        },
                        enumerable: false,
                        configurable: true
                    });
                    Object.defineProperty(ReportTable.prototype, "headerCellFormatted", {
                        get: function () {
                            return this._headerCellFormatted;
                        },
                        enumerable: false,
                        configurable: true
                    });
                    Object.defineProperty(ReportTable.prototype, "dataRowFormatted", {
                        get: function () {
                            return this._dataRowFormatted;
                        },
                        enumerable: false,
                        configurable: true
                    });
                    Object.defineProperty(ReportTable.prototype, "dataCellFormatted", {
                        get: function () {
                            return this._dataCellFormatted;
                        },
                        enumerable: false,
                        configurable: true
                    });
                    ReportTable.prototype.addColumn = function (title, width, formatter) {
                        var column = new Reporting.ReportTableColumn(title, width, formatter);
                        this._columns.push(column);
                    };
                    ReportTable.prototype.groupLastColumns = function (title, numberOfColumns) {
                        var startColumn = this._columns.length - numberOfColumns;
                        this._columnGroups.push({
                            title: title,
                            start: startColumn,
                            count: numberOfColumns
                        });
                    };
                    ReportTable.prototype.setData = function (items, sort) {
                        this._items = items;
                        if (sort) {
                            this._items.sort(sort);
                        }
                    };
                    ReportTable.prototype.measureWidth = function (startColumn, numberOfColumns) {
                        var width = 0;
                        for (var i = 0; i < numberOfColumns; i++) {
                            var column = this._columns[startColumn + i];
                            width += column.width;
                        }
                        return width;
                    };
                    ReportTable.prototype.measureHeight = function (startRow, numberOfRows) {
                        var headerRows = 1;
                        if (this._columnGroups.length > 0) {
                            headerRows++;
                        }
                        return (numberOfRows + headerRows) * this._rowHeight;
                    };
                    ReportTable.prototype.renderFragment = function (startColumn, numberOfColumns, startRow, numberOfRows) {
                        var table = document.createElement("table");
                        var colGroup = document.createElement("colgroup");
                        for (var i = 0; i < numberOfColumns; i++) {
                            var column = this._columns[startColumn + i];
                            var col = document.createElement("col");
                            col.style.width = column.width + "px";
                            colGroup.appendChild(col);
                        }
                        table.appendChild(colGroup);
                        var head = table.createTHead();
                        var skippedColumns = [];
                        if (this._columnGroups.length > 0) {
                            var groupsRow = head.insertRow();
                            var lastGroup = undefined;
                            for (var i = 0; i < numberOfColumns; i++) {
                                var column = this._columns[startColumn + i];
                                var group = this.findGroup(startColumn + i);
                                if (group) {
                                    if (group !== lastGroup) {
                                        lastGroup = group;
                                        var th = document.createElement("th");
                                        th.colSpan = this.computeSpan(group.start, group.count, startColumn, numberOfColumns);
                                        var textContainer = document.createElement("div");
                                        textContainer.appendChild(document.createTextNode(group.title));
                                        th.appendChild(textContainer);
                                        groupsRow.appendChild(th);
                                    }
                                }
                                else {
                                    lastGroup = undefined;
                                    var th = document.createElement("th");
                                    th.rowSpan = 2;
                                    var textContainer = document.createElement("div");
                                    textContainer.appendChild(document.createTextNode(column.title));
                                    th.appendChild(textContainer);
                                    groupsRow.appendChild(th);
                                    skippedColumns.push(startColumn + i);
                                }
                            }
                        }
                        console.log(skippedColumns);
                        var headRow = head.insertRow();
                        for (var i = 0; i < numberOfColumns; i++) {
                            if (skippedColumns.indexOf(startColumn + i) >= 0) {
                                continue;
                            }
                            var column = this._columns[startColumn + i];
                            var th = document.createElement("th");
                            var textContainer = document.createElement("div");
                            textContainer.appendChild(document.createTextNode(column.title));
                            th.appendChild(textContainer);
                            headRow.appendChild(th);
                            this.formatHeaderCell(headRow, th, startColumn + i);
                        }
                        this.formatHeaderRow(headRow);
                        var body = table.createTBody();
                        for (var i = 0; i < numberOfRows; i++) {
                            var item = this._items[startRow + i];
                            var bodyRow = body.insertRow();
                            for (var j = 0; j < numberOfColumns; j++) {
                                var column = this._columns[startColumn + j];
                                var cell = bodyRow.insertCell();
                                var textContainer = document.createElement("div");
                                textContainer.appendChild(document.createTextNode(column.format(item)));
                                cell.appendChild(textContainer);
                                this.formatDataCell(bodyRow, cell, item, startColumn + j);
                            }
                            this.formatDataRow(bodyRow, item);
                        }
                        return table;
                    };
                    ReportTable.prototype.findGroup = function (index) {
                        for (var i = 0; i < this._columnGroups.length; i++) {
                            var group = this._columnGroups[i];
                            if (index >= group.start && index < (group.start + group.count)) {
                                return group;
                            }
                        }
                        return undefined;
                    };
                    ReportTable.prototype.computeSpan = function (groupStart, groupCount, fragmentStart, fragmentCount) {
                        var result = 0;
                        for (var i = 0; i < fragmentCount; i++) {
                            var index = fragmentStart + i;
                            if (index >= groupStart && index < (groupStart + groupCount)) {
                                result++;
                            }
                        }
                        return result;
                    };
                    ReportTable.prototype.formatHeaderRow = function (row) {
                        this._headerRowFormatted.raise(this, {
                            row: row
                        });
                    };
                    ReportTable.prototype.formatHeaderCell = function (row, cell, index) {
                        this._headerCellFormatted.raise(this, {
                            row: row,
                            cell: cell,
                            columnIndex: index,
                            column: this._columns[index]
                        });
                    };
                    ReportTable.prototype.formatDataRow = function (row, item) {
                        this._dataRowFormatted.raise(this, {
                            row: row,
                            item: item
                        });
                    };
                    ReportTable.prototype.formatDataCell = function (row, cell, item, index) {
                        this._dataCellFormatted.raise(this, {
                            row: row,
                            item: item,
                            cell: cell,
                            columnIndex: index,
                            column: this._columns[index]
                        });
                    };
                    return ReportTable;
                }());
                Reporting.ReportTable = ReportTable;
            })(Reporting = Controls.Reporting || (Controls.Reporting = {}));
        })(Controls = CRM.Controls || (CRM.Controls = {}));
    })(CRM = DentalDrill.CRM || (DentalDrill.CRM = {}));
})(DentalDrill || (DentalDrill = {}));
//# sourceMappingURL=ReportTable.js.map