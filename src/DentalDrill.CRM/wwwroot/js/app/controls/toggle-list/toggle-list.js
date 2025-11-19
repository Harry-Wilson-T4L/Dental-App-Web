var DentalDrill;
(function (DentalDrill) {
    var CRM;
    (function (CRM) {
        var Controls;
        (function (Controls) {
            var EventHandler = DevGuild.Utilities.EventHandler;
            var ToggleListItem = /** @class */ (function () {
                function ToggleListItem(root) {
                    this._root = root;
                    this._key = root.attr("data-key");
                    this._active = root.hasClass("toggle-list__item--active");
                    root.data("ToggleListItem", this);
                }
                Object.defineProperty(ToggleListItem.prototype, "key", {
                    get: function () {
                        return this._key;
                    },
                    enumerable: false,
                    configurable: true
                });
                Object.defineProperty(ToggleListItem.prototype, "active", {
                    get: function () {
                        return this._active;
                    },
                    enumerable: false,
                    configurable: true
                });
                ToggleListItem.prototype.activate = function () {
                    this._active = true;
                    this._root.addClass("toggle-list__item--active");
                };
                ToggleListItem.prototype.deactivate = function () {
                    this._active = false;
                    this._root.removeClass("toggle-list__item--active");
                };
                return ToggleListItem;
            }());
            Controls.ToggleListItem = ToggleListItem;
            var ToggleList = /** @class */ (function () {
                function ToggleList(root) {
                    var _this = this;
                    this._items = [];
                    this._selectionChanged = new EventHandler();
                    this._root = root;
                    this._root.find(".toggle-list__item").each(function (index, element) {
                        var control = $(element);
                        var item = new ToggleListItem(control);
                        control.on("click", function (e) {
                            if (item.active) {
                                item.deactivate();
                            }
                            else {
                                item.activate();
                            }
                            _this._selectionChanged.raise(_this, _this.selected);
                        });
                        _this._items.push(item);
                    });
                    root.data("ToggleList", this);
                }
                Object.defineProperty(ToggleList.prototype, "selectionChanged", {
                    get: function () {
                        return this._selectionChanged;
                    },
                    enumerable: false,
                    configurable: true
                });
                Object.defineProperty(ToggleList.prototype, "selected", {
                    get: function () {
                        return this._items.filter(function (x) { return x.active; }).map(function (x) { return x.key; });
                    },
                    enumerable: false,
                    configurable: true
                });
                ToggleList.prototype.isToggled = function (value) {
                    return this._items.some(function (x) { return x.active && x.key === value; });
                };
                return ToggleList;
            }());
            Controls.ToggleList = ToggleList;
            $(function () {
                $(".toggle-list[data-init='true']").each(function (index, element) {
                    var list = new ToggleList($(element));
                });
            });
        })(Controls = CRM.Controls || (CRM.Controls = {}));
    })(CRM = DentalDrill.CRM || (DentalDrill.CRM = {}));
})(DentalDrill || (DentalDrill = {}));
//# sourceMappingURL=toggle-list.js.map