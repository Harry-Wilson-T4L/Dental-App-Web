var DevGuild;
(function (DevGuild) {
    var AspNet;
    (function (AspNet) {
        var Routing;
        (function (Routing) {
            var Uri = /** @class */ (function () {
                function Uri(value) {
                    this._value = value;
                }
                Object.defineProperty(Uri.prototype, "value", {
                    get: function () {
                        return this._value;
                    },
                    enumerable: false,
                    configurable: true
                });
                Uri.prototype.navigate = function () {
                    window.location.href = this._value;
                };
                Uri.prototype.open = function () {
                    window.open(this._value, "_blank");
                };
                Uri.prototype.execute = function (open) {
                    if (open) {
                        this.open();
                    }
                    else {
                        this.navigate();
                    }
                };
                return Uri;
            }());
            Routing.Uri = Uri;
            var ControllerRoute = /** @class */ (function () {
                function ControllerRoute(prefix) {
                    this._prefix = prefix;
                }
                Object.defineProperty(ControllerRoute.prototype, "prefix", {
                    get: function () {
                        return this._prefix;
                    },
                    enumerable: false,
                    configurable: true
                });
                ControllerRoute.prototype.buildUri = function (uri) {
                    return new Uri(this._prefix + uri);
                };
                return ControllerRoute;
            }());
            Routing.ControllerRoute = ControllerRoute;
        })(Routing = AspNet.Routing || (AspNet.Routing = {}));
    })(AspNet = DevGuild.AspNet || (DevGuild.AspNet = {}));
})(DevGuild || (DevGuild = {}));
//# sourceMappingURL=routing-core.js.map