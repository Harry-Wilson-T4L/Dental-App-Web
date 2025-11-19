using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DentalDrill.CRM.Models;
using JetBrains.Annotations;

namespace DentalDrill.CRM.Domain.Abstractions
{
    public interface IInventorySKU
    {
        Guid Id { get; }

        String Name { get; }

        IInventorySKUType Type { get; }

        Decimal? AveragePrice { get; }

        Decimal? WarningQuantity { get; }

        String Description { get; }

        InventorySKUNodeType NodeType { get; }

        IMovementCollection Movements { get; }

        Task RefreshAsync();

        [Obsolete("Use overload with workshop", error: true)]
        Task<Decimal> GetAvailableQuantity();

        Task<Decimal> GetAvailableQuantity(IWorkshop workshop);

        [Obsolete("Use overload with workshop", error: true)]
        Task<Decimal> GetMissingQuantity();

        Task<Decimal> GetMissingQuantity(IWorkshop workshop);

        Task<IReadOnlyList<IInventorySKU>> GetDescendantsAndSelfAsync();

        Task TryProcessMovementsChangesForAllAsync([CanBeNull] Func<IInventoryRepairMovement, Task<Int32>> allocationPriority = null);

        Task TryProcessMovementsChangesAsync(IWorkshop workshop, [CanBeNull] Func<IInventoryRepairMovement, Task<Int32>> allocationPriority = null);

        public interface IMovementCollection
        {
            Task<IReadOnlyList<IInventoryMovement>> GetAllMovementsAsync(IWorkshop workshop);

            Task<IInventoryMovement> GetMovementByIdAsync(Guid id);

            Task<TMovement> GetMovementByIdAsync<TMovement>(Guid id)
                where TMovement : class, IInventoryMovement;

            Task<IReadOnlyList<IInventoryMovement>> GetMovementsByTypeAndStatusAsync(IWorkshop workshop, InventoryMovementType type, InventoryMovementStatus status);

            Task<IReadOnlyList<TMovement>> GetMovementsByTypeAndStatusAsync<TMovement>(IWorkshop workshop, InventoryMovementStatus status)
                where TMovement : class, IInventoryMovement;

            [Obsolete("Use overload with workshop (maybe)", error: true)]
            Task<IReadOnlyList<IInventoryMovement>> GetMovementsByTypeAndStatusAsync(InventoryMovementType type, InventoryMovementStatus status);

            [Obsolete("Use overload with workshop (maybe)", error: true)]
            Task<IReadOnlyList<TMovement>> GetMovementsByTypeAndStatusAsync<TMovement>(InventoryMovementStatus status)
                where TMovement : class, IInventoryMovement;

            Task<IReadOnlyList<IInventoryMovement>> GetMovementsByIdsAsync(IEnumerable<Guid> ids);

            Task<IReadOnlyList<TMovement>> GetMovementsByIdsAsync<TMovement>(IEnumerable<Guid> ids)
                where TMovement : class, IInventoryMovement;

            Task<IReadOnlyList<IInventoryMovement>> GetMovementsLinkedToPartAsync(Guid partId);

            Task<IReadOnlyList<TMovement>> GetMovementsLinkedToPartAsync<TMovement>(Guid partId)
                where TMovement : class, IInventoryMovement;

            Task<TMovement> GetSingleMovementLinkedToPartAsync<TMovement>(Guid partId)
                where TMovement : class, IInventoryMovement;

            Task<TMovement> CreateAsync<TMovement, TBuilder>(IWorkshop workshop, Decimal quantity, Action<TBuilder> builder = null)
                where TBuilder : IInventoryMovementBuilder<TMovement>, new()
                where TMovement : class, IInventoryMovement;

            Task ReallocateRepairsAsync(IWorkshop workshop, IReadOnlyList<Guid> sourceMovements, IReadOnlyList<Guid> destinationMovements);
        }
    }
}
