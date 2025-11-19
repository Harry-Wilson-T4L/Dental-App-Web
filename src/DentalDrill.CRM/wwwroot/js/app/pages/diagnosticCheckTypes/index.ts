namespace DentalDrill.CRM.Pages.DiagnosticCheckTypes.Index {
    import GridHandlers = DevGuild.AspNet.Controls.Grids.Handlers.GridHandlers;

    interface DiagnosticCheckType {
        Id: string;
        Name: string;
    }

    interface DiagnosticCheckItemSort {
        TypeId: string;
        ItemId: string;
        Name: string;
        OrderNo: number;
    }

    export class DiagnosticCheckTypesGrid {
        static async handleMoveUp(this: kendo.ui.Grid, e: JQueryEventObject) {
            e.preventDefault();
            const dataItem = this.dataItem<DiagnosticCheckType>(e.currentTarget.closest("tr"));
            console.log(dataItem);

            await fetch(`/DiagnosticCheckTypes/MoveUp/${dataItem.Id}`, {
                method: "POST",
                credentials: "same-origin",
                headers: {
                    "X-Requested-With": "XMLHttpRequest"
                },
                body: ""
            });
            await this.dataSource.read();
        }

        static async handleMoveDown(this: kendo.ui.Grid, e: JQueryEventObject) {
            e.preventDefault();
            const dataItem = this.dataItem<DiagnosticCheckType>(e.currentTarget.closest("tr"));
            console.log(dataItem);

            await fetch(`/DiagnosticCheckTypes/MoveDown/${dataItem.Id}`, {
                method: "POST",
                credentials: "same-origin",
                headers: {
                    "X-Requested-With": "XMLHttpRequest"
                },
                body: ""
            });
            await this.dataSource.read();
        }

        static handleSortItems = GridHandlers.createButtonClickPopupHandler<DiagnosticCheckType>(
            item => routes.diagnosticCheckTypes.sortItems(item.Id),
            item => ({
                title: `Sort diagnostic checklist for type ${item.Name}`,
                width: `1000px`,
                height: `auto`,
                refresh: (e: kendo.ui.WindowEvent) => {
                    e.sender.wrapper.on("click", ".editor__submit__cancel", clickEvent => {
                        clickEvent.preventDefault();
                        e.sender.close();
                        e.sender.destroy();
                    });
                    e.sender.center();
                },
            }));

        static async handleItemMoveUp(this: kendo.ui.Grid, e: JQueryEventObject) {
            e.preventDefault();
            const dataItem = this.dataItem<DiagnosticCheckItemSort>(e.currentTarget.closest("tr"));
            console.log(dataItem);

            await fetch(`/DiagnosticCheckTypes/ItemMoveUp?typeId=${dataItem.TypeId}&itemId=${dataItem.ItemId}`, {
                method: "POST",
                credentials: "same-origin",
                headers: {
                    "X-Requested-With": "XMLHttpRequest"
                },
                body: ""
            });
            await this.dataSource.read();
        }

        static async handleItemMoveDown(this: kendo.ui.Grid, e: JQueryEventObject) {
            e.preventDefault();
            const dataItem = this.dataItem<DiagnosticCheckItemSort>(e.currentTarget.closest("tr"));
            console.log(dataItem);

            await fetch(`/DiagnosticCheckTypes/ItemMoveDown?typeId=${dataItem.TypeId}&itemId=${dataItem.ItemId}`, {
                method: "POST",
                credentials: "same-origin",
                headers: {
                    "X-Requested-With": "XMLHttpRequest"
                },
                body: ""
            });
            await this.dataSource.read();
        }
    }
}
