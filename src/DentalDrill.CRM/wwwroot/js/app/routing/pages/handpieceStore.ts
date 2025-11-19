namespace DentalDrill.CRM.Routing.Pages {
    export class HandpieceStoreControllerRoute extends DevGuild.AspNet.Routing.ControllerRoute {
        constructor() {
            super("/HandpieceStore/");
        }

        index(): DevGuild.AspNet.Routing.Uri {
            return this.buildUri(``);
        }

        read(): DevGuild.AspNet.Routing.Uri {
            return this.buildUri(`Read`);
        }

        buy(id: string): DevGuild.AspNet.Routing.Uri {
            return this.buildUri(`Buy/${id}`);
        }
    }
}