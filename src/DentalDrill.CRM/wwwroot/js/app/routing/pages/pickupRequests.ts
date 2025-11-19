namespace DentalDrill.CRM.Routing.Pages {
    export class PickupRequestsControllerRoute extends DevGuild.AspNet.Routing.ControllerRoute {
        constructor() {
            super("/PickupRequests/");
        }

        index(): DevGuild.AspNet.Routing.Uri {
            return this.buildUri("");
        }

        details(id: string): DevGuild.AspNet.Routing.Uri {
            return this.buildUri(`Details/${id}`);
        }

        complete(id: string): DevGuild.AspNet.Routing.Uri {
            return this.buildUri(`Complete/${id}`);
        }

        cancel(id: string): DevGuild.AspNet.Routing.Uri {
            return this.buildUri(`Cancel/${id}`);
        }
    }
}
