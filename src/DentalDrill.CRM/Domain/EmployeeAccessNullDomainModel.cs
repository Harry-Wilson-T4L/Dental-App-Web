using System;
using System.Collections.Generic;
using DentalDrill.CRM.Domain.Abstractions;
using DentalDrill.CRM.Models;
using DentalDrill.CRM.Models.Permissions;

namespace DentalDrill.CRM.Domain
{
    public class EmployeeAccessNullDomainModel : IEmployeeAccess
    {
        public IEmployeeAccessGlobal Global { get; } = new EmployeeAccessGlobalNull();

        public IEmployeeAccessClient Clients { get; } = new EmployeeAccessClientNull();

        public IEmployeeAccessWorkshopCollection Workshops { get; } = new EmployeeAccessWorkshopCollectionNull();

        public IEmployeeAccessInventory Inventory { get; } = new EmployeeAccessInventoryNull();

        public Boolean CanAccessClients() => false;

        public Boolean CanAccessWorkshops() => false;

        public Boolean CanAccessInventory() => false;

        private class EmployeeAccessGlobalNull : IEmployeeAccessGlobal
        {
            public Boolean CanReadComponent(GlobalComponent component) => false;

            public Boolean CanWriteComponent(GlobalComponent component) => false;

            public Boolean CanReadAny() => false;

            public Boolean CanReadAnyOf(params GlobalComponent[] components) => components.Length == 0;

            public Boolean CanReadExact(GlobalComponent component)
            {
                return component switch
                {
                    GlobalComponent.None => true,
                    _ => false,
                };
            }
        }

        private class EmployeeAccessClientNull : IEmployeeAccessClient
        {
            public Boolean CanReadComponent(ClientEntityComponent clientComponent) => false;

            public Boolean CanReadAnyComponentOf(params ClientEntityComponent[] clientComponents) => false;

            public Boolean CanWriteComponent(ClientEntityComponent clientComponent) => false;

            public Boolean CanReadField(ClientEntityField clientField) => false;

            public Boolean CanWriteField(ClientEntityField clientField) => false;

            public Boolean CanInitField(ClientEntityField clientField) => false;

            public Boolean CanWriteOrInitField(ClientEntityField clientField, Boolean init) => false;
        }

        private class EmployeeAccessWorkshopCollectionNull : IEmployeeAccessWorkshopCollection
        {
            private readonly IEmployeeAccessWorkshop nullWorkshopAccess = new EmployeeAccessWorkshopNull();

            public IEmployeeAccessWorkshop this[Guid workshopId] => this.nullWorkshopAccess;

            public IReadOnlyList<Guid> GetAvailable() => Array.Empty<Guid>();

            public IReadOnlyList<Guid> GetAvailable(Func<IEmployeeAccessWorkshop, Boolean> predicate) => Array.Empty<Guid>();

            public IReadOnlyList<Guid> GetWorkshopAvailable() => Array.Empty<Guid>();

            public IReadOnlyList<Guid> GetInventoryAvailable() => Array.Empty<Guid>();

            public IReadOnlyList<Guid> GetWorkshopDeclared() => Array.Empty<Guid>();

            public IReadOnlyList<IEmployeeAccessWorkshop> GetAll() => Array.Empty<IEmployeeAccessWorkshop>();
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

        private class EmployeeAccessWorkshopJobTypeCollectionNull : IEmployeeAccessWorkshopJobTypeCollection
        {
            private readonly IEmployeeAccessWorkshopJobType nullJobType = new EmployeeAccessWorkshopJobTypeNull();

            public IEmployeeAccessWorkshopJobType this[String jobTypeId] => this.nullJobType;

            public IReadOnlyList<IEmployeeAccessWorkshopJobType> GetAll() => Array.Empty<IEmployeeAccessWorkshopJobType>();

            public IReadOnlyList<String> GetAvailable() => Array.Empty<String>();
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

        private class EmployeeAccessInventoryNull : IEmployeeAccessInventory
        {
            public Boolean HasPermission(InventoryPermissions inventoryPermissions) => false;
        }
    }
}
