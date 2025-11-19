namespace DentalDrill.CRM.Routing.Pages {
    export class WorkshopRoleJobTypesControllerRoute extends DevGuild.AspNet.Routing.ControllerRoute {
        constructor() {
            super("/WorkshopRoleJobTypes/");
        }

        create(workshopRoleId: string): DevGuild.AspNet.Routing.Uri {
            return this.buildUri(`Create?parentId=${workshopRoleId}`);
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
