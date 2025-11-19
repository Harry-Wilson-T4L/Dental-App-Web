namespace DentalDrill.CRM.Pages.CorporateSurgeries.Reports {
    import DateRangeSelector = DentalDrill.CRM.Controls.DateRangeSelector;
    import MultiSelectToggler = DentalDrill.CRM.Controls.MultiSelectToggler;

    export class ReportsPageMainControls {
        private readonly _dateRangeSelector: DateRangeSelector;
        private readonly _dateFrom: kendo.ui.DatePicker;
        private readonly _dateTo: kendo.ui.DatePicker;

        private readonly _clients: kendo.ui.MultiSelect;
        private readonly _states: MultiSelectToggler;

        private readonly _buttonClear: JQuery;
        private readonly _buttonSearch: JQuery;
        private readonly _buttonCompare: JQuery;

        constructor(root: JQuery) {
            this._dateRangeSelector = new DateRangeSelector(root.find(".reports__dates-range"));
            this._dateFrom = root.find("input.reports__dates-from").data("kendoDatePicker");
            this._dateTo = root.find("input.reports__dates-to").data("kendoDatePicker");

            this._clients = root.find("select.reports__clients").data("kendoMultiSelect");
            this._states = new MultiSelectToggler(root.find(".reports__states"));

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

        get clients(): kendo.ui.MultiSelect {
            return this._clients;
        }

        get states(): MultiSelectToggler {
            return this._states;
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