namespace DentalDrill.CRM.Pages.InventoryMovements.Index {
    import GridHandlers = DevGuild.AspNet.Controls.Grids.Handlers.GridHandlers;
    import AjaxFormsManager = DevGuild.AspNet.Forms.AjaxForms.AjaxFormsManager;
    import GenericFlagControl = DentalDrill.CRM.Controls.GenericFlag.GenericFlagControl;

    export class GridInventoryGroupMovementsApprovedTab extends GridInventoryGroupMovementsTabBase {
        private readonly _api: IInventoryMovementApi;
        private _showWarnings: boolean = false;

        constructor(id: string, root: HTMLElement, options: InventoryMovementsIndexOptions, api: IInventoryMovementApi) {
            super(id, root, options);
            this._api = api;
        }

        protected getTabName(): string {
            return `Approved`;
        }

        protected getEndpointUrl(): string {
            return this._showWarnings
                ? "/InventoryMovements/ReadGroupApprovedAndMissing"
                : "/InventoryMovements/ReadGroupApproved";
        }

        protected getColumnsConfig(): GridInventoryGroupMovementsColumnsConfig {
            const config = super.getColumnsConfig();
            config.QuantityAbsolute = config.Quantity;
            config.Quantity = 0;

            config.TotalPriceAbsolute = config.TotalPrice;
            config.TotalPrice = 0;

            config.Actions += config.Type;
            config.Actions += config.Status;
            config.Type = 0;
            config.Status = 0;

            return config;
        }

        getColumnName(field: string, defaultName: string): string {
            switch (field) {
                case "Quantity":
                    return "Requested";
                case "QuantityAbsolute":
                    return "Requested";
                default:
                    return defaultName;
            }
        }

        protected initializeColumns(): kendo.ui.GridColumn[] {
            const columns = super.initializeColumns();
            const selectedColumn: kendo.ui.GridColumn = {
                title: "Selected",
                width: "25px",
                template: (data: InventoryMovementGroupReadModel) => {
                    const outerSpan = document.createElement("div");
                    outerSpan.classList.add("generic-flag-container");
                    outerSpan.classList.add("text-center");
                    outerSpan.setAttribute("data-flag", `/Inventory/${data.SKUId}/Movements/SelectedForOrder/Grouped`);
                    return outerSpan.outerHTML;
                },
            };

            columns.splice(1, 0, selectedColumn);
            return columns;
        }

        protected initializeGrid(grid: kendo.ui.Grid): void {
            super.initializeGrid(grid);
            const nameHeader = grid.wrapper.find("th[data-field=SKUName]")[0];

            let checkboxWrapper = nameHeader.querySelector(".inventory-movements-show-warnings");
            if (!checkboxWrapper) {
                checkboxWrapper = nameHeader.appendChild(document.createElement("div"));
                checkboxWrapper.classList.add("inventory-movements-show-warnings");

                const label = checkboxWrapper.appendChild(document.createElement("label"));
                label.classList.add("inventory-movements-show-warnings__label");
                label.addEventListener("click", e => e.stopPropagation());

                const checkbox = label.appendChild(document.createElement("input"));
                checkbox.type = "checkbox";
                checkbox.classList.add("inventory-movements-show-warnings__checkbox", "k-checkbox");;
                checkbox.addEventListener("click", e => e.stopPropagation());
                checkbox.addEventListener("change", e => {
                    e.stopPropagation();
                    this._showWarnings = !this._showWarnings;
                    this.grid.dataSource["transport"].options.read.url = this.getEndpointUrl();
                    this.grid.dataSource.read();
                });

                label.appendChild(document.createTextNode("Show Warnings"));
            }

            const genericFlags = grid.wrapper.find(".generic-flag-container");
            const containers: HTMLElement[] = [];
            for (let i = 0; i < genericFlags.length; i++) {
                containers.push(genericFlags[i]);
            }

            GenericFlagControl.initContainers(containers);
        }

