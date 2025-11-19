namespace DentalDrill.CRM.Pages.InventoryMovements.Index {
    export class GridInventoryMovementsTrayTab extends GridInventoryMovementsTabBase {
        constructor(id: string, root: HTMLElement, options: InventoryMovementsIndexOptions) {
            super(id, root, options);
        }

        protected getEndpointUrl(): string {
            return "/InventoryMovements/ReadTray";
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
            return commands;
        }
    }
}