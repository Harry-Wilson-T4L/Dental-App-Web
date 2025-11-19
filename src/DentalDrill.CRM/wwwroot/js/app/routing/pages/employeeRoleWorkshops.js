var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (Object.prototype.hasOwnProperty.call(b, p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        if (typeof b !== "function" && b !== null)
            throw new TypeError("Class extends value " + String(b) + " is not a constructor or null");
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var DentalDrill;
(function (DentalDrill) {
    var CRM;
    (function (CRM) {
        var Routing;
        (function (Routing) {
            var Pages;
            (function (Pages) {
                var EmployeeRoleWorkshopsControllerRoute = /** @class */ (function (_super) {
                    __extends(EmployeeRoleWorkshopsControllerRoute, _super);
                    function EmployeeRoleWorkshopsControllerRoute() {
                        return _super.call(this, "/EmployeeRoleWorkshops/") || this;
                    }
                    EmployeeRoleWorkshopsControllerRoute.prototype.create = function (employeeRoleId) {
                        return this.buildUri("Create?parentId=" + employeeRoleId);
                    };
                    EmployeeRoleWorkshopsControllerRoute.prototype.details = function (id) {
                        return this.buildUri("Details/" + id);
                    };
                    EmployeeRoleWorkshopsControllerRoute.prototype.edit = function (id) {
                        return this.buildUri("Edit/" + id);
                    };
                    EmployeeRoleWorkshopsControllerRoute.prototype.delete = function (id) {
                        return this.buildUri("Delete/" + id);
                    };
                    return EmployeeRoleWorkshopsControllerRoute;
                }(DevGuild.AspNet.Routing.ControllerRoute));
                Pages.EmployeeRoleWorkshopsControllerRoute = EmployeeRoleWorkshopsControllerRoute;
            })(Pages = Routing.Pages || (Routing.Pages = {}));
        })(Routing = CRM.Routing || (CRM.Routing = {}));
    })(CRM = DentalDrill.CRM || (DentalDrill.CRM = {}));
})(DentalDrill || (DentalDrill = {}));
//# sourceMappingURL=employeeRoleWorkshops.js.map