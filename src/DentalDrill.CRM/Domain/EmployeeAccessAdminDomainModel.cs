using System;
using System.Collections.Generic;
using System.Linq;
using DentalDrill.CRM.Domain.Abstractions;
using DentalDrill.CRM.Models;
using DentalDrill.CRM.Models.Permissions;

namespace DentalDrill.CRM.Domain
{
    public class EmployeeAccessAdminDomainModel : IEmployeeAccess
    {
        public EmployeeAccessAdminDomainModel(IReadOnlyList<Workshop> allWorkshops, IReadOnlyList<JobType> allJobTypes)
        {
            this.Global = new EmployeeAccessGlobalAdmin();
            this.Clients = new EmployeeAccessClientAdmin();
            this.Workshops = new EmployeeAccessWorkshopCollectionAdmin(allWorkshops, allJobTypes);
            this.Inventory = new EmployeeAccessInventoryAdmin();
        }

        public IEmployeeAccessGlobal Global { get; }

        public IEmployeeAccessClient Clients { get; }

        public IEmployeeAccessWorkshopCollection Workshops { get; }

        public IEmployeeAccessInventory Inventory { get; }

        public Boolean CanAccessClients() => true;

        public Boolean CanAccessWorkshops() => true;

        public Boolean CanAccessInventory() => true;

        private class EmployeeAccessGlobalAdmin : IEmployeeAccessGlobal
        {
            public Boolean CanReadComponent(GlobalComponent component) => true;

            public Boolean CanWriteComponent(GlobalComponent component) => true;

            public Boolean CanReadAny() => true;

            public Boolean CanReadAnyOf(params GlobalComponent[] components) => true;

            public Boolean CanReadExact(GlobalComponent component)
            {
                return component switch
                {
                    GlobalComponent.All => true,
                    _ => false,
                };
            }
        }

        private class EmployeeAccessClientAdmin : IEmployeeAccessClient
        {
            public Boolean CanReadComponent(ClientEntityComponent clientComponent) => true;

            public Boolean CanReadAnyComponentOf(params ClientEntityComponent[] clientComponents) => true;

            public Boolean CanWriteComponent(ClientEntityComponent clientComponent) => true;

            public Boolean CanReadField(ClientEntityField clientField) => true;

            public Boolean CanWriteField(ClientEntityField clientField) => true;

            public Boolean CanInitField(ClientEntityField clientField) => true;

            public Boolean CanWriteOrInitField(ClientEntityField clientField, Boolean init) => true;
        }

        private class EmployeeAccessWorkshopCollectionAdmin : IEmployeeAccessWorkshopCollection
        {
            private readonly IReadOnlyList<Workshop> allWorkshops;
            private readonly IEmployeeAccessWorkshop nullWorkshopAccess = new EmployeeAccessWorkshopNull();
            private readonly Dictionary<Guid, IEmployeeAccessWorkshop> workshopsMap = new();

            public EmployeeAccessWorkshopCollectionAdmin(IReadOnlyList<Workshop> allWorkshops, IReadOnlyList<JobType> allJobTypes)
            {
                this.allWorkshops = allWorkshops;
                foreach (var workshop in allWorkshops)
                {
                    this.workshopsMap.Add(workshop.Id, new EmployeeAccessWorkshopAdmin(workshop.Id, allJobTypes));
                }
            }

            public IEmployeeAccessWorkshop this[Guid workshopId] => this.workshopsMap.TryGetValue(workshopId, out var workshopAccess) ? workshopAccess : this.nullWorkshopAccess;

            public IReadOnlyList<Guid> GetAvailable()
            {
                return this.allWorkshops.Select(x => x.Id).ToList();
            }

            public IReadOnlyList<Guid> GetAvailable(Func<IEmployeeAccessWorkshop, Boolean> predicate)
            {
                return this.workshopsMap.Where(x => predicate.Invoke(x.Value)).Select(x => x.Key).ToList();
            }

