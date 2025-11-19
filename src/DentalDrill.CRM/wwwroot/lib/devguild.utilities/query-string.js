/// <reference path="query-string-item.ts" />
var DevGuild;
(function (DevGuild) {
    var Utilities;
    (function (Utilities) {
        var QueryString = /** @class */ (function () {
            function QueryString() {
                this._items = [];
            }
            QueryString.parse = function (query) {
                var result = new QueryString();
                if (!query || query.length <= 1) {
                    return result;
                }
                var params = query.substring(1).split("&");
                for (var i = 0; i < params.length; i++) {
                    var pair = params[i].split("=");
                    var item = new Utilities.QueryStringItem(decodeURIComponent(pair[0]), decodeURIComponent(pair[1]));
                    result._items.push(item);
                }
                return result;
            };
            QueryString.prototype.build = function () {
                var result = "";
                for (var _i = 0, _a = this._items; _i < _a.length; _i++) {
                    var item = _a[_i];
                    if (item.key && item.value) {
                        result += "&" + encodeURIComponent(item.key) + "=" + encodeURIComponent(item.value);
                    }
                }
                return result === "" ? result : "?" + result.substring(1);
            };
            QueryString.prototype.get = function (key) {
                var item = this.find(key);
                return item ? item.value : undefined;
            };
            QueryString.prototype.set = function (key, value) {
                var item = this.find(key);
                if (item) {
                    item.value = value;
                }
                else {
                    item = new Utilities.QueryStringItem(key, value);
                    this._items.push(item);
                }
                return this;
            };
            QueryString.prototype.find = function (key) {
                for (var _i = 0, _a = this._items; _i < _a.length; _i++) {
                    var item = _a[_i];
                    if (item.key.toLowerCase() === key.toLowerCase()) {
                        return item;
                    }
                }
                return undefined;
            };
            return QueryString;
        }());
        Utilities.QueryString = QueryString;
    })(Utilities = DevGuild.Utilities || (DevGuild.Utilities = {}));
})(DevGuild || (DevGuild = {}));
//# sourceMappingURL=query-string.js.map