namespace DentalDrill.CRM.Pages.Workshops.Index {
    import GridHandlers = DevGuild.AspNet.Controls.Grids.Handlers.GridHandlers;

    interface Workshop {
        Id: string;
        Name: string;
    }

    export class WorkshopsGrid {
        static async handleMoveUp(this: kendo.ui.Grid, e: JQueryEventObject) {
            e.preventDefault();
            const dataItem = this.dataItem<Workshop>(e.currentTarget.closest("tr"));
            console.log(dataItem);

            await fetch(`/Workshops/MoveUp/${dataItem.Id}`, {
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
            const dataItem = this.dataItem<Workshop>(e.currentTarget.closest("tr"));
            console.log(dataItem);

            await fetch(`/Workshops/MoveDown/${dataItem.Id}`, {
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