namespace DentalDrill.CRM.Pages.InventoryMovements.Index {
    import AjaxFormsManager = DevGuild.AspNet.Forms.AjaxForms.AjaxFormsManager;

    interface InventoryMovementsTabPageDescriptor {
        Title: string;
        Content: string;
    }

    export enum InventoryMovementsTabsSetState {
        Default,
        Minimized,
        Maximized,
    }

    export class InventoryMovementsTabsSet {
        private readonly _collection: InventoryMovementsTabsSetCollection;
        private readonly _container: HTMLElement;
        private readonly _descriptors: InventoryMovementsTabPageDescriptor[];

        private readonly _tabFactory: InventoryMovementsTabFactory;
        private readonly _tabs: Map<string, InventoryMovementsTabBase>;
        private readonly _tabStrip: kendo.ui.TabStrip;

        private _state: InventoryMovementsTabsSetState = InventoryMovementsTabsSetState.Default;
        private _activeTab: InventoryMovementsTabBase;

        constructor(collection: InventoryMovementsTabsSetCollection, container: HTMLElement, options: InventoryMovementsIndexOptions, descriptors: InventoryMovementsTabPageDescriptor[]) {
            collection.add(this);
            this._collection = collection;
            this._container = container;
            this._descriptors = descriptors;

            this._tabFactory = new InventoryMovementsTabFactory(options);
            this._tabs = new Map<string, InventoryMovementsTabBase>();
            this._tabStrip = this.createTabStrip();
            this.createToolbar();
        }

        get state(): InventoryMovementsTabsSetState {
            return this._state;
        }

        get activeTab(): InventoryMovementsTabBase {
            return this._activeTab;
        }

        select(tabIndex: number): void {
            this._tabStrip.select(tabIndex);
        }

        private createTabStrip(): kendo.ui.TabStrip {
            const element = this._container.appendChild(document.createElement("div"));
            const dataSource = new kendo.data.DataSource({
                data: this._descriptors,
            });
            return $(element).kendoTabStrip({
                dataSource: dataSource,
                dataTextField: "Title",
                dataContentField: "Content",
                activate: (e: kendo.ui.TabStripActivateEvent) => {
                    const tabContent = e.contentElement.querySelector(".inventory-movements-tab");
                    if (!tabContent) {
                        throw new Error("Invalid tab content");
                    }

                    const tabId = tabContent.getAttribute("data-id");
                    if (!tabId) {
                        throw new Error("Invalid tab id");
                    }

                    if (this._tabs.has(tabId)) {
                        const tab = this._tabs.get(tabId);
                        this._activeTab = tab;
                        tab.activate();
                    } else {
                        const tab = this._tabFactory.createTab(tabId, tabContent as HTMLElement);
                        this._tabs.set(tabId, tab);
                        this._activeTab = tab;
                        tab.init();
                        tab.activate();
                    }
                }
            }).data("kendoTabStrip");
        }

        private createToolbar(): void {
            const element = this._container.appendChild(document.createElement("div"));
            element.classList.add("inventory-movements__toolbar");
            element.classList.add("btn-group");

            const buttonMinimize = element.appendChild(document.createElement("button"));
            buttonMinimize.type = "button";
            buttonMinimize.classList.add("btn", "btn-sm", "btn-outline-secondary", "inventory-movements--when-not-minimized");
            buttonMinimize.appendChild(document.createElement("span")).classList.add("far", "fa-fw", "fa-window-minimize");
            buttonMinimize.addEventListener("click", e => {
                this._collection.minimize(this);
            });

            const buttonRestore = element.appendChild(document.createElement("button"));
            buttonRestore.type = "button";
            buttonRestore.classList.add("btn", "btn-sm", "btn-outline-secondary", "inventory-movements--when-not-default");
            buttonRestore.appendChild(document.createElement("span")).classList.add("far", "fa-fw", "fa-window-restore");
            buttonRestore.addEventListener("click", e => {
                this._collection.restore(this);
            });

            const buttonMaximize = element.appendChild(document.createElement("button"));
            buttonMaximize.type = "button";
            buttonMaximize.classList.add("btn", "btn-sm", "btn-outline-secondary", "inventory-movements--when-not-maximized");
            buttonMaximize.appendChild(document.createElement("span")).classList.add("far", "fa-fw", "fa-window-maximize");
            buttonMaximize.addEventListener("click", e => {
                this._collection.maximize(this);
            });
        }

