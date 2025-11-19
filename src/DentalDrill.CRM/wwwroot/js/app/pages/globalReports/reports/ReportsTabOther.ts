namespace DentalDrill.CRM.Pages.GlobalReports.Reports {
    import ReportsPageTabGridCollectionBase = CRM.Controls.Reporting.ReportsPageTabGridCollectionBase;
    import ReportsPageDateRangeGlobalFilters = CRM.Controls.Reporting.ReportsPageDateRangeGlobalFilters;

    export class ReportsTabOther extends ReportsPageTabGridCollectionBase<GlobalReportsPageIdentifier, ReportsPageDateRangeGlobalFilters> {
        constructor(pageIdentifier: GlobalReportsPageIdentifier, tabRoot: JQuery<HTMLElement>) {
            super(pageIdentifier, tabRoot);
            this.registerGridDefinition("TechWarranty", this.renderTechWarrantyGrid.bind(this));
            this.registerGridDefinition("BatchReturns", this.renderBatchReturnsGrid.bind(this));
        }

        private async renderTechWarrantyGrid(wrapper: JQuery<HTMLElement>): Promise<kendo.ui.Grid> {
            wrapper.css("height", "500px");
            const dataSource = new kendo.data.DataSource({
                type: "aspnetmvc-ajax",
                transport: {
                    read: {
                        url: `/Reports/ReadReportTechWarranty`,
                        data: {
                            From: this.globalFilters.from.toISOString(),
                            To: this.globalFilters.to.toISOString(),
                        }
                    }
                },
                schema: {
                    data: "Data",
                    total: "Total",
                    errors: "Errors",
                    model: {
                        HandpieceId: { type: "string" },
                        JobId: { type: "string" },
                        JobNumber: { type: "number" },
                        JobReceived: { type: "date" },
                        RepairedById: { type: "string" },
                        RepairedByName: { type: "string" },
                        Brand: { type: "string" },
                        Model: { type: "string" },
                        Serial: { type: "string" },
                        HandpieceCount: { type: "number" },
                        Warranty: { type: "number" },
                        DaysPassed: { type: "number" },
                    }
                },
                group: [
                    {
                        field: "RepairedByName",
                        aggregates: [
                            { field: "HandpieceCount", aggregate: "sum" },
                            { field: "Warranty", aggregate: "sum" },
                            { field: "DaysPassed", aggregate: "average" },
                        ]
                    }
                ],
                sort: [
                    { field: "JobNumber", dir: "asc" },
                ],
            });

            wrapper.kendoGrid({
                dataSource: dataSource,
                columns: [
                    {
                        field: "RepairedByName",
                        title: "Technician",
                        width: "100px",
                        hidden: true
                    },
                    {
                        field: "JobNumber",
                        title: "Technician / Job",
                        width: "400px",
                        template: "Estimate \\##:JobNumber# | #:Brand# #:Model# | #:Serial#",
                    },
                    {
                        field: "HandpieceCount",
                        title: "Repairs",
                        width: "100px",
                        groupHeaderColumnTemplate: `#:sum#`,
                    },
                    {
                        field: "Warranty",
                        title: "Came back on warranty",
                        width: "200px",
                        groupHeaderColumnTemplate: `#:sum#`,
                    }, 
                    {
                        field: "DaysPassed",
                        title: "Last seen",
                        width: "100px",
                        groupHeaderColumnTemplate: `#:average ? kendo.toString(average, "n2") : ""#`,
                    },
                    {
                        title: "",
                        command: [
                            {
                                name: "CustomDetails",
                                click: function(this: kendo.ui.Grid, e: JQueryEventObject) {
                                    e.preventDefault();
                                    const row = $(e.target).closest("tr") as JQuery<HTMLElement>;
                                    const item = this.dataItem(row) as any;
                                    routes.jobs.details(item.JobId).open();
                                },
                                text: `<span class="fas fa-fw fa-link"></span> Estimate`,
                            }
                        ],
                        width: "150px",
                    }
                ],
                scrollable: true,
                sortable: {
                    mode: "multiple",
                    allowUnsort: true,
                },
                noRecords: {
                    template: "No data available",
                },
                dataBound: (e: kendo.ui.GridDataBoundEvent) => {
                    e.sender.wrapper.find(".k-grouping-row").each((index, row) => {
                        e.sender.collapseGroup(row);
                    });
                }
            });

            return wrapper.data("kendoGrid");
        }

        private async renderBatchReturnsGrid(wrapper: JQuery<HTMLElement>): Promise<kendo.ui.Grid> {
            wrapper.css("height", "500px");
            const dataSource = new kendo.data.DataSource({
                type: "aspnetmvc-ajax",
                transport: {
                    read: {
                        url: `/Reports/ReadReportBatchReturns`,
                        data: {
                            From: this.globalFilters.from.toISOString(),
                            To: this.globalFilters.to.toISOString(),
                        }
                    }
                },
                schema: {
                    data: "Data",
                    total: "Total",
                    errors: "Errors",
                    model: {
                        JobId: { type: "string" },
                        JobNumber: { type: "number" },
                        CompletedFirst: { type: "date" },
                        CountOfHandpieces: { type: "number" },
                        CountOfDistinctDates: { type: "number" },
                        ListOfDates: { type: "string" },
                    }
                },
                sort: [
                    { field: "JobNumber", dir: "asc" },
                ],
            });
            
            wrapper.kendoGrid({
                dataSource: dataSource,
                columns: [
                    {
                        field: "JobNumber",
                        title: "Job returned in batches",
                        width: "200px",
                        template: "Estimate \\##:JobNumber# | #:CountOfHandpieces# handpieces",
                    },
                    {
                        field: "CompletedFirst",
                        title: "Date of first batch",
                        width: "100px",
                        template: (data: any): string => {
                            if (typeof data.CompletedFirst === "string") {
                                return kendo.toString(kendo.parseDate(data.CompletedFirst), "dd MMM yyyy");
                            } else {
                                return kendo.toString(data.CompletedFirst, "dd MMM yyyy");
                            }
                        }
                    },
                    {
                        field: "CountOfDistinctDates",
                        title: "Number of batches",
                        width: "400px",
                        template: (data: any): string => {
                            let result = `${data.CountOfDistinctDates.toString()}`;
                            let dates = (data.ListOfDates as string).split(",").filter((x, i, a) => a.indexOf(x) === i);
                            result += ` (`;
                            result += dates.map(x => kendo.toString(kendo.parseDate(x), "dd MMM yyyy")).join(", ");
                            result += `)`;
                            return result;
                        },
                    }, 
                    {
                        title: "",
                        command: [
                            {
                                name: "CustomDetails",
                                click: function(this: kendo.ui.Grid, e: JQueryEventObject) {
                                    e.preventDefault();
                                    const row = $(e.target).closest("tr") as JQuery<HTMLElement>;
                                    const item = this.dataItem(row) as any;
                                    routes.jobs.details(item.JobId).open();
                                },
                                text: `<span class="fas fa-fw fa-link"></span> Estimate`,
                            }
                        ],
                        width: "150px",
                    }
                ],
                scrollable: true,
                sortable: {
                    mode: "multiple",
                    allowUnsort: true,
                },
                noRecords: {
                    template: "No data available",
                },
            });

            return wrapper.data("kendoGrid");
        }
    }
}