namespace DentalDrill.CRM.Controls.PdfReports {
    export interface IPage {
        exportGroups(): Promise<kendo.drawing.Group[]>;
    }
}