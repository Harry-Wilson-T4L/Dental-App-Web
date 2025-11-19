namespace DentalDrill.CRM.Pages.InventoryMovements.Index {
    import GridHandlers = DevGuild.AspNet.Controls.Grids.Handlers.GridHandlers;
    import AjaxFormsManager = DevGuild.AspNet.Forms.AjaxForms.AjaxFormsManager;

    export class GridInventoryMovementsOrderedTab extends GridInventoryMovementsTabBase {
        private readonly _api: IInventoryMovementApi;

        constructor(id: string, root: HTMLElement, options: InventoryMovementsIndexOptions, api: IInventoryMovementApi) {
            super(id, root, options);
            this._api = api;
        }

        getEndpointUrl(): string {
            return "/InventoryMovements/ReadOrdered";
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
                name: "CustomVerify",
                className: this.workshopId === "" ? "k-state-disabled" : undefined,
                iconClass: "far fa-check-circle",
                text: "&nbsp; Verify",
                click: GridHandlers.createButtonClickPopupHandler<InventoryMovementReadModel>(
                    item => new DevGuild.AspNet.Routing.Uri(`/InventoryMovements/Verify/${item.Id}`),
                    item => ({
                        title: `Verify Movement`,
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
                            await AjaxFormsManager.waitFor("InventoryMovementsVerify");
                            await this.grid.dataSource.read();
                            e.sender.close();
                            e.sender.destroy();
                        },
                    })),
            });
            commands.push({
                name: "CustomVerifyWithEdit",
                className: this.workshopId === "" ? "k-state-disabled" : undefined,
                iconClass: "fas fa-check-circle",
                text: "&nbsp; Verify with Edit",
                click: GridHandlers.createButtonClickPopupHandler<InventoryMovementReadModel>(
                    item => new DevGuild.AspNet.Routing.Uri(`/InventoryMovements/VerifyWithEdit/${item.Id}`),
                    item => ({
                        title: `Verify Movement`,
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
                            await AjaxFormsManager.waitFor("InventoryMovementsVerifyWithEdit");
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