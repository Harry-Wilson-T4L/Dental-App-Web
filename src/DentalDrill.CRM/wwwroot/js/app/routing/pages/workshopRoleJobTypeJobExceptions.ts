namespace DentalDrill.CRM.Routing.Pages {
    export class WorkshopRoleJobTypeJobExceptionsControllerRoute extends DevGuild.AspNet.Routing.ControllerRoute {
        constructor() {
            super("/WorkshopRoleJobTypeJobExceptions/");
        }

        create(workshopRoleJobTypeId: string): DevGuild.AspNet.Routing.Uri {
            return this.buildUri(`Create?parentId=${workshopRoleJobTypeId}`);
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
