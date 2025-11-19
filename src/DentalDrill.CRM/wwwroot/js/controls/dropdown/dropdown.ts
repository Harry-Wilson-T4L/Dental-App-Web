/// <reference types="jquery" />
/// <reference types="bootstrap" />

//import * as $ from 'jquery';
//import 'bootstrap'; // Import Bootstrap's JS for dropdown functionality

namespace DentalDrill.CRM.Controls {
    import EventHandler = DevGuild.Utilities.EventHandler;

    export class Dropdown {
        private readonly _root: HTMLElement;
        private readonly _menu: HTMLElement;
        private readonly _node: JQuery;

        private readonly _show: EventHandler<JQueryEventObject> = new EventHandler<JQueryEventObject>();
        private readonly _shown: EventHandler<JQueryEventObject> = new EventHandler<JQueryEventObject>();
        private readonly _hide: EventHandler<JQueryEventObject> = new EventHandler<JQueryEventObject>();
        private readonly _hidden: EventHandler<JQueryEventObject> = new EventHandler<JQueryEventObject>();

        constructor(root: HTMLElement) {
            this._root = root;
            this._menu = root.querySelector(".dropdown-menu");
            this._node = $(root);

            this._node.on("show.bs.dropdown", (event: JQueryEventObject) => {
                this._show.raise(this, event);
            });
            this._node.on("shown.bs.dropdown", (event: JQueryEventObject) => {
                this._shown.raise(this, event);
            });
            this._node.on("hide.bs.dropdown", (event: JQueryEventObject) => {
                this._hide.raise(this, event);
            });
            this._node.on("hidden.bs.dropdown", (event: JQueryEventObject) => {
                this._hidden.raise(this, event);
            });
        }

        get onShow(): EventHandler<JQueryEventObject> {
            return this._show;
        }

        get onShown(): EventHandler<JQueryEventObject> {
            return this._shown;
        }

        get onHide(): EventHandler<JQueryEventObject> {
            return this._hide;
        }

        get onHidden(): EventHandler<JQueryEventObject> {
            return this._hidden;
        }

        isShown(): boolean {
            return this._root.classList.contains("show");
        }

        show(): Promise<void> {
            if (this.isShown()) {
                return new Promise<void>((resolve, reject) => resolve());
            }

            return new Promise<void>((resolve, reject) => {
                this._node.one("shown.bs.dropdown", (e) => {
                    resolve();
                });
                this._node.dropdown("toggle");
            });
        }

        hide(): Promise<void> {
            if (!this.isShown()) {
                return new Promise<void>((resolve, reject) => resolve());
            }

            return new Promise<void>((resolve, reject) => {
                this._node.one("hidden.bs.dropdown", (e) => {
                    resolve();
                });
                this._node.dropdown("toggle");
            });
        }

        toggle(): Promise<void> {
            if (this.isShown()) {
                return this.hide();
            } else {
                return this.show();
            }
        }

        preventContentClickHide(): void {
            this._node.on("hide.bs.dropdown", (e: JQueryEventObject & { clickEvent?: JQueryEventObject }) => {
                if (e.clickEvent) {
                    let target = e.clickEvent.target as HTMLElement;

                    while (target && target !== document.body) {
                        if (target === this._menu) {
                            e.preventDefault();
                            return;
                        }

                        target = target.parentElement;
                    }
                }
            });
        }
    }
}