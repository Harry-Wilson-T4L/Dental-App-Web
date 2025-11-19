namespace DentalDrill.CRM.Controls.PdfReports {
    export class PdfPageBuilder implements IPage {
        private readonly _page: JQuery;

        constructor(container: JQuery) {
            this._page = $(`<div class="pdf-report__page pdf-report__page--size-a4"></div>`);
            container.append(this._page);
        }

        appendJQuery(node: JQuery) {
            this._page.append(node);
        }

        appendHTML(node: Element) {
            this._page.append($(node));
        }

        appendText(text: string) {
            this._page.append(document.createTextNode(text));
        }

        async exportGroups(): Promise<kendo.drawing.Group[]> {
            const group = await kendo.drawing.drawDOM(this._page, { });
            return [group];
        }

        removeHeightOverflow(): HTMLElement[] {
            const containerHeight = this._page.height();
            let consumedHeight = 0;
            let overflown = false;
            const children = this._page.children();
            if (children.length <= 1) {
                return [];
            }

            const removed: HTMLElement[] = [];
            for (let i = 0; i < children.length; i++) {
                const child = children[i];
                const childHeight = child.scrollHeight;
                if (overflown || consumedHeight + childHeight > containerHeight) {
                    overflown = true;
                    removed.push(child);
                } else {
                    consumedHeight += childHeight;
                }
            }

            for (let i = 0; i < removed.length; i++) {
                removed[i].parentElement.removeChild(removed[i]);
            }

            return removed;
        }

        usedHeight(): number {
            const containerHeight = this._page.height();
            let consumedHeight = 0;
            const children = this._page.children();
            for (let i = 0; i < children.length; i++) {
                const child = children[i];
                const childHeight = child.scrollHeight;
                if (consumedHeight + childHeight > containerHeight) {
                    return containerHeight;
                } else {
                    consumedHeight += childHeight;
                }
            }

            return consumedHeight;
        }

        remainingHeight(): number {
            const containerHeight = this._page.height();
            let consumedHeight = 0;
            const children = this._page.children();
            for (let i = 0; i < children.length; i++) {
                const child = children[i];
                const childHeight = child.scrollHeight;
                if (consumedHeight + childHeight > containerHeight) {
                    return 0;
                } else {
                    consumedHeight += childHeight;
                }
            }

            return containerHeight - consumedHeight;
        }

        remove(): void {
            this._page.remove();
        }
    }
}