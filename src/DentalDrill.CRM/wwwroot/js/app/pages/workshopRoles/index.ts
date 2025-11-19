namespace DentalDrill.CRM.Pages.WorkshopRoles.Index {
    import GridHandlers = DevGuild.AspNet.Controls.Grids.Handlers.GridHandlers;
    import AjaxFormsManager = DevGuild.AspNet.Forms.AjaxForms.AjaxFormsManager;

    interface WorkshopRole {
        Id: string;
        Name: string;
    }

    export class WorkshopRolesGrid {
        static get instance(): kendo.ui.Grid {
            return $("#WorkshopRolesGrid").data("kendoGrid");
        }

        static handleDetails = GridHandlers.createButtonClickNavigationHandler<WorkshopRole>(
            item => routes.workshopRoles.details(item.Id));

        static handleEdit = GridHandlers.createButtonClickPopupHandler<WorkshopRole>(
            item => routes.workshopRoles.edit(item.Id),
            item => ({
                title: `Edit workshop role ${item.Name}`,
                width: "1000px",
                height: "auto",
                open: async (e: kendo.ui.WindowEvent) => {
                    await AjaxFormsManager.waitFor("WorkshopRolesEdit");
                    await WorkshopRolesGrid.instance.dataSource.read();
                    e.sender.close();
                    e.sender.destroy();
                },
                refresh: (e: kendo.ui.WindowEvent) => {
                    e.sender.wrapper.on("click", ".editor__submit__cancel", clickEvent => {
                        clickEvent.preventDefault();
                        e.sender.close();
                        e.sender.destroy();
                    });
                    e.sender.center();
                },
            }));

        static handleDelete = GridHandlers.createButtonClickPopupHandler<WorkshopRole>(
            item => routes.workshopRoles.delete(item.Id),
            item => ({
                title: `Delete workshop role ${item.Name}`,
                width: "1000px",
                height: "auto",
                open: async (e: kendo.ui.WindowEvent) => {
                    await AjaxFormsManager.waitFor("WorkshopRolesDelete");
                    await WorkshopRolesGrid.instance.dataSource.read();
                    e.sender.close();
                    e.sender.destroy();
                },
                refresh: (e: kendo.ui.WindowEvent) => {
                    e.sender.wrapper.on("click", ".editor__submit__cancel", clickEvent => {
                        clickEvent.preventDefault();
                        e.sender.close();
                        e.sender.destroy();
                    });
                    e.sender.center();
                },
            }));

        static handleCreate = GridHandlers.createGridButtonClickPopupHandler(
            "#WorkshopRolesGrid .k-grid-CustomCreate",
            target => routes.workshopRoles.create(),
            target => ({
                title: `Create workshop role`,
                width: "1000px",
                height: "auto",
                open: async (e: kendo.ui.WindowEvent) => {
                    await AjaxFormsManager.waitFor("WorkshopRolesCreate");
                    await WorkshopRolesGrid.instance.dataSource.read();
                    e.sender.close();
                    e.sender.destroy();
                },
                refresh: (e: kendo.ui.WindowEvent) => {
                    e.sender.wrapper.on("click", ".editor__submit__cancel", clickEvent => {
                        clickEvent.preventDefault();
                        e.sender.close();
                        e.sender.destroy();
                    });
                    e.sender.center();
                },
            })
        );
    }
}