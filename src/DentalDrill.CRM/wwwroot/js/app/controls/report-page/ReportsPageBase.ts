namespace DentalDrill.CRM.Controls.Reporting {
    import EventHandler = DevGuild.Utilities.EventHandler;

    export abstract class ReportsPageBase<
        TPageIdentifier extends ReportsPageIdentifierBase,
        TGlobalFilters extends ReportsPageGlobalFiltersBase,
        TMainControls extends ReportsPageMainControlsBase<TGlobalFilters>,
        TTabsCollection extends ReportsPageTabCollectionBase<TGlobalFilters>
    > {

        private readonly _root: JQuery<HTMLElement>;
        private readonly _identifier: TPageIdentifier;
        private readonly _mainControls: TMainControls;
        private readonly _tabsCollection: TTabsCollection;

        constructor(root: JQuery<HTMLElement>) {
            this._root = root;
            this._identifier = this.loadIdentifier(root);
            this._mainControls = this.loadMainControls(root, this._identifier);
            this._tabsCollection = this.loadTabs(root, this._identifier, this._mainControls);

            this._mainControls.changed.subscribe(async (sender, args) => {
                await this._tabsCollection.applyGlobalFiltersToAll(args.globalFilters);
                await this._tabsCollection.initializeAll();
            });
        }

        async initialize(): Promise<void> {
            const globalFilters = await this._mainControls.getGlobalFilters();
            await this._tabsCollection.applyGlobalFiltersToAll(globalFilters);
            await this._tabsCollection.initializeAll();
        }

        get identifier(): TPageIdentifier {
            return this._identifier;
        }

        protected abstract loadIdentifier(root: JQuery<HTMLElement>): TPageIdentifier;

        protected abstract loadMainControls(root: JQuery<HTMLElement>, identifier: TPageIdentifier): TMainControls;

        protected abstract loadTabs(root: JQuery<HTMLElement>, identifier: TPageIdentifier, mainControls: TMainControls): TTabsCollection;
    }

    export abstract class ReportsPageIdentifierBase {
    }

    export interface ReportsPageMainControlsChanged<TGlobalFilters extends ReportsPageGlobalFiltersBase> {
        globalFilters: TGlobalFilters;
    }

    export abstract class ReportsPageMainControlsBase<TGlobalFilters extends ReportsPageGlobalFiltersBase> {
        private readonly _changed: EventHandler<ReportsPageMainControlsChanged<TGlobalFilters>>;

        constructor() {
            this._changed = new EventHandler<ReportsPageMainControlsChanged<TGlobalFilters>>();
        }

        get changed(): EventHandler<ReportsPageMainControlsChanged<TGlobalFilters>> {
            return this._changed;
        }

        abstract getGlobalFilters(): Promise<TGlobalFilters>;

        protected raiseChanged(globalFilters: TGlobalFilters): void {
            this._changed.raise(this, { globalFilters: globalFilters });
        }
    }

    export abstract class ReportsPageTabCollectionBase<TGlobalFilters extends ReportsPageGlobalFiltersBase> {
        abstract applyGlobalFiltersToAll(globalFilters: TGlobalFilters): Promise<void>;
        abstract initializeAll(): Promise<void>;
    }

    export abstract class ReportsPageGlobalFiltersBase {
    }

    export abstract class ReportsPageTabBase<TGlobalFilters> {
        abstract applyGlobalFilters(globalFilters: TGlobalFilters): Promise<void>;
        abstract initialize(): Promise<void>;
    }

    export abstract class ReportsPageTabCollection<TGlobalFilters extends ReportsPageGlobalFiltersBase> extends ReportsPageTabCollectionBase<TGlobalFilters> {
        private _tabs: ReportsPageTabBase<TGlobalFilters>[] = [];

        protected addTab(tab: ReportsPageTabBase<TGlobalFilters>): void {
            this._tabs.push(tab);
        }

        async applyGlobalFiltersToAll(globalFilters: TGlobalFilters): Promise<void> {
            for (let tab of this._tabs) {
                await tab.applyGlobalFilters(globalFilters);
            }
        }

        async initializeAll(): Promise<void> {
            for (let tab of this._tabs) {
                await tab.initialize();
            }
        }
    }
}