namespace DentalDrill.CRM.Pages.PickupRequests.Index {
    import GridHandlers = DevGuild.AspNet.Controls.Grids.Handlers.GridHandlers;
    import AjaxFormsManager = DevGuild.AspNet.Forms.AjaxForms.AjaxFormsManager;

    export class PickupRequestsGrid {
        static get instance(): kendo.ui.Grid {
            return $("#PickupRequestsGrid").data("kendoGrid");
        }

        static handleDetails = GridHandlers.createButtonClickPopupHandler<{ Id: string, PracticeName: string }>(
            item => routes.pickupRequests.details(item.Id),
            item => ({
                title: `Pickup request from ${item.PracticeName}`,
                height: "auto",
                refresh: (e: kendo.ui.WindowEvent) => {
                    e.sender.center();
                },
            }));
    }
}