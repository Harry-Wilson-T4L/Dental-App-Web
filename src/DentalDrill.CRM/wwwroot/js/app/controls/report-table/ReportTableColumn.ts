namespace DentalDrill.CRM.Controls.Reporting {
    export class ReportTableColumn<TDataItem> {
        private readonly _title: string;
        private readonly _width: number;
        private _formatter: (item: TDataItem) => string;

        constructor(title: string, width: number, formatter: (item: TDataItem) => string) {
            this._title = title;
            this._width = width;
            this._formatter = formatter;
        }

        get title(): string {
            return this._title;
        }

        get width(): number {
            return this._width;
        }

        setFormatter(formatter: (item: TDataItem) => string) {
            this._formatter = formatter;
        }

        format(item: TDataItem): string {
            try {
                return this._formatter(item);
            }
            catch (exception) {
                return ``;
            }
        }
    }
}