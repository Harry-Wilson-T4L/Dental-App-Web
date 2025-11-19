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
                var DiagnosticCheckItemsControllerRoute = /** @class */ (function (_super) {
                    __extends(DiagnosticCheckItemsControllerRoute, _super);
                    function DiagnosticCheckItemsControllerRoute() {
                        return _super.call(this, "/DiagnosticCheckItems/") || this;
                    }
                    DiagnosticCheckItemsControllerRoute.prototype.index = function () {
                        return this.buildUri("");
                    };
                    return DiagnosticCheckItemsControllerRoute;
                }(DevGuild.AspNet.Routing.ControllerRoute));
                Pages.DiagnosticCheckItemsControllerRoute = DiagnosticCheckItemsControllerRoute;
                var DiagnosticCheckTypesControllerRoute = /** @class */ (function (_super) {
                    __extends(DiagnosticCheckTypesControllerRoute, _super);
                    function DiagnosticCheckTypesControllerRoute() {
                        return _super.call(this, "/DiagnosticCheckTypes/") || this;
                    }
                    DiagnosticCheckTypesControllerRoute.prototype.index = function () {
                        return this.buildUri("");
                    };
                    DiagnosticCheckTypesControllerRoute.prototype.sortItems = function (typeId) {
                        return this.buildUri("SortItems/" + typeId);
                    };
                    return DiagnosticCheckTypesControllerRoute;
                }(DevGuild.AspNet.Routing.ControllerRoute));
                Pages.DiagnosticCheckTypesControllerRoute = DiagnosticCheckTypesControllerRoute;
            })(Pages = Routing.Pages || (Routing.Pages = {}));
        })(Routing = CRM.Routing || (CRM.Routing = {}));
    })(CRM = DentalDrill.CRM || (DentalDrill.CRM = {}));
})(DentalDrill || (DentalDrill = {}));
//# sourceMappingURL=diagnosticCheckItems.js.map