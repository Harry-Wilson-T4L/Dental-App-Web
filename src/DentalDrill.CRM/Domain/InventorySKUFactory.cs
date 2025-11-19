using System;
using DentalDrill.CRM.Domain.Abstractions;
using DentalDrill.CRM.Models;
using DentalDrill.CRM.Services;
using DevGuild.AspNetCore.Services.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DentalDrill.CRM.Domain
{
    public class InventorySKUFactory : IInventorySKUFactory
    {
        private readonly IServiceProvider serviceProvider;

        public InventorySKUFactory(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public IInventorySKU Create(InventorySKU entity, IInventorySKUType type)
        {
            return new InventorySKUDomainModel(
                entity,
                type,
                this.serviceProvider.GetRequiredService<ILogger<InventorySKUDomainModel>>(),
                this.serviceProvider.GetRequiredService<IDateTimeService>(),
                this.serviceProvider.GetRequiredService<IRepository>(),
                this.serviceProvider.GetRequiredService<IInventorySKUFactory>(),
                this.serviceProvider.GetRequiredService<IInventorySKUTypeFactory>(),
                this.serviceProvider.GetRequiredService<IInventoryMovementManager>(),
                this.serviceProvider.GetRequiredService<IInventoryMovementFactory>(),
                this.serviceProvider.GetRequiredService<IWorkshopManager>());
        }
    }
}
