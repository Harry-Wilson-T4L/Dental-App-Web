namespace DentalDrill.CRM.Pages.WorkshopRoleJobTypeHandpieceExceptions.Index {
    import GridHandlers = DevGuild.AspNet.Controls.Grids.Handlers.GridHandlers;
    import AjaxFormsManager = DevGuild.AspNet.Forms.AjaxForms.AjaxFormsManager;

    interface WorkshopRoleJobTypeHandpieceException {
        Id: string;
        JobTypeId: string;
        JobTypeName: string;
        WhenJobStatus: string;
        WhenHandpieceStatus: string;
        ReadOnlyFields: string;
        HiddenFields: string;
    }

    export class WorkshopRoleJobTypeHandpieceExceptionsGrid {
        static get instance(): kendo.ui.Grid {
            return $("#WorkshopRoleJobTypeHandpieceExceptionsGrid").data("kendoGrid");
        }

        static handleDetails = GridHandlers.createButtonClickNavigationHandler<WorkshopRoleJobTypeHandpieceException>(
            item => routes.workshopRoleJobTypeHandpieceExceptions.details(item.Id));

        static handleEdit = GridHandlers.createButtonClickPopupHandler<WorkshopRoleJobTypeHandpieceException>(
            item => routes.workshopRoleJobTypeHandpieceExceptions.edit(item.Id),
            item => ({
                title: `Edit workshop role ${item.JobTypeName} permission exception`,
                width: "1000px",
                height: "auto",
                open: async (e: kendo.ui.WindowEvent) => {
                    await AjaxFormsManager.waitFor("WorkshopRoleJobTypeHandpieceExceptionsEdit");
                    await WorkshopRoleJobTypeHandpieceExceptionsGrid.instance.dataSource.read();
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

        static handleDelete = GridHandlers.createButtonClickPopupHandler<WorkshopRoleJobTypeHandpieceException>(
            item => routes.workshopRoleJobTypeHandpieceExceptions.delete(item.Id),
            item => ({
                title: `Delete workshop role ${item.JobTypeName} permission exception`,
                width: "1000px",
                height: "auto",
                open: async (e: kendo.ui.WindowEvent) => {
                    await AjaxFormsManager.waitFor("WorkshopRoleJobTypeHandpieceExceptionsDelete");
                    await WorkshopRoleJobTypeHandpieceExceptionsGrid.instance.dataSource.read();
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
            "#WorkshopRoleJobTypeHandpieceExceptionsGrid .k-grid-CustomCreate",
            target => routes.workshopRoleJobTypeHandpieceExceptions.create($("#WorkshopRoleJobTypeHandpieceExceptionsGrid").attr("data-parent-id")),
            target => ({
                title: `Create workshop role job type permission exception`,
                width: "1000px",
                height: "auto",
                open: async (e: kendo.ui.WindowEvent) => {
                    await AjaxFormsManager.waitFor("WorkshopRoleJobTypeHandpieceExceptionsCreate");
                    await WorkshopRoleJobTypeHandpieceExceptionsGrid.instance.dataSource.read();
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