        protected initializeCommands(): kendo.ui.GridColumnCommandItem[] {
            const commands: kendo.ui.GridColumnCommandItem[] = [];
            commands.push({
                name: "CustomOpenJob",
                iconClass: "fas fa-link",
                text: "&nbsp; Jobs",
                click: (e: JQueryEventObject) => {
                    e.preventDefault();
                    const data = this.grid.dataItem<InventoryMovementGroupReadModel>(e.target.closest("tr"));
                    const url = new DevGuild.AspNet.Routing.Uri(`/InventoryMovements?workshop=${this.workshopId}&sku=${data.SKUId}&tab=${this.getTabName()}&group=false`);
                    url.navigate();
                }
            });
            commands.push({
                name: "CustomOrder",
                className: this.workshopId === "" ? "k-state-disabled" : undefined,
                iconClass: "far fa-check-circle",
                text: "&nbsp; Order",
                click: GridHandlers.createButtonClickPopupHandler<InventoryMovementGroupReadModel>(
                    item => new DevGuild.AspNet.Routing.Uri(`/InventoryMovements/GroupOrder?workshop=${this.workshopId}&sku=${item.SKUId}`),
                    item => ({
                        title: `Order movements`,
                        width: `1000px`,
                        height: `auto`,
                        refresh: (e: kendo.ui.WindowEvent) => {
                            e.sender.wrapper.on("click", ".editor__submit__cancel", clickEvent => {
                                clickEvent.preventDefault();
                                e.sender.close();
                                e.sender.destroy();
                            });
                            e.sender.center();
                        },
                        open: async (e: kendo.ui.WindowEvent) => {
                            await AjaxFormsManager.waitFor("InventoryMovementsGroupOrder");
                            await this.grid.dataSource.read();
                            e.sender.close();
                            e.sender.destroy();
                        },
                    })),
            });
            commands.push({
                name: "CustomOrderWithEdit",
                className: this.workshopId === "" ? "k-state-disabled" : undefined,
                iconClass: "fas fa-check-circle",
                text: "&nbsp; Order with Edit",
                click: GridHandlers.createButtonClickPopupHandler<InventoryMovementGroupReadModel>(
                    item => new DevGuild.AspNet.Routing.Uri(`/InventoryMovements/GroupOrderWithEdit?workshop=${this.workshopId}&sku=${item.SKUId}`),
                    item => ({
                        title: `Order movements with edit`,
                        width: `1000px`,
                        height: `auto`,
                        refresh: (e: kendo.ui.WindowEvent) => {
                            e.sender.wrapper.on("click", ".editor__submit__cancel", clickEvent => {
                                clickEvent.preventDefault();
                                e.sender.close();
                                e.sender.destroy();
                            });
                            e.sender.center();
                        },
                        open: async (e: kendo.ui.WindowEvent) => {
                            await AjaxFormsManager.waitFor("InventoryMovementsGroupOrderWithEdit");
                            await this.grid.dataSource.read();
                            e.sender.close();
                            e.sender.destroy();
                        },
                    })),
            });
            commands.push({
                name: "CustomCancel",
                className: this.workshopId === "" ? "k-state-disabled" : undefined,
                iconClass: "fas fa-ban",
                text: "&nbsp; Cancel",
                click: GridHandlers.createButtonClickPopupHandler<InventoryMovementGroupReadModel>(
                    item => new DevGuild.AspNet.Routing.Uri(`/InventoryMovements/GroupCancel?workshop=${this.workshopId}&sku=${item.SKUId}&status=Approved`),
                    item => ({
                        title: `Cancel movements`,
                        width: `1000px`,
                        height: `auto`,
                        refresh: (e: kendo.ui.WindowEvent) => {
                            e.sender.wrapper.on("click", ".editor__submit__cancel", clickEvent => {
                                clickEvent.preventDefault();
                                e.sender.close();
                                e.sender.destroy();
                            });
                            e.sender.center();
                        },
                        open: async (e: kendo.ui.WindowEvent) => {
                            await AjaxFormsManager.waitFor("InventoryMovementsGroupCancel");
                            await this.grid.dataSource.read();
                            e.sender.close();
                            e.sender.destroy();
                        },
                    })),
            });
            return commands;
        }
    }
}