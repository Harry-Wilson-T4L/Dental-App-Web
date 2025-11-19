namespace DentalDrill.CRM.Routing.Pages {
    export class JobInvoicesControllerRoute extends DevGuild.AspNet.Routing.ControllerRoute {
        constructor() {
            super("/JobInvoices/");
        }

        create(parentId: string): DevGuild.AspNet.Routing.Uri {
            return this.buildUri(`Create?parentId=${parentId}`);
        }

        download(id: string): DevGuild.AspNet.Routing.Uri {
            return this.buildUri(`Download/${id}`);
        }

        delete(id: string): DevGuild.AspNet.Routing.Uri {
            return this.buildUri(`Delete/${id}`);
        }
    }
}
