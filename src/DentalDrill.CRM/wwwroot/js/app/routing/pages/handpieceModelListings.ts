namespace DentalDrill.CRM.Routing.Pages {
    export class HandpieceModelListingsControllerRoute extends DevGuild.AspNet.Routing.ControllerRoute {
        constructor() {
            super("/HandpieceModelListings/");
        }
        
        create(parentId: string): DevGuild.AspNet.Routing.Uri {
            return this.buildUri(`Create?parentId=${parentId}`);
        }

        read(parentId: string): DevGuild.AspNet.Routing.Uri {
            return this.buildUri(`Read?parentId=${parentId}`);
        }

        edit(id: string): DevGuild.AspNet.Routing.Uri {
            return this.buildUri(`Edit/${id}`);
        }

        delete(id: string): DevGuild.AspNet.Routing.Uri {
            return this.buildUri(`Delete/${id}`);
        }
    }
}