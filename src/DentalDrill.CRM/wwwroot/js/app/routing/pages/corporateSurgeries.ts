namespace DentalDrill.CRM.Routing.Pages {
    export class CorporateSurgeriesControllerRoute extends DevGuild.AspNet.Routing.ControllerRoute {
        constructor() {
            super("/CorporateSurgeries/");
        }

        handpiece(corporateId: string, id: string): DevGuild.AspNet.Routing.Uri {
            return this.buildUri(`${corporateId}/Handpiece/${id}`);
        }

        exportSurgeriesToExcel(clientId: string, from: Date, to: Date, dateAggregate: string, reportFields: string, clients: string): DevGuild.AspNet.Routing.Uri {
            const fromString = (new Date(from.getTime() - from.getTimezoneOffset() * 60 * 1000)).toISOString().substr(0, 10);
            const toString = (new Date(to.getTime() - to.getTimezoneOffset() * 60 * 1000)).toISOString().substr(0, 10);
            return this.buildUri(`${clientId}/Reports/Handpieces/Export/Excel?from=${encodeURIComponent(fromString)}&to=${encodeURIComponent(toString)}&dateAggregate=${encodeURIComponent(dateAggregate)}&reportFields=${encodeURIComponent(reportFields)}&clients=${clients}`);
        }

        exportBrandsToExcel(clientId: string, from: Date, to: Date, dateAggregate: string, reportFields: string, clients: string): DevGuild.AspNet.Routing.Uri {
            const fromString = (new Date(from.getTime() - from.getTimezoneOffset() * 60 * 1000)).toISOString().substr(0, 10);
            const toString = (new Date(to.getTime() - to.getTimezoneOffset() * 60 * 1000)).toISOString().substr(0, 10);
            return this.buildUri(`${clientId}/Reports/Brands/Export/Excel?from=${encodeURIComponent(fromString)}&to=${encodeURIComponent(toString)}&dateAggregate=${encodeURIComponent(dateAggregate)}&reportFields=${encodeURIComponent(reportFields)}&clients=${clients}`);
        }

        exportStatusesToExcel(clientId: string, from: Date, to: Date, clients: string): DevGuild.AspNet.Routing.Uri {
            const fromString = (new Date(from.getTime() - from.getTimezoneOffset() * 60 * 1000)).toISOString().substr(0, 10);
            const toString = (new Date(to.getTime() - to.getTimezoneOffset() * 60 * 1000)).toISOString().substr(0, 10);
            return this.buildUri(`${clientId}/Reports/Statuses/Export/Excel?from=${encodeURIComponent(fromString)}&to=${encodeURIComponent(toString)}&clients=${clients}`);
        }
    }
}
