using System;
using System.Collections.Generic;
using System.Linq;
using DentalDrill.CRM.Domain.Abstractions;
using DentalDrill.CRM.Models;
using DentalDrill.CRM.Models.Permissions;

namespace DentalDrill.CRM.Domain
{
    public class EmployeeAccessDomainModel : IEmployeeAccess
    {
        private readonly EmployeeRole roleEntity;

        public EmployeeAccessDomainModel(EmployeeRole roleEntity)
        {
            this.roleEntity = roleEntity;
            this.Global = new EmployeeAccessGlobal(this.roleEntity);
            this.Clients = new EmployeeAccessClient(this.roleEntity);
            this.Workshops = new EmployeeAccessWorkshopCollection(this.roleEntity);
            this.Inventory = new EmployeeAccessInventory(this.roleEntity);
        }

        public IEmployeeAccessGlobal Global { get; }

        public IEmployeeAccessClient Clients { get; }

        public IEmployeeAccessWorkshopCollection Workshops { get; }

        public IEmployeeAccessInventory Inventory { get; }

        public Boolean CanAccessClients()
        {
            return this.Clients.CanReadComponent(ClientEntityComponent.Client);
        }

        public Boolean CanAccessWorkshops()
        {
            return this.Workshops.GetAll().Any(x => x.CanAccessWorkshop());
        }

        public Boolean CanAccessInventory()
        {
            return this.roleEntity.InventoryAccess != InventoryPermissions.None ||
                   this.Workshops.GetAll().Any(x => x.CanAccessInventory());
        }

        private class EmployeeAccessGlobal : IEmployeeAccessGlobal
        {
            private readonly EmployeeRole roleEntity;

            public EmployeeAccessGlobal(EmployeeRole roleEntity)
            {
                this.roleEntity = roleEntity;
            }

            public Boolean CanReadComponent(GlobalComponent component)
            {
                return (this.roleEntity.GlobalComponentRead & component) == component;
            }

            public Boolean CanWriteComponent(GlobalComponent component)
            {
                return (this.roleEntity.GlobalComponentWrite & component) == component;
            }

            public Boolean CanReadAny()
            {
                return this.roleEntity.GlobalComponentRead != GlobalComponent.None;
            }

            public Boolean CanReadAnyOf(params GlobalComponent[] components)
            {
                return components.Any(this.CanReadComponent);
            }

            public Boolean CanReadExact(GlobalComponent component)
            {
                return this.roleEntity.GlobalComponentRead == component;
            }
        }

        private class EmployeeAccessClient : IEmployeeAccessClient
        {
            private readonly EmployeeRole roleEntity;

            public EmployeeAccessClient(EmployeeRole roleEntity)
            {
                this.roleEntity = roleEntity;
            }

            public Boolean CanReadComponent(ClientEntityComponent clientComponent)
            {
                return (this.roleEntity.ClientComponentRead & clientComponent) == clientComponent;
            }

            public Boolean CanReadAnyComponentOf(params ClientEntityComponent[] clientComponents)
            {
                return clientComponents.Any(this.CanReadComponent);
            }

            public Boolean CanWriteComponent(ClientEntityComponent clientComponent)
            {
                return (this.roleEntity.ClientComponentWrite & clientComponent) == clientComponent;
            }

            public Boolean CanReadField(ClientEntityField clientField)
            {
                return (this.roleEntity.ClientFieldRead & clientField) == clientField;
            }

            public Boolean CanWriteField(ClientEntityField clientField)
            {
                return (this.roleEntity.ClientFieldWrite & clientField) == clientField;
            }

            public Boolean CanInitField(ClientEntityField clientField)
            {
                return (this.roleEntity.ClientFieldInit & clientField) == clientField;
            }

            public Boolean CanWriteOrInitField(ClientEntityField clientField, Boolean init)
            {
                return init ? this.CanInitField(clientField) : this.CanWriteField(clientField);
            }
        }

        private class EmployeeAccessWorkshopCollection : IEmployeeAccessWorkshopCollection
        {
            private readonly List<IEmployeeAccessWorkshop> workshopsList;
            private readonly Dictionary<Guid, IEmployeeAccessWorkshop> workshopsMap;

