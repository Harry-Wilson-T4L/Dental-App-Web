namespace DentalDrill.CRM.Routing.Pages {
    export class HandpieceImagesControllerRoute extends DevGuild.AspNet.Routing.ControllerRoute {
        constructor() {
            super("/HandpieceImages/");
        }

        index(parentId: string): DevGuild.AspNet.Routing.Uri {
            return this.buildUri(`?parentId=${parentId}`);
        }

        create(parentId: string): DevGuild.AspNet.Routing.Uri {
            return this.buildUri(`Create/?parentId=${parentId}`);
        }
    }
}
