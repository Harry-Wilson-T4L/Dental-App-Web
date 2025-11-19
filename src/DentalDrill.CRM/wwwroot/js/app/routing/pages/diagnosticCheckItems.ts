namespace DentalDrill.CRM.Routing.Pages {
    export class DiagnosticCheckItemsControllerRoute extends DevGuild.AspNet.Routing.ControllerRoute {
        constructor() {
            super("/DiagnosticCheckItems/");
        }

        index(): DevGuild.AspNet.Routing.Uri {
            return this.buildUri("");
        }
    }

    export class DiagnosticCheckTypesControllerRoute extends DevGuild.AspNet.Routing.ControllerRoute {
        constructor() {
            super("/DiagnosticCheckTypes/");
        }

        index(): DevGuild.AspNet.Routing.Uri {
            return this.buildUri("");
        }

        sortItems(typeId: string): DevGuild.AspNet.Routing.Uri {
            return this.buildUri(`SortItems/${typeId}`);
        }
    }
}
