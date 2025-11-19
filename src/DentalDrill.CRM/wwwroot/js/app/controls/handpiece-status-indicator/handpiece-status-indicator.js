var DentalDrill;
(function (DentalDrill) {
    var CRM;
    (function (CRM) {
        var Controls;
        (function (Controls) {
            var HandpieceStatusIndicator = /** @class */ (function () {
                function HandpieceStatusIndicator() {
                    this._max = 7;
                    this._value = 0;
                    this._danger = false;
                    this._overrides = [];
                    this._counts = [];
                }
                Object.defineProperty(HandpieceStatusIndicator.prototype, "max", {
                    get: function () {
                        return this._max;
                    },
                    enumerable: false,
                    configurable: true
                });
                Object.defineProperty(HandpieceStatusIndicator.prototype, "value", {
                    get: function () {
                        return this._value;
                    },
                    set: function (val) {
                        if (val < 0 || val > this._max) {
                            throw new Error("Invalid value");
                        }
                        this._value = val;
                    },
                    enumerable: false,
                    configurable: true
                });
                Object.defineProperty(HandpieceStatusIndicator.prototype, "danger", {
                    get: function () {
                        return this._danger;
                    },
                    set: function (val) {
                        this._danger = val;
                    },
                    enumerable: false,
                    configurable: true
                });
                HandpieceStatusIndicator.prototype.getOverride = function (index) {
                    if (index < 1 || index > this._max) {
                        throw new Error("Invalid index");
                    }
                    var val = this._overrides[index];
                    return val ? val : false;
                };
                HandpieceStatusIndicator.prototype.setOverride = function (index, val) {
                    if (index < 1 || index > this._max) {
                        throw new Error("Invalid index");
                    }
                    this._overrides[index] = val;
                };
                HandpieceStatusIndicator.prototype.getCount = function (index) {
                    if (index < 1 || index > this._max) {
                        throw new Error("Invalid index");
                    }
                    var val = this._counts[index];
                    return val ? val : 0;
                };
                HandpieceStatusIndicator.prototype.setCount = function (index, val) {
                    if (index < 1 || index > this._max) {
                        throw new Error("Invalid index");
                    }
                    this._counts[index] = val;
                };
                HandpieceStatusIndicator.prototype.render = function () {
                    var mainDiv = this.createMainDiv();
                    mainDiv.appendChild(this.createProgress());
                    mainDiv.appendChild(this.createPoints());
                    return mainDiv;
                };
                HandpieceStatusIndicator.prototype.createMainDiv = function () {
                    var div = document.createElement("div");
                    div.classList.add("handpiece-status-indicator");
                    div.setAttribute("data-max", "" + this._max);
                    div.setAttribute("data-value", "" + this._value);
                    div.setAttribute("data-danger", "" + (this._danger ? "True" : "False"));
                    div.setAttribute("data-override", this.computeOverrides());
                    return div;
                };
                HandpieceStatusIndicator.prototype.computeOverrides = function () {
                    var result = "";
                    for (var i = 1; i <= this._max; i++) {
                        if (this.getOverride(i)) {
                            result += " " + i;
                        }
                    }
                    return result.trim();
                };
                HandpieceStatusIndicator.prototype.createProgress = function () {
                    var progress = document.createElement("div");
                    progress.classList.add("progress");
                    progress.classList.add("handpiece-status-indicator__progress");
                    progress.appendChild(this.createProgressBar());
                    return progress;
                };
                HandpieceStatusIndicator.prototype.createProgressBar = function () {
                    var progressBar = document.createElement("div");
                    progressBar.classList.add("progress-bar");
                    return progressBar;
                };
                HandpieceStatusIndicator.prototype.createPoints = function () {
                    var points = document.createElement("div");
                    points.classList.add("handpiece-status-indicator__points");
                    for (var i = 1; i <= this._max; i++) {
                        points.appendChild(this.createPoint(i));
                    }
                    return points;
                };
                HandpieceStatusIndicator.prototype.createPoint = function (index) {
                    var point = document.createElement("div");
                    point.classList.add("handpiece-status-indicator__points__point");
                    var count = this.getCount(index);
                    if (count > 0) {
                        point.innerText = count.toString();
                    }
                    return point;
                };
                return HandpieceStatusIndicator;
            }());
            Controls.HandpieceStatusIndicator = HandpieceStatusIndicator;
        })(Controls = CRM.Controls || (CRM.Controls = {}));
    })(CRM = DentalDrill.CRM || (DentalDrill.CRM = {}));
})(DentalDrill || (DentalDrill = {}));
//# sourceMappingURL=handpiece-status-indicator.js.map