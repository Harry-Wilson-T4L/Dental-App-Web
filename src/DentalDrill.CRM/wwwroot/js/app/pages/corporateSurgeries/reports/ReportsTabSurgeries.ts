namespace DentalDrill.CRM.Pages.CorporateSurgeries.Reports {
    import ToggleButton = DentalDrill.CRM.Controls.ToggleButton;
    import ToggleList = DentalDrill.CRM.Controls.ToggleList;
    import PdfReportBuilder = DentalDrill.CRM.Controls.PdfReports.PdfReportBuilder;
    import ReportTable = DentalDrill.CRM.Controls.Reporting.ReportTable;
    import SurgeryReportFlowWriter = DentalDrill.CRM.Controls.Reporting.SurgeryReportFlowWriter;

    export class ReportsTabSurgeries extends ReportsTabBase {
        private readonly _page: ReportsPage;
        private readonly _root: JQuery;
        private readonly _container: JQuery;
        private readonly _insightsContainer: JQuery;

        private readonly _reportFields: ToggleList;
        private readonly _dateAggregate: kendo.ui.DropDownList;

        private readonly _insightsToggle: ToggleButton;

        private _dataSource: kendo.data.DataSource;
        private _dataSourceModels: kendo.data.DataSource;
        private _dataSourceBrands: kendo.data.DataSource;

        private _mainGrid: kendo.ui.Grid;

        private _filterDateFrom: Date;
        private _filterDateTo: Date;
        private _filterClients: string[];

        constructor(page: ReportsPage, root: JQuery) {
            super();
            this._page = page;
            this._root = root;
            this._container = root.find(".reports__surgeries__container");
            this._insightsContainer = root.find(".reports__surgeries__insights-container");

            this._reportFields = new ToggleList(root.find(".reports__surgeries__fields"));
            this._reportFields.selectionChanged.subscribe(async (sender, e) => {
                await this.initialize();
            });

            this._dateAggregate = root.find("input.reports__surgeries__date-aggregate").data("kendoDropDownList");
            this._dateAggregate.bind("change",
                async (e) => {
                    await this.initialize();
                });

            this._insightsToggle = new ToggleButton(root.find(".reports__surgeries__insights"));
            this._insightsToggle.changed.subscribe(async (sender, e) => {
                await this.toggleInsights();
            });

            root.find(".reports__surgeries__export__excel").click(async (e) => {
                await this.exportExcel();
            });

            root.find(".reports__surgeries__export__pdf").click(async e => {
                await this.exportPdf();
            });
        }

        private async exportExcel(): Promise<void> {
            routes.corporateSurgeries.exportSurgeriesToExcel(this._page.corporateUrlPath, this._filterDateFrom, this._filterDateTo, this._dateAggregate.value(), this._reportFields.selected.join(","), "").open();
        }

        private async exportPdf(): Promise<void> {
            const formatAsNumber = (x: any, decimalDigits: number) => {
                if (typeof x === "number") {
                    return x.toFixed(decimalDigits);
                }

                return ``;
            };

            const formatAsPercent = (x: any, decimalDigits: number) => {
                if (typeof x === "number") {
                    return `${(x * 100).toFixed(decimalDigits)}%`;
                }

                return ``;
            }

            const reportBuilder = new PdfReportBuilder();
            reportBuilder.fileName = `${this._page.corporateUrlPath}-BySurgery.pdf`;
            reportBuilder.open();

            const reportWriter = new SurgeryReportFlowWriter(reportBuilder);
            reportWriter.addHeading(1, "Handpieces by surgeries");
            if (this._insightsToggle.active) {
                const children = this._insightsContainer.children();
                reportWriter.addInsightsTotals(undefined, $(children[0]));
                reportWriter.addCharts($(children[1]).find(".k-chart").toArray().map((element, index) => $(element).data("kendoChart")));
            }

            const table = new ReportTable<ReportSurgeryBrandModelItem>();
            table.rowHeight = 23;
            table.addColumn("Surgery Name", 200, x => x.ClientName);
            if (this._dateAggregate.value() === "EntirePeriod") {
                if (this._reportFields.isToggled("RatingAverage")) {
                    table.addColumn("Avg. Rating", 50, x => formatAsNumber(x.RatingAverage, 2));
                }

                if (this._reportFields.isToggled("UnrepairedPercent")) {
                    table.addColumn("% Unrep.", 50, x => formatAsPercent(x.UnrepairedPercent, 2));
                }

                if (this._reportFields.isToggled("CostSum")) {
                    table.addColumn("Cost", 50, x => formatAsNumber(x.CostSum, 2));
                }

                if (this._reportFields.isToggled("CostAverage")) {
                    table.addColumn("Avg. Cost", 50, x => formatAsNumber(x.CostAverage, 2));
                }

                if (this._reportFields.isToggled("HandpiecesCount")) {
                    table.addColumn("# HPs", 50, x => formatAsNumber(x.HandpiecesCount, 0));
                }
            } else {
                const dateRanges = this.generateDateRange(this._filterDateFrom, this._filterDateTo, this._dateAggregate.value());
                if (this._reportFields.isToggled("RatingAverage")) {
                    for (let dateRange of dateRanges) {
                        table.addColumn(`${dateRange}`, 50, x => formatAsNumber(x.RatingAverage[dateRange], 2));
                    }

                    table.groupLastColumns("Avg. Rating", dateRanges.length);
                }

                if (this._reportFields.isToggled("UnrepairedPercent")) {
                    for (let dateRange of dateRanges) {
                        table.addColumn(`${dateRange}`, 50, x => formatAsPercent(x.UnrepairedPercent[dateRange], 2));
                    }

                    table.groupLastColumns("% Unrep.", dateRanges.length);
                }

                if (this._reportFields.isToggled("CostSum")) {
                    for (let dateRange of dateRanges) {
                        table.addColumn(`${dateRange}`, 50, x => formatAsNumber(x.CostSum[dateRange], 2));
                    }

                    table.groupLastColumns("Cost", dateRanges.length);
                }

                if (this._reportFields.isToggled("CostAverage")) {
                    for (let dateRange of dateRanges) {
                        table.addColumn(`${dateRange}`, 50, x => formatAsNumber(x.CostAverage[dateRange], 2));
                    }

                    table.groupLastColumns("Avg. Cost", dateRanges.length);
                }

                if (this._reportFields.isToggled("HandpiecesCount")) {
                    for (let dateRange of dateRanges) {
                        table.addColumn(`${dateRange}`, 50, x => formatAsNumber(x.HandpiecesCount[dateRange], 0));
                    }

                    table.groupLastColumns("# HPs", dateRanges.length);
                }
            }

            const detailRows = this._mainGrid.wrapper.find(".k-grid-content .k-detail-row:visible");
            if (detailRows.length > 0) {
                const firstRow = detailRows.eq(0);
                const tabStrip = firstRow.find(".k-tabstrip").data("kendoTabStrip");
                const index = tabStrip.select().index();
                if (index === 1) {
                    // Models
                    const dataSet: ReportSurgeryBrandModelItem[] = this._dataSource.data<ReportSurgeryItem>().map(x => x as ReportSurgeryBrandModelItem);
                    const models: ReportSurgeryBrandModelItem[] = this._dataSourceModels.data<ReportSurgeryBrandModelItem>().map(x => x as ReportSurgeryBrandModelItem);

                    table.columns[0].setFormatter(item => {
                        if (item.Model) {
                            return `${item.Brand} ${item.Model}`;
                        }

                        return item.ClientName;
                    });

                    table.setData(dataSet.concat(models), (left, right) => {
                        if (left.ClientName < right.ClientName) return -1;
                        if (left.ClientName > right.ClientName) return 1;
                        if (left.ClientId < right.ClientId) return -1;
                        if (left.ClientId > right.ClientId) return 1;
                        const leftBrandModel = left && left.Brand && left.Model ? `${left.Brand} ${left.Model}` : ``;
                        const rightBrandModel = right && right.Brand && right.Model ? `${right.Brand} ${right.Model}` : ``;
                        if (leftBrandModel < rightBrandModel) return -1;
                        if (leftBrandModel > rightBrandModel) return 1;
                        return 0;
                    });

                    table.dataRowFormatted.subscribe((sender, args) => {
                        if (!args.item.Model) {
                            args.row.classList.add("report-table__row--parent");
                        }
                    });
                } else {
                    // Brands
                    const dataSet: ReportSurgeryBrandModelItem[] = this._dataSource.data<ReportSurgeryItem>().map(x => x as ReportSurgeryBrandModelItem);
                    const brands: ReportSurgeryBrandModelItem[] = this._dataSourceBrands.data<ReportSurgeryBrandItem>().map(x => x as ReportSurgeryBrandModelItem);

                    table.columns[0].setFormatter(item => {
                        if (item.Brand) {
                            return item.Brand;
                        }

                        return item.ClientName;
                    });

                    table.setData(dataSet.concat(brands), (left, right) => {
                        if (left.ClientName < right.ClientName) return -1;
                        if (left.ClientName > right.ClientName) return 1;
                        if (left.ClientId < right.ClientId) return -1;
                        if (left.ClientId > right.ClientId) return 1;
                        const leftBrandModel = left && left.Brand ? `${left.Brand}` : ``;
                        const rightBrandModel = right && right.Brand ? `${right.Brand}` : ``;
                        if (leftBrandModel < rightBrandModel) return -1;
                        if (leftBrandModel > rightBrandModel) return 1;
                        return 0;
                    });

                    table.dataRowFormatted.subscribe((sender, args) => {
                        if (!args.item.Brand) {
                            args.row.classList.add("report-table__row--parent");
                        }
                    });
                }
            } else {
                const dataSet: ReportSurgeryBrandModelItem[] = this._dataSource.data<ReportSurgeryItem>().map(x => x as ReportSurgeryBrandModelItem);
                table.setData(dataSet, (left, right) => {
                    if (left.ClientName < right.ClientName) return -1;
                    if (left.ClientName > right.ClientName) return 1;
                    if (left.ClientId < right.ClientId) return -1;
                    if (left.ClientId > right.ClientId) return 1;
                    return 0;
                });
            }

            reportWriter.addTable(table);
        }

        async toggleInsights(): Promise<void> {
            this._insightsContainer.empty();
            if (!this._insightsToggle.active) {
                return;
            }

            const totalsWrapper = $("<div></div>").addClass("row");
            const chartsWrapper = $("<div></div>").addClass("row");

            const charts = new ChartsCollectionContainer(chartsWrapper);
            const self = this;

            /* General rendering */
            function addValue(key: string, value: string) {
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

                totalsWrapper.append(valueCol);
            }

            function renderNoRecords() {
                const noRecordsWrapper = $("<div></div>").addClass("row");
                const noRecordValue = $("<div></div>")
                    .addClass("reports__insights__total__value")
                    .addClass("col-12")
                    .text("No data available for selected date range and surgeries")
                    .appendTo(noRecordsWrapper);
                self._insightsContainer.append(noRecordsWrapper);
            }

            /* Flat Values */
            function renderFlatValueInsights(totalCount: number, totalRating: number, totalUnrepaired: number, totalCost: number) {
                if (self._reportFields.isToggled("RatingAverage")) {
                    addValue("Avg. rating", `${kendo.toString(totalRating / totalCount, "n2")}`);
                }

                if (self._reportFields.isToggled("UnrepairedPercent")) {
                    addValue("Unrepaired", `${kendo.toString(totalUnrepaired / totalCount, "p2")}`);
                }

                if (self._reportFields.isToggled("CostSum")) {
                    addValue("Total spent", `$${kendo.toString(totalCost, "n0")}`);
                }

                if (self._reportFields.isToggled("CostAverage")) {
                    addValue("Avg. spent", `$${kendo.toString(totalCost / totalCount, "n0")}`);
                }

                if (self._reportFields.isToggled("HandpiecesCount")) {
                    addValue("Handpieces", `${totalCount}`);
                }

                self._insightsContainer.append(totalsWrapper);
            }

            function renderAggregatedCharts() {
                self._insightsContainer.append(chartsWrapper);
                const aggregatedBarData = self.calculateAggregatedChartData();
                
                if (self._reportFields.isToggled("RatingAverage")) {
                    const barChartData = self.getAggregatedBarData(aggregatedBarData, "AverageRating");
                    charts.createLimitedBarChart()
                        .initialize(
                            "Average rating of handpieces by period",
                            barChartData,
                            "{0:n2}", 
                            "#= value ? value.toFixed(2) : '' #",
                            10);
                }

                if (self._reportFields.isToggled("UnrepairedPercent")) {
                    const barChartData = self.getAggregatedBarData(aggregatedBarData, "UnrepairedPercent");
                    charts.createLimitedBarChart()
                        .initialize(
                            "Average percent of unrepaired handpieces by period",
                            barChartData,
                            "{0:p2}",
                            "#= value ? value.toLocaleString('en', {style: 'percent'}) : '' #",
                            0.5);
                }

                if (self._reportFields.isToggled("CostSum")) {
                    const barChartData = self.getAggregatedBarData(aggregatedBarData, "CostSum");
                    charts.createBarChart()
                        .initialize(
                            "Sum of handpieces repair cost by period",
                            barChartData,
                            "{0:c0}",
                            "#= value ? value.toLocaleString('en-AU', { style: 'currency', currency: 'AUD', minimumFractionDigits: 0, maximumFractionDigits: 0 }) : '' #",
                            undefined);
                }

                if (self._reportFields.isToggled("CostAverage")) {
                    const barChartData = self.getAggregatedBarData(aggregatedBarData, "CostAverage");
                    charts.createBarChart()
                        .initialize(
                            "Average of handpieces repair cost by period",
                            barChartData as any,
                            "{0:c0}",
                            "#= value ? value.toLocaleString('en-AU', { style: 'currency', currency: 'AUD', minimumFractionDigits: 0, maximumFractionDigits: 0 }) : '' #",
                            undefined);
                }

                if (self._reportFields.isToggled("HandpiecesCount")) {
                    const barChartData = self.getAggregatedBarData(aggregatedBarData, "HandpiecesCount");
                    charts.createBarChart()
                        .initialize(
                            "Sum of handpieces repaired by period",
                            (barChartData) as any,
                            "{0:n0}",
                            "#= value ? value : '' #",
                            undefined);
                }
            }

            function renderEntirePeriodCharts() {
                self._insightsContainer.append(chartsWrapper);

                {
                    const pieChartData = self.getHandpieceNumberPieChartData();
                    charts.createPieChart()
                        .initialize(
                            "Brands by number of handpieces received",
                            pieChartData,
                            "{0:n0}",
                            "#= category #: \n #= value #",
                            undefined);
                }

                {
                    const pieChartData = self.getHandpieceCostPieChartData();
                    charts.createPieChart()
                        .initialize(
                            "Brands by cost of handpieces repaired",
                            pieChartData,
                            "{0:c0}",
                            "#= category #: \n #= value.toLocaleString('en-AU', { style: 'currency', currency: 'AUD', minimumFractionDigits: 0, maximumFractionDigits: 0 }) #",
                            undefined);
                }

                {
                    const mapChartData = self.calculateSurgeryChartData();
                    charts.createTreeMap()
                        .initialize(
                            "Surgeries map by cost of repairs",
                            mapChartData,
                            "{0:c0}",
                            undefined,
                            undefined);
                }
            }

            const data = this._dataSource.data<ReportSurgeryItem>();

            if (data.length === 0) {
                renderNoRecords();
                return;
            }

            let totalCount = 0;
            let totalRating = 0;
            let totalUnrepaired = 0;
            let totalCost = 0;

            if (this._dateAggregate.value() === "EntirePeriod") {
                for (let i = 0; i < data.length; i++) {
                    const item = data[i];
                    if (item.HandpiecesCount !== undefined && typeof item.HandpiecesCount === "number") {
                        totalCount += item.HandpiecesCount;

                        if (item.CostSum !== undefined && typeof item.CostSum === "number") {
                            totalCost += item.CostSum;
                        }

                        if (item.RatingAverage !== undefined && typeof item.RatingAverage === "number") {
                            totalRating += item.RatingAverage * item.HandpiecesCount;
                        }

                        if (item.UnrepairedPercent !== undefined && typeof item.UnrepairedPercent === "number") {
                            totalUnrepaired += item.UnrepairedPercent * item.HandpiecesCount;
                        }
                    }
                }
                renderFlatValueInsights(totalCount, totalRating, totalUnrepaired, totalCost);
                renderEntirePeriodCharts();
            } else {
                for (let i = 0; i < data.length; i++) {
                    const item = data[i];
                    if (item.HandpiecesCount !== undefined && typeof item.HandpiecesCount === "object") {
                        for (let key in item.HandpiecesCount) {
                            if (item.HandpiecesCount.hasOwnProperty(key) && typeof item.HandpiecesCount[key] === "number") {
                                totalCount += item.HandpiecesCount[key];

                                if (item.CostSum !== undefined && item.CostSum.hasOwnProperty(key) && typeof item.CostSum[key] === "number") {
                                    totalCost += item.CostSum[key];
                                }

                                if (item.RatingAverage !== undefined &&
                                    item.RatingAverage.hasOwnProperty(key) &&
                                    typeof item.RatingAverage[key] === "number") {
                                    totalRating += item.RatingAverage[key] * item.HandpiecesCount[key];
                                }

                                if (item.UnrepairedPercent !== undefined &&
                                    item.UnrepairedPercent.hasOwnProperty(key) &&
                                    typeof item.UnrepairedPercent[key] === "number") {
                                    totalUnrepaired += item.UnrepairedPercent[key] * item.HandpiecesCount[key];
                                }
                            }
                        }
                    }
                }
                renderFlatValueInsights(totalCount, totalRating, totalUnrepaired, totalCost);
                renderAggregatedCharts();
            }
        }

        private initializeDataSourceFields(fields: object): object {
            const dateAggregate = this._dateAggregate.value();
            const baseFields = ["RatingAverage", "CostSum", "CostAverage", "UnrepairedPercent", "HandpiecesCount"];
            if (dateAggregate === "EntirePeriod") {
                for (let field of baseFields) {
                    fields[field] = { type: "number", from: field };
                }
            } else {
                const dateRanges = this.generateDateRange(this._filterDateFrom, this._filterDateTo, dateAggregate);
                for (let field of baseFields) {
                    for (let range of dateRanges) {
                        fields[`${field}_${range.replace("-", "_")}`] = { type: "number", from: `${field}["${range}"]` };
                    }
                }
            }

            return fields;
        }

        private async initializeDataSources(): Promise<void> {
            this._dataSource = new kendo.data.DataSource({
                type: "aspnetmvc-ajax",
                transport: {
                    read: {
                        url: `/CorporateSurgeries/${this._page.corporateUrlPath}/Reports/Handpieces`,
                        data: {
                            From: this._filterDateFrom.toISOString(),
                            To: this._filterDateTo.toISOString(),
                            Clients: this._filterClients,
                            DateAggregate: this._dateAggregate.value()
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
                            ClientName: { type: "string" },
                        })
                    }
                }
            });

            await this._dataSource.read();

            this._dataSourceModels = new kendo.data.DataSource({
                type: "aspnetmvc-ajax",
                transport: {
                    read: {
                        url: `/CorporateSurgeries/${this._page.corporateUrlPath}/Reports/Handpieces/ByModel`,
                        data: {
                            From: this._filterDateFrom.toISOString(),
                            To: this._filterDateTo.toISOString(),
                            Clients: this._filterClients,
                            DateAggregate: this._dateAggregate.value()
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
                            ClientName: { type: "string" },
                            Brand: { type: "string" },
                            Model: { type: "string" }
                        })
                    }
                }
            });

            await this._dataSourceModels.read();

            this._dataSourceBrands = new kendo.data.DataSource({
                type: "aspnetmvc-ajax",
                transport: {
                    read: {
                        url: `/CorporateSurgeries/${this._page.corporateUrlPath}/Reports/Handpieces/ByBrands`,
                        data: {
                            From: this._filterDateFrom.toISOString(),
                            To: this._filterDateTo.toISOString(),
                            Clients: this._filterClients,
                            DateAggregate: this._dateAggregate.value()
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
                            ClientName: { type: "string" },
                            Brand: { type: "string" },
                            Model: { type: "string" }
                        })
                    }
                }
            });

            await this._dataSourceBrands.read();
        }

        private getFieldsCandidates(): { field: string, title: string, format: string }[] {
            return [
                {
                    field: "RatingAverage",
                    title: "Avg. Rating",
                    format: "{0:n}"
                },
                {
                    field: "UnrepairedPercent",
                    title: "% Unrep.",
                    format: "{0:p2}"
                },
                {
                    field: "CostSum",
                    title: "Cost",
                    format: "{0:c0}"
                },
                {
                    field: "CostAverage",
                    title: "Avg. Cost",
                    format: "{0:c0}"
                },
                {
                    field: "HandpiecesCount",
                    title: "# HPs",
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
                template: "#: data.ClientName #",
                title: "Surgery Name",
                width: "300px"
            });

            const selectedFields = this._reportFields.selected;
            const fieldsCandidates = this.getFieldsCandidates();
            const dateAggregate = this._dateAggregate.value();

            for (let candidate of fieldsCandidates.filter(x => selectedFields.indexOf(x.field) >= 0)) {
                if (dateAggregate === "EntirePeriod") {
                    columnsConfig.push({
                        field: candidate.field,
                        title: candidate.title,
                        width: "125px",
                        format: candidate.format,
                    });
                } else {
                    const dateRanges = this.generateDateRange(this._filterDateFrom, this._filterDateTo, dateAggregate);
                    const childColumns: kendo.ui.GridColumn[] = [];
                    for (let range of dateRanges) {
                        childColumns.push({
                            title: range,
                            field: `${candidate.field}_${range.replace("-", "_")}`,
                            width: "125px",
                            format: candidate.format,
                        });
                    }

                    columnsConfig.push({
                        title: candidate.title,
                        columns: childColumns
                    });
                }
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
                detailTemplate: (data) => {
                    let html = "";
                    html += `<div class="reports__surgeries__item__details">`;
                    html += `  <ul>`;
                    html += `    <li class="k-state-active">By Brands</li>`;
                    html += `    <li>By Models</li>`;
                    html += `  </ul>`;
                    html += `  <div>`;
                    html += `    <div class="reports__surgeries__item__details__brands-grid"></div>`;
                    html += `  </div>`;
                    html += `  <div>`;
                    html += `    <div class="reports__surgeries__item__details__models-grid"></div>`;
                    html += `  </div>`;
                    html += `</div>`;

                    return html;
                },
                noRecords: {
                    template: "No data available for selected date range and surgeries"
                },
                detailInit: (e: kendo.ui.GridDetailInitEvent) => {
                    const tabStripHost = e.detailCell.find(".reports__surgeries__item__details");
                    tabStripHost.kendoTabStrip();

                    const brandsGridHost = e.detailCell.find(".reports__surgeries__item__details__brands-grid");
                    const modelsGridHost = e.detailCell.find(".reports__surgeries__item__details__models-grid");

                    const clientId = e.data["ClientId"] as string;
                    this.createBrandsGrid(brandsGridHost, clientId);
                    this.createModelsGrid(modelsGridHost, clientId);
                }
            });

            this._mainGrid = wrapper.data("kendoGrid");
        }

        private createBrandsGrid(host: JQuery, clientId: string) {
            const subset: any[] = [];
            const superset = this._dataSourceBrands.data();

            for (let i = 0; i < superset.length; i++) {
                const item = superset[i];
                if (item["ClientId"] === clientId)
                    subset.push({
                        ClientId: item["ClientId"],
                        ClientName: item["ClientName"],
                        Brand: item["Brand"],
                        RatingAverage: item["RatingAverage"],
                        CostSum: item["CostSum"],
                        CostAverage: item["CostAverage"],
                        HandpiecesCount: item["HandpiecesCount"],
                        UnrepairedPercent: item["UnrepairedPercent"]
                    });
            }

            const brandsDataSource = new kendo.data.DataSource({
                data: subset,
                schema: {
                    model: {
                        fields: this.initializeDataSourceFields({
                            ClientId: { type: "string" },
                            ClientName: { type: "string" },
                            Brand: { type: "string" },
                        })
                    }
                }
            });

            const brandsColumnsConfig: kendo.ui.GridColumn[] = [];
            brandsColumnsConfig.push({
                field: "Brand",
                title: "Brand",
                width: "200px",
            });

            const selectedFields = this._reportFields.selected;
            const fieldsCandidates = this.getFieldsCandidates();
            const dateAggregate = this._dateAggregate.value();

            for (let candidate of fieldsCandidates.filter(x => selectedFields.indexOf(x.field) >= 0)) {
                if (dateAggregate === "EntirePeriod") {
                    brandsColumnsConfig.push({
                        field: candidate.field,
                        title: candidate.title,
                        width: "125px",
                        format: candidate.format,
                    });
                } else {
                    const dateRanges = this.generateDateRange(this._filterDateFrom, this._filterDateTo, dateAggregate);
                    const childColumns: kendo.ui.GridColumn[] = [];
                    for (let range of dateRanges) {
                        childColumns.push({
                            title: range,
                            field: `${candidate.field}_${range.replace("-", "_")}`,
                            width: "125px",
                            format: candidate.format,
                        });
                    }

                    brandsColumnsConfig.push({
                        title: candidate.title,
                        columns: childColumns
                    });
                }
            }

            brandsColumnsConfig.push({
                title: ""
            });

            host.kendoGrid({
                dataSource: brandsDataSource,
                columns: brandsColumnsConfig,
                scrollable: true,
                sortable: {
                    mode: "multiple",
                    allowUnsort: true,
                },
            });
        }

        private createModelsGrid(host: JQuery, clientId: string) {
            const subset: any[] = [];
            const superset = this._dataSourceModels.data();
            for (let i = 0; i < superset.length; i++) {
                const item = superset[i];
                if (item["ClientId"] === clientId)
                    subset.push({
                        ClientId: item["ClientId"],
                        ClientName: item["ClientName"],
                        Brand: item["Brand"],
                        Model: item["Model"],
                        RatingAverage: item["RatingAverage"],
                        CostSum: item["CostSum"],
                        CostAverage: item["CostAverage"],
                        HandpiecesCount: item["HandpiecesCount"],
                        UnrepairedPercent: item["UnrepairedPercent"]
                    });
            }

            const modelsDataSource = new kendo.data.DataSource({
                data: subset,
                schema: {
                    model: {
                        fields: this.initializeDataSourceFields({
                            ClientId: { type: "string" },
                            ClientName: { type: "string" },
                            Model: { type: "string" }
                        })
                    }
                }
            });

            const modelsColumnsConfig: kendo.ui.GridColumn[] = [];
            modelsColumnsConfig.push({
                field: "Model",
                template: "#: Brand # #: Model #",
                title: "Model",
                width: "200px"
            });

            const selectedFields = this._reportFields.selected;
            const fieldsCandidates = this.getFieldsCandidates();
            const dateAggregate = this._dateAggregate.value();

            for (let candidate of fieldsCandidates.filter(x => selectedFields.indexOf(x.field) >= 0)) {
                if (dateAggregate === "EntirePeriod") {
                    modelsColumnsConfig.push({
                        field: candidate.field,
                        title: candidate.title,
                        width: "125px",
                        format: candidate.format,
                    });
                } else {
                    const dateRanges = this.generateDateRange(this._filterDateFrom, this._filterDateTo, dateAggregate);
                    const childColumns: kendo.ui.GridColumn[] = [];
                    for (let range of dateRanges) {
                        childColumns.push({
                            title: range,
                            field: `${candidate.field}_${range.replace("-", "_")}`,
                            width: "125px",
                            format: candidate.format,
                        });
                    }

                    modelsColumnsConfig.push({
                        title: candidate.title,
                        columns: childColumns
                    });
                }
            }

            modelsColumnsConfig.push({
                title: ""
            });

            host.kendoGrid({
                dataSource: modelsDataSource,
                columns: modelsColumnsConfig,
                scrollable: true,
                sortable: {
                    mode: "multiple",
                    allowUnsort: true,
                },
            });
        }

        private getAggregatedBarData(data: { series: { } }, reportFieldName: string) {
            let response = new Array<{ name: string, value: number[] }>();

            for (let i in data.series[reportFieldName].data) {
                response.push({
                    name: data.series[reportFieldName].data[i].name,
                    value: data.series[reportFieldName].data[i].values,
                });
            }

            return response;
        }

        private getHandpieceCostPieChartData() {
            let response = new Array<{ category: string, value: number }>();
            let data = this.calculateHandpiecesChartData();

            for (let i in data) {
                response.push({
                    category: data[i].Brand,
                    value: data[i].TotalCost
                });
            }

            return response;
        }

        private getHandpieceNumberPieChartData() {
            let response = new Array<{ category: string, value: number }>();
            let data = this.calculateHandpiecesChartData();

            for (let i in data) {
                response.push({
                    category: data[i].Brand,
                    value: data[i].TotalNumber
                });
            }

            return response;
        }

        private calculateAggregatedChartData() {
            let response = {
                series:
                {
                    AverageRating: {
                        name: "Average Rating",
                        data: []
                    },
                    UnrepairedPercent: {
                        name: "Unrepaired Percent",
                        data: []
                    },
                    CostSum: {
                        name: "Cost Sum",
                        data: []
                    },
                    CostAverage: {
                        name: "Cost Average",
                        data: []
                    },
                    HandpiecesCount: {
                        name: "Handpieces Count",
                        data: []
                    }
                },
            }
            let data = this._dataSourceModels.data() as any;

            let dateAggregate = this._dateAggregate.value();
            let dateRanges = this.generateDateRange(this._filterDateFrom, this._filterDateTo, dateAggregate);

            for (let range of dateRanges) {
                let handpiecesCount = data.reduce((x, hbs) => x + hbs[`HandpiecesCount_${range.replace("-", "_")}`], 0);
                response.series['HandpiecesCount'].data.push({
                    name: range,
                    values: handpiecesCount
                });
                let averageRating =
                    data.reduce((x, hbs) => x + hbs[`RatingAverage_${range.replace("-", "_")}`] * hbs[`HandpiecesCount_${range.replace("-", "_")}`],
                            0) /
                        handpiecesCount;
                response.series['AverageRating'].data.push({
                    name: range,
                    values: averageRating
                });
                let unrepairedPercent =
                    data.reduce((x, hbs) => x +
                            hbs[`UnrepairedPercent_${range.replace("-", "_")}`] * hbs[`HandpiecesCount_${range.replace("-", "_")}`],
                            0) /
                        handpiecesCount;
                response.series['UnrepairedPercent'].data.push({
                    name: range,
                    values: unrepairedPercent
                });
                let costSum = data.reduce((x, hbs) => x + hbs[`CostSum_${range.replace("-", "_")}`], 0);
                response.series['CostSum'].data.push({
                    name: range,
                    values: costSum
                });
                let costAverage = costSum / handpiecesCount;
                response.series['CostAverage'].data.push({
                    name: range,
                    values: costAverage
                });

            }

            return response;
        }

        private calculateSurgeryChartData() {
            let response = new Array<{ name: string, value: number }>();
            let data = this._dataSourceModels.data() as any;

            let handpiecesBySurgery = data.reduce((hbs, d) => ({
                    ...hbs,
                    [d.ClientName]: [...(hbs[d.ClientName] || []), d],
                }),
                { });
            let surgeries = Object.keys(handpiecesBySurgery);

            for (let i in surgeries) {
                var value = handpiecesBySurgery[surgeries[i]].reduce((x, hbs) => x + hbs.CostSum, 0);
                response.push({
                    name: `${surgeries[i]} &#10; ${value.toLocaleString("en-AU",
                        { style: 'currency', currency: 'AUD', minimumFractionDigits: 0, maximumFractionDigits: 0 })}`,
                    value: value
                });
            }

            return response;
        }

        private calculateHandpiecesChartData() {
            let response = new Array<HandpieceBrandPieItem>();
            let data = this._dataSourceModels.data() as any;

            let handpiecesByBrand = data.reduce((hbb, d) => ({
                    ...hbb,
                    [d.Brand]: [...(hbb[d.Brand] || []), d],
                }),
                { });
            let brands = Object.keys(handpiecesByBrand);

            for (let i in brands) {
                response.push({
                    Brand: brands[i],
                    TotalNumber: handpiecesByBrand[brands[i]].reduce((x, hbb) => x + hbb.HandpiecesCount, 0),
                    TotalCost: handpiecesByBrand[brands[i]].reduce((x, hbb) => x + hbb.CostSum, 0)
                });
            }

            return response;
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
    }
}