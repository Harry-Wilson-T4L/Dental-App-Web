namespace DentalDrill.CRM.Controls.Reporting {
    export class ReportsPageDateRangeGlobalFilters extends ReportsPageGlobalFiltersBase {
        private readonly _from: Date;
        private readonly _to: Date;

        constructor(from: Date, to: Date) {
            super();
            this._from = from;
            this._to = to;
        }

        get from(): Date {
            return this._from;
        }

        get to(): Date {
            return this._to;
        }
    }
}