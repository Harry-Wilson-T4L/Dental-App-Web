using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DentalDrill.CRM.Models;

namespace DentalDrill.CRM.Domain.Abstractions
{
    public interface IHandpiece
    {
        Guid Id { get; }

        IJob Job { get; }

        IClientHandpiece ClientHandpiece { get; }

        Handpiece Entity { get; }

        String Number { get; }

        HandpieceStatus Status { get; }

        String Brand { get; }

        String Model { get; }

        String Serial { get; }

        Int32 Rating { get; }

        HandpieceSpeed SpeedType { get; }

        Int32? Speed { get; }

        IParts Parts { get; }

        String PartsComment { get; }

        DateTime ReceivedOn { get; }

        DateTime? EstimatedOn { get; }

        DateTime? ApprovedOn { get; }

        DateTime? RepairedOn { get; }

        DateTime? CompletedOn { get; }

        public interface IParts
        {
            Task<HandpiecePartsStockStatus> GetStockStatusAsync();

            Task UpdateStockStatusAsync(Boolean trackChange = false);

            Task<Boolean> CanCompleteAsync();

            Task CompleteAsync();

            Task CancelAsync();

            Task<IReadOnlyList<IHandpieceRequiredPart>> GetRequiredPartsAsync();

            Task<IHandpieceRequiredPart> FindRequiredPartAsync(Guid skuId);

            Task<IHandpieceRequiredPart> FindRequiredPartAsync(IInventorySKU sku);

            Task<IHandpieceRequiredPart> GetRequiredPartAsync(Guid id);

            Task<IHandpieceRequiredPart> AddRequiredPartAsync(IInventorySKU sku, Decimal quantity);

            Task<IHandpieceRequiredPart> UpdateQuantityAsync(IInventorySKU sku, Decimal currentQuantity, Decimal newQuantity);
        }
    }
}
