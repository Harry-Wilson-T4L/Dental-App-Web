namespace DentalDrill.CRM.Pages.Clients.Details {
    import GridHandlers = DevGuild.AspNet.Controls.Grids.Handlers.GridHandlers;
    import AjaxFormsManager = DevGuild.AspNet.Forms.AjaxForms.AjaxFormsManager;

    export class ClientInvoicesGrid {
        static get instance(): kendo.ui.Grid {
            return $("#ClientInvoicesGrid").data("kendoGrid");
        }

        static handleCreate = GridHandlers.createGridButtonClickPopupHandler(
            "#ClientInvoicesGrid .k-grid-CustomCreate",
            target => {
                return routes.clientInvoices.create(ClientInvoicesGrid.instance.element.attr("data-client-id"));
            },
            target => {
                const grid = ClientInvoicesGrid.instance;

                return {
                    title: `Upload Invoice`,
                    width: "800px",
                    height: "auto",
                    refresh: (e: kendo.ui.WindowEvent) => {
                        e.sender.wrapper.on("click", ".editor__submit__cancel", clickEvent => {
                            clickEvent.preventDefault();
                            e.sender.close();
                            e.sender.destroy();
                        });

                        e.sender.center();
                    },
                    open: async (e: kendo.ui.WindowEvent) => {
                        await AjaxFormsManager.waitFor("ClientInvoicesCreate");
                        await grid.dataSource.read();
                        e.sender.close();
                        e.sender.destroy();
                    }
                };
            });

        static handleDownload = GridHandlers.createButtonClickNavigationHandler<{ Id: string, FileName: string }>(x => routes.jobInvoices.download(x.Id));
    }
}