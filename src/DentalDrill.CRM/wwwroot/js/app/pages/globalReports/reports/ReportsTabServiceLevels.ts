namespace DentalDrill.CRM.Pages.GlobalReports.Reports {
    import Collection = DevGuild.Utilities.Collection;
    import PdfReportBuilder = DentalDrill.CRM.Controls.PdfReports.PdfReportBuilder;
    import ReportTable = DentalDrill.CRM.Controls.Reporting.ReportTable;
    import SurgeryReportFlowWriter = DentalDrill.CRM.Controls.Reporting.SurgeryReportFlowWriter;
    import ReportsPageTabGridWithInsightsBase = CRM.Controls.Reporting.ReportsPageTabGridWithInsightsBase;
    import ReportsPageDateRangeGlobalFilters = CRM.Controls.Reporting.ReportsPageDateRangeGlobalFilters;
    import ReportsPageDataSourceGroupHelper = CRM.Controls.Reporting.ReportsPageDataSourceGroupHelper;
    import ReportsPageGridColumn = CRM.Controls.Reporting.ReportsPageGridColumn;
    import ReportsPageGridColumns = CRM.Controls.Reporting.ReportsPageGridColumns;

    interface ReportItem {
        ClientId: string;
        ClientName: string;
        ServiceLevelName: string;
        Brand: string;
        Model: string;
        RepairedByName: string;
        HandpieceStatus: number;
        RatingAverage: number | object;
        CostSum: number | object;
        CostAverage: number | object;
        UnrepairedPercent: number | object;
        ReturnUnrepairedPercent: number | object;
        HandpiecesCount: number | object;
        TurnaroundAverage: number | object;
        WarrantyCount: number | object;
    }

    export class ReportsTabServiceLevels extends ReportsPageTabGridWithInsightsBase<GlobalReportsPageIdentifier, ReportsPageDateRangeGlobalFilters> {
        async initializeInsights(): Promise<void> {
            const data = this.dataSource.data<ReportItem>();
            if (!data || data.length === 0) {
                return;
            }

            // Building flat insights
            let totalCount = 0;
            let totalCost = 0;
            let totalUnrepairable = 0;
            let totalReturnUnrepaired = 0;
            let totalTurnaround = 0;
            let totalWarranty = 0;
            if (this.dateAggregate === "EntirePeriod") {
                for (let i = 0; i < data.length; i++) {
                    const item = data[i];
                    if (item && item.HandpiecesCount !== undefined && item.HandpiecesCount !== null && typeof item.HandpiecesCount === "number") {
                        totalCount += item.HandpiecesCount;
                        if (item.CostSum && typeof item.CostSum === "number") totalCost += item.CostSum;
                        if (item.UnrepairedPercent && typeof item.UnrepairedPercent === "number") totalUnrepairable += item.UnrepairedPercent * item.HandpiecesCount;
                        if (item.ReturnUnrepairedPercent && typeof item.ReturnUnrepairedPercent === "number") totalReturnUnrepaired += item.ReturnUnrepairedPercent * item.HandpiecesCount;
                        if (item.TurnaroundAverage && typeof item.TurnaroundAverage === "number") totalTurnaround += item.TurnaroundAverage * item.HandpiecesCount;
                        if (item.WarrantyCount && typeof item.WarrantyCount === "number") totalWarranty += item.WarrantyCount;
                    }
                }

            } else {
                const dateRanges = this.generateDateRange(this.globalFilters.from, this.globalFilters.to, this.dateAggregate);
                for (let i = 0; i < data.length; i++) {
                    const item = data[i];
                    for (let dateRange of dateRanges) {
                        if (item && item.HandpiecesCount[dateRange] !== undefined && item.HandpiecesCount[dateRange] !== null && typeof item.HandpiecesCount[dateRange] === "number") {
                            totalCount += item.HandpiecesCount[dateRange];
                            if (item.CostSum[dateRange] && typeof item.CostSum[dateRange] === "number") totalCost += item.CostSum[dateRange];
                            if (item.UnrepairedPercent[dateRange] && typeof item.UnrepairedPercent[dateRange] === "number") totalUnrepairable += item.UnrepairedPercent[dateRange] * item.HandpiecesCount[dateRange];
                            if (item.ReturnUnrepairedPercent[dateRange] && typeof item.ReturnUnrepairedPercent[dateRange] === "number") totalReturnUnrepaired += item.ReturnUnrepairedPercent[dateRange] * item.HandpiecesCount[dateRange];
                            if (item.TurnaroundAverage[dateRange] && typeof item.TurnaroundAverage[dateRange] === "number") totalTurnaround += item.TurnaroundAverage[dateRange] * item.HandpiecesCount[dateRange];
                            if (item.WarrantyCount[dateRange] && typeof item.WarrantyCount[dateRange] === "number") totalWarranty += item.WarrantyCount[dateRange];
                        }
                    }
                }
            }

            if (totalCount === 0) {
                return;
            }
            
            await this.insightsContainer.addFlatValuesAsync(async flat => {
                if (this.reportFields.isToggled("HandpiecesCount")) flat.addValue("Repairs", `${kendo.toString(totalCount, "n0")}`);
                if (this.reportFields.isToggled("UnrepairedPercent")) flat.addValue("Unrepairable", `${kendo.toString(totalUnrepairable / totalCount, "p1")}`);
                if (this.reportFields.isToggled("ReturnUnrepairedPercent")) flat.addValue("Ret. unrep", `${kendo.toString(totalReturnUnrepaired / totalCount, "p1")}`);
                if (this.reportFields.isToggled("TurnaroundAverage")) flat.addValue("Avg. turnaround", `${kendo.toString(totalTurnaround / totalCount, "n1")}d`);
                if (this.reportFields.isToggled("WarrantyCount")) flat.addValue("Warranty jobs", `${kendo.toString(totalWarranty, "n0")}`);
                if (this.reportFields.isToggled("CostSum")) flat.addValue("Total cost", `$${kendo.toString(totalCost, "n0")}`);
                if (this.reportFields.isToggled("CostAverage")) flat.addValue("Average cost", `$${kendo.toString(totalCost / totalCount, "n0")}`);
            });

            await this.insightsContainer.addChartsAsync(async charts => {
                const serviceLevelDataSource = await this.createDataSource({ transport: { read: { data: { DateAggregate: "EntirePeriod", Fields: ["ServiceLevel"] } } } });
                {
                    const collection = new Collection<ReportItem>(serviceLevelDataSource.data<ReportItem>().map(x => x));
                    charts.createPieChart()
                        .override(options => {
                            options.seriesDefaults.labels.visible = false;
                            options.tooltip.template = `#: category #: #: kendo.toString(value, 'n0') #`;
                        })
                        .initialize(
                            "Repairs",
                            collection.groupBy(x => x.ServiceLevelName).select(x => ({ category: x.key, value: x.items.count() })).toArray(),
                            "{0:n0}",
                            "#: category #: \n #: kendo.toString(value, 'n0') #",
                            undefined);
                }
                {
                    const collection = new Collection<ReportItem>(serviceLevelDataSource.data<ReportItem>().map(x => x));
                    charts.createPieChart()
                        .override(options => {
                            options.seriesDefaults.labels.visible = false;
                            options.tooltip.template = `#: category #: $#: kendo.toString(value, 'n0') #`;
                        })
                        .initialize(
                            "Cost",
                            collection.groupBy(x => x.ServiceLevelName).select(x => ({ category: x.key, value: x.items.sum((y, i) => y.CostSum as number) })).toArray(),
                            "{0:n2}",
                            "#: category #: \n $#: kendo.toString(value, 'n0') #",
                            undefined);
                }
            });
        }

        async initializeDataSourceTransportRead(): Promise<kendo.data.DataSourceTransportRead> {
            return {
                url: `/Reports/ReadReportHandpieces`,
                data: {
                    From: this.globalFilters.from.toISOString(),
                    To: this.globalFilters.to.toISOString(),
                    DateAggregate: this.dateAggregate,
                    Fields: ["ServiceLevel", "Brand"]
                }
            }
        }

        initializeDataSourceValueFields(): string[] {
            return [
                "HandpiecesCount",
                "UnrepairedPercent",
                "ReturnUnrepairedPercent",
                "TurnaroundAverage",
                "WarrantyCount",
                "CostSum",
                "CostAverage"
            ];
        }

        initializeDataSourceGroupFields(): object {
            return {
                Brand: { type: "string" },
                ServiceLevelName: { type: "string" },
            };
        }

        initializeGridValueColumns(): ReportsPageGridColumn[] {
            return [
                ReportsPageGridColumns.handpieces.handpiecesCount(),
                ReportsPageGridColumns.handpieces.unrepairablePercent(),
                ReportsPageGridColumns.handpieces.returnUnrepairedPercent(),
                ReportsPageGridColumns.handpieces.turnaroundAverage(),
                ReportsPageGridColumns.handpieces.warrantyCount(),
                ReportsPageGridColumns.handpieces.costSum(),
                ReportsPageGridColumns.handpieces.costAverage()
            ];
        }

        initializeGridGroupColumns(): kendo.ui.GridColumn[] {
            const columnsConfig: kendo.ui.GridColumn[] = [];
            columnsConfig.push({
                field: "ServiceLevelName",
                title: "Service Level",
                width: "200px",
                hidden: true,
            });
            columnsConfig.push({
                field: "Brand",
                title: "Brand",
                width: "300px"
            });
            return columnsConfig;
        }

        alterDataSourceOptions(dataSourceOptions: kendo.data.DataSourceOptions): void {
            dataSourceOptions.group = [
                { field: "ServiceLevelName" }
            ];
        }

        alterGridOptions(gridOptions: kendo.ui.GridOptions): void {
            gridOptions.dataBound = (e: kendo.ui.GridDataBoundEvent) => {
                e.sender.wrapper.find(".k-grouping-row").each((index, row) => {
                    e.sender.collapseGroup(row);
                });
            };
        }
    }
}