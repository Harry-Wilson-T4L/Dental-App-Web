namespace DentalDrill.CRM.Routing.Pages {
    export class ZonesControllerRoute extends DevGuild.AspNet.Routing.ControllerRoute {
        constructor() {
            super("/Zones/");
        }

        index(): DevGuild.AspNet.Routing.Uri {
            return this.buildUri("");
        }
    }
}
