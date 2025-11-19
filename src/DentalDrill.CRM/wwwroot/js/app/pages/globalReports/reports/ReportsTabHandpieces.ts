namespace DentalDrill.CRM.Pages.GlobalReports.Reports {
    import PdfReportBuilder = DentalDrill.CRM.Controls.PdfReports.PdfReportBuilder;
    import ReportTable = DentalDrill.CRM.Controls.Reporting.ReportTable;
    import SurgeryReportFlowWriter = DentalDrill.CRM.Controls.Reporting.SurgeryReportFlowWriter;
    import ReportsPageTabGridWithInsightsBase = CRM.Controls.Reporting.ReportsPageTabGridWithInsightsBase;
    import ReportsPageDateRangeGlobalFilters = CRM.Controls.Reporting.ReportsPageDateRangeGlobalFilters;
    import ReportsPageDataSourceGroupHelper = CRM.Controls.Reporting.ReportsPageDataSourceGroupHelper;
    import ReportsPageGridColumn = CRM.Controls.Reporting.ReportsPageGridColumn;
    import ReportsPageGridColumns = CRM.Controls.Reporting.ReportsPageGridColumns;

    export class ReportsTabHandpieces extends ReportsPageTabGridWithInsightsBase<GlobalReportsPageIdentifier, ReportsPageDateRangeGlobalFilters> {
        async initializeDataSourceTransportRead(): Promise<kendo.data.DataSourceTransportRead> {
            return {
                url: `/Reports/ReadReportHandpieces`,
                data: {
                    From: this.globalFilters.from.toISOString(),
                    To: this.globalFilters.to.toISOString(),
                    DateAggregate: this.dateAggregate,
                    Fields: ["Client", "ServiceLevel", "Brand", "Model", "RepairedBy"]
                }
            }
        }

        initializeDataSourceValueFields(): string[] {
            return [
                "HandpiecesCount",
                "RatingAverage",
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
                ClientId: { type: "string" },
                ClientName: { type: "string" },
                ServiceLevelName: { type: "string" },
                Brand: { type: "string" },
                Model: { type: "string" },
                RepairedByName: { type: "string" },
            };
        }

        initializeGridValueColumns(): ReportsPageGridColumn[] {
            return [
                ReportsPageGridColumns.handpieces.handpiecesCount(),
                ReportsPageGridColumns.handpieces.ratingAverage(),
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
                field: "ClientId",
                template: "#: data.ClientName #",
                groupHeaderTemplate: (group: kendo.data.DataSourceGroup) => {
                    const first = ReportsPageDataSourceGroupHelper.getFirstItemFromGroup(group, "ClientName");
                    return first ? first.ClientName : "";
                },
                title: "Surgery Name",
                width: "200px"
            });
            columnsConfig.push({
                field: "ServiceLevelName",
                title: "Service Level",
                width: "100px",
            });
            columnsConfig.push({
                field: "Brand",
                title: "Brand",
                width: "100px",
            });
            columnsConfig.push({
                field: "Model",
                title: "Model",
                width: "100px",
            });
            columnsConfig.push({
                field: "RepairedByName",
                title: "Repaired By",
                width: "100px",
            });
            return columnsConfig;
        }

        alterGridOptions(gridOptions: kendo.ui.GridOptions): void {
            gridOptions.groupable = true;
        }
    }
}