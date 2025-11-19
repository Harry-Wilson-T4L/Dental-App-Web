namespace DentalDrill.CRM.Controls {
    import EventHandler = DevGuild.Utilities.EventHandler;

    export class ToggleButton {
        private readonly _root: JQuery;
        private readonly _changed: EventHandler<boolean> = new EventHandler<boolean>();

        private _active: boolean;

        constructor(root: JQuery) {
            this._root = root;
            this._active = root.hasClass("toggle-button--active");

            this._root.on("click", () => this.toggle());
        }

        get active(): boolean {
            return this._active;
        }

        get changed(): EventHandler<boolean> {
            return this._changed;
        }

        activate(): void {
            this._active = true;
            this._root.addClass("toggle-button--active");
            this._changed.raise(this, this._active);
        }

        deactivate(): void {
            this._active = false;
            this._root.removeClass("toggle-button--active");
            this._changed.raise(this, this._active);
        }

        toggle(): void {
            if (this.active) {
                this.deactivate();
            } else {
                this.activate();
            }
        }
    }
}