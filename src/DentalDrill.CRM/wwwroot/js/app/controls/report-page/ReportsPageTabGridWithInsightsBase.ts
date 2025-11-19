namespace DentalDrill.CRM.Controls.Reporting {
    import ToggleButton = DentalDrill.CRM.Controls.ToggleButton;
    import ToggleList = DentalDrill.CRM.Controls.ToggleList;

    export abstract class ReportsPageTabGridWithInsightsBase<
        TPageIdentifier extends ReportsPageIdentifierBase,
        TGlobalFilters extends ReportsPageDateRangeGlobalFilters
    > extends ReportsPageTabDateRangeBase<TGlobalFilters> {
        private readonly _pageIdentifier: TPageIdentifier;
        private readonly _tabRoot: JQuery<HTMLElement>;

        // Controls
        private readonly _reportFields: ToggleList;
        private readonly _dateAggregate: kendo.ui.DropDownList;

        // Data
        private _globalFilters: TGlobalFilters;
        private _dataSource: kendo.data.DataSource;

        // Grid
        private readonly _gridContainer: JQuery<HTMLElement>;
        private _grid: kendo.ui.Grid;

        // Insights
        private readonly _insightsToggle: ToggleButton;
        private readonly _insightsContainer: ReportInsightsContainer;
        
        constructor(pageIdentifier: TPageIdentifier, tabRoot: JQuery<HTMLElement>) {
            super();
            this._pageIdentifier = pageIdentifier;
            this._tabRoot = tabRoot;

            // Initializing Controls
            this._reportFields = new ToggleList(tabRoot.find(".reports__tab__fields"));
            this._reportFields.selectionChanged.subscribe(async(sender, args) => {
                await this.initialize();
            });

            this._dateAggregate = tabRoot.find("input.reports__tab__date-aggregate").data("kendoDropDownList");
            this._dateAggregate.bind("change", async (e) => {
                await this.initialize();
            });

            // Initializing Data
            this._globalFilters = undefined;
            this._dataSource = undefined;

            // Initializing Grid
            this._gridContainer = tabRoot.find(".reports__tab__grid-container");
            this._grid = undefined;

            // Initializing Insights
            this._insightsToggle = new ToggleButton(tabRoot.find(".reports__tab__insights"));
            this._insightsToggle.changed.subscribe(async (sender, args) => {
                await this.toggleInsights();
            });
            this._insightsContainer = new ReportInsightsContainer(tabRoot.find(".reports__tab__insights-container"));

            // Initializing Export
            tabRoot.find(".reports__tab__export__excel").on("click", async (e) => {
                await this.exportExcel();
            });

            tabRoot.find(".reports__tab__export__pdf").on("click", async (e) => {
                await this.exportPdf();
            });
        }

        get reportFields(): ToggleList {
            return this._reportFields;
        }

        get globalFilters(): TGlobalFilters {
            return this._globalFilters;
        }

        get dataSource(): kendo.data.DataSource {
            return this._dataSource;
        }

        get dateAggregate(): string {
            return this._dateAggregate.value();
        }

        get insightsToggle(): ToggleButton {
            return this._insightsToggle;
        }

        get insightsContainer(): ReportInsightsContainer {
            return this._insightsContainer;
        }

        async initialize(): Promise<void> {
            this._dataSource = await this.initializeDataSource();
            this._grid = await this.initializeGrid();
            await this.toggleInsights();
        }

        async exportExcel(): Promise<void> {
            throw new Error("Excel export is not implemented");
        }

        async exportPdf(): Promise<void> {
            throw new Error("Pdf export is not implemented");
        }

        async toggleInsights(): Promise<void> {
            this._insightsContainer.clear();
            if (!this._insightsToggle.active) {
                return;
            }

            await this.initializeInsights();
        }

        protected async initializeDataSource(): Promise<kendo.data.DataSource> {
            this._dataSource = undefined;
            this._dataSource = await this.createDataSource();
            return this._dataSource;
        }

        protected async createDataSource(customOptions?: kendo.data.DataSourceOptions): Promise<kendo.data.DataSource> {
            const dataSourceOptions: kendo.data.DataSourceOptions = {
                type: "aspnetmvc-ajax",
                transport: {
                    read: await this.initializeDataSourceTransportRead()
                },
                schema: {
                    data: "Data",
                    total: "Total",
                    errors: "Errors",
                    model: {
                        fields: await this.initializeDataSourceFields(this.initializeDataSourceGroupFields())
                    }
                }
            };

            this.alterDataSourceOptions(dataSourceOptions);
            if (customOptions) {
                $.extend(true, dataSourceOptions, customOptions);
            }

            const dataSource = new kendo.data.DataSource(dataSourceOptions);
            await dataSource.read();
            return dataSource;
        }

        protected async initializeDataSourceFields(fields: object): Promise<object> {
            const dateAggregate = this._dateAggregate.value();
            const baseFields = this.initializeDataSourceValueFields();
            if (dateAggregate === "EntirePeriod") {
                for (let field of baseFields) {
                    fields[field] = { type: "number", from: field };
                }
            } else {
                const dateRanges = this.generateDateRange(this._globalFilters.from, this._globalFilters.to, dateAggregate);
                for (let field of baseFields) {
                    for (let range of dateRanges) {
                        fields[`${field}_${range.replace(/-/g, "_")}`] = { type: "number", from: `${field}["${range}"]` };
                    }
                }
            }

            return fields;
        }

        protected abstract initializeDataSourceTransportRead(): Promise<kendo.data.DataSourceTransportRead>;

        protected abstract initializeDataSourceGroupFields(): object;

        protected abstract initializeDataSourceValueFields(): string[];

        protected alterDataSourceOptions(dataSourceOptions: kendo.data.DataSourceOptions): void {
        }

        protected async initializeGrid(): Promise<kendo.ui.Grid> {
            const wrapper = $("<div></div>");
            this._gridContainer.empty();
            this._gridContainer.append(wrapper);

            const columnsConfig = this.initializeGridGroupColumns();
            const selectedFields = this._reportFields.selected;
            const fieldsCandidates = this.initializeGridValueColumns();
            const dateAggregate = this._dateAggregate.value();

            if (dateAggregate === "EntirePeriod") {
                for (let candidate of fieldsCandidates.filter(x => selectedFields.indexOf(x.field) >= 0)) {
                    columnsConfig.push({
                        field: candidate.field,
                        title: candidate.title,
                        width: "125px",
                        format: candidate.format,
                        groupHeaderColumnTemplate: candidate.groupHeaderColumnTemplate ? candidate.groupHeaderColumnTemplate("") : undefined
                    });
                }
            } else {
                const dateRanges = this.generateDateRange(this._globalFilters.from, this._globalFilters.to, dateAggregate);
                for (let candidate of fieldsCandidates.filter(x => selectedFields.indexOf(x.field) >= 0)) {
                    const childColumns: kendo.ui.GridColumn[] = [];
                    for (let range of dateRanges) {
                        childColumns.push({
                            title: range,
                            field: `${candidate.field}_${range.replace(/-/g, "_")}`,
                            width: "125px",
                            format: candidate.format,
                            groupHeaderColumnTemplate: candidate.groupHeaderColumnTemplate ? candidate.groupHeaderColumnTemplate(`_${range.replace(/-/g, "_")}`) : undefined
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

            const gridOptions: kendo.ui.GridOptions = {
                dataSource: this._dataSource,
                columns: columnsConfig,
                scrollable: true,
                sortable: {
                    mode: "multiple",
                    allowUnsort: true,
                },
                noRecords: {
                    template: "No data available"
                }
            }

            this.alterGridOptions(gridOptions);

            wrapper.kendoGrid(gridOptions);
            return wrapper.data("kendoGrid");
        }

        protected abstract initializeGridGroupColumns(): kendo.ui.GridColumn[];

        protected abstract initializeGridValueColumns(): ReportsPageGridColumn[];

        protected alterGridOptions(gridOptions: kendo.ui.GridOptions): void {
        }

        protected async initializeInsights(): Promise<void> {
        }

        async applyGlobalFilters(globalFilters: TGlobalFilters): Promise<void> {
            this._globalFilters = globalFilters;
        }
    }
}