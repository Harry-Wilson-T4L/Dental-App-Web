namespace DentalDrill.CRM.Pages.InventoryMovements.Index {
    export class GridInventoryGroupMovementsCompleteTab extends GridInventoryGroupMovementsTabBase {
        constructor(id: string, root: HTMLElement, options: InventoryMovementsIndexOptions) {
            super(id, root, options);
        }

        protected getTabName(): string {
            return `Complete`;
        }

        protected getEndpointUrl(): string {
            return "/InventoryMovements/ReadGroupComplete";
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
            return commands;
        }
    }
}