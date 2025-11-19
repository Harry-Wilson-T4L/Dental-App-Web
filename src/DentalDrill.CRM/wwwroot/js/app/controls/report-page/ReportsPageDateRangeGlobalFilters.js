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
        var Controls;
        (function (Controls) {
            var Reporting;
            (function (Reporting) {
                var ReportsPageDateRangeGlobalFilters = /** @class */ (function (_super) {
                    __extends(ReportsPageDateRangeGlobalFilters, _super);
                    function ReportsPageDateRangeGlobalFilters(from, to) {
                        var _this = _super.call(this) || this;
                        _this._from = from;
                        _this._to = to;
                        return _this;
                    }
                    Object.defineProperty(ReportsPageDateRangeGlobalFilters.prototype, "from", {
                        get: function () {
                            return this._from;
                        },
                        enumerable: false,
                        configurable: true
                    });
                    Object.defineProperty(ReportsPageDateRangeGlobalFilters.prototype, "to", {
                        get: function () {
                            return this._to;
                        },
                        enumerable: false,
                        configurable: true
                    });
                    return ReportsPageDateRangeGlobalFilters;
                }(Reporting.ReportsPageGlobalFiltersBase));
                Reporting.ReportsPageDateRangeGlobalFilters = ReportsPageDateRangeGlobalFilters;
            })(Reporting = Controls.Reporting || (Controls.Reporting = {}));
        })(Controls = CRM.Controls || (CRM.Controls = {}));
    })(CRM = DentalDrill.CRM || (DentalDrill.CRM = {}));
})(DentalDrill || (DentalDrill = {}));
//# sourceMappingURL=ReportsPageDateRangeGlobalFilters.js.map