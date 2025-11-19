namespace DentalDrill.CRM.Controls.Reporting {
    import PdfReportBuilder = DentalDrill.CRM.Controls.PdfReports.PdfReportBuilder;
    import PdfReportFlowWriter = DentalDrill.CRM.Controls.PdfReports.PdfReportFlowWriter;

    export class SurgeryReportFlowWriter extends PdfReportFlowWriter {
        constructor(builder: PdfReportBuilder) {
            super(builder);
        }

        addInsightsTotals(title: string, dataContainer: JQuery): void {
            const row1 = $(`<div class="row"></div>`);
            const totals = dataContainer.find(".reports__insights__total");
            totals.each((index, element) => {
                const key = $(element).find(".reports__insights__total__key").text();
                const value = $(element).find(".reports__insights__total__value").text();

                const item = $(`<div class="reports__insights__total col text-center"></div>`);
                const itemValue = $(`<div class="reports__insights__total__value"></div>`)
                    .text(value)
                    .appendTo(item);
                const itemKey = $(`<div class="reports__insights__total__key"></div>`)
                    .text(key)
                    .appendTo(item);

                row1.append(item);
            });

            if (title) {
                const row2 = $(`<div class="row"></div>`);
                const wrapper = $(`<div class="col-12 reports__insights__total__caption text-center font-italic mt-2"></div>`).appendTo(row2);

                this.addRow([row1, row2]);
            } else {
                this.addRow([row1]);
            }
        }

        addCharts(charts: kendo.dataviz.ui.Chart[]) {
            for (let i = 0; i < charts.length; i++) {
                const chart = charts[i];
                const chartOptions = JSON.parse(JSON.stringify(chart.options)) as kendo.dataviz.ui.ChartOptions;
                if (!chartOptions.chartArea) {
                    chartOptions.chartArea = { };
                }

                chartOptions.chartArea.width = 350;
                chartOptions.chartArea.height = 280;

                const chartWrapper = $(`<div class="chart chart__container col-8 offset-2"></div>`);
                const chartHost = $(`<div class="chart__host"></div>`).appendTo(chartWrapper);

                chartHost.kendoChart(chartOptions);

                this.addRow(chartWrapper);
            }
        }

        addTable<TDataItem>(table: ReportTable<TDataItem>) {
            const addRenderedTable = (rendered: HTMLTableElement, adjustments?: (rendered: HTMLTableElement) => void) => {
                rendered.classList.add("report-table");
                if (adjustments) {
                    adjustments(rendered);
                }

                const wrapped = $(rendered);
                this.addRow(wrapped);
            };

            const getAvailableNumberOfRows = (startFrom: number) => {
                const totalItems = table.items.length;
                if (startFrom >= totalItems) {
                    console.log(`Row from ${startFrom} count ${0}`);
                    return 0;
                }

                const remainingHeight = this.activePage.remainingHeight();
                let count = 1;
                while (startFrom + count < totalItems && table.measureHeight(startFrom, count + 1) <= remainingHeight) {
                    count++;
                }

                console.log(`Row from ${startFrom} count ${count}`);
                return count;
            }

            const pageWidth = 530;
            const columnSets: {
                start: number,
                count: number,
            }[] = [];

            let column = 0;
            while (column < table.columns.length) {
                let count = 1;
                while ((column + count) < table.columns.length && table.measureWidth(column, count) < pageWidth) {
                    if (table.measureWidth(column, count + 1) < pageWidth) {
                        count++;
                    } else {
                        break;
                    }
                }

                columnSets.push({ start: column, count: count });
                column = column + count;
            }

            let row = 0;
            if (this.activePage.usedHeight() === 0) { 
                // Current page is empty - nothing special, rendering first segment, and then breaking page and rendering next segments
                let rowCount = getAvailableNumberOfRows(row);
                for (let i = 0; i < columnSets.length; i++) {
                    addRenderedTable(table.renderFragment(columnSets[i].start, columnSets[i].count, row, rowCount));
                    this.nextPage();
                }

                row += rowCount;
                rowCount = getAvailableNumberOfRows(row);
                while (rowCount > 0) {
                    for (let i = 0; i < columnSets.length; i++) {
                        addRenderedTable(table.renderFragment(columnSets[i].start, columnSets[i].count, row, rowCount));
                        this.nextPage();
                    }

                    row += rowCount;
                    rowCount = getAvailableNumberOfRows(row);
                }

                this.popPage();
            } else if (this.activePage.remainingHeight() < table.rowHeight * 3) {
                // Not enough space on current page - starting new page, rendering first segment, and then breaking page and rendering next segments
                this.nextPage();
                let rowCount = getAvailableNumberOfRows(row);
                for (let i = 0; i < columnSets.length; i++) {
                    addRenderedTable(table.renderFragment(columnSets[i].start, columnSets[i].count, row, rowCount));
                    this.nextPage();
                }

                row += rowCount;
                rowCount = getAvailableNumberOfRows(row);
                while (rowCount > 0) {
                    for (let i = 0; i < columnSets.length; i++) {
                        addRenderedTable(table.renderFragment(columnSets[i].start, columnSets[i].count, row, rowCount));
                        this.nextPage();
                    }

                    row += rowCount;
                    rowCount = getAvailableNumberOfRows(row);
                }

                this.popPage();
            } else {
                // Current page is not empty - remembering remaining space, which would be added for every starting segment page
                const usedHeight = this.activePage.usedHeight();
                let rowCount = getAvailableNumberOfRows(row);

                for (let i = 0; i < columnSets.length; i++) {
                    const col = i;
                    addRenderedTable(table.renderFragment(columnSets[i].start, columnSets[i].count, row, rowCount), t => {
                        if (col > 0) {
                            t.style.marginTop = `${usedHeight}px`;
                        }
                    });
                    this.nextPage();
                }

                row += rowCount;
                rowCount = getAvailableNumberOfRows(row);
                while (rowCount > 0) {
                    for (let i = 0; i < columnSets.length; i++) {
                        addRenderedTable(table.renderFragment(columnSets[i].start, columnSets[i].count, row, rowCount));
                        this.nextPage();
                    }

                    row += rowCount;
                    rowCount = getAvailableNumberOfRows(row);
                }

                this.popPage();
            }
        }
    }
}