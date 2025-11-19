namespace DentalDrill.CRM.Pages.CorporateSurgeries.Reports {
    export interface ReportSurgeryItem {
        ClientId: string;
        ClientName: string;
        RatingAverage: number | object;
        CostSum: number | object;
        CostAverage: number | object;
        UnrepairedPercent: number | object;
        HandpiecesCount: number | object;
    }

    export interface ReportSurgeryBrandItem {
        ClientId: string;
        ClientName: string;
        Brand: string;
        RatingAverage: number | object;
        CostSum: number | object;
        CostAverage: number | object;
        UnrepairedPercent: number | object;
        HandpiecesCount: number | object;
    }

    export interface ReportSurgeryBrandModelItem {
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

    export interface ReportStatusItem {
        ClientId: string;
        ClientName: string;
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