namespace DentalDrill.CRM.Pages.CorporateSurgeries.Reports {
    export class ReportsPage {
        private readonly _root: JQuery;
        private readonly _corporateId: string;
        private readonly _corporateUrlPath: string;
        private readonly _mainControls: ReportsPageMainControls;
        private readonly _surgeries: ReportsTabSurgeries;
        private readonly _brands: ReportsTabBrands;
        private readonly _statuses: ReportsTabStatuses;

        constructor(root: JQuery) {
            this._root = root;
            this._corporateId = root.attr("data-corporate-id");
            this._corporateUrlPath = root.attr("data-corporate-urlpath");
            this._mainControls = new ReportsPageMainControls(root);
            this._surgeries = new ReportsTabSurgeries(this, root.find(".reports__surgeries"));
            this._brands = new ReportsTabBrands(this, root.find(".reports__brands"));
            this._statuses = new ReportsTabStatuses(this, root.find(".reports__statuses"));

            this._mainControls.buttonClear.on("click", async e => {
                this._mainControls.dateRangeSelector.reset();
                const allClients = this._mainControls.clients.dataSource.data<{ Id: string }>().map(x => x.Id);
                this._mainControls.clients.value(allClients);
                this._mainControls.clients.trigger("change");

                await this._surgeries.applyGlobalFilters(
                    this._mainControls.dateFrom.value(),
                    this._mainControls.dateTo.value(),
                    this._mainControls.clients.value() as string[]);
                await this._brands.applyGlobalFilters(
                    this._mainControls.dateFrom.value(),
                    this._mainControls.dateTo.value(),
                    this._mainControls.clients.value() as string[]);
                await this._statuses.applyGlobalFilters(
                    this._mainControls.dateFrom.value(),
                    this._mainControls.dateTo.value(),
                    this._mainControls.clients.value());

                await this._surgeries.initialize();
                await this._brands.initialize();
                await this._statuses.initialize();
            });

            this._mainControls.buttonSearch.on("click", async e => {

                await this._surgeries.applyGlobalFilters(
                    this._mainControls.dateFrom.value(),
                    this._mainControls.dateTo.value(),
                    this._mainControls.clients.value() as string[]);
                await this._brands.applyGlobalFilters(
                    this._mainControls.dateFrom.value(),
                    this._mainControls.dateTo.value(),
                    this._mainControls.clients.value() as string[]);
                await this._statuses.applyGlobalFilters(
                    this._mainControls.dateFrom.value(),
                    this._mainControls.dateTo.value(),
                    this._mainControls.clients.value());

                await this._surgeries.initialize();
                await this._brands.initialize();
                await this._statuses.initialize();
            });

            root.data("ReportsPage", this);
            console.log("ReportsPage constructed");
        }

        get corporateId(): string {
            return this._corporateId;
        }

        get corporateUrlPath(): string {
            return this._corporateUrlPath;
        }

        async initialize(): Promise<void> {
            await this._surgeries.applyGlobalFilters(
                this._mainControls.dateFrom.value(),
                this._mainControls.dateTo.value(),
                this._mainControls.clients.value() as string[]);
            await this._brands.applyGlobalFilters(
                this._mainControls.dateFrom.value(),
                this._mainControls.dateTo.value(),
                this._mainControls.clients.value() as string[]);
            await this._statuses.applyGlobalFilters(
                this._mainControls.dateFrom.value(),
                this._mainControls.dateTo.value(),
                this._mainControls.clients.value());

            await this._surgeries.initialize();
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