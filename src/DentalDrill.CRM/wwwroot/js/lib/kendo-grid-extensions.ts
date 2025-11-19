declare namespace kendo.ui {
    interface Grid {
        saveExpandedState(field: string): object[];
        saveExpandedState(field: string, customSave: (dest: object, row: HTMLElement, dataItem: kendo.data.ObservableObject) => void): object[];
        saveExpandedState(field: string, customSave?: (dest: object, row: HTMLElement, dataItem: kendo.data.ObservableObject) => void): object[];

        restoreExpandedState(field: string, expanded: object[]): void;
        restoreExpandedState(field: string, expanded: object[], customLoad: (src: object, row: HTMLElement, dataItem: kendo.data.ObservableObject) => void): void;
        restoreExpandedState(field: string, expanded: object[], customLoad?: (src: object, row: HTMLElement, dataItem: kendo.data.ObservableObject) => void): void;

        persistExpandedState(field: string): void;
        persistExpandedState(field: string, customSave: (dest: object, row: HTMLElement, dataItem: kendo.data.ObservableObject) => void, customLoad: (src: object, row: HTMLElement, dataItem: kendo.data.ObservableObject) => void): void;
        persistExpandedState(field: string, customSave?: (dest: object, row: HTMLElement, dataItem: kendo.data.ObservableObject) => void, customLoad?: (src: object, row: HTMLElement, dataItem: kendo.data.ObservableObject) => void): void;

        autoResize(): void;
        autoResize(resizer: (grid: kendo.ui.Grid) => void): void;
        autoResize(resizer?: (grid: kendo.ui.Grid) => void): void;

        autoResizeWhen(predicate: () => boolean): void;
        autoResizeWhen(predicate: () => boolean, resizer: (grid: kendo.ui.Grid) => void): void;
        autoResizeWhen(predicate: () => boolean, resizer?: (grid: kendo.ui.Grid) => void): void;
    }

    interface TreeList {
        saveExpandedState(field: string): object[];
        restoreExpandedState(field: string, expanded: object[]): Promise<void>;
        persistExpandedState(field: string): void;
    }
}

kendo.ui.Grid.prototype["saveExpandedState"] = function(this: kendo.ui.Grid, field: string, customSave?: (dest: object, row: HTMLElement, dataItem: kendo.data.ObservableObject) => void): object[] {
    const grid: kendo.ui.Grid = this;
    const expanded: object[] = [];
    grid.tbody.children(":has(> .k-hierarchy-cell > .k-i-collapse)").each((index, row) => {
        const item = grid.dataItem(row);
        const fieldValue = item.get(field);

        const info: object = { };
        info[field] = fieldValue;
        if (customSave) {
            customSave(info, row, item);
        }

        expanded.push(info);
    });

    return expanded;
}

kendo.ui.Grid.prototype["restoreExpandedState"] = function(this: kendo.ui.Grid, field: string, expanded: object[], customLoad?: (src: object, row: HTMLElement, dataItem: kendo.data.ObservableObject) => void): void {
    const grid: kendo.ui.Grid = this;
    const rows: { src: object, row: HTMLElement, dataItem: kendo.data.ObservableObject }[] = [];
    grid.tbody.children().each((index, row) => {
        const item = grid.dataItem(row);
        const fieldValue = item.get(field);
        const matches = expanded.filter(x => x[field] == fieldValue)
        if (matches.length > 0) {
            const match = matches[0];
            rows.push({
                src: match,
                row: row,
                dataItem: item,
            });
        }
    });

    for (let row of rows) {
        grid.expandRow(row.row);
        if (customLoad) {
            customLoad(row.src, row.row, row.dataItem);
        }
    }
}

kendo.ui.Grid.prototype["persistExpandedState"] = function(this: kendo.ui.Grid, field: string, customSave?: (dest: object, row: HTMLElement, dataItem: kendo.data.ObservableObject) => void, customLoad?: (src: object, row: HTMLElement, dataItem: kendo.data.ObservableObject) => void) {
    const grid: kendo.ui.Grid = this;
    const state = grid.saveExpandedState(field, customSave);

    grid.one("dataBound", (e: kendo.ui.GridDataBoundEvent) => {
        grid.restoreExpandedState(field, state, customLoad);
    });
}

kendo.ui.TreeList.prototype["saveExpandedState"] = function(this: kendo.ui.TreeList, field: string): object[] {
    const treeList: kendo.ui.TreeList = this;
    const expanded: object[] = [];
    treeList.tbody.children(":has(> td:first-child > .k-i-collapse)").each((index, row) => {
        const level = $(row).find("td:first-child > .k-i-none").length;
        const item = treeList.dataItem(row);
        const fieldValue = item.get(field);

        const info: object = { };
        info[field] = fieldValue;
        info["__level"] = level;
        
        expanded.push(info);
    });

    return expanded;
}

kendo.ui.TreeList.prototype["restoreExpandedState"] = async function(this: kendo.ui.TreeList, field: string, expanded: object[]): Promise<void> {
    const treeList: kendo.ui.TreeList = this;
    
    let level = 0;
    let expandedOfLevel = expanded.filter(x => x["__level"] === level);
    while (expandedOfLevel.length > 0) {
        for (let match of expandedOfLevel) {
            const expectedFieldValue = match[field];
            const rows = treeList.tbody.children().filter((index, row) => {
                const item = treeList.dataItem(row);
                const fieldValue = item.get(field);
                return fieldValue === expectedFieldValue;
            });

            if (rows.length > 0) {
                await treeList.expand(rows[0]);
            }
        }

        level++;
        expandedOfLevel = expanded.filter(x => x["__level"] === level);
    }
}

kendo.ui.TreeList.prototype["persistExpandedState"] = function(this: kendo.ui.TreeList, field: string): void {
    const treeList: kendo.ui.TreeList = this;
    const state = treeList.saveExpandedState(field);

    treeList.one("dataBound", async (e: kendo.ui.TreeListDataBoundEvent) => {
        await treeList.restoreExpandedState(field, state);
    });
}

kendo.ui.Grid.prototype["autoResize"] = function (this: kendo.ui.Grid, resizer?: (grid: kendo.ui.Grid) => void): void {
    this.autoResizeWhen(() => true, resizer);
}

kendo.ui.Grid.prototype["autoResizeWhen"] = function (this: kendo.ui.Grid, predicate: () => boolean, resizer?: (grid: kendo.ui.Grid) => void): void {
    const grid: kendo.ui.Grid = this;

    let lastKnownHeight: number = document.documentElement.clientHeight;
    let timeoutHandle: number = undefined;
    let handler = () => {
        if (lastKnownHeight != document.documentElement.clientHeight) {
            lastKnownHeight = document.documentElement.clientHeight;
            if (predicate()) {
                if (resizer !== undefined) {
                    resizer(grid);
                } else {
                    grid.setOptions({ height: "100px" });
                    grid.resize();

                    grid.setOptions({ height: "100%" });
                    grid.resize();
                }
            }
        }

        timeoutHandle = undefined;
    };

    $(window).on("resize", e => {
        if (timeoutHandle !== undefined) {
            clearTimeout(timeoutHandle);
        }

        timeoutHandle = setTimeout(handler, 500);
    });
}

$(() => {
    setTimeout(() => {
        const autoResizeGrids = document.querySelectorAll(".k-grid.k-grid--auto-resize");
        for (let i = 0; i < autoResizeGrids.length; i++) {
            const grid = $(autoResizeGrids[i]).data("kendoGrid");
            if (grid) {
                grid.autoResize();
            }
        }
    });
});