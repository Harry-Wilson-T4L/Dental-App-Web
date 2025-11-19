namespace DentalDrill.CRM.Controls.PdfReports {
    export class PdfReportFlowWriter {
        private readonly _builder: PdfReportBuilder;
        private _activePage: PdfPageBuilder;

        constructor(builder: PdfReportBuilder) {
            this._builder = builder;
            this._activePage = this._builder.addPage();
        }

        get activePage(): PdfPageBuilder {
            return this._activePage;
        }

        addNode(node: JQuery): void {
            this._activePage.appendJQuery(node);
            this.fixOverflow();
        }

        nextPage(): void {
            this._activePage = this._builder.addPage();
        }

        popPage(): void {
            this._builder.popPage();
            this._activePage = this._builder.lastPage();
        }

        addRow(content: JQuery | JQuery[]): void {
            if (typeof content === "object") {
                const row = $(`<div class="row"></div>`);
                const col12 = $(`<div class="col-12"></div>`).appendTo(row);

                if (Array.isArray(content)) {
                    const contentItems = content as JQuery[];
                    for (let i = 0; i < contentItems.length; i++) {
                        col12.append(contentItems[i]);
                    }
                } else {
                    col12.append(content);
                }

                this.addNode(row);
            }
        }

        addHeading(level: number, text: string) {
            if (level < 1 || level > 6) {
                throw new Error("invalid Heading Level");
            }

            this.addRow($(`<h${level}></h${level}>`).text(text));
        }

        remainingHeight(): number {
            return this._activePage.remainingHeight();
        }

        private fixOverflow(): void {
            let overflow = this._activePage.removeHeightOverflow();
            while (overflow.length > 0) {
                this.nextPage();
                for (let i = 0; i < overflow.length; i++) {
                    this._activePage.appendHTML(overflow[i]);
                }

                overflow = this._activePage.removeHeightOverflow();
            }
        }
    }
}