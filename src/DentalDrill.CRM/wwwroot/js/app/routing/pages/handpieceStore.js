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
                var HandpieceStoreControllerRoute = /** @class */ (function (_super) {
                    __extends(HandpieceStoreControllerRoute, _super);
                    function HandpieceStoreControllerRoute() {
                        return _super.call(this, "/HandpieceStore/") || this;
                    }
                    HandpieceStoreControllerRoute.prototype.index = function () {
                        return this.buildUri("");
                    };
                    HandpieceStoreControllerRoute.prototype.read = function () {
                        return this.buildUri("Read");
                    };
                    HandpieceStoreControllerRoute.prototype.buy = function (id) {
                        return this.buildUri("Buy/" + id);
                    };
                    return HandpieceStoreControllerRoute;
                }(DevGuild.AspNet.Routing.ControllerRoute));
                Pages.HandpieceStoreControllerRoute = HandpieceStoreControllerRoute;
            })(Pages = Routing.Pages || (Routing.Pages = {}));
        })(Routing = CRM.Routing || (CRM.Routing = {}));
    })(CRM = DentalDrill.CRM || (DentalDrill.CRM = {}));
})(DentalDrill || (DentalDrill = {}));
//# sourceMappingURL=handpieceStore.js.map