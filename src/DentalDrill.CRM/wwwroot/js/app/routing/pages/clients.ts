namespace DentalDrill.CRM.Routing.Pages {
    export class ClientsControllerRoute extends DevGuild.AspNet.Routing.ControllerRoute {
        constructor() {
            super("/Clients/");
        }

        index(): DevGuild.AspNet.Routing.Uri {
            return this.buildUri("");
        }

        create(id: string): DevGuild.AspNet.Routing.Uri {
            return this.buildUri(`Create/${id}`);
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
