namespace DentalDrill.CRM.Pages.InventoryMovements.Index {
    import GridHandlers = DevGuild.AspNet.Controls.Grids.Handlers.GridHandlers;
    import AjaxFormsManager = DevGuild.AspNet.Forms.AjaxForms.AjaxFormsManager;

    export class GridInventoryGroupMovementsRequestedTab extends GridInventoryGroupMovementsTabBase {
        private readonly _api: IInventoryMovementApi;

        constructor(id: string, root: HTMLElement, options: InventoryMovementsIndexOptions, api: IInventoryMovementApi) {
            super(id, root, options);
            this._api = api;
        }

        protected getTabName(): string {
            return `Requested`;
        }

        protected getEndpointUrl(): string {
            return "/InventoryMovements/ReadGroupRequested";
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
                name: "CustomApprove",
                className: this.workshopId === "" ? "k-state-disabled" : undefined,
                iconClass: "far fa-check-circle",
                text: "&nbsp; Approve",
                click: GridHandlers.createButtonClickPopupHandler<InventoryMovementReadModel>(
                    item => new DevGuild.AspNet.Routing.Uri(`/InventoryMovements/GroupApprove?workshop=${this.workshopId}&sku=${item.SKUId}`),
                    item => ({
                        title: `Approve movements`,
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
                            await AjaxFormsManager.waitFor("InventoryMovementsGroupApprove");
                            await this.grid.dataSource.read();
                            e.sender.close();
                            e.sender.destroy();
                        },
                    })),
            });
            commands.push({
                name: "CustomApproveWithEdit",
                className: this.workshopId === "" ? "k-state-disabled" : undefined,
                iconClass: "fas fa-check-circle",
                text: "&nbsp; Approve with Edit",
                click: GridHandlers.createButtonClickPopupHandler<InventoryMovementReadModel>(
                    item => new DevGuild.AspNet.Routing.Uri(`/InventoryMovements/GroupApproveWithEdit?workshop=${this.workshopId}&sku=${item.SKUId}`),
                    item => ({
                        title: `Approve movements with edit`,
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
                            await AjaxFormsManager.waitFor("InventoryMovementsGroupApproveWithEdit");
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
                    item => new DevGuild.AspNet.Routing.Uri(`/InventoryMovements/GroupCancel?workshop=${this.workshopId}&sku=${item.SKUId}&status=Requested`),
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