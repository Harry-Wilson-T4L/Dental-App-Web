namespace DentalDrill.CRM.Pages.Clients.Details {
    import GridHandlers = DevGuild.AspNet.Controls.Grids.Handlers.GridHandlers;

    export class ClientEmailsGrid {
        static get instance(): kendo.ui.Grid {
            return $("#ClientEmailsGrid").data("kendoGrid");
        }

        static handleDetails = GridHandlers.createButtonClickPopupHandler<{ Id: number, Subject: string }>(
            item => routes.clientEmails.details(item.Id),
            item => ({
                title: item.Subject,
                width: "1000px",
                height: "800px",
                refresh: (e: kendo.ui.WindowEvent) => {
                    e.sender.wrapper.on("click",
                        ".editor__submit__cancel",
                        clickEvent => {
                            clickEvent.preventDefault();
                            e.sender.close();
                            e.sender.destroy();
                        });
                    e.sender.center();
                }
            }));

        static handleSendButton = GridHandlers.createGridButtonClickPopupHandler(
            ".client-email-send-button",
            target => new DevGuild.AspNet.Routing.Uri(target.attr("href")),
            target => {
                const windowTitle = target.attr("data-title");
                const hybridFormId = target.attr("data-hybrid-id");
                const fixedHeight = target.attr("data-height");

                return {
                    title: windowTitle,
                    width: "1000px",
                    height: fixedHeight ? fixedHeight : "auto",
                    refresh: (e: kendo.ui.WindowEvent) => {
                        e.sender.wrapper.on("click", ".editor__submit__cancel", clickEvent => {
                            clickEvent.preventDefault();
                            e.sender.close();
                            e.sender.destroy();
                        });

                        if (!fixedHeight) {
                            e.sender.center();
                        }
                    },
                    open: async (e: kendo.ui.WindowEvent) => {
                        await DevGuild.AspNet.Forms.AjaxForms.AjaxFormsManager.waitFor(hybridFormId);
                        await ClientEmailsGrid.instance.dataSource.read();
                        e.sender.close();
                        e.sender.destroy();
                    }
                };
            });
    }
}