            private readonly IEmployeeAccessWorkshop nullWorkshop;

            public EmployeeAccessWorkshopCollection(EmployeeRole roleEntity)
            {
                this.workshopsList = new List<IEmployeeAccessWorkshop>();
                this.workshopsMap = new Dictionary<Guid, IEmployeeAccessWorkshop>();
                foreach (var employeeWorkshop in roleEntity.WorkshopRoles)
                {
                    var workshopAccess = new EmployeeAccessWorkshop(employeeWorkshop.WorkshopId, employeeWorkshop.WorkshopRole, employeeWorkshop.EmployeeType);
                    this.workshopsList.Add(workshopAccess);
                    this.workshopsMap.Add(employeeWorkshop.WorkshopId, workshopAccess);
                }

                this.nullWorkshop = new EmployeeAccessWorkshopNull();
            }

            public IEmployeeAccessWorkshop this[Guid workshopId] => this.workshopsMap.TryGetValue(workshopId, out var workshopAccess) ? workshopAccess : this.nullWorkshop;

            public IReadOnlyList<Guid> GetAvailable()
            {
                return this.workshopsMap.Select(x => x.Key).ToList();
            }

            public IReadOnlyList<Guid> GetAvailable(Func<IEmployeeAccessWorkshop, Boolean> predicate)
            {
                return this.workshopsMap.Where(x => predicate.Invoke(x.Value)).Select(x => x.Key).ToList();
            }

            public IReadOnlyList<Guid> GetWorkshopAvailable()
            {
                return this.workshopsMap.Where(x => x.Value.CanAccessWorkshop()).Select(x => x.Key).ToList();
            }

            public IReadOnlyList<Guid> GetInventoryAvailable()
            {
                return this.workshopsMap.Where(x => x.Value.CanAccessInventory()).Select(x => x.Key).ToList();
            }

            public IReadOnlyList<Guid> GetWorkshopDeclared()
            {
                return this.workshopsMap.Select(x => x.Key).ToList();
            }

            public IReadOnlyList<IEmployeeAccessWorkshop> GetAll()
            {
                return this.workshopsList;
            }
        }

        private class EmployeeAccessWorkshop : IEmployeeAccessWorkshop
        {
            private readonly Guid workshopId;
            private readonly WorkshopRole workshopRole;
            private readonly EmployeeType employeeType;

            public EmployeeAccessWorkshop(Guid workshopId, WorkshopRole workshopRole, EmployeeType employeeType)
            {
                this.workshopId = workshopId;
                this.workshopRole = workshopRole;
                this.employeeType = employeeType;
                this.JobTypes = new EmployeeAccessWorkshopJobTypeCollection(workshopRole);
            }

            public Guid WorkshopId => this.workshopId;

            public IEmployeeAccessWorkshopJobTypeCollection JobTypes { get; }

            public EmployeeType LegacyRole => this.employeeType;

            public Boolean CanAccessWorkshop()
            {
                return (this.workshopRole.WorkshopAccess & WorkshopPermissions.AccessWorkshop) == WorkshopPermissions.AccessWorkshop;
            }

            public Boolean HasWorkshopPermission(WorkshopPermissions workshopPermissions)
            {
                return (this.workshopRole.WorkshopAccess & workshopPermissions) == workshopPermissions;
            }

            public Boolean CanAccessInventory()
            {
                return (this.workshopRole.InventoryAccess & InventoryMovementPermissions.MovementRead) == InventoryMovementPermissions.MovementRead;
            }

            public Boolean HasInventoryPermission(InventoryMovementPermissions permission)
            {
                return (this.workshopRole.InventoryAccess & permission) == permission;
            }

            public NotificationScope GetNotificationScope()
            {
                return this.employeeType switch
                {
                    EmployeeType.WorkshopTechnician => NotificationScope.Workshop,
                    EmployeeType.OfficeAdministrator => NotificationScope.Office,
                    EmployeeType.CompanyAdministrator => NotificationScope.WorkshopSpecific,
                    EmployeeType.CompanyManager => NotificationScope.None,
                    _ => throw new ArgumentOutOfRangeException(),
                };
            }
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

