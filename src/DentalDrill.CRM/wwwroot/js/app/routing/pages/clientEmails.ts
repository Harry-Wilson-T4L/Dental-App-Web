namespace DentalDrill.CRM.Routing.Pages {
    export class ClientEmailsControllerRoute extends DevGuild.AspNet.Routing.ControllerRoute {
        constructor() {
            super("/ClientEmails/");
        }

        details(id: number): DevGuild.AspNet.Routing.Uri {
            return this.buildUri(`Details/${id}`);
        }
    }
}
