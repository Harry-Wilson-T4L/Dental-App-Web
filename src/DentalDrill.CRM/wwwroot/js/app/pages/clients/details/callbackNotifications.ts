namespace DentalDrill.CRM.Pages.Clients.Details {
    import GridHandlers = DevGuild.AspNet.Controls.Grids.Handlers.GridHandlers;

    export class ClientCallbackNotificationsGrid {
        static get instance(): kendo.ui.Grid {
            return $("#ClientCallbackNotificationsGrid").data("kendoGrid");
        }

        static handleEdit = GridHandlers.createButtonClickPopupHandler<ViewModels.ClientNotificationReadModel>(
            item => routes.clientCallbackNotifications.edit(item.Id),
            target => ({
                title: "Edit Callback",
                width: "600px",
                height: "auto",
                open: async (e: kendo.ui.WindowEvent) => {
                    await DevGuild.AspNet.Forms.AjaxForms.AjaxFormsManager.waitFor("EditCallbackNotification");
                    await ClientCallbackNotificationsGrid.instance.dataSource.read();
                    e.sender.close();
                    e.sender.destroy();
                },
                refresh: async (e: kendo.ui.WindowEvent) => {
                    e.sender.wrapper.on("click", ".editor__submit__cancel", clickEvent => {
                        clickEvent.preventDefault();
                        e.sender.close();
                        e.sender.destroy();
                    });
                    e.sender.center();
                }
            }));

        static handleDelete = GridHandlers.createButtonClickPopupHandler<ViewModels.ClientNotificationReadModel>(
            item => routes.clientCallbackNotifications.delete(item.Id),
            target => ({
                title: "Delete Callback",
                width: "600px",
                height: "auto",
                open: async (e: kendo.ui.WindowEvent) => {
                    await DevGuild.AspNet.Forms.AjaxForms.AjaxFormsManager.waitFor("DeleteCallbackNotification");
                    await ClientCallbackNotificationsGrid.instance.dataSource.read();
                    e.sender.close();
                    e.sender.destroy();
                },
                refresh: async (e: kendo.ui.WindowEvent) => {
                    e.sender.wrapper.on("click", ".editor__submit__cancel", clickEvent => {
                        clickEvent.preventDefault();
                        e.sender.close();
                        e.sender.destroy();
                    });
                    e.sender.center();
                }
            }));

        static handleCreate = GridHandlers.createGridButtonClickPopupHandler(
            "#ClientCallbackNotificationsGrid .k-grid-CustomCreate",
            target => new DevGuild.AspNet.Routing.Uri(target.attr("href")),
            target => ({
                title: "New Callback",
                width: "600px",
                height: "auto",
                open: async (e: kendo.ui.WindowEvent) => {
                    await DevGuild.AspNet.Forms.AjaxForms.AjaxFormsManager.waitFor("CreateCallbackNotification");
                    await ClientCallbackNotificationsGrid.instance.dataSource.read();
                    e.sender.close();
                    e.sender.destroy();
                },
                refresh: async (e: kendo.ui.WindowEvent) => {
                    e.sender.wrapper.on("click", ".editor__submit__cancel", clickEvent => {
                        clickEvent.preventDefault();
                        e.sender.close();
                        e.sender.destroy();
                    });
                    e.sender.center();
                }
            }));
    }
}