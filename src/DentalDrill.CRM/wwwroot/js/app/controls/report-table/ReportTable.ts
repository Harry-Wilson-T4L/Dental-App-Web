namespace DentalDrill.CRM.Controls.Reporting {
    import EventHandler = DevGuild.Utilities.EventHandler;

    export interface ReportTableHeaderRowFormatted<TDataItem> {
        row: HTMLTableRowElement;
    }

    export interface ReportTableDataRowFormatted<TDataItem> {
        row: HTMLTableRowElement;
        item: TDataItem;
    }

    export interface ReportTableHeaderCellFormatted<TDataItem> {
        row: HTMLTableRowElement;
        cell: HTMLTableCellElement;
        columnIndex: number;
        column: ReportTableColumn<TDataItem>;
    }

    export interface ReportTableDataCellFormatted<TDataItem> {
        row: HTMLTableRowElement;
        item: TDataItem;
        cell: HTMLTableCellElement;
        columnIndex: number;
        column: ReportTableColumn<TDataItem>;
    }

    export class ReportTable<TDataItem> {
        private readonly _columnGroups: { start: number, count: number, title: string }[] = [];
        private readonly _columns: ReportTableColumn<TDataItem>[] = [];
        private _items: TDataItem[];
        private _rowHeight: number;

        private readonly _headerRowFormatted: EventHandler<ReportTableHeaderRowFormatted<TDataItem>>;
        private readonly _headerCellFormatted: EventHandler<ReportTableHeaderCellFormatted<TDataItem>>;
        private readonly _dataRowFormatted: EventHandler<ReportTableDataRowFormatted<TDataItem>>;
        private readonly _dataCellFormatted: EventHandler<ReportTableDataCellFormatted<TDataItem>>;

        constructor() {
            this._headerRowFormatted = new EventHandler<ReportTableHeaderRowFormatted<TDataItem>>();
            this._headerCellFormatted = new EventHandler<ReportTableHeaderCellFormatted<TDataItem>>();
            this._dataRowFormatted = new EventHandler<ReportTableDataRowFormatted<TDataItem>>();
            this._dataCellFormatted = new EventHandler<ReportTableDataCellFormatted<TDataItem>>();
        }

        get columns(): ReportTableColumn<TDataItem>[] {
            return this._columns;
        }

        get items(): TDataItem[] {
            return this._items;
        }

        get rowHeight(): number {
            return this._rowHeight;
        }

        set rowHeight(val: number) {
            this._rowHeight = val;
        }

        get headerRowFormatted(): EventHandler<ReportTableHeaderRowFormatted<TDataItem>> {
            return this._headerRowFormatted;
        }

        get headerCellFormatted(): EventHandler<ReportTableHeaderCellFormatted<TDataItem>> {
            return this._headerCellFormatted;
        }

        get dataRowFormatted(): EventHandler<ReportTableDataRowFormatted<TDataItem>> {
            return this._dataRowFormatted;
        }

        get dataCellFormatted(): EventHandler<ReportTableDataCellFormatted<TDataItem>> {
            return this._dataCellFormatted;
        }

        addColumn(title: string, width: number, formatter: (item: TDataItem) => string) {
            const column = new ReportTableColumn<TDataItem>(title, width, formatter);
            this._columns.push(column);
        }

        groupLastColumns(title: string, numberOfColumns: number) {
            const startColumn = this._columns.length - numberOfColumns;
            this._columnGroups.push({
                title: title,
                start: startColumn,
                count: numberOfColumns
            });
        }

        setData(items: TDataItem[], sort?: (left: TDataItem, right: TDataItem) => number) {
            this._items = items;
            if (sort) {
                this._items.sort(sort);
            }
        }

        measureWidth(startColumn: number, numberOfColumns: number): number {
            let width = 0;
            for (let i = 0; i < numberOfColumns; i++) {
                const column = this._columns[startColumn + i];
                width += column.width;
            }

            return width;
        }

        measureHeight(startRow: number, numberOfRows: number): number {
            let headerRows = 1;
            if (this._columnGroups.length > 0) {
                headerRows ++;
            }

            return (numberOfRows + headerRows) * this._rowHeight;
        }

        renderFragment(startColumn: number, numberOfColumns: number, startRow: number, numberOfRows: number): HTMLTableElement {
            const table = document.createElement("table");
            const colGroup = document.createElement("colgroup");
            for (let i = 0; i < numberOfColumns; i++) {
                const column = this._columns[startColumn + i];
                const col = document.createElement("col");
                col.style.width = `${column.width}px`;

                colGroup.appendChild(col);
            }

            table.appendChild(colGroup);

            const head = table.createTHead();
            const skippedColumns: number[] = [];
            if (this._columnGroups.length > 0) {
                const groupsRow = head.insertRow();
                let lastGroup = undefined;
                for (let i = 0; i < numberOfColumns; i++) {
                    const column = this._columns[startColumn + i];
                    const group = this.findGroup(startColumn + i);
                    if (group) {
                        if (group !== lastGroup) {
                            lastGroup = group;
                            const th = document.createElement("th");
                            th.colSpan = this.computeSpan(group.start, group.count, startColumn, numberOfColumns);

                            const textContainer = document.createElement("div");
                            textContainer.appendChild(document.createTextNode(group.title));
                            th.appendChild(textContainer);

                            groupsRow.appendChild(th);
                        }
                    } else {
                        lastGroup = undefined;
                        const th = document.createElement("th");
                        th.rowSpan = 2;
                        const textContainer = document.createElement("div");
                        textContainer.appendChild(document.createTextNode(column.title));
                        th.appendChild(textContainer);

                        groupsRow.appendChild(th);
                        skippedColumns.push(startColumn + i);
                    }
                }
            }

            console.log(skippedColumns);

            const headRow = head.insertRow();
            for (let i = 0; i < numberOfColumns; i++) {
                if (skippedColumns.indexOf(startColumn + i) >= 0) {
                    continue;
                }

                const column = this._columns[startColumn + i];
                const th = document.createElement("th");

                const textContainer = document.createElement("div");
                textContainer.appendChild(document.createTextNode(column.title));

                th.appendChild(textContainer);
                headRow.appendChild(th);

                this.formatHeaderCell(headRow, th, startColumn + i);
            }

            this.formatHeaderRow(headRow);

            const body = table.createTBody();
            for (let i = 0; i < numberOfRows; i++) {
                const item = this._items[startRow + i];
                const bodyRow = body.insertRow();
                for (let j = 0; j < numberOfColumns; j++) {
                    const column = this._columns[startColumn + j];
                    const cell = bodyRow.insertCell();

                    const textContainer = document.createElement("div");
                    textContainer.appendChild(document.createTextNode(column.format(item)));
                    cell.appendChild(textContainer);

                    this.formatDataCell(bodyRow, cell, item, startColumn + j);
                }

                this.formatDataRow(bodyRow, item);
            }

            return table;
        }

        private findGroup(index: number): { title: string, start: number, count: number } {
            for (let i = 0; i < this._columnGroups.length; i++) {
                const group = this._columnGroups[i];
                if (index >= group.start && index < (group.start + group.count)) {
                    return group;
                }
            }

            return undefined;
        }

        private computeSpan(groupStart: number, groupCount: number, fragmentStart: number, fragmentCount: number) {
            let result = 0;
            for (let i = 0; i < fragmentCount; i++) {
                const index = fragmentStart + i;
                if (index >= groupStart && index < (groupStart + groupCount)) {
                    result++;
                }
            }

            return result;
        }

        private formatHeaderRow(row: HTMLTableRowElement): void {
            this._headerRowFormatted.raise(this, {
                row: row
            });
        }

        private formatHeaderCell(row: HTMLTableRowElement, cell: HTMLTableHeaderCellElement, index: number): void {
            this._headerCellFormatted.raise(this, {
                row: row,
                cell: cell,
                columnIndex: index,
                column: this._columns[index]
            });
        }

        private formatDataRow(row: HTMLTableRowElement, item: TDataItem): void {
            this._dataRowFormatted.raise(this, {
                row: row,
                item: item
            });
        }

        private formatDataCell(row: HTMLTableRowElement, cell: HTMLTableCellElement, item: TDataItem, index: number): void {
            this._dataCellFormatted.raise(this, {
                row: row,
                item: item,
                cell: cell,
                columnIndex: index,
                column: this._columns[index]
            });
        }
    }
}