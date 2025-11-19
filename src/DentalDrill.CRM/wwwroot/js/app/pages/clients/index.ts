namespace DentalDrill.CRM.Pages.Clients.Index {
    import GridHandlers = DevGuild.AspNet.Controls.Grids.Handlers.GridHandlers;

    export class ClientsGrid {
        static get instance(): kendo.ui.Grid {
            return $("#ClientsGrid").data("kendoGrid");
        }

        static handleDetailsClick = GridHandlers.createButtonClickNavigationHandler<{Id: string}>(
            item => routes.clients.details(item.Id));

        static handleEditClick = GridHandlers.createButtonClickPopupHandler<{Id: string, Name: string }>(
            item => routes.clients.edit(item.Id),
            item => ({
                title: `Edit Client ${item.Name}`,
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
                    await DevGuild.AspNet.Forms.AjaxForms.AjaxFormsManager.waitFor("ClientEdit");
                    await ClientsGrid.instance.dataSource.read();
                    e.sender.close();
                    e.sender.destroy();
                }
            }));

        static handleDeleteClick = GridHandlers.createButtonClickPopupHandler<{ Id: string, Name: string }>(
            item => routes.clients.delete(item.Id),
            item => ({
                title: `Delete Client ${item.Name}`,
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
                    await DevGuild.AspNet.Forms.AjaxForms.AjaxFormsManager.waitFor("ClientDelete");
                    await ClientsGrid.instance.dataSource.read();
                    e.sender.close();
                    e.sender.destroy();
                }
            }));

        static handleCreate = GridHandlers.createGridButtonClickPopupHandler(
            "#ClientsGrid .k-grid-CustomCreate",
            target => new DevGuild.AspNet.Routing.Uri(target.attr("href")),
            target => ({
                title: "Create Client",
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
                open: async function (this: kendo.ui.Window) {
                    await DevGuild.AspNet.Forms.AjaxForms.AjaxFormsManager.waitFor("CreateClients");
                    await ClientsGrid.instance.dataSource.read();
                    this.close();
                    this.destroy();
                }
            }));

        static handleDataBound(e: kendo.ui.GridDataBoundEvent) {
            e.sender.element.find("[data-toggle='tooltip']").tooltip();
        }
    }

    
    export class ClientsGridFilterFieldsCollection extends DevGuild.Filters.Grids.GridFilterFieldsCollection {
        private readonly _serial: DevGuild.Filters.Grids.StringInputGridFilterField;
        private readonly _makeAndModel: DevGuild.Filters.Grids.StringInputGridFilterField;
        private readonly _received: DevGuild.Filters.Grids.DatePickerFilterField;
        //private readonly _due: DevGuild.Filters.Grids.DatePickerFilterField;

        constructor(root: JQuery) {
            super(root);
            this._serial = new DevGuild.Filters.Grids.StringInputGridFilterField($("#SerialFilter"), "serial");
            this._makeAndModel = new DevGuild.Filters.Grids.StringInputGridFilterField($("#ModelFilter"), "model");
            this._received = new DevGuild.Filters.Grids.DatePickerFilterField($("#ReceivedFilter"), "received");
            //this._due = new DevGuild.Filters.Grids.DatePickerFilterField($("#DueFilter"), "Due");
        }

        applyAll(filters: kendo.data.DataSourceFilter[], exceptions?: string[]): void {
            this._serial.apply(filters, exceptions);
            this._makeAndModel.apply(filters, exceptions);
            this._received.apply(filters, exceptions);
            //this._due.apply(filters, exceptions);
        }

        resetAll(): void {
            this._serial.reset();
            this._makeAndModel.reset();
            this._received.reset();
            //this._due.reset();
        }
    }

    export class ClientsGridFilter extends DevGuild.Filters.Grids.GridFilterCore<ClientsGridFilterFieldsCollection> {
        private static _instance: ClientsGridFilter;

        static get instance(): ClientsGridFilter {
            if (!ClientsGridFilter._instance) {
                ClientsGridFilter._instance = new ClientsGridFilter($("body"));
            }

            return ClientsGridFilter._instance;
        }

        constructor(root: JQuery) {
            super(root);

            this.initialize();
        }

        createFields(root: JQuery<HTMLElement>): ClientsGridFilterFieldsCollection {
            return new ClientsGridFilterFieldsCollection(this.root);
        }

        applyFilter(filters) {
            const gridDataSource = ClientsGrid.instance.dataSource;

            gridDataSource.filter(filters);
            gridDataSource.read();
        }
    }
    
    export class ClientsFilters {
        static clickSearch() {
            //ClientsGridFilter.instance.apply();
            ClientsGrid.instance.dataSource.read();
        }

        static clickCancel() {
            ClientsGridFilter.instance.reset();
        }
    }
}
