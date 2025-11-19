namespace DentalDrill.CRM.Controls {
    import EventHandler = DevGuild.Utilities.EventHandler;

    export class ToggleListItem {
        private readonly _root: JQuery;
        private readonly _key: string;

        private _active: boolean;

        constructor(root: JQuery) {
            this._root = root;
            this._key = root.attr("data-key");
            this._active = root.hasClass("toggle-list__item--active");
            root.data("ToggleListItem", this);
        }

        get key(): string {
            return this._key;
        }

        get active(): boolean {
            return this._active;
        }

        activate(): void {
            this._active = true;
            this._root.addClass("toggle-list__item--active");
        }

        deactivate(): void {
            this._active = false;
            this._root.removeClass("toggle-list__item--active");
        }
    }

    export class ToggleList {
        private readonly _root: JQuery;
        private readonly _items: ToggleListItem[] = [];

        private readonly _selectionChanged: EventHandler<string[]> = new EventHandler<string[]>();

        constructor(root: JQuery) {
            this._root = root;
            this._root.find(".toggle-list__item").each((index, element) => {
                const control = $(element);
                const item = new ToggleListItem(control); 
                control.on("click", e => {
                    if (item.active) {
                        item.deactivate();
                    } else {
                        item.activate();
                    }

                    this._selectionChanged.raise(this, this.selected);
                });

                this._items.push(item);
            });

            root.data("ToggleList", this);
        }

        get selectionChanged(): EventHandler<string[]> {
            return this._selectionChanged;
        }

        get selected(): string[] {
            return this._items.filter(x => x.active).map(x => x.key);
        }

        isToggled(value: string): boolean {
            return this._items.some(x => x.active && x.key === value);
        }
    }

    $(() => {
        $(".toggle-list[data-init='true']").each((index, element) => {
            const list = new ToggleList($(element));
        });
    });
}