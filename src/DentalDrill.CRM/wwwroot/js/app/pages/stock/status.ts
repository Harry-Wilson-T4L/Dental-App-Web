namespace DentalDrill.CRM.Pages.Stock.Status {
    export class StockControlBeingRepairedGrid {
        static get instance(): kendo.ui.Grid {
            return $("#StockControlBeingRepairedGrid").data("kendoGrid");
        }

        static handleOpen = DevGuild.AspNet.Controls.Grids.Handlers.GridHandlers.createButtonClickHandler<{Id: string}>(item => {
            routes.handpieces.edit(item.Id).open();
        });
    }

    export class StockControlWaitingApprovalGrid {
        static get instance(): kendo.ui.Grid {
            return $("#StockControlWaitingApprovalGrid").data("kendoGrid");
        }

        static handleOpen = DevGuild.AspNet.Controls.Grids.Handlers.GridHandlers.createButtonClickHandler<{Id: string}>(item => {
            routes.handpieces.edit(item.Id).open();
        });
    }

    export class CollapseContainer {
        private readonly _root: HTMLElement;
        private readonly _toggle: JQuery;
        private readonly _content: JQuery;
        private readonly _chevron: JQuery;
        private readonly _chevronExpandedClass: string;
        private readonly _chevronCollapsedClass: string;

        constructor(root: HTMLElement) {
            this._root = root;
            this._toggle = $(this._root.querySelector(".collapse-toggle") as HTMLElement);
            this._content = $(this._root.querySelector(".collapse") as HTMLElement);
            this._chevron = $(this._root.querySelector(".collapse-toggle-chevron") as HTMLElement);
            this._chevronExpandedClass = this._chevron.attr("data-chevron-expanded");
            this._chevronCollapsedClass = this._chevron.attr("data-chevron-collapsed");

            this._content.on("show.bs.collapse", e => {
                this._chevron.removeClass(this._chevronCollapsedClass);
                this._chevron.addClass(this._chevronExpandedClass);
            });

            this._content.on("hide.bs.collapse", e => {
                this._chevron.addClass(this._chevronCollapsedClass);
                this._chevron.removeClass(this._chevronExpandedClass);
            });

            this._toggle.on("click", e => {
                this.toggle();
            });

            $(this._root).data("collapseContainer", this);
        }

        private show(): void {
            this._content.collapse("show");
        }

        private hide(): void {
            this._content.collapse("hide");
        }

        private toggle(): void {
            this._content.collapse("toggle");
        }
    }

    async function updateOrderedStatus(id: string, ordered: boolean): Promise<boolean> {
        let baseUrl = `/Stock/UpdateOrderedStatus/${id}?ordered=${ordered}`;
        
        const response = await fetch(baseUrl, {
            method: "POST",
            credentials: "same-origin",
            cache: "no-cache",
            body: ""
        });

        if (response.status === 200 || response.status === 204) {
            return true;
        } else {
            return false;
        }
    }

    $(() => {
        const collapseContainers = document.querySelectorAll(".collapse-container");
        for (let i = 0; i < collapseContainers.length; i++) {
            const container = new CollapseContainer(collapseContainers[i] as HTMLElement);
        }

        StockControlBeingRepairedGrid.instance.wrapper.on("click", ".stock__field__ordered", async (e: JQueryEventObject) => {
            const checkbox = e.target as HTMLInputElement;
            const item = StockControlBeingRepairedGrid.instance.dataItem<{Id: string}>(e.target.closest("tr"));
            if (checkbox.checked) {
                if (await updateOrderedStatus(item.Id, true)) {
                    item.set("Ordered", true);
                } else {
                    checkbox.checked = false;
                }
            } else {
                if (await updateOrderedStatus(item.Id, false)) {
                    item.set("Ordered", false);
                } else {
                    checkbox.checked = true;
                }
            }
        });

        StockControlWaitingApprovalGrid.instance.wrapper.on("click", ".stock__field__ordered", async (e: JQueryEventObject) => {
            const checkbox = e.target as HTMLInputElement;
            const item = StockControlWaitingApprovalGrid.instance.dataItem<{Id: string}>(e.target.closest("tr"));
            if (checkbox.checked) {
                if (await updateOrderedStatus(item.Id, true)) {
                    item.set("Ordered", true);
                } else {
                    checkbox.checked = false;
                }
            } else {
                if (await updateOrderedStatus(item.Id, false)) {
                    item.set("Ordered", false);
                } else {
                    checkbox.checked = true;
                }
            }
        });
    });
}