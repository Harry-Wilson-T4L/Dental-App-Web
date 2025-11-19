var DentalDrill;
(function (DentalDrill) {
    var CRM;
    (function (CRM) {
        var Pages;
        (function (Pages) {
            var CorporateSurgeries;
            (function (CorporateSurgeries) {
                var Reports;
                (function (Reports) {
                    var DateRangeSelector = DentalDrill.CRM.Controls.DateRangeSelector;
                    var MultiSelectToggler = DentalDrill.CRM.Controls.MultiSelectToggler;
                    var ReportsPageMainControls = /** @class */ (function () {
                        function ReportsPageMainControls(root) {
                            this._dateRangeSelector = new DateRangeSelector(root.find(".reports__dates-range"));
                            this._dateFrom = root.find("input.reports__dates-from").data("kendoDatePicker");
                            this._dateTo = root.find("input.reports__dates-to").data("kendoDatePicker");
                            this._clients = root.find("select.reports__clients").data("kendoMultiSelect");
                            this._states = new MultiSelectToggler(root.find(".reports__states"));
                            this._buttonClear = root.find(".reports__clear");
                            this._buttonSearch = root.find(".reports__search");
                            this._buttonCompare = root.find(".reports__compare");
                        }
                        Object.defineProperty(ReportsPageMainControls.prototype, "dateRangeSelector", {
                            get: function () {
                                return this._dateRangeSelector;
                            },
                            enumerable: false,
                            configurable: true
                        });
                        Object.defineProperty(ReportsPageMainControls.prototype, "dateFrom", {
                            get: function () {
                                return this._dateFrom;
                            },
                            enumerable: false,
                            configurable: true
                        });
                        Object.defineProperty(ReportsPageMainControls.prototype, "dateTo", {
                            get: function () {
                                return this._dateTo;
                            },
                            enumerable: false,
                            configurable: true
                        });
                        Object.defineProperty(ReportsPageMainControls.prototype, "clients", {
                            get: function () {
                                return this._clients;
                            },
                            enumerable: false,
                            configurable: true
                        });
                        Object.defineProperty(ReportsPageMainControls.prototype, "states", {
                            get: function () {
                                return this._states;
                            },
                            enumerable: false,
                            configurable: true
                        });
                        Object.defineProperty(ReportsPageMainControls.prototype, "buttonClear", {
                            get: function () {
                                return this._buttonClear;
                            },
                            enumerable: false,
                            configurable: true
                        });
                        Object.defineProperty(ReportsPageMainControls.prototype, "buttonSearch", {
                            get: function () {
                                return this._buttonSearch;
                            },
                            enumerable: false,
                            configurable: true
                        });
                        Object.defineProperty(ReportsPageMainControls.prototype, "buttonCompare", {
                            get: function () {
                                return this._buttonCompare;
                            },
                            enumerable: false,
                            configurable: true
                        });
                        return ReportsPageMainControls;
                    }());
                    Reports.ReportsPageMainControls = ReportsPageMainControls;
                })(Reports = CorporateSurgeries.Reports || (CorporateSurgeries.Reports = {}));
            })(CorporateSurgeries = Pages.CorporateSurgeries || (Pages.CorporateSurgeries = {}));
        })(Pages = CRM.Pages || (CRM.Pages = {}));
    })(CRM = DentalDrill.CRM || (DentalDrill.CRM = {}));
})(DentalDrill || (DentalDrill = {}));
//# sourceMappingURL=ReportsPageMainControls.js.map