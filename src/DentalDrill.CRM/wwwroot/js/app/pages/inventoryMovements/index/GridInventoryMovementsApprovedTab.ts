namespace DentalDrill.CRM.Pages.InventoryMovements.Index {
    import GridHandlers = DevGuild.AspNet.Controls.Grids.Handlers.GridHandlers;
    import AjaxFormsManager = DevGuild.AspNet.Forms.AjaxForms.AjaxFormsManager;
    import InventoryMovementType = Shared.InventoryMovementType;

    export class GridInventoryMovementsApprovedTab extends GridInventoryMovementsTabBase {
        private readonly _api: IInventoryMovementApi;
        private _showWarnings: boolean = false;

        constructor(id: string, root: HTMLElement, options: InventoryMovementsIndexOptions, api: IInventoryMovementApi) {
            super(id, root, options);
            this._api = api;
        }

        getEndpointUrl(): string {
            return this._showWarnings
                ? "/InventoryMovements/ReadApprovedAndMissing"
                : "/InventoryMovements/ReadApproved";
        }

        protected getColumnsConfig(): GridInventoryMovementsColumnsConfig {
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
        }

        protected initializeRow(row: HTMLTableRowElement, dataItem: InventoryMovementReadModel): void {
            super.initializeRow(row, dataItem);
            const cancelButton = row.querySelector("a.k-grid-CustomCancel")
            if (dataItem.Type === InventoryMovementType.EphemeralMissingRequiredQuantity) {
                cancelButton.classList.add("k-state-disabled");
            } else {
                cancelButton.classList.remove("k-state-disabled");
            }
        }

        protected initializeCommands(): kendo.ui.GridColumnCommandItem[] {
            const commands: kendo.ui.GridColumnCommandItem[] = [];
            commands.push({
                name: "CustomOpenJob",
                iconClass: "fas fa-link",
                text: "&nbsp; Job",
                click: (e: JQueryEventObject) => {
                    e.preventDefault();
                    const dataItem = this.grid.dataItem<InventoryMovementReadModel>(e.currentTarget.closest("tr"));
                    if (!dataItem) {
                        return;
                    }

                    if (dataItem.HandpieceId) {
                        const url = routes.handpieces.edit(dataItem.HandpieceId);
                        url.open();
                    }
                }
            });
            commands.push({
                name: "CustomOrder",
                className: this.workshopId === "" ? "k-state-disabled" : undefined,
                iconClass: "far fa-check-circle",
                text: "&nbsp; Order",
                click: GridHandlers.createButtonClickPopupHandler<InventoryMovementReadModel>(
                    item => {
                        if (item.Type === InventoryMovementType.EphemeralMissingRequiredQuantity) {
                            return new DevGuild.AspNet.Routing.Uri(`/InventoryMovements/OrderMissing?workshop=${this.workshopId}&sku=${item.Id}`);
                        } else {
                            return new DevGuild.AspNet.Routing.Uri(`/InventoryMovements/Order/${item.Id}`);
                        }
                    },
                    item => ({
                        title: `Order Movement`,
                        width: `800px`,
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
                            if (item.Type === InventoryMovementType.EphemeralMissingRequiredQuantity) {
                                await AjaxFormsManager.waitFor("InventoryMovementsOrderMissing");
                            } else {
                                await AjaxFormsManager.waitFor("InventoryMovementsOrder");
                            }
                            
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
                click: GridHandlers.createButtonClickPopupHandler<InventoryMovementReadModel>(
                    item => {
                        if (item.Type === InventoryMovementType.EphemeralMissingRequiredQuantity) {
                            return new DevGuild.AspNet.Routing.Uri(`/InventoryMovements/OrderMissingWithEdit?workshop=${this.workshopId}&sku=${item.Id}`);
                        } else {
                            return new DevGuild.AspNet.Routing.Uri(`/InventoryMovements/OrderWithEdit/${item.Id}`);
                        }
                    },
                    item => ({
                        title: `Order Movement`,
                        width: `800px`,
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
                            if (item.Type === InventoryMovementType.EphemeralMissingRequiredQuantity) {
                                await AjaxFormsManager.waitFor("InventoryMovementsOrderMissingWithEdit");
                            } else {
                                await AjaxFormsManager.waitFor("InventoryMovementsOrderWithEdit");
                            }

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
                click: GridHandlers.createButtonClickPopupHandler<InventoryMovementReadModel>(
                    item => new DevGuild.AspNet.Routing.Uri(`/InventoryMovements/Cancel/${item.Id}`),
                    item => ({
                        title: `Cancel Movement`,
                        width: `800px`,
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
                            await AjaxFormsManager.waitFor("InventoryMovementsCancel");
                            await this.grid.dataSource.read();
                            e.sender.close();
                            e.sender.destroy();
                        },
                    })),
            });
            commands.push({
                name: "CustomHistory",
                iconClass: "fas fa-history",
                text: "&nbsp;",
                click: GridHandlers.createButtonClickPopupHandler<InventoryMovementGroupReadModel>(
                    item => new DevGuild.AspNet.Routing.Uri(`/InventoryMovements/History/${item.Id}`),
                    item => ({
                        title: `Move history`,
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
                    })),
            });
            return commands;
        }
    }
}