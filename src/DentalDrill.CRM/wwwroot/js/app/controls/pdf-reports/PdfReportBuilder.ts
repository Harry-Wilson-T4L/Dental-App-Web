namespace DentalDrill.CRM.Controls.PdfReports {
    export class PdfReportBuilder {
        private readonly _windowWrapper: JQuery;
        private readonly _wrapper: JQuery;
        private readonly _toolbar: JQuery;
        private readonly _containerWrapper: JQuery;
        private readonly _container: JQuery;
        private readonly _window: kendo.ui.Window;

        private readonly _toolbarExportButton: JQuery;

        private readonly _pages: IPage[] = [];

        private _fileName: string;

        constructor() {
            this._fileName = "Report.pdf";

            this._windowWrapper = $(`<div class="pdf-report pdf-report__window"></div>`);
            this._windowWrapper.kendoWindow({
                width: "90%",
                height: "90%",
                title: "Report",
                actions: ["close"],
                modal: true,
                visible: false,
                refresh: (e: kendo.ui.WindowEvent) => {
                    e.sender.center();
                },
                open: (e: kendo.ui.WindowEvent) => {
                    e.sender.center();
                }
            });
            this._window = this._windowWrapper.data("kendoWindow");

            this._wrapper = $(`<div class="pdf-report__wrapper"></div>`).appendTo(this._windowWrapper);
            this._toolbar = $(`<div class="pdf-report__toolbar"></div>`).appendTo(this._wrapper);

            this._toolbarExportButton = $(`<button type="button" class="pdf-report__toolbar__button btn btn-secondary btn-sm"><span class="fas fa-fw fa-file-pdf"></span> Save as PDF</button>`)
                .appendTo(this._toolbar);
            this._toolbarExportButton.on("click", async (e) => {
                await this.saveAsPdf(this._fileName);
            });

            this._containerWrapper = $(`<div class="pdf-report__container-wrapper"></div>`).appendTo(this._wrapper);
            this._container = $(`<div class="pdf-report__container"></div>`).appendTo(this._containerWrapper);
        }

        open(): void {
            this._window.open();
        }

        addPage(initializer?: (page :PdfPageBuilder) => void): PdfPageBuilder {
            const pageBuilder = new PdfPageBuilder(this._container);
            if (initializer) {
                initializer(pageBuilder);
            }

            this._pages.push(pageBuilder);
            return pageBuilder;
        }

        popPage(): void {
            const page = this._pages.pop() as PdfPageBuilder;
            page.remove();
        }

        lastPage(): PdfPageBuilder {
            return this._pages.length > 0 ? this._pages[this._pages.length - 1] as PdfPageBuilder : undefined;
        }

        get fileName(): string {
            return this._fileName;
        }

        set fileName(val: string) {
            this._fileName = val;
        }

        async saveAsPdf(fileName: string): Promise<void> {
            const root = new kendo.drawing.Group({
                pdf: {
                    paperSize: "A4",
                    margin: "0cm",
                }
            });

            for (let page of this._pages) {
                const pageGroups = await page.exportGroups();
                for (let pageGroup of pageGroups) {
                    root.children.push(pageGroup);
                }
            }

            const pdf = await kendo.drawing.exportPDF(root, { multiPage: true } as any as kendo.drawing.PDFOptions);

            kendo.saveAs({
                dataURI: pdf,
                fileName: fileName
            });
        }
    }
}