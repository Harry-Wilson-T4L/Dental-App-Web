namespace DentalDrill.CRM.Pages.Corporates.Details {
    import GridHandlers = DevGuild.AspNet.Controls.Grids.Handlers.GridHandlers;
    import AjaxFormsManager = DevGuild.AspNet.Forms.AjaxForms.AjaxFormsManager;

    export class CorporateClientsGrid {
        static get instance(): kendo.ui.Grid {
            return $("#CorporateClientsGrid").data("kendoGrid");
        }

        static handleDetailsClick = GridHandlers.createButtonClickNavigationHandler<{ Id: string }>(
            item => routes.clients.details(item.Id));

        static handleDataBound(e: kendo.ui.GridDataBoundEvent) {
            e.sender.element.find("[data-toggle='tooltip']").tooltip();
        }
    }
}