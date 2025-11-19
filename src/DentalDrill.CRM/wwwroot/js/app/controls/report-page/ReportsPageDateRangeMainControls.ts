namespace DentalDrill.CRM.Controls.Reporting {
    export class ReportsPageDateRangeMainControls<TGlobalFilters extends ReportsPageDateRangeGlobalFilters> extends ReportsPageMainControlsBase<TGlobalFilters> {
        private readonly _dateRangeSelector: DateRangeSelector;
        private readonly _dateFrom: kendo.ui.DatePicker;
        private readonly _dateTo: kendo.ui.DatePicker;

        private readonly _buttonClear: JQuery;
        private readonly _buttonSearch: JQuery;

        constructor(root: JQuery) {
            super();
            this._dateRangeSelector = new DateRangeSelector(root.find(".reports__dates-range"));
            this._dateFrom = root.find("input.reports__dates-from").data("kendoDatePicker");
            this._dateTo = root.find("input.reports__dates-to").data("kendoDatePicker");

            this._buttonClear = root.find(".reports__clear");
            this._buttonSearch = root.find(".reports__search");
            
            this._buttonClear.on("click", async (e) => {
                await this.clearFilters();
                this.raiseChanged(await this.getGlobalFilters());
            });

            this._buttonSearch.on("click", async (e) => {
                this.raiseChanged(await this.getGlobalFilters());
            });

            this.setDatePickerValidation(this._dateFrom);
            this.setDatePickerValidation(this._dateTo);
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

        async getGlobalFilters(): Promise<TGlobalFilters> {
            const filters = new ReportsPageDateRangeGlobalFilters(
                this._dateFrom.value(),
                this._dateTo.value());
            return filters as TGlobalFilters;
        }

        protected async clearFilters(): Promise<void> {
        }

        private setDatePickerValidation(datePicker: kendo.ui.DatePicker): void {
            const datePickerInput = datePicker.element;
            datePickerInput.kendoValidator({
                rules: {
                    isValidDate: input => {
                        if (kendo.parseDate(input.val()) === null) {
                            return false;
                        } else {
                            return true;
                        }
                    }
                },
                messages: {
                    isValidDate: "Invalid date format"
                }
            });
        }
    }
}