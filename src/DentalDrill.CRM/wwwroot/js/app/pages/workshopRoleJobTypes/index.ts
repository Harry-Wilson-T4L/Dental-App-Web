namespace DentalDrill.CRM.Pages.WorkshopRoleJobTypes.Index {
    import GridHandlers = DevGuild.AspNet.Controls.Grids.Handlers.GridHandlers;
    import AjaxFormsManager = DevGuild.AspNet.Forms.AjaxForms.AjaxFormsManager;

    interface WorkshopRoleJobType {
        Id: string;
        WorkshopRoleId: string;
        WorkshopRoleName: string;
        JobTypeId: string;
        JobTypeName: string;
    }

    export class WorkshopRoleJobTypesGrid {
        static get instance(): kendo.ui.Grid {
            return $("#WorkshopRoleJobTypesGrid").data("kendoGrid");
        }

        static handleDetails = GridHandlers.createButtonClickNavigationHandler<WorkshopRoleJobType>(
            item => routes.workshopRoleJobTypes.details(item.Id));

        static handleEdit = GridHandlers.createButtonClickPopupHandler<WorkshopRoleJobType>(
            item => routes.workshopRoleJobTypes.edit(item.Id),
            item => ({
                title: `Edit workshop role ${item.JobTypeName} permission`,
                width: "1000px",
                height: "auto",
                open: async (e: kendo.ui.WindowEvent) => {
                    await AjaxFormsManager.waitFor("WorkshopRoleJobTypesEdit");
                    await WorkshopRoleJobTypesGrid.instance.dataSource.read();
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

        static handleDelete = GridHandlers.createButtonClickPopupHandler<WorkshopRoleJobType>(
            item => routes.workshopRoleJobTypes.delete(item.Id),
            item => ({
                title: `Delete workshop role ${item.JobTypeName} permission`,
                width: "1000px",
                height: "auto",
                open: async (e: kendo.ui.WindowEvent) => {
                    await AjaxFormsManager.waitFor("WorkshopRoleJobTypesDelete");
                    await WorkshopRoleJobTypesGrid.instance.dataSource.read();
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
            "#WorkshopRoleJobTypesGrid .k-grid-CustomCreate",
            target => routes.workshopRoleJobTypes.create($("#WorkshopRoleJobTypesGrid").attr("data-parent-id")),
            target => ({
                title: `Create workshop role job type permission`,
                width: "1000px",
                height: "auto",
                open: async (e: kendo.ui.WindowEvent) => {
                    await AjaxFormsManager.waitFor("WorkshopRoleJobTypesCreate");
                    await WorkshopRoleJobTypesGrid.instance.dataSource.read();
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