        minimize() {
            this._state = InventoryMovementsTabsSetState.Minimized;
            this._container.classList.add("inventory-movements--minimized");
            this._container.classList.remove("inventory-movements--maximized");
            this._container.classList.remove("inventory-movements--default");
            this.resize(false);
        }

        maximize() {
            this._state = InventoryMovementsTabsSetState.Maximized;
            this._container.classList.remove("inventory-movements--minimized");
            this._container.classList.add("inventory-movements--maximized");
            this._container.classList.remove("inventory-movements--default");
            this.resize(true);
        }

        restore() {
            this._state = InventoryMovementsTabsSetState.Default;
            this._container.classList.remove("inventory-movements--minimized");
            this._container.classList.remove("inventory-movements--maximized");
            this._container.classList.add("inventory-movements--default");
            this.resize(true);
        }

        resize(visible: boolean) {
            setTimeout(() => {
                this._tabs.forEach(tab => {
                    tab.resize(visible);
                });
            });
        }
    }

    export class InventoryMovementsTabsSetCollection {
        private readonly _tabSets: InventoryMovementsTabsSet[] = [];

        constructor() {
        }

        add(tabSet: InventoryMovementsTabsSet): void {
            this._tabSets.push(tabSet);
        }

        minimize(tabSet: InventoryMovementsTabsSet) {
            tabSet.minimize();
            const others = this.getOthers(tabSet);
            if (others.length === 1) {
                others[0].maximize();
                return;
            }
        }

        maximize(tabSet: InventoryMovementsTabsSet) {
            tabSet.maximize();
            const others = this.getOthers(tabSet);
            for (let other of others.filter(x => x.state !== InventoryMovementsTabsSetState.Minimized)) {
                other.minimize();
            }
        }

        restore(tabSet: InventoryMovementsTabsSet) {
            tabSet.restore();
            const others = this.getOthers(tabSet);
            for (let other of others) {
                other.restore();
            }
        }

        private getOthers(except: InventoryMovementsTabsSet): InventoryMovementsTabsSet[] {
            return this._tabSets.filter(x => x !== except);
        }
    }

    export class InventoryMovementsIndexPage {
        private readonly _root: HTMLElement;
        private readonly _options: InventoryMovementsIndexOptions;

        private _collection: InventoryMovementsTabsSetCollection;
        private _movements: InventoryMovementsTabsSet;
        private _charts: InventoryMovementsTabsSet;

        constructor(root: HTMLElement, options: InventoryMovementsIndexOptions) {
            this._root = root;
            this._options = options;
        }

        init() {
            this._collection = new InventoryMovementsTabsSetCollection();
            this._movements = new InventoryMovementsTabsSet(this._collection, this._root.querySelector(".inventory-movements__movements"), this._options, this.createMovementsTabStripDataSource());
            this._charts = new InventoryMovementsTabsSet(this._collection, this._root.querySelector(".inventory-movements__charts"), this._options, this.createChartsTabStripDataSource());

            this._movements.select(this.getInitialMovementsTab());
            this._charts.select(0);

            this.initActions();
        }

