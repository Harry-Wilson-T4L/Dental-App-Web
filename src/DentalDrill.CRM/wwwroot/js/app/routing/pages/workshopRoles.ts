namespace DentalDrill.CRM.Routing.Pages {
    export class WorkshopRolesControllerRoute extends DevGuild.AspNet.Routing.ControllerRoute {
        constructor() {
            super("/WorkshopRoles/");
        }

        index(): DevGuild.AspNet.Routing.Uri {
            return this.buildUri("");
        }

        create(): DevGuild.AspNet.Routing.Uri {
            return this.buildUri(`Create`);
        }

        details(id: string): DevGuild.AspNet.Routing.Uri {
            return this.buildUri(`Details/${id}`);
        }

        edit(id: string): DevGuild.AspNet.Routing.Uri {
            return this.buildUri(`Edit/${id}`);
        }

        delete(id: string): DevGuild.AspNet.Routing.Uri {
            return this.buildUri(`Delete/${id}`);
        }
    }
}
