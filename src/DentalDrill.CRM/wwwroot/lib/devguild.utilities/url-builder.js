var DevGuild;
(function (DevGuild) {
    var Utilities;
    (function (Utilities) {
        var UriBuilder = /** @class */ (function () {
            function UriBuilder(uri) {
                var regex = /^([^?]+)\??(.*)$/;
                var match = regex.exec(uri);
                if (!match) {
                    throw new Error("Invalid uri");
                }
                this._path = match[1];
                this._query = match[2] ? Utilities.QueryString.parse(match[2]) : new Utilities.QueryString();
            }
            Object.defineProperty(UriBuilder.prototype, "path", {
                get: function () {
                    return this._path;
                },
                set: function (value) {
                    this._path = value;
                },
                enumerable: false,
                configurable: true
            });
            Object.defineProperty(UriBuilder.prototype, "query", {
                get: function () {
                    return this._query;
                },
                enumerable: false,
                configurable: true
            });
            Object.defineProperty(UriBuilder.prototype, "value", {
                get: function () {
                    return this._path + this._query.build();
                },
                enumerable: false,
                configurable: true
            });
            return UriBuilder;
        }());
        Utilities.UriBuilder = UriBuilder;
    })(Utilities = DevGuild.Utilities || (DevGuild.Utilities = {}));
})(DevGuild || (DevGuild = {}));
//# sourceMappingURL=url-builder.js.map