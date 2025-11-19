using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DentalDrill.CRM.Models;
using DentalDrill.CRM.Models.Permissions;
using DentalDrill.CRM.Permissions;
using DentalDrill.CRM.Services.Permissions;
using DevGuild.AspNetCore.Services.Permissions;

namespace DentalDrill.CRM
{
    public partial class Startup
    {
        public PermissionsHubConfiguration ConfigurePermissions()
        {
            var configuration = new PermissionsHubConfiguration();

            configuration.AddEntry("/", ApplicationPermissions.Root, PermissionsManager.FixedByRole(builder => builder
                .RequireRoles(ApplicationPermissions.Root.Administer, ApplicationRoles.Administrator)));

            configuration.AddEntry("/Domain/Corporate", ApplicationPermissions.EntityType, PermissionsManager.FixedByRole(builder => builder
                .RequireRoles(ApplicationPermissions.EntityType, ApplicationRoles.Administrator)
                .RequireRoles(ApplicationPermissions.EntityType, ApplicationRoles.CompanyAdministrator)
                .RequireRoles(ApplicationPermissions.EntityType.Access, ApplicationRoles.Corporate)));

            configuration.AddEntry("/Domain/Corporate/Entities/{entity:Corporate}", ApplicationPermissions.Entity, PermissionsManager.ByRoleOrRelation<Corporate, Guid>(builder => builder
                .RequireRoles(ApplicationPermissions.Entity, ApplicationRoles.Administrator)
                .RequireRoles(ApplicationPermissions.Entity, ApplicationRoles.CompanyAdministrator)
                .RequireRoles(ApplicationPermissions.Entity.Read, ApplicationRoles.Corporate)));

            configuration.AddEntry("/Domain/Client", ApplicationPermissions.EntityType, PermissionsManager.FixedByRole(builder => builder
                .RequireRoles(ApplicationPermissions.EntityType, ApplicationRoles.Administrator)
                .RequireRoles(ApplicationPermissions.EntityType, ApplicationRoles.CompanyAdministrator)
                .RequireRoles(ApplicationPermissions.EntityType.Access, ApplicationRoles.Corporate)
                .RequireRoles(ApplicationPermissions.EntityType.Access, ApplicationRoles.Client)));

            configuration.AddEntry("/Domain/Client/Entities/{entity:Client}", ApplicationPermissions.Entity, PermissionsManager.ByRoleOrRelation<Client, Guid>(builder => builder
                .RequireRoles(ApplicationPermissions.Entity, ApplicationRoles.Administrator)
                .RequireRoles(ApplicationPermissions.Entity, ApplicationRoles.CompanyAdministrator)
                .RequireRoles(ApplicationPermissions.Entity.Read, ApplicationRoles.Corporate)
                .RequireRoles(ApplicationPermissions.Entity.Read, ApplicationRoles.Client)));

            configuration.AddEntry("/Domain/InternalCorporate", ApplicationPermissions.EntityType,
                EmployeeRolePermissionsManager.RequireGlobalPermissions<Corporate>(GlobalComponent.Corporate));

            configuration.AddEntry("/Domain/InternalCorporate/Entities/{entity}", ApplicationPermissions.Entity,
                EmployeeRolePermissionsManager.RequireGlobalPermissions<Corporate>(GlobalComponent.Corporate));

            configuration.AddEntry("/Domain/CorporatePricing", ApplicationPermissions.EntityType,
                EmployeeRolePermissionsManager.RequireGlobalPermissions<ServiceLevel>(GlobalComponent.CorporatePricing));

            configuration.AddEntry("/Domain/CorporatePricing/Entities/{entity}", ApplicationPermissions.Entity,
                EmployeeRolePermissionsManager.RequireGlobalPermissions<ServiceLevel>(GlobalComponent.CorporatePricing));

            configuration.AddEntry("/Domain/DiagnosticCheckType", ApplicationPermissions.EntityType,
                EmployeeRolePermissionsManager.RequireGlobalPermissions<DiagnosticCheckType>(GlobalComponent.DiagnosticChecklist));

            configuration.AddEntry("/Domain/DiagnosticCheckType/Entities/{entity}", ApplicationPermissions.Entity,
                EmployeeRolePermissionsManager.RequireGlobalPermissions<DiagnosticCheckType>(GlobalComponent.DiagnosticChecklist));

            configuration.AddEntry("/Domain/DiagnosticCheckItem", ApplicationPermissions.EntityType,
                EmployeeRolePermissionsManager.RequireGlobalPermissions<DiagnosticCheckItem>(GlobalComponent.DiagnosticChecklist));

            configuration.AddEntry("/Domain/DiagnosticCheckItem/Entities/{entity}", ApplicationPermissions.Entity,
                EmployeeRolePermissionsManager.RequireGlobalPermissions<DiagnosticCheckItem>(GlobalComponent.DiagnosticChecklist));

            configuration.AddEntry("/Domain/FeedbackForm", ApplicationPermissions.EntityType,
                EmployeeRolePermissionsManager.RequireGlobalPermissions<FeedbackForm>(GlobalComponent.Feedback));

            configuration.AddEntry("/Domain/FeedbackForm/Entities/{entity}", ApplicationPermissions.Entity,
                EmployeeRolePermissionsManager.RequireGlobalPermissions<FeedbackForm>(GlobalComponent.Feedback));

            configuration.AddEntry("/Domain/EmailTemplate", ApplicationPermissions.EntityType,
                EmployeeRolePermissionsManager.RequireGlobalPermissions<EmailTemplate>(GlobalComponent.EmailTemplate));

            configuration.AddEntry("/Domain/EmailTemplate/Entities/{entity}", ApplicationPermissions.Entity,
                EmployeeRolePermissionsManager.RequireGlobalPermissions<EmailTemplate>(GlobalComponent.EmailTemplate));

            configuration.AddEntry("/Domain/FeedbackConfiguration", ApplicationPermissions.EntityType,
                EmployeeRolePermissionsManager.RequireGlobalPermissions<FeedbackConfiguration>(GlobalComponent.EmailTemplate));

            configuration.AddEntry("/Domain/FeedbackConfiguration/Entities/{entity}", ApplicationPermissions.Entity,
                EmployeeRolePermissionsManager.RequireGlobalPermissions<FeedbackConfiguration>(GlobalComponent.EmailTemplate));

            configuration.AddEntry("/Domain/ProblemDescriptionOption", ApplicationPermissions.EntityType,
                EmployeeRolePermissionsManager.RequireGlobalPermissions<ProblemDescriptionOption>(GlobalComponent.ProblemOption));

            configuration.AddEntry("/Domain/ProblemDescriptionOption/Entities/{entity}", ApplicationPermissions.Entity,
                EmployeeRolePermissionsManager.RequireGlobalPermissions<ProblemDescriptionOption>(GlobalComponent.ProblemOption));

            configuration.AddEntry("/Domain/ReturnEstimate", ApplicationPermissions.EntityType,
                EmployeeRolePermissionsManager.RequireGlobalPermissions<ReturnEstimate>(GlobalComponent.ReturnEstimate));

            configuration.AddEntry("/Domain/ReturnEstimate/Entities/{entity}", ApplicationPermissions.Entity,
                EmployeeRolePermissionsManager.RequireGlobalPermissions<ReturnEstimate>(GlobalComponent.ReturnEstimate));

            configuration.AddEntry("/Domain/Employee", ApplicationPermissions.EntityType,
                EmployeeRolePermissionsManager.RequireGlobalPermissions<Employee>(GlobalComponent.Employee));

            configuration.AddEntry("/Domain/Employee/Entities/{entity}", ApplicationPermissions.Entity,
                EmployeeRolePermissionsManager.RequireGlobalPermissions<Employee>(GlobalComponent.Employee));

            configuration.AddEntry("/Domain/EmployeeRole", ApplicationPermissions.EntityType,
                EmployeeRolePermissionsManager.RequireGlobalPermissions<EmployeeRole>(GlobalComponent.EmployeeRole));

            configuration.AddEntry("/Domain/EmployeeRole/Entities/{entity}", ApplicationPermissions.Entity,
                EmployeeRolePermissionsManager.RequireGlobalPermissions<EmployeeRole>(GlobalComponent.EmployeeRole));

            configuration.AddEntry("/Domain/EmployeeRoleWorkshop", ApplicationPermissions.EntityType,
                EmployeeRolePermissionsManager.RequireGlobalPermissions<EmployeeRoleWorkshop>(GlobalComponent.EmployeeRole));

            configuration.AddEntry("/Domain/EmployeeRoleWorkshop/Entities/{entity}", ApplicationPermissions.Entity,
                EmployeeRolePermissionsManager.RequireGlobalPermissions<EmployeeRoleWorkshop>(GlobalComponent.EmployeeRole));

            configuration.AddEntry("/Domain/ServiceLevel", ApplicationPermissions.EntityType,
                EmployeeRolePermissionsManager.RequireGlobalPermissions<ServiceLevel>(GlobalComponent.ServiceLevel));

            configuration.AddEntry("/Domain/ServiceLevel/Entities/{entity}", ApplicationPermissions.Entity,
                EmployeeRolePermissionsManager.RequireGlobalPermissions<ServiceLevel>(GlobalComponent.ServiceLevel));

            configuration.AddEntry("/Domain/State", ApplicationPermissions.EntityType,
                EmployeeRolePermissionsManager.RequireGlobalPermissions<State>(GlobalComponent.State));

            configuration.AddEntry("/Domain/State/Entities/{entity}", ApplicationPermissions.Entity,
                EmployeeRolePermissionsManager.RequireGlobalPermissions<State>(GlobalComponent.State));

            configuration.AddEntry("/Domain/Zone", ApplicationPermissions.EntityType,
                EmployeeRolePermissionsManager.RequireGlobalPermissions<Zone>(GlobalComponent.Zone));

            configuration.AddEntry("/Domain/Zone/Entities/{entity}", ApplicationPermissions.Entity,
                EmployeeRolePermissionsManager.RequireGlobalPermissions<Zone>(GlobalComponent.Zone));

            configuration.AddEntry("/Domain/User", ApplicationPermissions.EntityType,
                EmployeeRolePermissionsManager.RequireGlobalPermissions<ApplicationUser>(GlobalComponent.User));

            configuration.AddEntry("/Domain/User/Entities/{entity}", ApplicationPermissions.Entity,
                EmployeeRolePermissionsManager.RequireGlobalPermissions<ApplicationUser>(GlobalComponent.User));

            configuration.AddEntry("/Domain/PickupRequest", ApplicationPermissions.EntityType,
                EmployeeRolePermissionsManager.RequireGlobalPermissions<PickupRequest>(GlobalComponent.PickupRequest));

            configuration.AddEntry("/Domain/PickupRequest/Entities/{entity}", ApplicationPermissions.Entity,
                EmployeeRolePermissionsManager.RequireGlobalPermissions<PickupRequest>(GlobalComponent.PickupRequest));

            configuration.AddEntry("/Domain/CalendarWeek", ApplicationPermissions.EntityType, PermissionsManager.FixedByRole(builder => builder
                .RequireRoles(ApplicationPermissions.EntityType, ApplicationRoles.Administrator)));

            configuration.AddEntry("/Domain/TutorialVideo", ApplicationPermissions.EntityType,
                EmployeeRolePermissionsManager.RequireGlobalPermissions<TutorialVideo>(GlobalComponent.Tutorial));

            configuration.AddEntry("/Domain/TutorialVideo/Entities/{entity}", ApplicationPermissions.Entity,
                EmployeeRolePermissionsManager.RequireGlobalPermissions<TutorialVideo>(GlobalComponent.Tutorial));

            configuration.AddEntry("/Domain/Workshop", ApplicationPermissions.EntityType,
                EmployeeRolePermissionsManager.RequireGlobalPermissions<Workshop>(GlobalComponent.Workshop));

            configuration.AddEntry("/Domain/Workshop/Entities/{entity}", ApplicationPermissions.Entity,
                EmployeeRolePermissionsManager.RequireGlobalPermissions<Workshop>(GlobalComponent.Workshop));

            configuration.AddEntry("/Domain/WorkshopRole", ApplicationPermissions.EntityType,
                EmployeeRolePermissionsManager.RequireGlobalPermissions<WorkshopRole>(GlobalComponent.EmployeeRole));

            configuration.AddEntry("/Domain/WorkshopRole/Entities/{entity}", ApplicationPermissions.Entity,
                EmployeeRolePermissionsManager.RequireGlobalPermissions<WorkshopRole>(GlobalComponent.EmployeeRole));

            configuration.AddEntry("/Domain/WorkshopRoleJobType", ApplicationPermissions.EntityType,
                EmployeeRolePermissionsManager.RequireGlobalPermissions<WorkshopRoleJobType>(GlobalComponent.EmployeeRole));

            configuration.AddEntry("/Domain/WorkshopRoleJobType/Entities/{entity}", ApplicationPermissions.Entity,
                EmployeeRolePermissionsManager.RequireGlobalPermissions<WorkshopRoleJobType>(GlobalComponent.EmployeeRole));

            configuration.AddEntry("/Domain/WorkshopRoleJobTypeStatus", ApplicationPermissions.EntityType,
                EmployeeRolePermissionsManager.RequireGlobalPermissions<WorkshopRoleJobTypeStatus>(GlobalComponent.EmployeeRole));

            configuration.AddEntry("/Domain/WorkshopRoleJobTypeStatus/Entities/{entity}", ApplicationPermissions.Entity,
                EmployeeRolePermissionsManager.RequireGlobalPermissions<WorkshopRoleJobTypeStatus>(GlobalComponent.EmployeeRole));

            configuration.AddEntry("/Domain/WorkshopRoleJobTypeJobException", ApplicationPermissions.EntityType,
                EmployeeRolePermissionsManager.RequireGlobalPermissions<WorkshopRoleJobTypeJobException>(GlobalComponent.EmployeeRole));

            configuration.AddEntry("/Domain/WorkshopRoleJobTypeJobException/Entities/{entity}", ApplicationPermissions.Entity,
                EmployeeRolePermissionsManager.RequireGlobalPermissions<WorkshopRoleJobTypeJobException>(GlobalComponent.EmployeeRole));

            configuration.AddEntry("/Domain/WorkshopRoleJobTypeHandpieceException", ApplicationPermissions.EntityType,
                EmployeeRolePermissionsManager.RequireGlobalPermissions<WorkshopRoleJobTypeHandpieceException>(GlobalComponent.EmployeeRole));

            configuration.AddEntry("/Domain/WorkshopRoleJobTypeHandpieceException/Entities/{entity}", ApplicationPermissions.Entity,
                EmployeeRolePermissionsManager.RequireGlobalPermissions<WorkshopRoleJobTypeHandpieceException>(GlobalComponent.EmployeeRole));

            configuration.AddEntry("/Domain/HandpieceDirectory/Brand", ApplicationPermissions.EntityType,
                EmployeeRolePermissionsManager.RequireGlobalPermissions<HandpieceBrand>(GlobalComponent.HandpiecesDirectory));

            configuration.AddEntry("/Domain/HandpieceDirectory/Brand/{entity}", ApplicationPermissions.Entity,
                EmployeeRolePermissionsManager.RequireGlobalPermissions<HandpieceBrand>(GlobalComponent.HandpiecesDirectory));

            configuration.AddEntry("/Domain/HandpieceDirectory/Model", ApplicationPermissions.EntityType,
                EmployeeRolePermissionsManager.RequireGlobalPermissions<HandpieceModel>(GlobalComponent.HandpiecesDirectory));

            configuration.AddEntry("/Domain/HandpieceDirectory/Model/{entity}", ApplicationPermissions.Entity,
                EmployeeRolePermissionsManager.RequireGlobalPermissions<HandpieceModel>(GlobalComponent.HandpiecesDirectory));

            configuration.AddEntry("/Domain/HandpieceDirectory/ModelSchematic", ApplicationPermissions.EntityType,
                EmployeeRolePermissionsManager.RequireGlobalPermissions<HandpieceModelSchematic>(GlobalComponent.HandpiecesDirectory));

            configuration.AddEntry("/Domain/HandpieceDirectory/ModelSchematic/{entity}", ApplicationPermissions.Entity,
                EmployeeRolePermissionsManager.RequireGlobalPermissions<HandpieceModelSchematic>(GlobalComponent.HandpiecesDirectory));

            configuration.AddEntry("/Domain/HandpieceDirectory/ModelListing", ApplicationPermissions.EntityType,
                EmployeeRolePermissionsManager.RequireGlobalPermissions<HandpieceStoreListing>(GlobalComponent.HandpiecesStore));

            configuration.AddEntry("/Domain/HandpieceDirectory/ModelListing/{entity}", ApplicationPermissions.Entity,
                EmployeeRolePermissionsManager.RequireGlobalPermissions<HandpieceStoreListing>(GlobalComponent.HandpiecesStore));

            configuration.AddEntry("/Domain/HandpieceStore/Order", ApplicationPermissions.EntityType,
                EmployeeRolePermissionsManager.RequireGlobalPermissions<HandpieceStoreOrder>(GlobalComponent.HandpiecesOrder));

            configuration.AddEntry("/Domain/HandpieceStore/Order/{entity}", ApplicationPermissions.Entity,
                EmployeeRolePermissionsManager.RequireGlobalPermissions<HandpieceStoreOrder>(GlobalComponent.HandpiecesOrder));

            configuration.AddEntry("/Domain/Internal/Client", ApplicationPermissions.EntityType,
                EmployeeRolePermissionsManager.RequireClientsPermissions<Client>(ClientEntityComponent.Client));

            configuration.AddEntry("/Domain/Internal/Client/{entity}", ApplicationPermissions.Entity,
                EmployeeRolePermissionsManager.RequireClientsPermissions<Client>(ClientEntityComponent.Client));

            configuration.AddEntry("/Domain/Internal/ClientNote", ApplicationPermissions.EntityType,
                EmployeeRolePermissionsManager.RequireClientsPermissions<ClientNote>(ClientEntityComponent.Note));

            configuration.AddEntry("/Domain/Internal/ClientNote/{entity}", ApplicationPermissions.Entity,
                EmployeeRolePermissionsManager.RequireClientsPermissions<ClientNote>(ClientEntityComponent.Note));

            configuration.AddEntry("/Domain/Internal/ClientCallbackNotification", ApplicationPermissions.EntityType,
                EmployeeRolePermissionsManager.RequireClientsPermissions<ClientCallbackNotification>(ClientEntityComponent.Callback));

            configuration.AddEntry("/Domain/Internal/ClientCallbackNotification/{entity}", ApplicationPermissions.Entity,
                EmployeeRolePermissionsManager.RequireClientsPermissions<ClientCallbackNotification>(ClientEntityComponent.Callback));

            configuration.AddEntry("/Domain/Internal/ClientHandpiece", ApplicationPermissions.EntityType,
                EmployeeRolePermissionsManager.RequireClientsPermissions<ClientHandpiece>(ClientEntityComponent.Repair));

            configuration.AddEntry("/Domain/Internal/ClientHandpiece/{entity}", ApplicationPermissions.Entity,
                EmployeeRolePermissionsManager.RequireClientsPermissions<ClientHandpiece>(ClientEntityComponent.Repair));

            configuration.AddEntry("/Domain/Internal/ClientEmailMessage", ApplicationPermissions.EntityType,
                EmployeeRolePermissionsManager.RequireClientsPermissions<ClientEmailMessage>(ClientEntityComponent.Email));

            configuration.AddEntry("/Domain/Internal/ClientEmailMessage/{entity}", ApplicationPermissions.Entity,
                EmployeeRolePermissionsManager.RequireClientsPermissions<ClientEmailMessage>(ClientEntityComponent.Email));

            configuration.AddEntry("/Domain/Internal/ClientInvoice", ApplicationPermissions.EntityType,
                EmployeeRolePermissionsManager.RequireClientsPermissions<JobInvoice>(ClientEntityComponent.Invoice));

            configuration.AddEntry("/Domain/Internal/ClientInvoice/{entity}", ApplicationPermissions.Entity,
                EmployeeRolePermissionsManager.RequireClientsPermissions<JobInvoice>(ClientEntityComponent.Invoice));

            configuration.AddEntry("/Domain/Internal/ClientFeedbackForm", ApplicationPermissions.EntityType,
                EmployeeRolePermissionsManager.RequireClientsPermissions<FeedbackForm>(ClientEntityComponent.Feedback));

            configuration.AddEntry("/Domain/Internal/ClientFeedbackForm/{entity}", ApplicationPermissions.Entity,
                EmployeeRolePermissionsManager.RequireClientsPermissions<FeedbackForm>(ClientEntityComponent.Feedback));

            configuration.AddEntry("/Domain/Internal/ClientAttachment", ApplicationPermissions.EntityType,
                EmployeeRolePermissionsManager.RequireClientsPermissions<ClientAttachment>(ClientEntityComponent.Attachment));

            configuration.AddEntry("/Domain/Internal/ClientAttachment/{entity}", ApplicationPermissions.Entity,
                EmployeeRolePermissionsManager.RequireClientsPermissions<ClientAttachment>(ClientEntityComponent.Attachment));

            configuration.AddEntry("/Domain/Internal/ClientAppearance", ApplicationPermissions.EntityType,
                EmployeeRolePermissionsManager.RequireClientsPermissions<Client>(ClientEntityComponent.Appearance));

            configuration.AddEntry("/Domain/Internal/ClientAppearance/{entity}", ApplicationPermissions.Entity,
                EmployeeRolePermissionsManager.RequireClientsPermissions<Client>(ClientEntityComponent.Appearance));

            configuration.AddEntry("/Domain/Internal/ClientAccess", ApplicationPermissions.EntityType,
                EmployeeRolePermissionsManager.RequireClientsPermissions<Client>(ClientEntityComponent.Access));

            configuration.AddEntry("/Domain/Internal/ClientAccess/{entity}", ApplicationPermissions.Entity,
                EmployeeRolePermissionsManager.RequireClientsPermissions<Client>(ClientEntityComponent.Access));

            return configuration;
        }
    }
}
