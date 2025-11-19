namespace DentalDrill.CRM.Controls.Reporting {
    import ToggleList = DentalDrill.CRM.Controls.ToggleList;

    export interface ReportsPageTabGridCollectionItem {
        key: string;
        gridConstructor(wrapper: JQuery<HTMLElement>): Promise<kendo.ui.Grid>;
    }

    export abstract class ReportsPageTabGridCollectionBase<
        TPageIdentifier extends ReportsPageIdentifierBase,
        TGlobalFilters extends ReportsPageDateRangeGlobalFilters
    > extends ReportsPageTabBase<TGlobalFilters> {
        private readonly _pageIdentifier: TPageIdentifier;
        private readonly _tabRoot: JQuery<HTMLElement>;

        // Controls
        private readonly _reportGrids: ToggleList;

        // Data
        private _globalFilters: TGlobalFilters;
        private readonly _gridsDefinitions: ReportsPageTabGridCollectionItem[];

        // Grid
        private readonly _gridContainer: JQuery<HTMLElement>;

        constructor(pageIdentifier: TPageIdentifier, tabRoot: JQuery<HTMLElement>) {
            super();
            this._pageIdentifier = pageIdentifier;
            this._tabRoot = tabRoot;

            // Initializing Controls
            this._reportGrids = new ToggleList(tabRoot.find(".reports__tab__grids-list"));
            this._reportGrids.selectionChanged.subscribe(async(sender, args) => {
                await this.initialize();
            });

            // Initializing Data
            this._globalFilters = undefined;
            this._gridsDefinitions = [];

            // Initializing Grid
            this._gridContainer = tabRoot.find(".reports__tab__grid-container");

            // Initializing Export
            tabRoot.find(".reports__tab__export__excel").on("click", async (e) => {
                await this.exportExcel();
            });

            tabRoot.find(".reports__tab__export__pdf").on("click", async (e) => {
                await this.exportPdf();
            });
        }

        get pageIdentifier(): TPageIdentifier {
            return this._pageIdentifier;
        }

        get globalFilters(): TGlobalFilters {
            return this._globalFilters;
        }

        async applyGlobalFilters(globalFilters: TGlobalFilters): Promise<void> {
            this._globalFilters = globalFilters;
        }

        async initialize(): Promise<void> {
            this._gridContainer.empty();

            for (let gridDefinition of this._gridsDefinitions) {
                if (this._reportGrids.isToggled(gridDefinition.key)) {
                    const wrapper = $("<div></div>");
                    this._gridContainer.append(wrapper);
                    await gridDefinition.gridConstructor(wrapper);
                }
            }
        }

        async exportExcel(): Promise<void> {
            throw new Error("Excel export is not implemented");
        }

        async exportPdf(): Promise<void> {
            throw new Error("Pdf export is not implemented");
        }

        protected registerGridDefinition(key: string, gridConstructor: (wrapper: JQuery<HTMLElement>) => Promise<kendo.ui.Grid>): void {
            this._gridsDefinitions.push({
                key: key,
                gridConstructor: gridConstructor
            });
        }
    }
}