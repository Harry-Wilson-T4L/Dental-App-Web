using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DentalDrill.CRM.Domain.Abstractions;
using DentalDrill.CRM.Extensions;

namespace DentalDrill.CRM.Models.ViewModels.HandpieceRequiredParts
{
    public static class HandpieceRequiredPartReadModelExtensions
    {
        public static async Task<HandpieceRequiredPartReadModel> ToReadModelAsync(this IHandpieceRequiredPart part)
        {
            var repairMovement = await part.SKU.Movements.GetSingleMovementLinkedToPartAsync<IInventoryRepairMovement>(part.Id);
            var repairFragmentMovement = await part.SKU.Movements.GetSingleMovementLinkedToPartAsync<IInventoryRepairFragmentMovement>(part.Id);
            var orderMovements = await part.SKU.Movements.GetMovementsLinkedToPartAsync<IInventoryOrderMovement>(part.Id);
            var availableQuantity = await part.SKU.GetAvailableQuantity(part.Handpiece.Job.Workshop);

            Decimal? price = null;
            var ordersWithPrice = orderMovements.Where(x => x.Price.HasValue).ToList();
            if (ordersWithPrice.Count > 0 && ordersWithPrice.Sum(x => x.QuantityAbsolute) > 0)
            {
                price = ordersWithPrice.Sum(x => x.Price.Value) / ordersWithPrice.Sum(x => x.QuantityAbsolute);
            }
            else if (part.SKU.AveragePrice.HasValue)
            {
                price = part.SKU.AveragePrice.Value;
            }

            return new HandpieceRequiredPartReadModel
            {
                Id = part.Id,
                Date = repairMovement?.CreatedOn ?? DateTime.MinValue,
                SKUId = part.SKU.Id,
                SKUName = part.SKU.Name,
                RequiredQuantity = part.Quantity,
                ShelfQuantity = availableQuantity
                                + (repairMovement != null && repairMovement.Status.IsOneOf(InventoryMovementStatus.Allocated, InventoryMovementStatus.Completed) ? part.Quantity : 0)
                                + (repairFragmentMovement?.QuantityAbsolute ?? 0m),
                Price = price,
                Status = repairMovement switch
                {
                    null => HandpieceRequiredPartStatus.Unknown,
                    { Status: InventoryMovementStatus.Cancelled } => HandpieceRequiredPartStatus.Cancelled,
                    { Status: InventoryMovementStatus.Completed } => HandpieceRequiredPartStatus.Completed,
                    { Status: InventoryMovementStatus.Allocated } => HandpieceRequiredPartStatus.Allocated,
                    { Status: InventoryMovementStatus.Waiting } when orderMovements.Any(x => x.Status == InventoryMovementStatus.Requested) => HandpieceRequiredPartStatus.WaitingRequested,
                    { Status: InventoryMovementStatus.Waiting } when orderMovements.Any(x => x.Status == InventoryMovementStatus.Approved) => HandpieceRequiredPartStatus.WaitingApproved,
                    { Status: InventoryMovementStatus.Waiting } when orderMovements.Any(x => x.Status == InventoryMovementStatus.Ordered) => HandpieceRequiredPartStatus.WaitingOrdered,
                    { Status: InventoryMovementStatus.Waiting } => HandpieceRequiredPartStatus.Waiting,
                    _ => HandpieceRequiredPartStatus.Unknown,
                },
            };
        }

        public static async Task<List<HandpieceRequiredPartReadModel>> ToReadModelAsync(this IHandpiece.IParts handpieceParts)
        {
            var parts = await handpieceParts.GetRequiredPartsAsync();
            var result = new List<HandpieceRequiredPartReadModel>();
            foreach (var part in parts)
            {
                result.Add(await part.ToReadModelAsync());
            }

            return result;
        }
    }
}
