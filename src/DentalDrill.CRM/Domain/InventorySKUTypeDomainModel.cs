using System;
using System.Linq;
using System.Threading.Tasks;
using DentalDrill.CRM.Domain.Abstractions;
using DentalDrill.CRM.Models;
using DevGuild.AspNetCore.Services.Data;
using Microsoft.EntityFrameworkCore;

namespace DentalDrill.CRM.Domain
{
    public class InventorySKUTypeDomainModel : IInventorySKUType
    {
        private readonly IRepository repository;

        private InventorySKUType entity;

        public InventorySKUTypeDomainModel(InventorySKUType entity, IRepository repository)
        {
            this.repository = repository;

            this.entity = entity;
        }

        public Guid Id => this.entity.Id;

        public String Name => this.entity.Name;

        public HandpieceSpeedCompatibility? HandpieceSpeedCompatibility => this.entity.HandpieceSpeedCompatibility;

        public InventorySKUTypeStatisticsMode StatisticsMode => this.entity.StatisticsMode;

        public Boolean IsCompatibleWith(IHandpiece handpiece)
        {
            if (this.HandpieceSpeedCompatibility.HasValue)
            {
                var supportedSpeed = this.HandpieceSpeedCompatibility.Value.ToSupportedHandpieceSpeed();
                if (!supportedSpeed.Contains(handpiece.SpeedType))
                {
                    return false;
                }
            }

            return true;
        }

        public async Task RefreshAsync()
        {
            var newEntity = await this.repository.QueryWithoutTracking<InventorySKUType>()
                .SingleOrDefaultAsync(x => x.Id == this.entity.Id);

            this.entity = newEntity ?? throw new InvalidOperationException("Entity is missing after refresh");
        }
    }
}
