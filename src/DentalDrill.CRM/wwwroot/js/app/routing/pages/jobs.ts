namespace DentalDrill.CRM.Routing.Pages {
    export class JobsControllerRoute extends DevGuild.AspNet.Routing.ControllerRoute {
        constructor() {
            super("/Jobs/");
        }

        index(): DevGuild.AspNet.Routing.Uri {
            return this.buildUri("");
        }

        create(type: string): DevGuild.AspNet.Routing.Uri {
            return this.buildUri(`Create?type=${type}`);
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