        private getInitialMovementsTab() {
            if (this._options.showGrouped) {
                switch (this._options.tab) {
                    case "Approved":
                        return 0;
                    case "Requested":
                        return 1;
                    case "Ordered":
                        return 2;
                    case "Complete":
                        return 3;
                    case "All":
                        return 4;
                    default:
                        return 0;
                }
            } else {
                switch (this._options.tab) {
                    case "Tray":
                        return 0;
                    case "Approved":
                        return 0;
                    case "Requested":
                        return 1;
                    case "Ordered":
                        return 2;
                    case "Complete":
                        return 3;
                    case "All":
                        return 4;
                    default:
                        return 0;
                }
            }

            return 0;
        }

        private createMovementsTabStripDataSource(): InventoryMovementsTabPageDescriptor[] {
            const data: InventoryMovementsTabPageDescriptor[] = [];
            if (this._options.showGrouped) {
                data.push({
                    Title: "Order",
                    Content: this.getMovementTabContent("ApprovedGroup"),
                });
                data.push({
                    Title: "Not Approved",
                    Content: this.getMovementTabContent("RequestedGroup"),
                });
                data.push({
                    Title: "Verify",
                    Content: this.getMovementTabContent("OrderedGroup"),
                });
                data.push({
                    Title: "Complete",
                    Content: this.getMovementTabContent("CompleteGroup"),
                });
                data.push({
                    Title: "All",
                    Content: this.getMovementTabContent("AllGroup"),
                });
            } else {
                if (this._options.tab === `Tray`) {
                    data.push({
                        Title: "Tray",
                        Content: this.getMovementTabContent("Tray"),
                    });
                }

                data.push({
                    Title: "Order",
                    Content: this.getMovementTabContent("Approved"),
                });
                data.push({
                    Title: "Not Approved",
                    Content: this.getMovementTabContent("Requested"),
                });
                data.push({
                    Title: "Verify",
                    Content: this.getMovementTabContent("Ordered"),
                });
                data.push({
                    Title: "Complete",
                    Content: this.getMovementTabContent("Complete"),
                });
                data.push({
                    Title: "All",
                    Content: this.getMovementTabContent("All"),
                });
            }

            return data;
        }

        private createChartsTabStripDataSource(): InventoryMovementsTabPageDescriptor[] {
            const data: InventoryMovementsTabPageDescriptor[] = [];
            data.push({
                Title: "Available Stock",
                Content: this.getMovementTabContent("StatsAvailableStock"),
            });

            return data;
        }

        private getMovementTabContent(id: string): string {
            return `<div class="inventory-movements-tab" data-id="${id}">Loading...</div>`;
        }

        private initActions() {
            const createButton = this._root.querySelector(".inventory-movements__actions__create");
            createButton.addEventListener("click", (e: MouseEvent) => {
                e.preventDefault();
                const url = new DevGuild.AspNet.Routing.Uri(createButton.getAttribute("href"));

                if (e.ctrlKey) {
                    url.open();
                } else {
                    const dialogRoot = $("<div></div>");
                    const dialogOptions = {
                        title: `Move SKU`,
                        actions: ["close"],
                        content: url.value,
                        width: "800px",
                        height: "auto",
                        modal: true,
                        visible: false,
                        close: () => dialogRoot.data("kendoWindow").destroy(),
                        refresh: (e: kendo.ui.WindowEvent) => {
                            e.sender.wrapper.on("click", ".editor__submit__cancel", clickEvent => {
                                clickEvent.preventDefault();
                                e.sender.close();
                                e.sender.destroy();
                            });
                            e.sender.center();
                        },
                        open: async (e: kendo.ui.WindowEvent) => {
                            await AjaxFormsManager.waitFor("InventoryMovementsCreate");
                            if (this._movements && this._movements.activeTab) {
                                this._movements.activeTab.activate();
                            }

                            e.sender.close();
                            e.sender.destroy();
                        },
                    };

                    const dialog = (dialogRoot as any).kendoWindow(dialogOptions).data("kendoWindow") as kendo.ui.Window;
                    dialog.center();
                    dialog.open();
                }
            });
        }
    }
}