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
                var JobsControllerRoute = /** @class */ (function (_super) {
                    __extends(JobsControllerRoute, _super);
                    function JobsControllerRoute() {
                        return _super.call(this, "/Jobs/") || this;
                    }
                    JobsControllerRoute.prototype.index = function () {
                        return this.buildUri("");
                    };
                    JobsControllerRoute.prototype.create = function (type) {
                        return this.buildUri("Create?type=" + type);
                    };
                    JobsControllerRoute.prototype.details = function (id) {
                        return this.buildUri("Details/" + id);
                    };
                    JobsControllerRoute.prototype.edit = function (id) {
                        return this.buildUri("Edit/" + id);
                    };
                    JobsControllerRoute.prototype.delete = function (id) {
                        return this.buildUri("Delete/" + id);
                    };
                    return JobsControllerRoute;
                }(DevGuild.AspNet.Routing.ControllerRoute));
                Pages.JobsControllerRoute = JobsControllerRoute;
            })(Pages = Routing.Pages || (Routing.Pages = {}));
        })(Routing = CRM.Routing || (CRM.Routing = {}));
    })(CRM = DentalDrill.CRM || (DentalDrill.CRM = {}));
})(DentalDrill || (DentalDrill = {}));
//# sourceMappingURL=jobs.js.map