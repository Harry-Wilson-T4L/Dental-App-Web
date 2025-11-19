namespace DentalDrill.CRM.Pages.CorporateSurgeries.Reports {
    import ToggleButton = DentalDrill.CRM.Controls.ToggleButton;
    import PdfReportBuilder = DentalDrill.CRM.Controls.PdfReports.PdfReportBuilder;
    import ReportTable = DentalDrill.CRM.Controls.Reporting.ReportTable;
    import SurgeryReportFlowWriter = DentalDrill.CRM.Controls.Reporting.SurgeryReportFlowWriter;
    
    export class ReportsTabStatuses extends ReportsTabBase {
        private readonly _page: ReportsPage;
        private readonly _root: JQuery;
        private readonly _container: JQuery;
        private readonly _insightsContainer: JQuery;

        private readonly _insightsToggle: ToggleButton;

        private _dataSource: kendo.data.DataSource;

        private _filterDateFrom: Date;
        private _filterDateTo: Date;
        private _filterClients: string[];

        constructor(page: ReportsPage, root: JQuery) {
            super();
            this._page = page;
            this._root = root;

            this._container = root.find(".reports__statuses__container");
            this._insightsContainer = root.find(".reports__statuses__insights-container");

            this._insightsToggle = new ToggleButton(root.find(".reports__statuses__insights"));
            this._insightsToggle.changed.subscribe(async (sender, e) => {
                await this.toggleInsights();
            });

            root.find(".reports__statuses__export__excel").click(async e => {
                await this.exportExcel();
            });

            root.find(".reports__statuses__export__pdf").click(async e => {
                await this.exportPdf();
            });
        }

        private async exportExcel(): Promise<void> {
            routes.corporateSurgeries.exportStatusesToExcel(this._page.corporateUrlPath, this._filterDateFrom, this._filterDateTo, "").open();
        }

        private async exportPdf(): Promise<void> {
            const formatAsNumber = (x: any, decimalDigits: number) => {
                if (typeof x === "number") {
                    return x.toFixed(decimalDigits);
                }

                return ``;
            };

            const reportBuilder = new PdfReportBuilder();
            reportBuilder.fileName = `${this._page.corporateUrlPath}-ByStatus.pdf`;
            reportBuilder.open();

            const reportWriter = new SurgeryReportFlowWriter(reportBuilder);
            reportWriter.addHeading(1, "Handpieces by status");
            if (this._insightsToggle.active) {
                const children = this._insightsContainer.children();

                reportWriter.addInsightsTotals("Total Handpieces by statuses", $(children[0]));
                reportWriter.addCharts($(children[2]).find(".k-chart").toArray().map((element, index) => $(element).data("kendoChart")));
                reportWriter.addInsightsTotals("Average Handpieces by statuses", $(children[3]));

                reportWriter.nextPage();
            }

            const table = new ReportTable<ReportStatusItem>();
            table.rowHeight = 23;
            table.addColumn("Surgery", 200, x => x.ClientName);
            table.addColumn("Received", 50, x => formatAsNumber(x.StatusReceived, 0));
            table.addColumn("Being Est.", 50, x => formatAsNumber(x.StatusBeingEstimated, 0));
            table.addColumn("Waiting App.", 50, x => formatAsNumber(x.StatusWaitingForApproval, 0));
            table.addColumn("Est. Sent", 50, x => formatAsNumber(x.StatusEstimateSent, 0));
            table.addColumn("Being Rep.", 50, x => formatAsNumber(x.StatusBeingRepaired, 0));
            table.addColumn("Rdy. For Ret.", 50, x => formatAsNumber(x.StatusReadyToReturn, 0));
            table.addColumn("Unrepaired", 50, x => formatAsNumber(x.StatusCancelled, 0));
            table.addColumn("Complete", 50, x => formatAsNumber(x.StatusSentComplete, 0));
            table.setData(this._dataSource.data<ReportStatusItem>().map(x => x), (left, right) => {
                if (left.ClientName < right.ClientName) return -1;
                if (left.ClientName > right.ClientName) return 1;
                if (left.ClientId < right.ClientId) return -1;
                if (left.ClientId > right.ClientId) return 1;
                return 0;
            });

            reportWriter.addTable(table);
        }

        private initializeDataSourceFields(fields: object): object {
            const baseFields = [
                "StatusReceived", "StatusBeingEstimated", "StatusWaitingForApproval",
                "StatusEstimateSent", "StatusBeingRepaired", "StatusReadyToReturn",
                "StatusSentComplete", "StatusCancelled", "StatusAny"
            ];

            for (let field of baseFields) {
                fields[field] = { type: "number", from: field };
            }

            return fields;
        }

        private async initializeDataSources(): Promise<void> {
            this._dataSource = new kendo.data.DataSource({
                type: "aspnetmvc-ajax",
                transport: {
                    read: {
                        url: `/CorporateSurgeries/${this._page.corporateUrlPath}/Reports/Statuses`,
                        data: {
                            From: this._filterDateFrom.toISOString(),
                            To: this._filterDateTo.toISOString(),
                            Clients: this._filterClients,
                        }
                    }
                },
                schema: {
                    data: "Data",
                    total: "Total",
                    errors: "Errors",
                    model: {
                        fields: this.initializeDataSourceFields({
                            ClientId: { type: "string" },
                            ClientName: { type: "string" }
                        })
                    }
                }
            });

            await this._dataSource.read();
        }

        private getFieldsCandidates(): { field: string, title: string, format: string }[] {
            return [
                {
                    field: "StatusReceived",
                    title: "Received",
                    format: "{0:n0}"
                },
                {
                    field: "StatusBeingEstimated",
                    title: "Being Est.",
                    format: "{0:n0}"
                },
                {
                    field: "StatusWaitingForApproval",
                    title: "Waiting App.",
                    format: "{0:n0}"
                },
                {
                    field: "StatusEstimateSent",
                    title: "Est. Sent",
                    format: "{0:n0}"
                },
                {
                    field: "StatusBeingRepaired",
                    title: "Being Rep.",
                    format: "{0:n0}"
                },
                {
                    field: "StatusReadyToReturn",
                    title: "Rdy. For Ret.",
                    format: "{0:n0}"
                },
                {
                    field: "StatusCancelled",
                    title: "Unrepaired",
                    format: "{0:n0}"
                },
                {
                    field: "StatusSentComplete",
                    title: "Complete",
                    format: "{0:n0}"
                }
            ];
        }

        private createMainGrid(): void {
            const wrapper = $("<div></div>");
            this._container.empty();
            this._container.append(wrapper);

            const columnsConfig: kendo.ui.GridColumn[] = [];
            columnsConfig.push({
                field: "ClientId",
                template: "#: ClientName #",
                title: "Surgery",
                width: "300px"
            });

            const fieldsCandidates = this.getFieldsCandidates();
            for (let candidate of fieldsCandidates) {
                columnsConfig.push({
                    field: candidate.field,
                    title: candidate.title,
                    width: "90px",
                    format: candidate.format,
                });
            }

            columnsConfig.push({
                title: ""
            });

            wrapper.kendoGrid({
                dataSource: this._dataSource,
                columns: columnsConfig,
                scrollable: true,
                sortable: {
                    mode: "multiple",
                    allowUnsort: true,
                },
                noRecords: {
                    template: "No data available for selected date range and surgeries"
                }
            });
        }

        async initialize(): Promise<void> {
            await this.initializeDataSources();
            this.createMainGrid();
            await this.toggleInsights();
        }

        async applyGlobalFilters(from: Date, to: Date, clients: string[]): Promise<void> {
            this._filterDateFrom = from;
            this._filterDateTo = to;
            this._filterClients = clients;
        }

        private async toggleInsights(): Promise<void> {
            this._insightsContainer.empty();
            if (!this._insightsToggle.active) {
                return;
            }

            const data = this._dataSource.data<ReportStatusItem>();
            if (data.length === 0) {
                this._insightsContainer.append(
                    $("<div></div>").addClass("row").append(
                        $("<div></div>")
                        .addClass("reports__insights__total__value")
                        .addClass("col-12")
                        .text("No data available for selected date range and surgeries")));
                return;
            }

            const charts = new ChartsCollectionContainer($("<div></div>").addClass("row"));
            const totalsSum = new TotalsCollectionContainer($("<div></div>").addClass("row"));
            const totalsAvg = new TotalsCollectionContainer($("<div></div>").addClass("row"));
            const self = this;

            const aggregate = {
                count: 0,
                sum: {
                    StatusReceived: 0,
                    StatusBeingEstimated: 0,
                    StatusWaitingForApproval: 0,
                    StatusEstimateSent: 0,
                    StatusBeingRepaired: 0,
                    StatusReadyToReturn: 0,
                    StatusSentComplete: 0,
                    StatusCancelled: 0,
                    StatusAny: 0
                }
            }

            for (let i = 0; i < data.length; i++) {
                const item = data[i];
                aggregate.count++;
                aggregate.sum.StatusReceived += item.StatusReceived || 0;
                aggregate.sum.StatusBeingEstimated += item.StatusBeingEstimated || 0;
                aggregate.sum.StatusWaitingForApproval += item.StatusWaitingForApproval || 0;
                aggregate.sum.StatusEstimateSent += item.StatusEstimateSent || 0;
                aggregate.sum.StatusBeingRepaired += item.StatusBeingRepaired || 0;
                aggregate.sum.StatusReadyToReturn += item.StatusReadyToReturn || 0;
                aggregate.sum.StatusSentComplete += item.StatusSentComplete || 0;
                aggregate.sum.StatusCancelled += item.StatusCancelled || 0;
                aggregate.sum.StatusAny += item.StatusAny || 0;
            }

            totalsSum.addValue("Received", kendo.format("{0:n0}", aggregate.sum.StatusReceived));
            totalsSum.addValue("Being Estimated", kendo.format("{0:n0}", aggregate.sum.StatusBeingEstimated));
            totalsSum.addValue("Waiting For App.", kendo.format("{0:n0}", aggregate.sum.StatusWaitingForApproval));
            totalsSum.addValue("Estimate Sent", kendo.format("{0:n0}", aggregate.sum.StatusEstimateSent));
            totalsSum.addValue("Being Repaired", kendo.format("{0:n0}", aggregate.sum.StatusBeingRepaired));
            totalsSum.addValue("Ready to Return", kendo.format("{0:n0}", aggregate.sum.StatusReadyToReturn));
            totalsSum.addValue("Unrepaired", kendo.format("{0:n0}", aggregate.sum.StatusCancelled));
            totalsSum.addValue("Sent Complete", kendo.format("{0:n0}", aggregate.sum.StatusSentComplete));

            totalsAvg.addValue("Received", kendo.format("{0:0.##}", aggregate.sum.StatusReceived / aggregate.count));
            totalsAvg.addValue("Being Estimated", kendo.format("{0:0.##}", aggregate.sum.StatusBeingEstimated / aggregate.count));
            totalsAvg.addValue("Waiting For App.", kendo.format("{0:0.##}", aggregate.sum.StatusWaitingForApproval / aggregate.count));
            totalsAvg.addValue("Estimate Sent", kendo.format("{0:0.##}", aggregate.sum.StatusEstimateSent / aggregate.count));
            totalsAvg.addValue("Being Repaired", kendo.format("{0:0.##}", aggregate.sum.StatusBeingRepaired / aggregate.count));
            totalsAvg.addValue("Ready to Return", kendo.format("{0:0.##}", aggregate.sum.StatusReadyToReturn / aggregate.count));
            totalsAvg.addValue("Unrepaired", kendo.format("{0:0.##}", aggregate.sum.StatusCancelled / aggregate.count));
            totalsAvg.addValue("Sent Complete", kendo.format("{0:0.##}", aggregate.sum.StatusSentComplete / aggregate.count));

            this._insightsContainer.append(totalsSum.container);
            this._insightsContainer.append(
                $("<div></div>").addClass("row").append(
                    $("<div></div>").addClass("col-12 reports__insights__total__caption text-center font-italic mt-2").text(
                        "Total Handpieces by statuses")));
            this._insightsContainer.append(charts.container);
            this._insightsContainer.append(totalsAvg.container);
            this._insightsContainer.append(
                $("<div></div>").addClass("row").append(
                    $("<div></div>").addClass("col-12 reports__insights__total__caption text-center font-italic mt-2").text(
                        "Average Handpieces by statuses")));

            const sumChartData = [
                { name: "Received", value: aggregate.sum.StatusReceived },
                { name: "Being Estimated", value: aggregate.sum.StatusBeingEstimated },
                { name: "Waiting For App.", value: aggregate.sum.StatusWaitingForApproval },
                { name: "Estimate Sent", value: aggregate.sum.StatusEstimateSent },
                { name: "Being Repaired", value: aggregate.sum.StatusBeingRepaired },
                { name: "Ready to Return", value: aggregate.sum.StatusReadyToReturn },
                { name: "Unrepaired", value: aggregate.sum.StatusCancelled },
                { name: "Sent Complete", value: aggregate.sum.StatusSentComplete }
            ];

            charts.createWaterfallChart().initialize("Total Handpieces", sumChartData, "{0:n0}", "#: value #", undefined);

            const averageChartData = [
                { name: "Received", value: aggregate.sum.StatusReceived / aggregate.count },
                { name: "Being Estimated", value: aggregate.sum.StatusBeingEstimated / aggregate.count },
                { name: "Waiting For App.", value: aggregate.sum.StatusWaitingForApproval / aggregate.count },
                { name: "Estimate Sent", value: aggregate.sum.StatusEstimateSent / aggregate.count },
                { name: "Being Repaired", value: aggregate.sum.StatusBeingRepaired / aggregate.count },
                { name: "Ready to Return", value: aggregate.sum.StatusReadyToReturn / aggregate.count },
                { name: "Unrepaired", value: aggregate.sum.StatusCancelled / aggregate.count },
                { name: "Sent Complete", value: aggregate.sum.StatusSentComplete / aggregate.count }
            ];

            charts.createWaterfallChart().initialize("Average Handpieces", averageChartData, "{0:0.##}", "#: kendo.format('{0:0.\\#\\#}', value) #", undefined);
        }
    }
}