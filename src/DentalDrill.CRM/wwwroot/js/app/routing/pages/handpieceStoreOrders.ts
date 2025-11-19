namespace DentalDrill.CRM.Routing.Pages {
    export class HandpieceStoreOrdersControllerRoute extends DevGuild.AspNet.Routing.ControllerRoute {
        constructor() {
            super("/HandpieceStoreOrders/");
        }

        index(): DevGuild.AspNet.Routing.Uri {
            return this.buildUri(``);
        }

        read(): DevGuild.AspNet.Routing.Uri {
            return this.buildUri(`Read`);
        }

        details(id: string): DevGuild.AspNet.Routing.Uri {
            return this.buildUri(`Details/${id}`);
        }
    }
}