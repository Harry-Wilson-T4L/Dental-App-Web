namespace DentalDrill.CRM.Pages.WorkshopRoleJobTypeStatuses.Index {
    import GridHandlers = DevGuild.AspNet.Controls.Grids.Handlers.GridHandlers;
    import AjaxFormsManager = DevGuild.AspNet.Forms.AjaxForms.AjaxFormsManager;

    interface WorkshopRoleJobTypeStatus {
        Id: string;
        JobTypeId: string;
        JobTypeName: string;
        JobStatus: string;
        JobTransitions: string;
        HandpieceTransitionsFrom: string;
        HandpieceTransitionsTo: string;
    }

    export class WorkshopRoleJobTypeStatusesGrid {
        static get instance(): kendo.ui.Grid {
            return $("#WorkshopRoleJobTypeStatusesGrid").data("kendoGrid");
        }

        static handleDetails = GridHandlers.createButtonClickNavigationHandler<WorkshopRoleJobTypeStatus>(
            item => routes.workshopRoleJobTypeStatuses.details(item.Id));

        static handleEdit = GridHandlers.createButtonClickPopupHandler<WorkshopRoleJobTypeStatus>(
            item => routes.workshopRoleJobTypeStatuses.edit(item.Id),
            item => ({
                title: `Edit workshop role ${item.JobTypeName} permission for status ${item.JobStatus}`,
                width: "1000px",
                height: "auto",
                open: async (e: kendo.ui.WindowEvent) => {
                    await AjaxFormsManager.waitFor("WorkshopRoleJobTypeStatusesEdit");
                    await WorkshopRoleJobTypeStatusesGrid.instance.dataSource.read();
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
    }
}