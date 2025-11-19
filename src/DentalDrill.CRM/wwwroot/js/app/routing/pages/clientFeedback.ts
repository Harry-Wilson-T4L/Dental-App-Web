namespace DentalDrill.CRM.Routing.Pages {
    export class ClientFeedbackFormsControllerRoute extends DevGuild.AspNet.Routing.ControllerRoute {
        constructor() {
            super("/ClientFeedbackForms/");
        }

        create(parentId: string): DevGuild.AspNet.Routing.Uri {
            return this.buildUri(`SendNewForm/?clientId=${parentId}`);
        }

        details(id: string): DevGuild.AspNet.Routing.Uri {
            return this.buildUri(`Details/${id}`);
        }
    }
}
