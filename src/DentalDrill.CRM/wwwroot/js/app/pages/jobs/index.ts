namespace DentalDrill.CRM.Pages.Jobs.Index {
    import GridHandlers = DevGuild.AspNet.Controls.Grids.Handlers.GridHandlers;
    import AjaxFormsManager = DevGuild.AspNet.Forms.AjaxForms.AjaxFormsManager;
    import Collapsible = DentalDrill.CRM.Controls.Collapsible;
    import HandpieceStatusIndicator = DentalDrill.CRM.Controls.HandpieceStatusIndicator;


    export class JobsFilters {
        static clickSearch() {
            JobsGridFilter.instance.apply();
        }

        static clickCancel() {
            JobsGridFilter.instance.reset();
        }
    }

    export class JobsGridFilterFieldsCollection extends DevGuild.Filters.Grids.GridFilterFieldsCollection {
        private readonly _workshop: DevGuild.Filters.Grids.DropDownListGridFilterField;
        private readonly _jobType: DevGuild.Filters.Grids.DropDownListGridFilterField;
        private readonly _jobNumber: DevGuild.Filters.Grids.StringInputGridFilterField;
        private readonly _client: DevGuild.Filters.Grids.DropDownListGridFilterField;
        private readonly _receivedFrom: DevGuild.Filters.Grids.DatePickerFilterField;
        private readonly _receivedTo: DevGuild.Filters.Grids.DatePickerFilterField;
        private readonly _serial: DevGuild.Filters.Grids.StringInputGridFilterField;
        private readonly _makeAndModel: DevGuild.Filters.Grids.DropDownListGridFilterField;
        private readonly _type: DevGuild.Filters.Grids.DropDownListGridFilterField;
        private readonly _jobStatus: DevGuild.Filters.Grids.DropDownListGridFilterField;
        private readonly _handpieceStatus: DevGuild.Filters.Grids.DropDownListGridFilterField;
        private readonly _parts: DevGuild.Filters.Grids.DropDownListGridFilterField;
        
        constructor(root: JQuery) {
            super(root);
            this._workshop = new DevGuild.Filters.Grids.DropDownListGridFilterField($("#WorkshopFilter"), "WorkshopId", { value: a => a.Value, defaultValue: "" })
            this._jobType = new DevGuild.Filters.Grids.DropDownListGridFilterField($("#JobTypeFilter"), "JobTypeId", { value: a => a.Value, defaultValue: "" })
            this._jobNumber = new DevGuild.Filters.Grids.StringInputGridFilterField($("#JobNumberFilter"), "JobNumberString");
            this._client = new DevGuild.Filters.Grids.DropDownListGridFilterField($("#ClientFilter"), "ClientId", { value: a => a.Id, defaultValue: "" });
            this._receivedFrom = new DevGuild.Filters.Grids.DatePickerFilterField($("#ReceivedFromFilter"), "Received", "gte");
            this._receivedTo = new DevGuild.Filters.Grids.DatePickerFilterField($("#ReceivedToFilter"), "Received", "lte");
            this._serial = new DevGuild.Filters.Grids.StringInputGridFilterField($("#SerialFilter"), "Serial");
            this._makeAndModel = new DevGuild.Filters.Grids.DropDownListGridFilterField($("#MakeAndModelFilter"), "MakeAndModel", { value: a => a.Value, defaultValue: "" });
            this._makeAndModel.applyValueDelegate = (filters, value) => {
                const parts = value.split("||");
                filters.push({ field: "Brand", operator: "eq", value: parts[0] });
                filters.push({ field: "MakeAndModel", operator: "eq", value: parts[1] });
            };
            this._type = new DevGuild.Filters.Grids.DropDownListGridFilterField($("#TypeFilter"), "SpeedType", { value: a => a.Value, defaultValue: "" });
            this._jobStatus = new DevGuild.Filters.Grids.DropDownListGridFilterField($("#JobStatusFilter"), "JobStatus", { value: a => a.Value });
            this._jobStatus.applyValueDelegate = (filters, value) => {
                if (value === "All") {
                    // Do nothing
                } else if (value === "Workshop") {
                    const combined: kendo.data.DataSourceFilter[] = [];
                    combined.push({ field: "JobStatus", operator: "eq", value: "Received" });
                    combined.push({ field: "JobStatus", operator: "eq", value: "BeingEstimated" });
                    combined.push({ field: "JobStatus", operator: "eq", value: "BeingRepaired" });
                    filters.push({ logic: "or", filters: combined } as kendo.data.DataSourceFilter);
                } else if (value === "Active") {
                    filters.push({ field: "JobStatus", operator: "neq", value: "SentComplete" });
                    filters.push({ field: "JobStatus", operator: "neq", value: "Cancelled" });
                } else {
                    filters.push({ field: "JobStatus", operator: "eq", value: value });
                }
            };
            this._jobStatus.resetValueDelegate = () => "Active";
            this._handpieceStatus = new DevGuild.Filters.Grids.DropDownListGridFilterField($("#HandpieceStatusFilter"), "HandpieceStatus", { value: a => a.Value });
            this._handpieceStatus.applyValueDelegate = (filters, value) => {
                if (value === "All") {
                    // Do nothing
                } else if (value === "BeingRepairedEx") {
                    const combined: kendo.data.DataSourceFilter[] = [];
                    combined.push({ field: "HandpieceStatus", operator: "eq", value: "BeingRepaired" });
                    combined.push({ field: "HandpieceStatus", operator: "eq", value: "WaitingForParts" });
                    filters.push({ logic: "or", filters: combined } as kendo.data.DataSourceFilter);
                } else if (value === "Workshop") {
                    const combined: kendo.data.DataSourceFilter[] = [];
                    combined.push({ field: "HandpieceStatus", operator: "eq", value: "Received" });
                    combined.push({ field: "HandpieceStatus", operator: "eq", value: "BeingEstimated" });
                    combined.push({ field: "HandpieceStatus", operator: "eq", value: "BeingRepaired" });
                    filters.push({ logic: "or", filters: combined } as kendo.data.DataSourceFilter);
                } else if (value === "Active") {
                    filters.push({ field: "HandpieceStatus", operator: "neq", value: "SentComplete" });
                    filters.push({ field: "HandpieceStatus", operator: "neq", value: "Cancelled" });
                } else {
                    filters.push({ field: "HandpieceStatus", operator: "eq", value: value });
                }
            };
            this._handpieceStatus.resetValueDelegate = () => "All";
            this._parts = new DevGuild.Filters.Grids.DropDownListGridFilterField($("#PartsOutOfStockFilter"), "PartsOutOfStock", { value: a => a.Value, defaultValue: "" });
            this._parts.applyValueDelegate = (filters, value) => {
                switch (value.toString())
                {
                    case "InStock":
                        filters.push({ field: "PartsOutOfStock", operator: "eq", value: 0 });
                        break;
                    case "OutOfStock":
                        filters.push({ field: "PartsOutOfStock", operator: "eq", value: 1 });
                        break;
                    case "PartialStock":
                        filters.push({ field: "PartsOutOfStock", operator: "eq", value: 2 });
                        break;
                }
            };

            this.subscribeEvents();
        }

        applyAll(filters: kendo.data.DataSourceFilter[], exceptions?: string[]): void {
            this._workshop.apply(filters, exceptions);
            this._jobType.apply(filters, exceptions);
            this._jobNumber.apply(filters, exceptions);
            this._client.apply(filters, exceptions);
            this._receivedFrom.apply(filters, exceptions);
            this._receivedTo.apply(filters, exceptions);
            this._serial.apply(filters, exceptions);
            this._makeAndModel.apply(filters, exceptions);
            this._type.apply(filters, exceptions);
            this._jobStatus.apply(filters, exceptions);
            this._handpieceStatus.apply(filters, exceptions);
            this._parts.apply(filters, exceptions);
        }

        resetAll(): void {
            this._workshop.reset();
            this._jobType.reset();
            this._jobNumber.reset();
            this._client.reset();
            this._receivedFrom.reset();
            this._receivedTo.reset();
            this._serial.reset();
            this._makeAndModel.reset();
            this._type.reset();
            this._jobStatus.reset();
            this._handpieceStatus.reset();
            this._parts.reset();
        }

        private subscribeEvents(): void {
            this._workshop.kendoControl.bind(`change`, e => {
                JobsGridFilter.instance.apply();
            });

            this._jobType.kendoControl.bind(`change`, e => {
                JobsGridFilter.instance.apply();
            });

            this._jobNumber.control.on(`keypress`, e => {
                if (e.which === 13) {
                    JobsGridFilter.instance.apply();
                }
            });
            this._jobNumber.control.on(`keyup`, e => {
                if (e.which === 27) {
                    this._jobNumber.control.val(``);
                    JobsGridFilter.instance.apply();
                }
            });

            this._serial.control.on(`keypress`, e => {
                if (e.which === 13) {
                    JobsGridFilter.instance.apply();
                }
            });
            this._serial.control.on(`keyup`, e => {
                if (e.which === 27) {
                    this._serial.control.val(``);
                    JobsGridFilter.instance.apply();
                }
            });

            this._jobStatus.kendoControl.bind(`change`, e => {
                JobsGridFilter.instance.apply();
            });

            this._client.kendoControl.bind(`change`, e => {
                JobsGridFilter.instance.apply();
            });

            this._makeAndModel.kendoControl.bind(`change`, e => {
                JobsGridFilter.instance.apply();
            });
            this._handpieceStatus.kendoControl.bind(`change`, e => {
                JobsGridFilter.instance.apply();
            });

            this._receivedFrom.kendoControl.bind(`change`, e => {
                JobsGridFilter.instance.apply();
            });
            this._receivedFrom.control.on(`keyup`, e => {
                if (e.which === 27) {
                    this._receivedFrom.kendoControl.value(``);
                    this._receivedFrom.control.val(``).trigger(`change`);
                    JobsGridFilter.instance.apply();
                }
            });

            this._receivedTo.kendoControl.bind(`change`, e => {
                JobsGridFilter.instance.apply();
            });
            this._receivedTo.control.on(`keyup`, e => {
                if (e.which === 27) {
                    this._receivedTo.kendoControl.value(``);
                    this._receivedTo.control.val(``).trigger(`change`);
                    JobsGridFilter.instance.apply();
                }
            });

            this._type.kendoControl.bind(`change`, e => {
                JobsGridFilter.instance.apply();
            });
            this._parts.kendoControl.bind(`change`, e => {
                JobsGridFilter.instance.apply();
            });

        }
    }

    export class JobsGridFilter extends DevGuild.Filters.Grids.GridFilterCore<JobsGridFilterFieldsCollection> {

        private static _instance: JobsGridFilter;

        static get instance(): JobsGridFilter {
            if (!JobsGridFilter._instance) {
                JobsGridFilter._instance = new JobsGridFilter($(".filters__jobs"));
            }

            return JobsGridFilter._instance;
        }

        constructor(root: JQuery) {
            super(root);

            this.initialize();
        }

        createFields(root: JQuery<HTMLElement>): JobsGridFilterFieldsCollection {
            return new JobsGridFilterFieldsCollection(this.root);
        }

        applyFilter(filters: kendo.data.DataSourceFilters) {
            const filteredFields = this.getFilteredFields(filters);
            if (filteredFields.some(x => x === "JobNumberString") ||
                filteredFields.some(x => x === "Brand") ||
                filteredFields.some(x => x === "MakeAndModel") ||
                filteredFields.some(x => x === "Serial") ||
                filteredFields.some(x => x === "SpeedType") ||
                filteredFields.some(x => x === "HandpieceStatus") ||
                filteredFields.some(x => x === "PartsOutOfStock")) {

                (JobsHandpiecesGrid.instance.dataSource as any)._query({ filter: filters, page: 1 }).then(() => {
                    JobsGridsSwitcher.instance.switchToHandpieces();
                });
            } else {
                (JobsGrid.instance.dataSource as any)._query({ filter: filters, page: 1 }).then(() => {
                    JobsGridsSwitcher.instance.switchToJobs();
                });
            }
        }

        private getFilteredFields(filter: kendo.data.DataSourceFilter): string[] {
            if ((filter as any).filters) {
                const filtersList = filter as kendo.data.DataSourceFilters;
                const result: string[] = [];
                for (let i = 0; i < filtersList.filters.length; i++) {
                    const subResult = this.getFilteredFields(filtersList.filters[i]);
                    for (let j = 0; j < subResult.length; j++) {
                        if (result.indexOf(subResult[j]) < 0) {
                            result.push(subResult[j]);
                        }
                    }
                }

                return result;
            } else if ((filter as any).field) {
                const filterItem = filter as kendo.data.DataSourceFilterItem;
                return [filterItem.field];
            } else {
                return [];
            }
        }
    }

    export enum JobsGridsSwitcherMode {
        JobsGrid,
        HandpiecesGrid
    }

    export class JobsGridsSwitcher {
        private readonly _jobsGrid: Collapsible;
        private readonly _handpiecesGrid: Collapsible;

        private static _instance: JobsGridsSwitcher;

        constructor(root: HTMLElement) {
            this._jobsGrid = new Collapsible(document.querySelector(".grid-switcher__jobs") as HTMLElement);
            this._handpiecesGrid = new Collapsible(document.querySelector(".grid-switcher__handpieces") as HTMLElement);
        }

        static get instance(): JobsGridsSwitcher {
            if (JobsGridsSwitcher._instance === undefined) {
                JobsGridsSwitcher._instance = new JobsGridsSwitcher(document.querySelector("body") as HTMLElement);
            }

            return JobsGridsSwitcher._instance;
        }

        isJobsShown(): boolean {
            return this._jobsGrid.isShown();
        }

        isHandpiecesShown(): boolean {
            return this._handpiecesGrid.isShown();
        }

        async switchToJobs(): Promise<void> {
            if (this._jobsGrid.isShown()) {
                return;
            }

            await this._handpiecesGrid.hideAsync();
            await this._jobsGrid.showAsync();
            setTimeout(() => {
                GridResizer.resize(JobsGrid.instance);
            }, 0);
        }

        async switchToHandpieces(): Promise<void> {
            if (this._handpiecesGrid.isShown()) {
                return;
            }

            await this._jobsGrid.hideAsync();
            await this._handpiecesGrid.showAsync();
            setTimeout(() => {
                GridResizer.resize(JobsHandpiecesGrid.instance);
            }, 0);
        }
    }

    export class JobsHandpiecesGrid {
        static get instance(): kendo.ui.Grid {
            return $("#JobsHandpiecesGrid").data("kendoGrid");
        }

        static handleDetails = GridHandlers.createButtonClickNavigationHandler<{ Id: string, JobId: string, Serial: string }>(
            item => routes.handpieces.details(item.Id));

        static handleEdit = GridHandlers.createButtonClickNavigationHandler<{ Id: string, JobId: string, Serial: string }>(
            item => routes.handpieces.edit(item.Id));

        static handleDelete = GridHandlers.createButtonClickPopupHandler<{ Id: string, JobId: string, Serial: string }>(
            item => routes.handpieces.delete(item.Id),
            item => ({
                title: `Delete Handpiece ${item.Serial}`,
                width: "1000px",
                height: "auto",
                refresh: (e: kendo.ui.WindowEvent) => {
                    e.sender.wrapper.on("click", ".editor__submit__cancel", clickEvent => {
                        clickEvent.preventDefault();
                        e.sender.close();
                        e.sender.destroy();
                    });

                    e.sender.center();
                },
                open: async (e: kendo.ui.WindowEvent) => {
                    await AjaxFormsManager.waitFor("HandpiecesDelete");
                    await JobsHandpiecesGrid.instance.dataSource.read();
                    e.sender.close();
                    e.sender.destroy();
                }
            }));

        static handleCreateEstimate = GridHandlers.createGridButtonClickPopupHandler(
            "#JobsHandpiecesGrid .k-grid-CustomCreateEstimate",
            target => routes.jobs.create(`Estimate`),
            target => ({
                title: `Create Estimate`,
                width: "1000px",
                height: "auto",
                refresh: (e: kendo.ui.WindowEvent) => {
                    e.sender.wrapper.on("click", ".editor__submit__cancel", clickEvent => {
                        clickEvent.preventDefault();
                        e.sender.close();
                        e.sender.destroy();
                    });

                    e.sender.center();
                },
                open: async (e: kendo.ui.WindowEvent) => {
                    await AjaxFormsManager.waitFor("JobsCreate");
                    await JobsGrid.instance.dataSource.read();
                    e.sender.close();
                    e.sender.destroy();
                }
            })
        );

        static handleCreateSale = GridHandlers.createGridButtonClickPopupHandler(
            "#JobsHandpiecesGrid .k-grid-CustomCreateSale",
            target => routes.jobs.create(`Sale`),
            target => ({
                title: `Create Sale`,
                width: "1000px",
                height: "auto",
                refresh: (e: kendo.ui.WindowEvent) => {
                    e.sender.wrapper.on("click", ".editor__submit__cancel", clickEvent => {
                        clickEvent.preventDefault();
                        e.sender.close();
                        e.sender.destroy();
                    });

                    e.sender.center();
                },
                open: async (e: kendo.ui.WindowEvent) => {
                    await AjaxFormsManager.waitFor("JobsCreate");
                    await JobsGrid.instance.dataSource.read();
                    e.sender.close();
                    e.sender.destroy();
                }
            })
        );

        static handleDataBound(e: kendo.ui.GridDataBoundEvent) {
            e.sender.element.find("[data-toggle='tooltip']").tooltip();
        }
    }

    

    export class JobsGrid {
        static get instance(): kendo.ui.Grid {
            return $("#JobsGrid").data("kendoGrid");
        }

        static handleEdit = GridHandlers.createButtonClickNavigationHandler<{ Id: string, JobNumber: number }>(
            item => routes.jobs.edit(item.Id));

        static handleCreateEstimate = GridHandlers.createGridButtonClickPopupHandler(
            "#JobsGrid .k-grid-CustomCreateEstimate",
            target => routes.jobs.create(`Estimate`),
            target => ({
                title: `Create Estimate`,
                width: "1000px",
                height: "auto",
                refresh: (e: kendo.ui.WindowEvent) => {
                    e.sender.wrapper.on("click", ".editor__submit__cancel", clickEvent => {
                        clickEvent.preventDefault();
                        e.sender.close();
                        e.sender.destroy();
                    });

                    e.sender.center();
                },
                open: async (e: kendo.ui.WindowEvent) => {
                    await AjaxFormsManager.waitFor("JobsCreate");
                    await JobsGrid.instance.dataSource.read();
                    e.sender.close();
                    e.sender.destroy();
                }
            })
        );

        static handleCreateSale = GridHandlers.createGridButtonClickPopupHandler(
            "#JobsGrid .k-grid-CustomCreateSale",
            target => routes.jobs.create(`Sale`),
            target => ({
                title: `Create Sale`,
                width: "1000px",
                height: "auto",
                refresh: (e: kendo.ui.WindowEvent) => {
                    e.sender.wrapper.on("click", ".editor__submit__cancel", clickEvent => {
                        clickEvent.preventDefault();
                        e.sender.close();
                        e.sender.destroy();
                    });

                    e.sender.center();
                },
                open: async (e: kendo.ui.WindowEvent) => {
                    await AjaxFormsManager.waitFor("JobsCreate");
                    await JobsGrid.instance.dataSource.read();
                    e.sender.close();
                    e.sender.destroy();
                }
            })
        );

        static renderStatusIndicator(data: any): string {
            const config = (data.JobStatusConfig as string).split(";").map(x => parseInt(x));
            const indicator = new HandpieceStatusIndicator();

            const indicatorValue = Math.abs(config[0]);
            indicator.value = indicatorValue;
            indicator.danger = config[0] < 0;
            for (let i = 1; i <= 7; i++) {
                indicator.setOverride(i, config[i] > 0 && i < indicatorValue);
                indicator.setCount(i, i < indicatorValue ? config[i] : 0);
            }

            return indicator.render().outerHTML;
        }
    }

    export class GridResizer {
        static resize(grid: kendo.ui.Grid) {
            if (grid) {
                grid.setOptions({ height: "100px" });
                grid.resize();

                grid.setOptions({ height: "100%" });
                grid.resize();
            }
        }
    }

    $(() => {
        JobsGrid.instance.autoResizeWhen(() => JobsGridsSwitcher.instance.isJobsShown());
        JobsHandpiecesGrid.instance.autoResizeWhen(() => JobsGridsSwitcher.instance.isHandpiecesShown());
        const filters = JobsGridFilter.instance;

        ////    $(window).on("resize", e => {
        ////        if (JobsGridsSwitcher.instance.isJobsShown()) {
        ////            const gridInstance = JobsGrid.instance;
        ////            GridResizer.resize(gridInstance);
        ////        }
        ////        else if (JobsGridsSwitcher.instance.isHandpiecesShown()) {
        ////            const gridInstance = $(document.querySelector("#JobsHandpiecesGrid")).data("kendoGrid");
        ////            GridResizer.resize(gridInstance);
        ////        }
        ////    });
    });
}