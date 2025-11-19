namespace DentalDrill.CRM.Pages.InventoryMovements.Index {
    import GridHandlers = DevGuild.AspNet.Controls.Grids.Handlers.GridHandlers;

    export class GridInventoryMovementsCompleteTab extends GridInventoryMovementsTabBase {
        constructor(id: string, root: HTMLElement, options: InventoryMovementsIndexOptions) {
            super(id, root, options);
        }

        getEndpointUrl(): string {
            return "/InventoryMovements/ReadComplete";
        }

        initializeCommands(): kendo.ui.GridColumnCommandItem[] {
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