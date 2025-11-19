var DevGuild;
(function (DevGuild) {
    var Utilities;
    (function (Utilities) {
        var EventHandler = /** @class */ (function () {
            function EventHandler() {
                this._handlers = [];
            }
            EventHandler.prototype.raise = function (sender, eventArgs) {
                for (var _i = 0, _a = this._handlers; _i < _a.length; _i++) {
                    var handler = _a[_i];
                    handler(sender, eventArgs);
                }
            };
            EventHandler.prototype.subscribe = function (handler) {
                for (var i = 0; i < this._handlers.length; i++) {
                    if (this._handlers[i] === handler) {
                        return;
                    }
                }
                this._handlers.push(handler);
            };
            EventHandler.prototype.unsubscribe = function (handler) {
                for (var i = 0; i < this._handlers.length; i++) {
                    if (this._handlers[i] === handler) {
                        this._handlers.splice(i, 1);
                        return;
                    }
                }
            };
            return EventHandler;
        }());
        Utilities.EventHandler = EventHandler;
    })(Utilities = DevGuild.Utilities || (DevGuild.Utilities = {}));
})(DevGuild || (DevGuild = {}));
//# sourceMappingURL=event-handler.js.map