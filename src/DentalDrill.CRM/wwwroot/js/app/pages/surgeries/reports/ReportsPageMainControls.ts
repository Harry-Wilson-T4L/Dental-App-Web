namespace DentalDrill.CRM.Pages.Surgeries.Reports {
    import DateRangeSelector = DentalDrill.CRM.Controls.DateRangeSelector;

    export class ReportsPageMainControls {
        private readonly _dateRangeSelector: DateRangeSelector;
        private readonly _dateFrom: kendo.ui.DatePicker;
        private readonly _dateTo: kendo.ui.DatePicker;

        private readonly _buttonClear: JQuery;
        private readonly _buttonSearch: JQuery;
        private readonly _buttonCompare: JQuery;

        constructor(root: JQuery) {
            this._dateRangeSelector = new DateRangeSelector(root.find(".reports__dates-range"));
            this._dateFrom = root.find("input.reports__dates-from").data("kendoDatePicker");
            this._dateTo = root.find("input.reports__dates-to").data("kendoDatePicker");

            this._buttonClear = root.find(".reports__clear");
            this._buttonSearch = root.find(".reports__search");
            this._buttonCompare = root.find(".reports__compare");
        }

        get dateRangeSelector(): DateRangeSelector {
            return this._dateRangeSelector;
        }

        get dateFrom(): kendo.ui.DatePicker {
            return this._dateFrom;
        }

        get dateTo(): kendo.ui.DatePicker {
            return this._dateTo;
        }

        get buttonClear(): JQuery {
            return this._buttonClear;
        }

        get buttonSearch(): JQuery {
            return this._buttonSearch;
        }

        get buttonCompare(): JQuery {
            return this._buttonCompare;
        }
    }
}