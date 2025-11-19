namespace DentalDrill.CRM.Pages.Stock.Week {
    import EventHandler = DevGuild.Utilities.EventHandler;

    export interface WeekInfo {
        weekId: string;
        weekName: string;
        hasPrevious: boolean;
        hasNext: boolean;
    }

    export class WeekChangedEventArgs {
        private readonly _weekId: string;
        private readonly _previousWeekId: string;

        constructor(weekId: string, previousWeekId: string) {
            this._weekId = weekId;
            this._previousWeekId = previousWeekId;
        }

        get weekId(): string {
            return this._weekId;
        }

        get previousWeekId(): string {
            return this._previousWeekId;
        }
    }

    export class WeekSelector {
        private readonly _root: HTMLElement;
        private readonly _nameElement: HTMLElement;
        private readonly _previousElement: HTMLElement;
        private readonly _nextElement: HTMLElement;

        private readonly _weekChanged: EventHandler<WeekChangedEventArgs>;

        private _weekId: string;

        constructor(root: HTMLElement) {
            this._weekChanged = new EventHandler<WeekChangedEventArgs>();

            this._root = root;
            this._nameElement = this._root.querySelector(".week-selector__name") as HTMLElement;
            this._previousElement = this._root.querySelector(".week-selector__previous") as HTMLElement;
            this._nextElement = this._root.querySelector(".week-selector__next") as HTMLElement;

            this._weekId = this._root.getAttribute("data-week-id");
            this._previousElement.addEventListener("click", async e => {
                await this.handlePrevious();
            });
            this._nextElement.addEventListener("click", async e => {
                await this.handleNext();
            });
        }

        static init(root: HTMLElement) : WeekSelector {
            const item = new WeekSelector(root);
            $(root).data("weekSelector", item);
            return item;
        }

        get weekId(): string {
            return this._weekId;
        }

        get weekChanged(): EventHandler<WeekChangedEventArgs> {
            return this._weekChanged;
        }

        private async handlePrevious() : Promise<void> {
            const originalWeek = this.weekId;
            const previousWeek = await this.fetchPreviousWeek();
            if (previousWeek && originalWeek === this.weekId) {
                this._weekId = previousWeek.weekId;
                this._root.setAttribute("data-week-id", previousWeek.weekId);
                this._nameElement.innerText = previousWeek.weekName;

                this._root.classList.toggle("week-selector--has-previous", previousWeek.hasPrevious);
                this._root.classList.toggle("week-selector--has-next", previousWeek.hasNext);

                this._weekChanged.raise(this, new WeekChangedEventArgs(this.weekId, originalWeek));
            }
        }

        private async handleNext(): Promise<void> {
            const originalWeek = this.weekId;
            const nextWeek = await this.fetchNextWeek();
            if (nextWeek && originalWeek === this.weekId) {
                this._weekId = nextWeek.weekId;
                this._root.setAttribute("data-week-id", nextWeek.weekId);
                this._nameElement.innerText = nextWeek.weekName;

                this._root.classList.toggle("week-selector--has-previous", nextWeek.hasPrevious);
                this._root.classList.toggle("week-selector--has-next", nextWeek.hasNext);

                this._weekChanged.raise(this, new WeekChangedEventArgs(this.weekId, originalWeek));
            }
        }

        private async fetchPreviousWeek(): Promise<WeekInfo> {
            const response = await fetch(`/Calendar/PreviousWeek/${this.weekId}`, {
                method: "POST",
                credentials: "same-origin",
                cache: "no-cache",
                body: ""
            });

            if (response.status === 200) {
                const data = await response.json() as WeekInfo;
                return data;
            }

            return undefined;
        }

        private async fetchNextWeek(): Promise<WeekInfo> {
            const response = await fetch(`/Calendar/NextWeek/${this.weekId}`, {
                method: "POST",
                credentials: "same-origin",
                cache: "no-cache",
                body: ""
            });

            if (response.status === 200) {
                const data = await response.json() as WeekInfo;
                return data;
            }

            return undefined;
        }
    }

    export class StockControlPartsOutGrid {
        static get instance(): kendo.ui.Grid {
            return $("#StockControlPartsOutGrid").data("kendoGrid");
        }

        static handleOpen = DevGuild.AspNet.Controls.Grids.Handlers.GridHandlers.createButtonClickHandler<{HandpieceId: string}>(item => {
            routes.handpieces.edit(item.HandpieceId).open();
        });

        static async updateStatus(id: string, status: string, weekId?: string) : Promise<boolean> {
            let baseUrl = `/Stock/UpdateStatus/${id}?status=${status}`;
            if (weekId) {
                baseUrl = baseUrl + `&weekId=${weekId}`;
            }

            const response = await fetch(baseUrl, {
                method: "POST",
                credentials: "same-origin",
                cache: "no-cache",
                body: ""
            });

            if (response.status === 200) {
                return true;
            } else {
                return false;
            }
        }
    }

    $(() => {
        const weekSelector = WeekSelector.init(document.getElementById("StockControlPartsOutGrid_WeekSelector"));
        weekSelector.weekChanged.subscribe(async (sender, e) => {
            const grid = StockControlPartsOutGrid.instance;
            const transport = grid.dataSource["transport"] as any;
            transport.options.read.url = `/Stock/ReadHandpiecesWithPartsOut?weekId=${e.weekId}`;
            await grid.dataSource.read();
        });

        StockControlPartsOutGrid.instance.wrapper.on("click", ".stock__field__ordered", async (e: JQueryEventObject) => {
            const checkbox = e.target as HTMLInputElement;
            const item = StockControlPartsOutGrid.instance.dataItem<{Id: string}>(e.target.closest("tr"));
            if (checkbox.checked) {
                if (await StockControlPartsOutGrid.updateStatus(item.Id, "Ordered", weekSelector.weekId)) {
                    item.set("Ordered", true);
                } else {
                    checkbox.checked = false;
                }
            } else {
                if (await StockControlPartsOutGrid.updateStatus(item.Id, "Active")) {
                    item.set("Ordered", false);
                } else {
                    checkbox.checked = true;
                }
            }
        });

        StockControlPartsOutGrid.instance.wrapper.on("click", ".stock__field__ignored", async (e: JQueryEventObject) => {
            const checkbox = e.target as HTMLInputElement;
            const item = StockControlPartsOutGrid.instance.dataItem<{Id: string}>(e.target.closest("tr"));
            if (checkbox.checked) {
                if (await StockControlPartsOutGrid.updateStatus(item.Id, "Ignored", weekSelector.weekId)) {
                    item.set("Ignored", true);
                } else {
                    checkbox.checked = false;
                }
            } else {
                if (await StockControlPartsOutGrid.updateStatus(item.Id, "Active")) {
                    item.set("Ignored", false);
                } else {
                    checkbox.checked = true;
                }
            }
        });
    });
}