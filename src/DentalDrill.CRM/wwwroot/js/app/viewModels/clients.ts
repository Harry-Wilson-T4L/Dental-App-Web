namespace DentalDrill.CRM.ViewModels {
    export interface ClientViewModel {
        Id: string;
        Name: string;
        Contact: string;
        SecondaryContact: string;
        Email: string;
        Phone: string;
        Address: string;
        PrincipalDentist: string;
        OpeningHours: string;
        Comment: string;
    }

    export interface ClientDetailsModel {
        Id: string;
        Name: string;
        Contact: string;
        SecondaryContact: string;
        Email: string;
        Phone: string;
        Address: string;
        PrincipalDentist: string;
        OpeningHours: string;
        Comment: string;
        StateName: string;
        ZoneName: string;
    }

    export interface ClientEditModel {
        Id: string;
        Name: string;
        Contact: string;
        SecondaryContact: string;
        Email: string;
        Phone: string;
        Address: string;
        PrincipalDentist: string;
        OpeningHours: string;
        Comment: string;
        StateId: string;
        ZoneId: string;
    }

    export interface ClientNotificationReadModel {
        Id: string;
    }
}