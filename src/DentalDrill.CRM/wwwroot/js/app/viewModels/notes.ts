namespace DentalDrill.CRM.ViewModels {
    export interface ClientNoteViewModel {
        Id: string;
        Created: number;
        Text: string;
        CreatorName: string;
    }

    export interface ClientNoteEditViewModel {
        Id: string;
        Text: string;
    }
}