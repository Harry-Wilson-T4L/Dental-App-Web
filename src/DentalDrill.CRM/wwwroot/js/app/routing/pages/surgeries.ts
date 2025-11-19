namespace DentalDrill.CRM.Routing.Pages {
    export class SurgeriesControllerRoute extends DevGuild.AspNet.Routing.ControllerRoute {
        constructor() {
            super("/Surgeries/");
        }

        index(clientId: string): DevGuild.AspNet.Routing.Uri {
            return this.buildUri(clientId);
        }

        handpiece(clientId: string, id: string): DevGuild.AspNet.Routing.Uri {
            return this.buildUri(`${clientId}/Handpiece/${id}`);
        }

        exportBrandsToExcel(clientId: string, from: Date, to: Date, dateAggregate: string, reportFields: string): DevGuild.AspNet.Routing.Uri {
            const fromString = (new Date(from.getTime() - from.getTimezoneOffset() * 60 * 1000)).toISOString().substr(0, 10);
            const toString = (new Date(to.getTime() - to.getTimezoneOffset() * 60 * 1000)).toISOString().substr(0, 10);
            return this.buildUri(`${clientId}/Reports/Brands/Export/Excel?from=${encodeURIComponent(fromString)}&to=${encodeURIComponent(toString)}&dateAggregate=${encodeURIComponent(dateAggregate)}&reportFields=${encodeURIComponent(reportFields)}`);
        }

        exportStatusesToExcel(clientId: string, from: Date, to: Date): DevGuild.AspNet.Routing.Uri {
            const fromString = (new Date(from.getTime() - from.getTimezoneOffset() * 60 * 1000)).toISOString().substr(0, 10);
            const toString = (new Date(to.getTime() - to.getTimezoneOffset() * 60 * 1000)).toISOString().substr(0, 10);
            return this.buildUri(`${clientId}/Reports/Statuses/Export/Excel?from=${encodeURIComponent(fromString)}&to=${encodeURIComponent(toString)}`);
        }
    }
}
