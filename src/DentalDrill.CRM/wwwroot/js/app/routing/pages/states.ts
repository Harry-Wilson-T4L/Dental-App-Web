namespace DentalDrill.CRM.Routing.Pages {
    export class StatesControllerRoute extends DevGuild.AspNet.Routing.ControllerRoute {
        constructor() {
            super("/States/");
        }

        index(): DevGuild.AspNet.Routing.Uri {
            return this.buildUri("");
        }
    }
}
