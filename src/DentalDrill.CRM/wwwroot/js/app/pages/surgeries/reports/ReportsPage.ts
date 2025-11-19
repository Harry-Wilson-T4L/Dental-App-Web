namespace DentalDrill.CRM.Pages.Surgeries.Reports {
    export class ReportsPage {
        private readonly _root: JQuery;
        private readonly _clientId: string;
        private readonly _clientUrlPath: string;
        private readonly _mainControls: ReportsPageMainControls;
        private readonly _brands: ReportsTabBrands;
        private readonly _statuses: ReportsTabStatuses;

        constructor(root: JQuery) {
            this._root = root;
            this._clientId = root.attr("data-client-id");
            this._clientUrlPath = root.attr("data-client-urlpath");
            this._mainControls = new ReportsPageMainControls(root);
            this._brands = new ReportsTabBrands(this, root.find(".reports__brands"));
            this._statuses = new ReportsTabStatuses(this, root.find(".reports__statuses"));

            this._mainControls.buttonClear.on("click", async e => {
                this._mainControls.dateRangeSelector.reset();
                
                await this._brands.applyGlobalFilters(
                    this._mainControls.dateFrom.value(),
                    this._mainControls.dateTo.value());
                await this._statuses.applyGlobalFilters(
                    this._mainControls.dateFrom.value(),
                    this._mainControls.dateTo.value());

                await this._brands.initialize();
                await this._statuses.initialize();
            });

            this._mainControls.buttonSearch.on("click", async e => {

                await this._brands.applyGlobalFilters(
                    this._mainControls.dateFrom.value(),
                    this._mainControls.dateTo.value());
                await this._statuses.applyGlobalFilters(
                    this._mainControls.dateFrom.value(),
                    this._mainControls.dateTo.value());

                await this._brands.initialize();
                await this._statuses.initialize();
            });

            root.data("ReportsPage", this);
            console.log("ReportsPage constructed");
        }

        get clientId(): string {
            return this._clientId;
        }

        get clientUrlPath(): string {
            return this._clientUrlPath;
        }

        async initialize(): Promise<void> {
            await this._brands.applyGlobalFilters(
                this._mainControls.dateFrom.value(),
                this._mainControls.dateTo.value());
            await this._statuses.applyGlobalFilters(
                this._mainControls.dateFrom.value(),
                this._mainControls.dateTo.value());

            await this._brands.initialize();
            await this._statuses.initialize();
            this.setValidation();

            console.log("ReportsPage initialized");
        }

        setValidation() {
            let containerFrom = $("#FilterDateFrom");
            containerFrom.kendoValidator({
                rules: {
                    isValidDate: function (input) {
                        if (kendo.parseDate(input.val()) === null) {
                            return false;
                        } else {
                            return true;
                        }
                    }
                },
                messages: {
                    isValidDate: "Invalid date format"
                }
            });

            let containerTo = $("#FilterDateTo");
            containerTo.kendoValidator({
                rules: {
                    isValidDate: function (input) {
                        if (kendo.parseDate(input.val()) === null) {
                            return false;
                        } else {
                            return true;
                        }
                    }
                },
                messages: {
                    isValidDate: "Invalid date format"
                }
            });
        }
    }
}