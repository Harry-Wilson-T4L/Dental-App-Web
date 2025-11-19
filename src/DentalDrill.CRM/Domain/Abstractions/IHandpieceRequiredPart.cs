using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DentalDrill.CRM.Domain.Abstractions
{
    public interface IHandpieceRequiredPart
    {
        Guid Id { get; }

        IHandpiece Handpiece { get; }

        IInventorySKU SKU { get; }

        Decimal Quantity { get; }

        Boolean Deleted { get; }

        Task RefreshAsync();

        Task<Decimal> GetAllocatedQuantity();

        Task<Decimal> GetMissingQuantity();

        Task<Decimal> IncreaseAllocationAsync(Decimal quantityToAllocate);

        Task<Decimal> DecreaseAllocationAsync(Decimal quantityToDeallocate);

        Task LinkMovementsAsync(IReadOnlyList<IInventoryMovement> movements);

        Task UpdateQuantityAsync(Decimal newQuantity);

        Task<Boolean> CanCompleteAsync();

        Task CompleteAsync();

        Task CancelAsync();

        Task DeleteAsync();
    }
}
