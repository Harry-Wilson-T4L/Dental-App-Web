namespace DevGuild.AspNet.Controls.Grids.Handlers {
    export class GridHandlers {
        static createButtonClickHandler<T>(action: (item: kendo.data.ObservableObject & T) => void): Function {
            return function (this: kendo.ui.Grid, e: JQueryEventObject, original?: JQueryEventObject) {
                e.preventDefault();
                const dataItem = this.dataItem<T>(e.currentTarget.closest("tr"));
                action(dataItem);
            };
        }

        static createButtonClickNavigationHandler<T>(routeSelector: (item: kendo.data.ObservableObject & T) => AspNet.Routing.Uri): Function {
            return function (this: kendo.ui.Grid, e: JQueryEventObject, original?: JQueryEventObject) {
                e.preventDefault();
                const dataItem = this.dataItem<T>(e.currentTarget.closest("tr"));
                const url = routeSelector(dataItem);
                url.execute(e.ctrlKey || original && original.which === 2);
            };
        }

        static createGridButtonClickNavigationHandler(
            buttonSelector: string,
            routeSelector: (target: JQuery<Element>) => AspNet.Routing.Uri): object {
            $(document).on("click", buttonSelector, async (e: JQueryEventObject) => {
                e.preventDefault();
                const target = $(e.target);
                const url = routeSelector(target);
                url.navigate();
            });

            return {};
        }

        static createGridButtonClickPopupHandler(
            buttonSelector: string,
            routeSelector: (target: JQuery<Element>) => AspNet.Routing.Uri,
            options?: (target: JQuery<Element>) => object
        ): object {
            $(document).on("click", buttonSelector, async (e: JQueryEventObject) => {
                e.preventDefault();
                const target = $(e.target);
                const url = routeSelector(target);

                if (e.ctrlKey) {
                    url.open();
                } else {
                    const dialogRoot = $("<div></div>");
                    const dialogOptions = {
                        title: "",
                        actions: ["close"],
                        content: url.value,
                        width: "800px",
                        height: "600px",
                        modal: true,
                        visible: false,
                        close: () => dialogRoot.data("kendoWindow").destroy()
                    };

                    if (options) {
                        $.extend(dialogOptions, options(target));
                    }

                    (dialogRoot as any).kendoWindow(dialogOptions);

                    const dialog = dialogRoot.data("kendoWindow");
                    dialog.center();
                    dialog.open();
                }
            });

            $(document).on("auxclick", buttonSelector, (e: JQueryEventObject) => {
                if (e.which === 2) {
                    e.preventDefault();
                    const target = $(e.target);
                    const url = routeSelector(target);
                    url.open();
                }
            });

            return {};
        }

        static createButtonClickPopupHandler<T>(
            routeSelector: (item: kendo.data.ObservableObject & T) => AspNet.Routing.Uri,
            options?: (item: kendo.data.ObservableObject & T) => object
        ): Function {
            return function (this: kendo.ui.Grid, e: JQueryEventObject, original?: JQueryEventObject) {
                e.preventDefault();
                const dataItem = this.dataItem<T>(e.currentTarget.closest("tr"));
                const url = routeSelector(dataItem);
                if (url === undefined || url === null) {
                    return;
                }

                if (e.ctrlKey || original && original.which === 2) {
                    url.open();
                } else {
                    const dialogRoot = $("<div></div>");
                    const dialogOptions = {
                        title: "",
                        actions: ["close"],
                        content: url.value,
                        width: "800px",
                        height: "600px",
                        modal: true,
                        visible: false,
                        close: () => dialogRoot.data("kendoWindow").destroy()
                    };

                    if (options) {
                        $.extend(dialogOptions, options(dataItem));
                    }

                    (dialogRoot as any).kendoWindow(dialogOptions);

                    const dialog = dialogRoot.data("kendoWindow");
                    dialog.center();
                    dialog.open();

                    $(window).on("resize", e => {
                        if (!(dialog.element.closest("html").length === 0 || dialog.element.is(":hidden"))) {
                            dialog.center();
                        }
                    });
                }
            }
        }
    }
}