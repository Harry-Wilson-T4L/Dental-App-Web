using System;
using System.Threading.Tasks;
using DentalDrill.CRM.Models;

namespace DentalDrill.CRM.Domain.Abstractions
{
    public interface IInventorySKUType
    {
        Guid Id { get; }

        String Name { get; }

        HandpieceSpeedCompatibility? HandpieceSpeedCompatibility { get; }

        InventorySKUTypeStatisticsMode StatisticsMode { get; }

        Boolean IsCompatibleWith(IHandpiece handpiece);

        Task RefreshAsync();
    }
}
