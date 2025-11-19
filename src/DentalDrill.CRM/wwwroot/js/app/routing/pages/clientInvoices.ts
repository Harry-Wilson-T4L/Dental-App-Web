namespace DentalDrill.CRM.Routing.Pages {
    export class ClientInvoicesControllerRoute extends DevGuild.AspNet.Routing.ControllerRoute {
        constructor() {
            super("/ClientInvoices/");
        }

        create(parentId: string): DevGuild.AspNet.Routing.Uri {
            return this.buildUri(`Create/?parentId=${parentId}`);
        }
    }
}
