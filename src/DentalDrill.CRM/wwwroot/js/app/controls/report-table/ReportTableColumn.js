var DentalDrill;
(function (DentalDrill) {
    var CRM;
    (function (CRM) {
        var Controls;
        (function (Controls) {
            var Reporting;
            (function (Reporting) {
                var ReportTableColumn = /** @class */ (function () {
                    function ReportTableColumn(title, width, formatter) {
                        this._title = title;
                        this._width = width;
                        this._formatter = formatter;
                    }
                    Object.defineProperty(ReportTableColumn.prototype, "title", {
                        get: function () {
                            return this._title;
                        },
                        enumerable: false,
                        configurable: true
                    });
                    Object.defineProperty(ReportTableColumn.prototype, "width", {
                        get: function () {
                            return this._width;
                        },
                        enumerable: false,
                        configurable: true
                    });
                    ReportTableColumn.prototype.setFormatter = function (formatter) {
                        this._formatter = formatter;
                    };
                    ReportTableColumn.prototype.format = function (item) {
                        try {
                            return this._formatter(item);
                        }
                        catch (exception) {
                            return "";
                        }
                    };
                    return ReportTableColumn;
                }());
                Reporting.ReportTableColumn = ReportTableColumn;
            })(Reporting = Controls.Reporting || (Controls.Reporting = {}));
        })(Controls = CRM.Controls || (CRM.Controls = {}));
    })(CRM = DentalDrill.CRM || (DentalDrill.CRM = {}));
})(DentalDrill || (DentalDrill = {}));
//# sourceMappingURL=ReportTableColumn.js.map