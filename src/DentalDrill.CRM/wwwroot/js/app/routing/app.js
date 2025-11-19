var DentalDrill;
(function (DentalDrill) {
    var CRM;
    (function (CRM) {
        var Routing;
        (function (Routing) {
            var ApplicationRoutes = /** @class */ (function () {
                function ApplicationRoutes() {
                    this.home = new Routing.Pages.HomeControllerRoute();
                    this.zones = new Routing.Pages.ZonesControllerRoute();
                    this.serviceLevels = new Routing.Pages.ServiceLevelsControllerRoute();
                    this.states = new Routing.Pages.StatesControllerRoute();
                    this.diagnosticCheckItems = new Routing.Pages.DiagnosticCheckItemsControllerRoute();
                    this.diagnosticCheckTypes = new Routing.Pages.DiagnosticCheckTypesControllerRoute();
                    this.clientAttachments = new Routing.Pages.ClientAttachmentsControllerRoute();
                    this.clients = new Routing.Pages.ClientsControllerRoute();
                    this.surgeries = new Routing.Pages.SurgeriesControllerRoute();
                    this.clientNotes = new Routing.Pages.ClientNotesControllerRoute();
                    this.clientCallbackNotifications = new Routing.Pages.ClientCallbackNotificationsControllerRoute();
                    this.clientInvoices = new Routing.Pages.ClientInvoicesControllerRoute();
                    this.clientEmails = new Routing.Pages.ClientEmailsControllerRoute();
                    this.clientFeedback = new Routing.Pages.ClientFeedbackFormsControllerRoute();
                    this.handpieceImages = new Routing.Pages.HandpieceImagesControllerRoute();
                    this.employees = new Routing.Pages.EmployeesControllerRoute();
                    this.employeeRoles = new Routing.Pages.EmployeeRolesControllerRoute();
                    this.employeeRoleWorkshops = new Routing.Pages.EmployeeRoleWorkshopsControllerRoute();
                    this.corporates = new Routing.Pages.CorporatesControllerRoute();
                    this.corporateSurgeries = new Routing.Pages.CorporateSurgeriesControllerRoute();
                    this.jobs = new Routing.Pages.JobsControllerRoute();
                    this.jobInvoices = new Routing.Pages.JobInvoicesControllerRoute();
                    this.handpieceBrands = new Routing.Pages.HandpieceBrandsControllerRoute();
                    this.handpieceModels = new Routing.Pages.HandpieceModelsControllerRoute();
                    this.handpieceModelSchematics = new Routing.Pages.HandpieceModelSchematicsControllerRoute();
                    this.handpieceModelListings = new Routing.Pages.HandpieceModelListingsControllerRoute();
                    this.handpieceStore = new Routing.Pages.HandpieceStoreControllerRoute();
                    this.handpieceStoreOrders = new Routing.Pages.HandpieceStoreOrdersControllerRoute();
                    this.handpieces = new Routing.Pages.HandpiecesControllerRoute();
                    this.users = new Routing.Pages.UsersControllerRoute();
                    this.pickupRequests = new Routing.Pages.PickupRequestsControllerRoute();
                    this.feedbackForms = new Routing.Pages.FeedbackFormsControllerRoute();
                    this.workshopRoles = new Routing.Pages.WorkshopRolesControllerRoute();
                    this.workshopRoleJobTypes = new Routing.Pages.WorkshopRoleJobTypesControllerRoute();
                    this.workshopRoleJobTypeStatuses = new Routing.Pages.WorkshopRoleJobTypeStatusesControllerRoute();
                    this.workshopRoleJobTypeJobExceptions = new Routing.Pages.WorkshopRoleJobTypeJobExceptionsControllerRoute();
                    this.workshopRoleJobTypeHandpieceExceptions = new Routing.Pages.WorkshopRoleJobTypeHandpieceExceptionsControllerRoute();
                }
                return ApplicationRoutes;
            }());
            Routing.ApplicationRoutes = ApplicationRoutes;
        })(Routing = CRM.Routing || (CRM.Routing = {}));
    })(CRM = DentalDrill.CRM || (DentalDrill.CRM = {}));
})(DentalDrill || (DentalDrill = {}));
var routes = new DentalDrill.CRM.Routing.ApplicationRoutes();
//# sourceMappingURL=app.js.map