            public IReadOnlyList<Guid> GetWorkshopAvailable()
            {
                return this.allWorkshops.Select(x => x.Id).ToList();
            }

            public IReadOnlyList<Guid> GetInventoryAvailable()
            {
                return this.allWorkshops.Select(x => x.Id).ToList();
            }

            public IReadOnlyList<Guid> GetWorkshopDeclared()
            {
                return this.allWorkshops.Select(x => x.Id).ToList();
            }

            public IReadOnlyList<IEmployeeAccessWorkshop> GetAll()
            {
                return this.workshopsMap.Select(x => x.Value).ToList();
            }
        }

        private class EmployeeAccessWorkshopAdmin : IEmployeeAccessWorkshop
        {
            private readonly Guid workshopId;

            public EmployeeAccessWorkshopAdmin(Guid workshopId, IReadOnlyList<JobType> allJobTypes)
            {
                this.workshopId = workshopId;
                this.JobTypes = new EmployeeAccessWorkshopJobTypeCollectionAdmin(allJobTypes);
            }

            public Guid WorkshopId => this.workshopId;

            public IEmployeeAccessWorkshopJobTypeCollection JobTypes { get; }

            public EmployeeType LegacyRole => EmployeeType.CompanyAdministrator;

            public Boolean CanAccessWorkshop() => true;

            public Boolean HasWorkshopPermission(WorkshopPermissions workshopPermissions) => true;

            public Boolean CanAccessInventory() => true;

            public Boolean HasInventoryPermission(InventoryMovementPermissions permission) => true;

            public NotificationScope GetNotificationScope() => NotificationScope.WorkshopSpecific;
        }

        private class EmployeeAccessWorkshopNull : IEmployeeAccessWorkshop
        {
            public Guid WorkshopId => Guid.Empty;

            public IEmployeeAccessWorkshopJobTypeCollection JobTypes { get; } = new EmployeeAccessWorkshopJobTypeCollectionNull();

            public EmployeeType LegacyRole => (EmployeeType)(-1);

            public Boolean CanAccessWorkshop() => false;

            public Boolean HasWorkshopPermission(WorkshopPermissions workshopPermissions) => false;

            public Boolean CanAccessInventory() => false;

            public Boolean HasInventoryPermission(InventoryMovementPermissions permission) => false;

            public NotificationScope GetNotificationScope() => NotificationScope.None;
        }

        private class EmployeeAccessWorkshopJobTypeCollectionAdmin : IEmployeeAccessWorkshopJobTypeCollection
        {
            private readonly IReadOnlyList<JobType> allJobTypes;
            private readonly EmployeeAccessWorkshopJobTypeNull nullJobType = new EmployeeAccessWorkshopJobTypeNull();
            private readonly Dictionary<String, IEmployeeAccessWorkshopJobType> jobTypesMap = new();

            public EmployeeAccessWorkshopJobTypeCollectionAdmin(IReadOnlyList<JobType> allJobTypes)
            {
                this.allJobTypes = allJobTypes;
                foreach (var jobType in allJobTypes)
                {
                    this.jobTypesMap.Add(jobType.Id, new EmployeeAccessWorkshopJobTypeAdmin(jobType.Id));
                }
            }

            public IEmployeeAccessWorkshopJobType this[String jobTypeId] => this.jobTypesMap.TryGetValue(jobTypeId, out var jobTypeAccess) ? jobTypeAccess : this.nullJobType;

            public IReadOnlyList<IEmployeeAccessWorkshopJobType> GetAll()
            {
                return this.jobTypesMap.Select(x => x.Value).ToList();
            }

            public IReadOnlyList<String> GetAvailable()
            {
                return this.allJobTypes.Select(x => x.Id).ToList();
            }
        }

        private class EmployeeAccessWorkshopJobTypeCollectionNull : IEmployeeAccessWorkshopJobTypeCollection
        {
            private readonly IEmployeeAccessWorkshopJobType nullJobType = new EmployeeAccessWorkshopJobTypeNull();

