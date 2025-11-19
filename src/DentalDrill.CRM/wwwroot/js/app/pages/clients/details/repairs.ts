namespace DentalDrill.CRM.Pages.Clients.Details {
    import GridHandlers = DevGuild.AspNet.Controls.Grids.Handlers.GridHandlers;
    import HandpieceStatusIndicator = DentalDrill.CRM.Controls.HandpieceStatusIndicator;

    export class RepairsGrid {
        static get instance(): kendo.ui.Grid {
            return $("#RepairsGrid").data("kendoGrid");
        }

        static handleDetailsClick = GridHandlers.createButtonClickNavigationHandler<{ Id: string }>(item => routes.jobs.edit(item.Id));

        static handleCreateEstimate = GridHandlers.createGridButtonClickPopupHandler(
            "#RepairsGrid .k-grid-CustomCreateEstimate",
            target => new DevGuild.AspNet.Routing.Uri(target.attr("href")),
            target => ({
                title: "Create Estimate",
                width: "1000px",
                height: "auto",
                refresh: (e: kendo.ui.WindowEvent) => {
                    e.sender.wrapper.on("click", ".editor__submit__cancel", clickEvent => {
                        clickEvent.preventDefault();
                        e.sender.close();
                        e.sender.destroy();
                    });
                    e.sender.center();
                },
                open: async function (this: kendo.ui.Window) {
                    await DevGuild.AspNet.Forms.AjaxForms.AjaxFormsManager.waitFor("JobsCreate");
                    await RepairsGrid.instance.dataSource.read();
                    this.close();
                    this.destroy();
                }
            }));

        static handleCreateSale = GridHandlers.createGridButtonClickPopupHandler(
            "#RepairsGrid .k-grid-CustomCreateSale",
            target => new DevGuild.AspNet.Routing.Uri(target.attr("href")),
            target => ({
                title: "Create Sale",
                width: "1000px",
                height: "auto",
                refresh: (e: kendo.ui.WindowEvent) => {
                    e.sender.wrapper.on("click", ".editor__submit__cancel", clickEvent => {
                        clickEvent.preventDefault();
                        e.sender.close();
                        e.sender.destroy();
                    });
                    e.sender.center();
                },
                open: async function (this: kendo.ui.Window) {
                    await DevGuild.AspNet.Forms.AjaxForms.AjaxFormsManager.waitFor("JobsCreate");
                    await RepairsGrid.instance.dataSource.read();
                    this.close();
                    this.destroy();
                }
            }));

        static renderStatusIndicator(data: any): string {
            const config = (data.JobStatusConfig as string).split(";").map(x => parseInt(x));
            const indicator = new HandpieceStatusIndicator();

            const indicatorValue = Math.abs(config[0]);
            indicator.value = indicatorValue;
            indicator.danger = config[0] < 0;
            for (let i = 1; i <= 6; i++) {
                indicator.setOverride(i, config[i] > 0 && i < indicatorValue);
                indicator.setCount(i, i < indicatorValue ? config[i] : 0);
            }

            return indicator.render().outerHTML;
        }
    }
}
