namespace DentalDrill.CRM.Routing.Pages {
    export class HandpieceBrandsControllerRoute extends DevGuild.AspNet.Routing.ControllerRoute {
        constructor() {
            super("/HandpieceBrands/");
        }

        index(): DevGuild.AspNet.Routing.Uri {
            return this.buildUri("");
        }

        create(): DevGuild.AspNet.Routing.Uri {
            return this.buildUri(`Create`);
        }

        edit(id: string): DevGuild.AspNet.Routing.Uri {
            return this.buildUri(`Edit/${id}`);
        }

        delete(id: string): DevGuild.AspNet.Routing.Uri {
            return this.buildUri(`Delete/${id}`);
        }
    }
}