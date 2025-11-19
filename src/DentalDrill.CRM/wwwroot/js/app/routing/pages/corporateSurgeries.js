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
                var CorporateSurgeriesControllerRoute = /** @class */ (function (_super) {
                    __extends(CorporateSurgeriesControllerRoute, _super);
                    function CorporateSurgeriesControllerRoute() {
                        return _super.call(this, "/CorporateSurgeries/") || this;
                    }
                    CorporateSurgeriesControllerRoute.prototype.handpiece = function (corporateId, id) {
                        return this.buildUri(corporateId + "/Handpiece/" + id);
                    };
                    CorporateSurgeriesControllerRoute.prototype.exportSurgeriesToExcel = function (clientId, from, to, dateAggregate, reportFields, clients) {
                        var fromString = (new Date(from.getTime() - from.getTimezoneOffset() * 60 * 1000)).toISOString().substr(0, 10);
                        var toString = (new Date(to.getTime() - to.getTimezoneOffset() * 60 * 1000)).toISOString().substr(0, 10);
                        return this.buildUri(clientId + "/Reports/Handpieces/Export/Excel?from=" + encodeURIComponent(fromString) + "&to=" + encodeURIComponent(toString) + "&dateAggregate=" + encodeURIComponent(dateAggregate) + "&reportFields=" + encodeURIComponent(reportFields) + "&clients=" + clients);
                    };
                    CorporateSurgeriesControllerRoute.prototype.exportBrandsToExcel = function (clientId, from, to, dateAggregate, reportFields, clients) {
                        var fromString = (new Date(from.getTime() - from.getTimezoneOffset() * 60 * 1000)).toISOString().substr(0, 10);
                        var toString = (new Date(to.getTime() - to.getTimezoneOffset() * 60 * 1000)).toISOString().substr(0, 10);
                        return this.buildUri(clientId + "/Reports/Brands/Export/Excel?from=" + encodeURIComponent(fromString) + "&to=" + encodeURIComponent(toString) + "&dateAggregate=" + encodeURIComponent(dateAggregate) + "&reportFields=" + encodeURIComponent(reportFields) + "&clients=" + clients);
                    };
                    CorporateSurgeriesControllerRoute.prototype.exportStatusesToExcel = function (clientId, from, to, clients) {
                        var fromString = (new Date(from.getTime() - from.getTimezoneOffset() * 60 * 1000)).toISOString().substr(0, 10);
                        var toString = (new Date(to.getTime() - to.getTimezoneOffset() * 60 * 1000)).toISOString().substr(0, 10);
                        return this.buildUri(clientId + "/Reports/Statuses/Export/Excel?from=" + encodeURIComponent(fromString) + "&to=" + encodeURIComponent(toString) + "&clients=" + clients);
                    };
                    return CorporateSurgeriesControllerRoute;
                }(DevGuild.AspNet.Routing.ControllerRoute));
                Pages.CorporateSurgeriesControllerRoute = CorporateSurgeriesControllerRoute;
            })(Pages = Routing.Pages || (Routing.Pages = {}));
        })(Routing = CRM.Routing || (CRM.Routing = {}));
    })(CRM = DentalDrill.CRM || (DentalDrill.CRM = {}));
})(DentalDrill || (DentalDrill = {}));
//# sourceMappingURL=corporateSurgeries.js.map