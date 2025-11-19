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
                var HomeControllerRoute = /** @class */ (function (_super) {
                    __extends(HomeControllerRoute, _super);
                    function HomeControllerRoute() {
                        return _super.call(this, "/Home/") || this;
                    }
                    HomeControllerRoute.prototype.index = function () {
                        return new DevGuild.AspNet.Routing.Uri("/");
                    };
                    HomeControllerRoute.prototype.about = function () {
                        return this.buildUri("About");
                    };
                    HomeControllerRoute.prototype.contacts = function () {
                        return this.buildUri("Contacts");
                    };
                    return HomeControllerRoute;
                }(DevGuild.AspNet.Routing.ControllerRoute));
                Pages.HomeControllerRoute = HomeControllerRoute;
            })(Pages = Routing.Pages || (Routing.Pages = {}));
        })(Routing = CRM.Routing || (CRM.Routing = {}));
    })(CRM = DentalDrill.CRM || (DentalDrill.CRM = {}));
})(DentalDrill || (DentalDrill = {}));
//# sourceMappingURL=home.js.map