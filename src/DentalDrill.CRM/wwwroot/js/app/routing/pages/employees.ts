namespace DentalDrill.CRM.Routing.Pages {
    export class EmployeesControllerRoute extends DevGuild.AspNet.Routing.ControllerRoute {
        constructor() {
            super("/Employees/");
        }

        index(): DevGuild.AspNet.Routing.Uri {
            return this.buildUri("");
        }

        indexWithDeleted(): DevGuild.AspNet.Routing.Uri {
            return this.buildUri("?ShowDeleted=true");
        }

        create(): DevGuild.AspNet.Routing.Uri {
            return this.buildUri(`Create`);
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
