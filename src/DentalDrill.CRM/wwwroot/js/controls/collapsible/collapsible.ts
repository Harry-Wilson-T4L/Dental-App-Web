namespace DentalDrill.CRM.Controls {
    export class Collapsible {
        private readonly _root: HTMLElement;
        private readonly _node: JQuery;

        constructor(root: HTMLElement) {
            this._root = root;
            this._node = $(this._root);
        }

        isShown(): boolean {
            return this._root.classList.contains("show");
        }

        showAsync(): Promise<void> {
            return new Promise<void>((resolve, reject) => {
                this._node.one("shown.bs.collapse", (e) => {
                    resolve();
                });
                this._node.collapse("show");
            });
        }

        hideAsync(): Promise<void> {
            return new Promise<void>((resolve, reject) => {
                this._node.one("hidden.bs.collapse", (e) => {
                    resolve();
                });
                this._node.collapse("hide");
            });
        }
    }
}