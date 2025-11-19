namespace DentalDrill.CRM.Pages.TutorialVideos.Index {
    import GridHandlers = DevGuild.AspNet.Controls.Grids.Handlers.GridHandlers;

    interface TutorialVideo {
        Id: string;
        Title: string;
        VideoUrl: string;
    }

    export class TutorialVideosGrid {
        static async handleMoveUp(this: kendo.ui.Grid, e: JQueryEventObject) {
            e.preventDefault();
            const dataItem = this.dataItem<TutorialVideo>(e.currentTarget.closest("tr"));
            console.log(dataItem);

            await fetch(`/TutorialVideos/MoveUp/${dataItem.Id}`, {
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
            const dataItem = this.dataItem<TutorialVideo>(e.currentTarget.closest("tr"));
            console.log(dataItem);

            await fetch(`/TutorialVideos/MoveDown/${dataItem.Id}`, {
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