        private class EmployeeAccessWorkshopJobTypeCollection : IEmployeeAccessWorkshopJobTypeCollection
        {
            private readonly IEmployeeAccessWorkshopJobType nullJobType = new EmployeeAccessWorkshopJobTypeNull();
            private readonly Dictionary<String, IEmployeeAccessWorkshopJobType> jobTypesMap = new();

            public EmployeeAccessWorkshopJobTypeCollection(WorkshopRole workshopRole)
            {
                foreach (var jobTypeEntity in workshopRole.JobTypePermissions)
                {
                    var jobTypeAccess = new EmployeeAccessWorkshopJobType(jobTypeEntity);
                    this.jobTypesMap.Add(jobTypeEntity.JobTypeId, jobTypeAccess);
                }
            }

            public IEmployeeAccessWorkshopJobType this[String jobTypeId] => this.jobTypesMap.TryGetValue(jobTypeId, out var jobTypeAccess) ? jobTypeAccess : this.nullJobType;

            public IReadOnlyList<IEmployeeAccessWorkshopJobType> GetAll() => this.jobTypesMap.Select(x => x.Value).ToList();

            public IReadOnlyList<String> GetAvailable() => this.jobTypesMap.Select(x => x.Key).ToList();
        }

        private class EmployeeAccessWorkshopJobTypeCollectionNull : IEmployeeAccessWorkshopJobTypeCollection
        {
            private readonly IEmployeeAccessWorkshopJobType nullJobType = new EmployeeAccessWorkshopJobTypeNull();

            public IEmployeeAccessWorkshopJobType this[String jobTypeId] => this.nullJobType;

            public IReadOnlyList<IEmployeeAccessWorkshopJobType> GetAll() => Array.Empty<IEmployeeAccessWorkshopJobType>();

            public IReadOnlyList<String> GetAvailable() => Array.Empty<String>();
        }

        private class EmployeeAccessWorkshopJobType : IEmployeeAccessWorkshopJobType
        {
            private readonly WorkshopRoleJobType jobTypeEntity;

            public EmployeeAccessWorkshopJobType(WorkshopRoleJobType jobTypeEntity)
            {
                this.jobTypeEntity = jobTypeEntity;
            }

            public String JobTypeId => this.jobTypeEntity.JobTypeId;

            public Boolean CanAccessJobType()
            {
                return true;
            }

            public Boolean CanPerformJobTransition(JobStatus jobSourceStatus, JobStatus jobDestinationStatus)
            {
                var statusAccess = this.jobTypeEntity.StatusPermissions.SingleOrDefault(x => x.JobStatus == jobSourceStatus);
                if (statusAccess == null)
                {
                    return false;
                }

                var requiredFlags = jobDestinationStatus.ToFlag();
                return (statusAccess.JobTransitions & requiredFlags) == requiredFlags;
            }

            public Boolean CanPerformHandpieceTransition(JobStatus jobStatus, HandpieceStatus handpieceSourceStatus, HandpieceStatus handpieceDestinationStatus)
            {
                var statusAccess = this.jobTypeEntity.StatusPermissions.SingleOrDefault(x => x.JobStatus == jobStatus);
                if (statusAccess == null)
                {
                    return false;
                }

                if (handpieceSourceStatus == handpieceDestinationStatus)
                {
                    var requiredFlagSource = handpieceSourceStatus.ToFlag();
                    return (statusAccess.HandpieceTransitionsFrom & requiredFlagSource) == requiredFlagSource;
                }
                else
                {
                    var requiredFlagSource = handpieceSourceStatus.ToFlag();
                    var requiredFlagDestination = handpieceDestinationStatus.ToFlag();

                    return (statusAccess.HandpieceTransitionsFrom & requiredFlagSource) == requiredFlagSource &&
                           (statusAccess.HandpieceTransitionsTo & requiredFlagDestination) == requiredFlagDestination;

                }
            }

