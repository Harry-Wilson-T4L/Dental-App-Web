namespace DentalDrill.CRM.Routing.Pages {
    export class WorkshopRoleJobTypeStatusesControllerRoute extends DevGuild.AspNet.Routing.ControllerRoute {
        constructor() {
            super("/WorkshopRoleJobTypeStatuses/");
        }

        details(id: string): DevGuild.AspNet.Routing.Uri {
            return this.buildUri(`Details/${id}`);
        }

        edit(id: string): DevGuild.AspNet.Routing.Uri {
            return this.buildUri(`Edit/${id}`);
        }
    }
}
