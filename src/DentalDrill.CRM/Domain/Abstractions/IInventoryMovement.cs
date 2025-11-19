using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DentalDrill.CRM.Models;

namespace DentalDrill.CRM.Domain.Abstractions
{
    public interface IInventoryMovement
    {
        Guid Id { get; }

        IWorkshop Workshop { get; }

        InventoryMovementDirection Direction { get; }

        InventoryMovementType Type { get; }

        InventoryMovementStatus Status { get; }

        Decimal Quantity { get; }

        Decimal QuantityAbsolute { get; }

        Decimal? Price { get; }

        DateTime CreatedOn { get; }

        DateTime? CompletedOn { get; }

        String Comment { get; }

        Task RefreshAsync();

        Task<IInventorySKU> GetSKUAsync();

        Task<IReadOnlyList<InventoryMovementChange>> GetChangesAsync();

        Task TrackCreation();
    }

    public interface IInventoryInitialMovement : IInventoryMovement
    {
    }

    public interface IInventoryOrderMovement : IInventoryMovement
    {
        Task<IHandpieceRequiredPart> GetHandpieceRequiredPartAsync();

        Task UpdateRequestedQuantityAsync(Decimal newQuantity);

        Task UpdateOrderedQuantityAsync(Decimal newQuantity);

        Task RemoveRequestedAsync();

        Task SetPriceAsync(Decimal price);

        Task ClearPriceAsync();

        Task<Boolean> TryChangePriceAsync(Decimal? newPrice);

        Task ApproveAsync();

        Task ApproveWithEditAsync(Decimal quantity, String comment);

        Task OrderAsync();

        Task OrderWithEditAsync(Decimal quantity, String comment);

        Task VerifyAsync();

        Task VerifyWithEditAsync(Decimal quantity, String comment);

        Task CancelAsync();

        Task RestoreAsync();

        Task UnlinkFromPartAsync();
    }

    public interface IInventoryFoundMovement : IInventoryMovement
    {
    }

    public interface IInventoryRepairMovement : IInventoryMovement
    {
        Task<IHandpieceRequiredPart> GetHandpieceRequiredPartAsync();

        Task UpdateQuantityAsync(Decimal newQuantity);

        Task AllocateAsync();

        Task DeallocateAsync();

        Task SetPriceInternalAsync(Decimal price);

        Task ClearPriceInternalAsync();

        Task DeleteAsync();
    }

    public interface IInventoryRepairFragmentMovement : IInventoryMovement
    {
        Task<IHandpieceRequiredPart> GetHandpieceRequiredPartAsync();

        Task UpdateQuantityAsync(Decimal newQuantity);

        Task DeleteAsync();
    }

    public interface IInventoryLostMovement : IInventoryMovement
    {
    }

    public interface IInventoryMoveFromAnotherWorkshopMovement : IInventoryMovement
    {
    }

    public interface IInventoryMoveToAnotherWorkshopMovement : IInventoryMovement
    {
    }
}
