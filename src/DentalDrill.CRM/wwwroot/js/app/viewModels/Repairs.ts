namespace DentalDrill.CRM.ViewModels {
    export interface RepairViewModel {
        Id: string;
        JobArticle: number;
        ClientName: string;
        MakeAndModel: string;
        Serial: string;
        DiagnosticReport: string;
        Service: string;
        Status: number;
        Rating: number;
        Received: number;
    }

    export interface RepairDetailsModel {
        Id: string;
        JobArticle: number;
        ClientName: string;
        MakeAndModel: string;
        Serial: string;
        DiagnosticReport: string;
        Service: string;
        Status: number;
        Rating: number;
        Received: number;
    }

    export interface RepairEditModel {
        Id: string;
        JobArticle: number;
        ClientName: string;
        MakeAndModel: string;
        Serial: string;
        DiagnosticReport: string;
        Service: string;
        Status: number;
        Rating: number;
        Received: number;
    }
}