            public IEmployeeAccessWorkshopJobType this[String jobTypeId] => this.nullJobType;

            public IReadOnlyList<IEmployeeAccessWorkshopJobType> GetAll() => Array.Empty<IEmployeeAccessWorkshopJobType>();

            public IReadOnlyList<String> GetAvailable() => Array.Empty<String>();
        }

        private class EmployeeAccessWorkshopJobTypeAdmin : IEmployeeAccessWorkshopJobType
        {
            private readonly String jobTypeId;

            public EmployeeAccessWorkshopJobTypeAdmin(String jobTypeId)
            {
                this.jobTypeId = jobTypeId;
            }

            public String JobTypeId => this.jobTypeId;

            public Boolean CanAccessJobType() => true;

            public Boolean CanPerformJobTransition(JobStatus jobSourceStatus, JobStatus jobDestinationStatus) => true;

            public Boolean CanPerformHandpieceTransition(JobStatus jobStatus, HandpieceStatus handpieceSourceStatus, HandpieceStatus handpieceDestinationStatus) => true;

            public Boolean CanReadJobComponent(JobEntityComponent jobComponent) => true;

            public Boolean CanWriteJobComponent(JobEntityComponent jobComponent) => true;

            public Boolean CanReadJobField(JobStatus jobStatus, JobEntityField jobField) => true;

            public Boolean CanWriteJobField(JobStatus jobStatus, JobEntityField jobField) => true;

            public Boolean CanInitJobField(JobEntityField jobField) => true;

            public Boolean CanReadHandpieceComponent(HandpieceEntityComponent handpieceComponent) => true;

            public Boolean CanWriteHandpieceComponent(HandpieceEntityComponent handpieceComponent) => true;

            public Boolean CanReadHandpieceField(JobStatus jobStatus, HandpieceStatus handpieceStatus, HandpieceEntityField handpieceField) => true;

            public Boolean CanWriteHandpieceField(JobStatus jobStatus, HandpieceStatus handpieceStatus, HandpieceEntityField handpieceField) => true;

            public Boolean CanInitHandpieceField(HandpieceEntityField handpieceField) => true;
        }

        private class EmployeeAccessWorkshopJobTypeNull : IEmployeeAccessWorkshopJobType
        {
            public String JobTypeId => String.Empty;

            public Boolean CanAccessJobType() => false;

            public Boolean CanPerformJobTransition(JobStatus jobSourceStatus, JobStatus jobDestinationStatus) => false;

            public Boolean CanPerformHandpieceTransition(JobStatus jobStatus, HandpieceStatus handpieceSourceStatus, HandpieceStatus handpieceDestinationStatus) => false;

            public Boolean CanReadJobComponent(JobEntityComponent jobComponent) => false;

            public Boolean CanWriteJobComponent(JobEntityComponent jobComponent) => false;

            public Boolean CanReadJobField(JobStatus jobStatus, JobEntityField jobField) => false;

            public Boolean CanWriteJobField(JobStatus jobStatus, JobEntityField jobField) => false;

            public Boolean CanInitJobField(JobEntityField jobField) => false;

            public Boolean CanReadHandpieceComponent(HandpieceEntityComponent handpieceComponent) => false;

            public Boolean CanWriteHandpieceComponent(HandpieceEntityComponent handpieceComponent) => false;

            public Boolean CanReadHandpieceField(JobStatus jobStatus, HandpieceStatus handpieceStatus, HandpieceEntityField handpieceField) => false;

            public Boolean CanWriteHandpieceField(JobStatus jobStatus, HandpieceStatus handpieceStatus, HandpieceEntityField handpieceField) => false;

            public Boolean CanInitHandpieceField(HandpieceEntityField handpieceField) => false;
        }

        private class EmployeeAccessInventoryAdmin : IEmployeeAccessInventory
        {
            public Boolean HasPermission(InventoryPermissions inventoryPermissions) => true;
        }
    }
}
