/// <reference types="jquery" />
/// <reference types="bootstrap" />
//import * as $ from 'jquery';
//import 'bootstrap'; // Import Bootstrap's JS for dropdown functionality
var DentalDrill;
(function (DentalDrill) {
    var CRM;
    (function (CRM) {
        var Controls;
        (function (Controls) {
            var EventHandler = DevGuild.Utilities.EventHandler;
            var Dropdown = /** @class */ (function () {
                function Dropdown(root) {
                    var _this = this;
                    this._show = new EventHandler();
                    this._shown = new EventHandler();
                    this._hide = new EventHandler();
                    this._hidden = new EventHandler();
                    this._root = root;
                    this._menu = root.querySelector(".dropdown-menu");
                    this._node = $(root);
                    this._node.on("show.bs.dropdown", function (event) {
                        _this._show.raise(_this, event);
                    });
                    this._node.on("shown.bs.dropdown", function (event) {
                        _this._shown.raise(_this, event);
                    });
                    this._node.on("hide.bs.dropdown", function (event) {
                        _this._hide.raise(_this, event);
                    });
                    this._node.on("hidden.bs.dropdown", function (event) {
                        _this._hidden.raise(_this, event);
                    });
                }
                Object.defineProperty(Dropdown.prototype, "onShow", {
                    get: function () {
                        return this._show;
                    },
                    enumerable: false,
                    configurable: true
                });
                Object.defineProperty(Dropdown.prototype, "onShown", {
                    get: function () {
                        return this._shown;
                    },
                    enumerable: false,
                    configurable: true
                });
                Object.defineProperty(Dropdown.prototype, "onHide", {
                    get: function () {
                        return this._hide;
                    },
                    enumerable: false,
                    configurable: true
                });
                Object.defineProperty(Dropdown.prototype, "onHidden", {
                    get: function () {
                        return this._hidden;
                    },
                    enumerable: false,
                    configurable: true
                });
                Dropdown.prototype.isShown = function () {
                    return this._root.classList.contains("show");
                };
                Dropdown.prototype.show = function () {
                    var _this = this;
                    if (this.isShown()) {
                        return new Promise(function (resolve, reject) { return resolve(); });
                    }
                    return new Promise(function (resolve, reject) {
                        _this._node.one("shown.bs.dropdown", function (e) {
                            resolve();
                        });
                        _this._node.dropdown("toggle");
                    });
                };
                Dropdown.prototype.hide = function () {
                    var _this = this;
                    if (!this.isShown()) {
                        return new Promise(function (resolve, reject) { return resolve(); });
                    }
                    return new Promise(function (resolve, reject) {
                        _this._node.one("hidden.bs.dropdown", function (e) {
                            resolve();
                        });
                        _this._node.dropdown("toggle");
                    });
                };
                Dropdown.prototype.toggle = function () {
                    if (this.isShown()) {
                        return this.hide();
                    }
                    else {
                        return this.show();
                    }
                };
                Dropdown.prototype.preventContentClickHide = function () {
                    var _this = this;
                    this._node.on("hide.bs.dropdown", function (e) {
                        if (e.clickEvent) {
                            var target = e.clickEvent.target;
                            while (target && target !== document.body) {
                                if (target === _this._menu) {
                                    e.preventDefault();
                                    return;
                                }
                                target = target.parentElement;
                            }
                        }
                    });
                };
                return Dropdown;
            }());
            Controls.Dropdown = Dropdown;
        })(Controls = CRM.Controls || (CRM.Controls = {}));
    })(CRM = DentalDrill.CRM || (DentalDrill.CRM = {}));
})(DentalDrill || (DentalDrill = {}));
//# sourceMappingURL=dropdown.js.map