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
                var ClientFeedbackFormsControllerRoute = /** @class */ (function (_super) {
                    __extends(ClientFeedbackFormsControllerRoute, _super);
                    function ClientFeedbackFormsControllerRoute() {
                        return _super.call(this, "/ClientFeedbackForms/") || this;
                    }
                    ClientFeedbackFormsControllerRoute.prototype.create = function (parentId) {
                        return this.buildUri("SendNewForm/?clientId=" + parentId);
                    };
                    ClientFeedbackFormsControllerRoute.prototype.details = function (id) {
                        return this.buildUri("Details/" + id);
                    };
                    return ClientFeedbackFormsControllerRoute;
                }(DevGuild.AspNet.Routing.ControllerRoute));
                Pages.ClientFeedbackFormsControllerRoute = ClientFeedbackFormsControllerRoute;
            })(Pages = Routing.Pages || (Routing.Pages = {}));
        })(Routing = CRM.Routing || (CRM.Routing = {}));
    })(CRM = DentalDrill.CRM || (DentalDrill.CRM = {}));
})(DentalDrill || (DentalDrill = {}));
//# sourceMappingURL=clientFeedback.js.map