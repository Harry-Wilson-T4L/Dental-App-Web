namespace DentalDrill.CRM.Pages.GlobalReports.Reports {
    export interface GlobalHandpiecesReportItem {
        ClientId: string;
        ClientName: string;
        Brand: string;
        Model: string;
        RatingAverage: number | object;
        CostSum: number | object;
        CostAverage: number | object;
        UnrepairedPercent: number | object;
        HandpiecesCount: number | object;
    }
}