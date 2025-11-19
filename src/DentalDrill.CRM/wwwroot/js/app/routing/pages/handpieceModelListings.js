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
                var HandpieceModelListingsControllerRoute = /** @class */ (function (_super) {
                    __extends(HandpieceModelListingsControllerRoute, _super);
                    function HandpieceModelListingsControllerRoute() {
                        return _super.call(this, "/HandpieceModelListings/") || this;
                    }
                    HandpieceModelListingsControllerRoute.prototype.create = function (parentId) {
                        return this.buildUri("Create?parentId=" + parentId);
                    };
                    HandpieceModelListingsControllerRoute.prototype.read = function (parentId) {
                        return this.buildUri("Read?parentId=" + parentId);
                    };
                    HandpieceModelListingsControllerRoute.prototype.edit = function (id) {
                        return this.buildUri("Edit/" + id);
                    };
                    HandpieceModelListingsControllerRoute.prototype.delete = function (id) {
                        return this.buildUri("Delete/" + id);
                    };
                    return HandpieceModelListingsControllerRoute;
                }(DevGuild.AspNet.Routing.ControllerRoute));
                Pages.HandpieceModelListingsControllerRoute = HandpieceModelListingsControllerRoute;
            })(Pages = Routing.Pages || (Routing.Pages = {}));
        })(Routing = CRM.Routing || (CRM.Routing = {}));
    })(CRM = DentalDrill.CRM || (DentalDrill.CRM = {}));
})(DentalDrill || (DentalDrill = {}));
//# sourceMappingURL=handpieceModelListings.js.map