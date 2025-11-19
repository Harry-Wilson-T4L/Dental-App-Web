var DentalDrill;
(function (DentalDrill) {
    var CRM;
    (function (CRM) {
        var Controls;
        (function (Controls) {
            var EventHandler = DevGuild.Utilities.EventHandler;
            var ToggleButton = /** @class */ (function () {
                function ToggleButton(root) {
                    var _this = this;
                    this._changed = new EventHandler();
                    this._root = root;
                    this._active = root.hasClass("toggle-button--active");
                    this._root.on("click", function () { return _this.toggle(); });
                }
                Object.defineProperty(ToggleButton.prototype, "active", {
                    get: function () {
                        return this._active;
                    },
                    enumerable: false,
                    configurable: true
                });
                Object.defineProperty(ToggleButton.prototype, "changed", {
                    get: function () {
                        return this._changed;
                    },
                    enumerable: false,
                    configurable: true
                });
                ToggleButton.prototype.activate = function () {
                    this._active = true;
                    this._root.addClass("toggle-button--active");
                    this._changed.raise(this, this._active);
                };
                ToggleButton.prototype.deactivate = function () {
                    this._active = false;
                    this._root.removeClass("toggle-button--active");
                    this._changed.raise(this, this._active);
                };
                ToggleButton.prototype.toggle = function () {
                    if (this.active) {
                        this.deactivate();
                    }
                    else {
                        this.activate();
                    }
                };
                return ToggleButton;
            }());
            Controls.ToggleButton = ToggleButton;
        })(Controls = CRM.Controls || (CRM.Controls = {}));
    })(CRM = DentalDrill.CRM || (DentalDrill.CRM = {}));
})(DentalDrill || (DentalDrill = {}));
//# sourceMappingURL=toggle-button.js.map