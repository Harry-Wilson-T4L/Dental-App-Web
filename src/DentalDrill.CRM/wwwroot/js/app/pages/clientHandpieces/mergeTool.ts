namespace DentalDrill.CRM.Pages.ClientHandpieces.MergeTool {
    interface Item {
        Id: string;
    }

    export class ClientHandpieceItemsGrid {
        static handleDataBound(e: kendo.ui.GridDataBoundEvent) {
            const grid = e.sender;
            grid.wrapper.find(".client-handpiece-items__draggable").each((index, element) => {
                const draggableNode = $(element);
                
                if (draggableNode.attr("data-draggable-initialized") === undefined) {
                    draggableNode.attr("data-draggable-initialized", "true");
                }

                draggableNode.kendoDraggable({
                    hint: () => {
                        const row = draggableNode.closest("tr");
                        const dataItem = grid.dataItem<Item>(row);

                        draggableNode.attr("data-client-handpiece-id", grid.wrapper.attr("data-chid"));
                        draggableNode.attr("data-handpiece-id", dataItem.Id);
                        const nodeClone = draggableNode.clone();
                        return nodeClone;
                    },
                    dragstart: e => ClientHandpieceItemsGrid.draggableStart(grid, e),
                    dragend: e => ClientHandpieceItemsGrid.draggableEnd(grid, e),
                });
            });

            if (grid.wrapper.attr("data-drop-initialized") === undefined) {
                grid.wrapper.attr("data-drop-initialized", "true");
                grid.wrapper.kendoDropTarget({
                    drop: e => ClientHandpieceItemsGrid.drop(grid, e),
                })
            }
        }

        static draggableStart(grid: kendo.ui.Grid, e: kendo.ui.DraggableEvent) {
            const handpieceId = e.sender.element.attr("data-handpiece-id");
        }

        static draggableEnd(grid: kendo.ui.Grid, e: kendo.ui.DraggableEvent) {
            const handpieceId = e.sender.element.attr("data-handpiece-id");
        }

        static async drop(grid: kendo.ui.Grid, e: kendo.ui.DropTargetDropEvent): Promise<void> {
            const sourceClientHandpieceId = e.draggable.element.attr("data-client-handpiece-id");
            const destinationClientHandpieceId = grid.wrapper.attr("data-chid");
            const handpieceId = e.draggable.element.attr("data-handpiece-id");
            if (sourceClientHandpieceId !== destinationClientHandpieceId) {
                // console.log(`Dropped HP ${handpieceId} on CHP ${destinationClientHandpieceId} (from ${sourceClientHandpieceId})`);
                const response = await fetch(`/ClientHandpieces/MoveToExisting/?handpieceId=${handpieceId}&clientHandpieceId=${destinationClientHandpieceId}`, {
                    method: "POST",
                    credentials: "same-origin",
                    headers: { "X-Requested-With": "XMLHttpRequest" },
                    body: ""
                });

                if (response.status === 200) {
                    const responseContent = await response.json();
                    if (responseContent && responseContent.Success === true) {
                        // Refresh related grids
                        if (responseContent.RefreshAll == true) {
                            await $("#RepairedItemsFirstGrid").data("kendoGrid").dataSource.read();
                            await $("#RepairedItemsSecondGrid").data("kendoGrid").dataSource.read();
                        } else {
                            const relatedSelectors = [
                                `#ClientHandpiece_${sourceClientHandpieceId}_FirstGrid`,
                                `#ClientHandpiece_${sourceClientHandpieceId}_SecondGrid`,
                                `#ClientHandpiece_${destinationClientHandpieceId}_FirstGrid`,
                                `#ClientHandpiece_${destinationClientHandpieceId}_SecondGrid`
                            ];

                            for (let i = 0; i < relatedSelectors.length; i++) {
                                const gridNode = $(relatedSelectors[i]);
                                if (gridNode.length > 0) {
                                    await gridNode.data("kendoGrid").dataSource.read();
                                }
                            }
                        }
                    }
                }
            }
        }

        static async dropOnNew(e: kendo.ui.DropTargetDropEvent): Promise<void> {
            const sourceClientHandpieceId = e.draggable.element.attr("data-client-handpiece-id");
            const handpieceId = e.draggable.element.attr("data-handpiece-id");
            // console.log(`Dropped HP ${handpieceId} on New CHP (from ${sourceClientHandpieceId})`);
            const response = await fetch(`/ClientHandpieces/MoveToNew/?handpieceId=${handpieceId}`, {
                method: "POST",
                credentials: "same-origin",
                headers: { "X-Requested-With": "XMLHttpRequest" },
                body: ""
            });

            if (response.status === 200) {
                const responseContent = await response.json();
                if (responseContent && responseContent.Success === true) {
                    // Refresh related grids
                    await $("#RepairedItemsFirstGrid").data("kendoGrid").dataSource.read();
                    await $("#RepairedItemsSecondGrid").data("kendoGrid").dataSource.read();
                }
            }
        }
    }

    $(() => {
        $("#ClientHandpieceNewArea").kendoDropTarget({
            drop: e => ClientHandpieceItemsGrid.dropOnNew(e),
        });
    })
}