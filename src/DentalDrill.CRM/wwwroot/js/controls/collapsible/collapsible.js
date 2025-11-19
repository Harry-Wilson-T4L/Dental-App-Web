var DentalDrill;
(function (DentalDrill) {
    var CRM;
    (function (CRM) {
        var Controls;
        (function (Controls) {
            var Collapsible = /** @class */ (function () {
                function Collapsible(root) {
                    this._root = root;
                    this._node = $(this._root);
                }
                Collapsible.prototype.isShown = function () {
                    return this._root.classList.contains("show");
                };
                Collapsible.prototype.showAsync = function () {
                    var _this = this;
                    return new Promise(function (resolve, reject) {
                        _this._node.one("shown.bs.collapse", function (e) {
                            resolve();
                        });
                        _this._node.collapse("show");
                    });
                };
                Collapsible.prototype.hideAsync = function () {
                    var _this = this;
                    return new Promise(function (resolve, reject) {
                        _this._node.one("hidden.bs.collapse", function (e) {
                            resolve();
                        });
                        _this._node.collapse("hide");
                    });
                };
                return Collapsible;
            }());
            Controls.Collapsible = Collapsible;
        })(Controls = CRM.Controls || (CRM.Controls = {}));
    })(CRM = DentalDrill.CRM || (DentalDrill.CRM = {}));
})(DentalDrill || (DentalDrill = {}));
//# sourceMappingURL=collapsible.js.map