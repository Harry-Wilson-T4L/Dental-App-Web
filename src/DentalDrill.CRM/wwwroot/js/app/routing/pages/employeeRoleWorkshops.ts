namespace DentalDrill.CRM.Routing.Pages {
    export class EmployeeRoleWorkshopsControllerRoute extends DevGuild.AspNet.Routing.ControllerRoute {
        constructor() {
            super("/EmployeeRoleWorkshops/");
        }

        create(employeeRoleId: string): DevGuild.AspNet.Routing.Uri {
            return this.buildUri(`Create?parentId=${employeeRoleId}`);
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
