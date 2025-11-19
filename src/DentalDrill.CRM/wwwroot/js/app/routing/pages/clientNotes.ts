namespace DentalDrill.CRM.Routing.Pages {
    export class ClientNotesControllerRoute extends DevGuild.AspNet.Routing.ControllerRoute {
        constructor() {
            super("/ClientNotes/");
        }

        create(parentId: string): DevGuild.AspNet.Routing.Uri {
            return this.buildUri(`Create/?parentId=${parentId}`);
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