            public Boolean CanReadJobComponent(JobEntityComponent jobComponent)
            {
                return (this.jobTypeEntity.JobComponentRead & jobComponent) == jobComponent;
            }

            public Boolean CanWriteJobComponent(JobEntityComponent jobComponent)
            {
                return (this.jobTypeEntity.JobComponentWrite & jobComponent) == jobComponent;
            }

            public Boolean CanReadJobField(JobStatus jobStatus, JobEntityField jobField)
            {
                var jobStatusFlag = jobStatus.ToFlag();

                return (this.jobTypeEntity.JobFieldRead & jobField) == jobField &&
                       !this.jobTypeEntity.JobExceptions.Any(x => (x.WhenJobStatus & jobStatusFlag) == jobStatusFlag &&
                                                                  (x.HiddenFields & jobField) == jobField);
            }

            public Boolean CanWriteJobField(JobStatus jobStatus, JobEntityField jobField)
            {
                var jobStatusFlag = jobStatus.ToFlag();

                return (this.jobTypeEntity.JobFieldWrite & jobField) == jobField &&
                       !this.jobTypeEntity.JobExceptions.Any(x => (x.WhenJobStatus & jobStatusFlag) == jobStatusFlag &&
                                                                  (x.ReadOnlyFields & jobField) == jobField);
            }

            public Boolean CanInitJobField(JobEntityField jobField)
            {
                return (this.jobTypeEntity.JobFieldInit & jobField) == jobField;
            }

            public Boolean CanReadHandpieceComponent(HandpieceEntityComponent handpieceComponent)
            {
                return (this.jobTypeEntity.HandpieceComponentRead & handpieceComponent) == handpieceComponent;
            }

            public Boolean CanWriteHandpieceComponent(HandpieceEntityComponent handpieceComponent)
            {
                return (this.jobTypeEntity.HandpieceComponentWrite & handpieceComponent) == handpieceComponent;
            }

            public Boolean CanReadHandpieceField(JobStatus jobStatus, HandpieceStatus handpieceStatus, HandpieceEntityField handpieceField)
            {
                var jobStatusFlag = jobStatus.ToFlag();
                var handpieceStatusFlag = handpieceStatus.ToFlag();

                return (this.jobTypeEntity.HandpieceFieldRead & handpieceField) == handpieceField &&
                       !this.jobTypeEntity.HandpieceExceptions.Any(x => (x.WhenJobStatus & jobStatusFlag) == jobStatusFlag &&
                                                                        (x.WhenHandpieceStatus & handpieceStatusFlag) == handpieceStatusFlag &&
                                                                        (x.HiddenFields & handpieceField) == handpieceField);
            }

            public Boolean CanWriteHandpieceField(JobStatus jobStatus, HandpieceStatus handpieceStatus, HandpieceEntityField handpieceField)
            {
                var jobStatusFlag = jobStatus.ToFlag();
                var handpieceStatusFlag = handpieceStatus.ToFlag();

                return (this.jobTypeEntity.HandpieceFieldWrite & handpieceField) == handpieceField &&
                       !this.jobTypeEntity.HandpieceExceptions.Any(x => (x.WhenJobStatus & jobStatusFlag) == jobStatusFlag &&
                                                                        (x.WhenHandpieceStatus & handpieceStatusFlag) == handpieceStatusFlag &&
                                                                        (x.ReadOnlyFields & handpieceField) == handpieceField);
            }

            public Boolean CanInitHandpieceField(HandpieceEntityField handpieceField)
            {
                return (this.jobTypeEntity.HandpieceFieldInit & handpieceField) == handpieceField;
            }
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

        private class EmployeeAccessInventory : IEmployeeAccessInventory
        {
            private readonly EmployeeRole roleEntity;

            public EmployeeAccessInventory(EmployeeRole roleEntity)
            {
                this.roleEntity = roleEntity;
            }

            public Boolean HasPermission(InventoryPermissions inventoryPermissions)
            {
                return (this.roleEntity.InventoryAccess & inventoryPermissions) == inventoryPermissions;
            }
        }
    }
}
