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
                var HandpieceModelSchematicsControllerRoute = /** @class */ (function (_super) {
                    __extends(HandpieceModelSchematicsControllerRoute, _super);
                    function HandpieceModelSchematicsControllerRoute() {
                        return _super.call(this, "/HandpieceModelSchematics/") || this;
                    }
                    HandpieceModelSchematicsControllerRoute.prototype.createText = function (parentId) {
                        return this.buildUri("CreateText?parentId=" + parentId);
                    };
                    HandpieceModelSchematicsControllerRoute.prototype.createAttachment = function (parentId) {
                        return this.buildUri("CreateAttachment?parentId=" + parentId);
                    };
                    HandpieceModelSchematicsControllerRoute.prototype.createImage = function (parentId) {
                        return this.buildUri("CreateImage?parentId=" + parentId);
                    };
                    HandpieceModelSchematicsControllerRoute.prototype.read = function (parentId) {
                        return this.buildUri("Read?parentId=" + parentId);
                    };
                    HandpieceModelSchematicsControllerRoute.prototype.details = function (id) {
                        return this.buildUri("Details/" + id);
                    };
                    HandpieceModelSchematicsControllerRoute.prototype.editText = function (id) {
                        return this.buildUri("EditText/" + id);
                    };
                    HandpieceModelSchematicsControllerRoute.prototype.editAttachment = function (id) {
                        return this.buildUri("EditAttachment/" + id);
                    };
                    HandpieceModelSchematicsControllerRoute.prototype.editImage = function (id) {
                        return this.buildUri("EditImage/" + id);
                    };
                    HandpieceModelSchematicsControllerRoute.prototype.delete = function (id) {
                        return this.buildUri("Delete/" + id);
                    };
                    HandpieceModelSchematicsControllerRoute.prototype.moveUp = function (id) {
                        return this.buildUri("MoveUp/" + id);
                    };
                    HandpieceModelSchematicsControllerRoute.prototype.moveDown = function (id) {
                        return this.buildUri("MoveDown/" + id);
                    };
                    HandpieceModelSchematicsControllerRoute.prototype.downloadAttachment = function (id) {
                        return this.buildUri("DownloadAttachment/" + id);
                    };
                    return HandpieceModelSchematicsControllerRoute;
                }(DevGuild.AspNet.Routing.ControllerRoute));
                Pages.HandpieceModelSchematicsControllerRoute = HandpieceModelSchematicsControllerRoute;
            })(Pages = Routing.Pages || (Routing.Pages = {}));
        })(Routing = CRM.Routing || (CRM.Routing = {}));
    })(CRM = DentalDrill.CRM || (DentalDrill.CRM = {}));
})(DentalDrill || (DentalDrill = {}));
//# sourceMappingURL=handpieceModelSchematics.js.map