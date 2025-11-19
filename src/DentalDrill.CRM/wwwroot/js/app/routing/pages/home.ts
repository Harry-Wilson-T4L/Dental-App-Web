namespace DentalDrill.CRM.Routing.Pages {
    export class HomeControllerRoute extends DevGuild.AspNet.Routing.ControllerRoute {
        constructor() {
            super("/Home/");
        }

        index(): DevGuild.AspNet.Routing.Uri {
            return new DevGuild.AspNet.Routing.Uri("/");
        }

        about(): DevGuild.AspNet.Routing.Uri {
            return this.buildUri("About");
        }

        contacts(): DevGuild.AspNet.Routing.Uri {
            return this.buildUri("Contacts");
        }
    }
}