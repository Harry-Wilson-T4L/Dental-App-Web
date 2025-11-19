declare namespace kendo {
    var dom: any;
}

declare namespace kendo.dataviz.ui {
    interface TreeMap {
        dataItem(tile: JQuery | Element | string): kendo.data.Node;
    }
}