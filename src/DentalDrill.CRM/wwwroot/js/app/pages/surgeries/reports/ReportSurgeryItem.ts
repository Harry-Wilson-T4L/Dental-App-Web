namespace DentalDrill.CRM.Pages.Surgeries.Reports {
    export interface ReportBrandItem {
        Brand: string;
        RatingAverage: number | object;
        CostSum: number | object;
        CostAverage: number | object;
        UnrepairedPercent: number | object;
        HandpiecesCount: number | object;
    }

    export interface ReportBrandModelItem {
        Brand: string;
        Model: string;
        RatingAverage: number | object;
        CostSum: number | object;
        CostAverage: number | object;
        UnrepairedPercent: number | object;
        HandpiecesCount: number | object;
    }

    export interface ReportStatusItem {
        Brand: string;
        StatusReceived: number;
        StatusBeingEstimated: number;
        StatusWaitingForApproval: number;
        StatusEstimateSent: number;
        StatusBeingRepaired: number;
        StatusReadyToReturn: number;
        StatusSentComplete: number;
        StatusCancelled: number;
        StatusAny: number;
    }
}