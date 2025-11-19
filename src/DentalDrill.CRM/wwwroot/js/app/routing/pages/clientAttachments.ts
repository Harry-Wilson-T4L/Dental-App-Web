namespace DentalDrill.CRM.Routing.Pages {
    export class ClientAttachmentsControllerRoute extends DevGuild.AspNet.Routing.ControllerRoute {
        constructor() {
            super("/ClientAttachments/");
        }

        create(id: string): DevGuild.AspNet.Routing.Uri {
            return this.buildUri(`Create?parentId=${id}`);
        }

        download(id: string): DevGuild.AspNet.Routing.Uri {
            return this.buildUri(`Download/${id}`);
        }

        edit(id: string): DevGuild.AspNet.Routing.Uri {
            return this.buildUri(`Edit/${id}`);
        }

        delete(id: string): DevGuild.AspNet.Routing.Uri {
            return this.buildUri(`Delete/${id}`);
        }
    }
}
