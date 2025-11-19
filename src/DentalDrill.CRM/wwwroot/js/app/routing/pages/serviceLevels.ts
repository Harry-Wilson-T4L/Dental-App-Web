namespace DentalDrill.CRM.Routing.Pages {
    export class ServiceLevelsControllerRoute extends DevGuild.AspNet.Routing.ControllerRoute {
        constructor() {
            super("/ServiceLevels/");
        }

        index(): DevGuild.AspNet.Routing.Uri {
            return this.buildUri("");
        }
    }
}
