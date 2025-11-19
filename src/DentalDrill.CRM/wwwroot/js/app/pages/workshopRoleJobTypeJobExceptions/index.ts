namespace DentalDrill.CRM.Pages.WorkshopRoleJobTypeJobExceptions.Index {
    import GridHandlers = DevGuild.AspNet.Controls.Grids.Handlers.GridHandlers;
    import AjaxFormsManager = DevGuild.AspNet.Forms.AjaxForms.AjaxFormsManager;

    interface WorkshopRoleJobTypeJobException {
        Id: string;
        JobTypeId: string;
        JobTypeName: string;
        WhenJobStatus: string;
        ReadOnlyFields: string;
        HiddenFields: string;
    }

    export class WorkshopRoleJobTypeJobExceptionsGrid {
        static get instance(): kendo.ui.Grid {
            return $("#WorkshopRoleJobTypeJobExceptionsGrid").data("kendoGrid");
        }

        static handleDetails = GridHandlers.createButtonClickNavigationHandler<WorkshopRoleJobTypeJobException>(
            item => routes.workshopRoleJobTypeJobExceptions.details(item.Id));

        static handleEdit = GridHandlers.createButtonClickPopupHandler<WorkshopRoleJobTypeJobException>(
            item => routes.workshopRoleJobTypeJobExceptions.edit(item.Id),
            item => ({
                title: `Edit workshop role ${item.JobTypeName} permission exception`,
                width: "1000px",
                height: "auto",
                open: async (e: kendo.ui.WindowEvent) => {
                    await AjaxFormsManager.waitFor("WorkshopRoleJobTypeJobExceptionsEdit");
                    await WorkshopRoleJobTypeJobExceptionsGrid.instance.dataSource.read();
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

        static handleDelete = GridHandlers.createButtonClickPopupHandler<WorkshopRoleJobTypeJobException>(
            item => routes.workshopRoleJobTypeJobExceptions.delete(item.Id),
            item => ({
                title: `Delete workshop role ${item.JobTypeName} permission exception`,
                width: "1000px",
                height: "auto",
                open: async (e: kendo.ui.WindowEvent) => {
                    await AjaxFormsManager.waitFor("WorkshopRoleJobTypeJobExceptionsDelete");
                    await WorkshopRoleJobTypeJobExceptionsGrid.instance.dataSource.read();
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
            "#WorkshopRoleJobTypeJobExceptionsGrid .k-grid-CustomCreate",
            target => routes.workshopRoleJobTypeJobExceptions.create($("#WorkshopRoleJobTypeJobExceptionsGrid").attr("data-parent-id")),
            target => ({
                title: `Create workshop role job type permission exception`,
                width: "1000px",
                height: "auto",
                open: async (e: kendo.ui.WindowEvent) => {
                    await AjaxFormsManager.waitFor("WorkshopRoleJobTypeJobExceptionsCreate");
                    await WorkshopRoleJobTypeJobExceptionsGrid.instance.dataSource.read();
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