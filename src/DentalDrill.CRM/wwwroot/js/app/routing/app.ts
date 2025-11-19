namespace DentalDrill.CRM.Routing {
    export class ApplicationRoutes {
        home = new Pages.HomeControllerRoute();
        zones = new Pages.ZonesControllerRoute();
        serviceLevels = new Pages.ServiceLevelsControllerRoute();
        states = new Pages.StatesControllerRoute();
        diagnosticCheckItems = new Pages.DiagnosticCheckItemsControllerRoute();
        diagnosticCheckTypes = new Pages.DiagnosticCheckTypesControllerRoute();
        clientAttachments = new Pages.ClientAttachmentsControllerRoute();
        clients = new Pages.ClientsControllerRoute();
        surgeries = new Pages.SurgeriesControllerRoute();
        clientNotes = new Pages.ClientNotesControllerRoute();
        clientCallbackNotifications = new Pages.ClientCallbackNotificationsControllerRoute();
        clientInvoices = new Pages.ClientInvoicesControllerRoute();
        clientEmails = new Pages.ClientEmailsControllerRoute();
        clientFeedback = new Pages.ClientFeedbackFormsControllerRoute();
        handpieceImages = new Pages.HandpieceImagesControllerRoute();
        employees = new Pages.EmployeesControllerRoute();
        employeeRoles = new Pages.EmployeeRolesControllerRoute();
        employeeRoleWorkshops = new Pages.EmployeeRoleWorkshopsControllerRoute();
        corporates = new Pages.CorporatesControllerRoute();
        corporateSurgeries = new Pages.CorporateSurgeriesControllerRoute();
        jobs = new Pages.JobsControllerRoute();
        jobInvoices = new Pages.JobInvoicesControllerRoute();
        handpieceBrands = new Pages.HandpieceBrandsControllerRoute();
        handpieceModels = new Pages.HandpieceModelsControllerRoute();
        handpieceModelSchematics = new Pages.HandpieceModelSchematicsControllerRoute();
        handpieceModelListings = new Pages.HandpieceModelListingsControllerRoute();
        handpieceStore = new Pages.HandpieceStoreControllerRoute();
        handpieceStoreOrders = new Pages.HandpieceStoreOrdersControllerRoute();
        handpieces = new Pages.HandpiecesControllerRoute();
        users = new Pages.UsersControllerRoute();
        pickupRequests = new Pages.PickupRequestsControllerRoute();
        feedbackForms = new Pages.FeedbackFormsControllerRoute();
        workshopRoles = new Pages.WorkshopRolesControllerRoute();
        workshopRoleJobTypes = new Pages.WorkshopRoleJobTypesControllerRoute();
        workshopRoleJobTypeStatuses = new Pages.WorkshopRoleJobTypeStatusesControllerRoute();
        workshopRoleJobTypeJobExceptions = new Pages.WorkshopRoleJobTypeJobExceptionsControllerRoute();
        workshopRoleJobTypeHandpieceExceptions = new Pages.WorkshopRoleJobTypeHandpieceExceptionsControllerRoute();
    }
}

var routes = new DentalDrill.CRM.Routing.ApplicationRoutes();