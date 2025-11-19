namespace DentalDrill.CRM.Controls {
    export class DateRangeSelector {
        private readonly _root: JQuery;
        private readonly _targetFrom: JQuery;
        private readonly _targetTo: JQuery;
        private readonly _defaultFrom: Date;
        private readonly _defaultTo: Date;

        constructor(root: JQuery) {
            this._root = root;
            this._root.on("click", ".date-range-selector__item", e => {
                const target = $(e.target);
                const rangeFrom = this.parseRangeDate(target.attr("data-range-from"));
                const rangeTo = this.parseRangeDate(target.attr("data-range-to"));

                this.applyRange(rangeFrom, rangeTo);
            });

            this._targetFrom = $(root.attr("data-range-target-from"));
            this._targetTo = $(root.attr("data-range-target-to"));

            this._defaultFrom = this.parseRangeDate(root.attr("data-range-default-from"));
            this._defaultTo = this.parseRangeDate(root.attr("data-range-default-to"));

            this._root.data("DateRangeSelector", this);
        }

        reset(): void {
            this.applyRange(this._defaultFrom, this._defaultTo);
        }

        private applyRange(from: Date, to: Date): void {
            this._targetFrom.each((index, element) => {
                const datePicker = $(element).data("kendoDatePicker");
                datePicker.value(from);
            });

            this._targetTo.each((index, element) => {
                const datePicker = $(element).data("kendoDatePicker");
                datePicker.value(to);
            });
        }

        private parseRangeDate(dateString: string): Date {
            const match = /^(\d{4})-(\d{2})-(\d{2})$/.exec(dateString);
            if (match && match.length > 3) {
                return new Date(parseInt(match[1]), parseInt(match[2]) - 1, parseInt(match[3]));
            } else {
                throw new Error("Invalid date");
            }
        }
    }

    $(() => {
        $(".date-range-selector[data-init='true']").each((index, element) => {
            const selector = new DateRangeSelector($(element));
        });
    });
}