namespace DentalDrill.CRM.Controls.Reporting {
    export class ReportInsightsContainer {
        private readonly _root: JQuery<HTMLElement>;

        constructor(root: JQuery<HTMLElement>) {
            this._root = root;
        }

        clear(): void {
            this._root.empty();
        }

        async addFlatValuesAsync(builder: ReportInsightsFlatValuesBuilder): Promise<void> {
            const flatValues = $("<div></div>").addClass("row");
            this._root.append(flatValues);
            const flatContainer = new ReportInsightsFlatValuesContainer(flatValues);

            await builder(flatContainer);
        }

        async addChartsAsync(builder: ReportInsightsChartsBuilder): Promise<void> {
            const chartsRoot = $("<div></div>").addClass("row");
            this._root.append(chartsRoot);
            const chartsContainer = new ChartsCollectionContainer(chartsRoot);

            await builder(chartsContainer);
        }
    }

    export interface ReportInsightsFlatValuesBuilder {
        (flatValues: ReportInsightsFlatValuesContainer): Promise<void>;
    }

    export interface ReportInsightsChartsBuilder {
        (charts: ChartsCollectionContainer): Promise<void>;
    }

    export class ReportInsightsFlatValuesContainer {
        private readonly _root: JQuery<HTMLElement>;

        constructor(root: JQuery<HTMLElement>) {
            this._root = root;
        }

        addValue(key: string, value: string): void {
            const valueColValue = $("<div></div>")
                .addClass("reports__insights__total__value")
                .text(value);

            const valueColKey = $("<div></div>")
                .addClass("reports__insights__total__key")
                .text(key);

            const valueCol = $("<div></div>")
                .addClass("col")
                .addClass("reports__insights__total")
                .addClass("text-center")
                .append(valueColValue)
                .append(valueColKey);

            this._root.append(valueCol);
        }
    }
}