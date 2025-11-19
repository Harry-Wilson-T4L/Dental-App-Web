using System;
using DentalDrill.CRM.Domain.Abstractions;
using DentalDrill.CRM.Models;
using DentalDrill.CRM.Services;
using DevGuild.AspNetCore.Services.Data;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DentalDrill.CRM.Domain
{
    public class InventoryMovementFactory : IInventoryMovementFactory
    {
        private readonly IServiceProvider serviceProvider;

        public InventoryMovementFactory(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public IInventoryMovement Create(InventoryMovement entity, IWorkshop workshop)
        {
            return entity.Type switch
            {
                InventoryMovementType.Initial => new InventoryInitialMovementDomainModel(
                    entity,
                    workshop,
                    this.serviceProvider.GetRequiredService<IRepository>(),
                    this.serviceProvider.GetRequiredService<IInventorySKUManager>(),
                    this.serviceProvider.GetRequiredService<UserEntityResolver>(),
                    this.serviceProvider.GetRequiredService<IDateTimeService>()),
                InventoryMovementType.Order => new InventoryOrderMovementDomainModel(
                    entity,
                    workshop,
                    this.serviceProvider.GetRequiredService<IRepository>(),
                    this.serviceProvider.GetRequiredService<IInventorySKUManager>(),
                    this.serviceProvider.GetRequiredService<UserEntityResolver>(),
                    this.serviceProvider.GetRequiredService<IHandpieceRequiredPartManager>(),
                    this.serviceProvider.GetRequiredService<IDateTimeService>(),
                    this.serviceProvider.GetRequiredService<ILogger<InventoryOrderMovementDomainModel>>()),
                InventoryMovementType.Found => new InventoryFoundMovementDomainModel(
                    entity,
                    workshop,
                    this.serviceProvider.GetRequiredService<IRepository>(),
                    this.serviceProvider.GetRequiredService<IInventorySKUManager>(),
                    this.serviceProvider.GetRequiredService<UserEntityResolver>(),
                    this.serviceProvider.GetRequiredService<IDateTimeService>()),
                InventoryMovementType.Repair => new InventoryRepairMovementDomainModel(
                    entity,
                    workshop,
                    this.serviceProvider.GetRequiredService<IRepository>(),
                    this.serviceProvider.GetRequiredService<IInventorySKUManager>(),
                    this.serviceProvider.GetRequiredService<UserEntityResolver>(),
                    this.serviceProvider.GetRequiredService<IDateTimeService>(),
                    this.serviceProvider.GetRequiredService<IHandpieceRequiredPartManager>()),
                InventoryMovementType.Lost => new InventoryLostMovementDomainModel(
                    entity,
                    workshop,
                    this.serviceProvider.GetRequiredService<IRepository>(),
                    this.serviceProvider.GetRequiredService<IInventorySKUManager>(),
                    this.serviceProvider.GetRequiredService<UserEntityResolver>(),
                    this.serviceProvider.GetRequiredService<IDateTimeService>()),
                InventoryMovementType.RepairFragment => new InventoryRepairFragmentMovementDomainModel(
                    entity,
                    workshop,
                    this.serviceProvider.GetRequiredService<IRepository>(),
                    this.serviceProvider.GetRequiredService<IInventorySKUManager>(),
                    this.serviceProvider.GetRequiredService<UserEntityResolver>(),
                    this.serviceProvider.GetRequiredService<IDateTimeService>(),
                    this.serviceProvider.GetRequiredService<IHandpieceRequiredPartManager>()),
                InventoryMovementType.MoveFromAnotherWorkshop => new InventoryMoveFromAnotherWorkshopMovementDomainModel(
                    entity,
                    workshop,
                    this.serviceProvider.GetRequiredService<IRepository>(),
                    this.serviceProvider.GetRequiredService<IInventorySKUManager>(),
                    this.serviceProvider.GetRequiredService<UserEntityResolver>(),
                    this.serviceProvider.GetRequiredService<IDateTimeService>()),
                InventoryMovementType.MoveToAnotherWorkshop => new InventoryMoveToAnotherWorkshopMovementDomainModel(
                    entity,
                    workshop,
                    this.serviceProvider.GetRequiredService<IRepository>(),
                    this.serviceProvider.GetRequiredService<IInventorySKUManager>(),
                    this.serviceProvider.GetRequiredService<UserEntityResolver>(),
                    this.serviceProvider.GetRequiredService<IDateTimeService>()),
                InventoryMovementType.EphemeralMissingRequiredQuantity => throw new NotSupportedException("Cannot instantiate ephemeral movement"),
                _ => throw new InvalidOperationException($"Type {entity.Type} is not supported"),
            };
        }

        public TMovement Create<TMovement>(InventoryMovement entity, IWorkshop workshop)
            where TMovement : class, IInventoryMovement
        {
            IInventoryMovement domain = entity.Type switch
            {
                InventoryMovementType.Initial when typeof(TMovement) == typeof(IInventoryInitialMovement) => new InventoryInitialMovementDomainModel(
                    entity,
                    workshop,
                    this.serviceProvider.GetRequiredService<IRepository>(),
                    this.serviceProvider.GetRequiredService<IInventorySKUManager>(),
                    this.serviceProvider.GetRequiredService<UserEntityResolver>(),
                    this.serviceProvider.GetRequiredService<IDateTimeService>()),
                InventoryMovementType.Order when typeof(TMovement) == typeof(IInventoryOrderMovement) => new InventoryOrderMovementDomainModel(
                    entity,
                    workshop,
                    this.serviceProvider.GetRequiredService<IRepository>(),
                    this.serviceProvider.GetRequiredService<IInventorySKUManager>(),
                    this.serviceProvider.GetRequiredService<UserEntityResolver>(),
                    this.serviceProvider.GetRequiredService<IHandpieceRequiredPartManager>(),
                    this.serviceProvider.GetRequiredService<IDateTimeService>(),
                    this.serviceProvider.GetRequiredService<ILogger<InventoryOrderMovementDomainModel>>()),
                InventoryMovementType.Found when typeof(TMovement) == typeof(IInventoryFoundMovement) => new InventoryFoundMovementDomainModel(
                    entity,
                    workshop,
                    this.serviceProvider.GetRequiredService<IRepository>(),
                    this.serviceProvider.GetRequiredService<IInventorySKUManager>(),
                    this.serviceProvider.GetRequiredService<UserEntityResolver>(),
                    this.serviceProvider.GetRequiredService<IDateTimeService>()),
                InventoryMovementType.Repair when typeof(TMovement) == typeof(IInventoryRepairMovement) => new InventoryRepairMovementDomainModel(
                    entity,
                    workshop,
                    this.serviceProvider.GetRequiredService<IRepository>(),
                    this.serviceProvider.GetRequiredService<IInventorySKUManager>(),
                    this.serviceProvider.GetRequiredService<UserEntityResolver>(),
                    this.serviceProvider.GetRequiredService<IDateTimeService>(),
                    this.serviceProvider.GetRequiredService<IHandpieceRequiredPartManager>()),
                InventoryMovementType.Lost when typeof(TMovement) == typeof(IInventoryLostMovement) => new InventoryLostMovementDomainModel(
                    entity,
                    workshop,
                    this.serviceProvider.GetRequiredService<IRepository>(),
                    this.serviceProvider.GetRequiredService<IInventorySKUManager>(),
                    this.serviceProvider.GetRequiredService<UserEntityResolver>(),
                    this.serviceProvider.GetRequiredService<IDateTimeService>()),
                InventoryMovementType.RepairFragment when typeof(TMovement) == typeof(IInventoryRepairFragmentMovement) => new InventoryRepairFragmentMovementDomainModel(
                    entity,
                    workshop,
                    this.serviceProvider.GetRequiredService<IRepository>(),
                    this.serviceProvider.GetRequiredService<IInventorySKUManager>(),
                    this.serviceProvider.GetRequiredService<UserEntityResolver>(),
                    this.serviceProvider.GetRequiredService<IDateTimeService>(),
                    this.serviceProvider.GetRequiredService<IHandpieceRequiredPartManager>()),
                InventoryMovementType.MoveFromAnotherWorkshop when typeof(TMovement) == typeof(IInventoryMoveFromAnotherWorkshopMovement) => new InventoryMoveFromAnotherWorkshopMovementDomainModel(
                    entity,
                    workshop,
                    this.serviceProvider.GetRequiredService<IRepository>(),
                    this.serviceProvider.GetRequiredService<IInventorySKUManager>(),
                    this.serviceProvider.GetRequiredService<UserEntityResolver>(),
                    this.serviceProvider.GetRequiredService<IDateTimeService>()),
                InventoryMovementType.MoveToAnotherWorkshop when typeof(TMovement) == typeof(IInventoryMoveToAnotherWorkshopMovement) => new InventoryMoveToAnotherWorkshopMovementDomainModel(
                    entity,
                    workshop,
                    this.serviceProvider.GetRequiredService<IRepository>(),
                    this.serviceProvider.GetRequiredService<IInventorySKUManager>(),
                    this.serviceProvider.GetRequiredService<UserEntityResolver>(),
                    this.serviceProvider.GetRequiredService<IDateTimeService>()),
                InventoryMovementType.EphemeralMissingRequiredQuantity => throw new NotSupportedException("Cannot instantiate ephemeral movement"),
                _ => throw new InvalidOperationException($"Type {entity.Type} is not supported or not compatible with type {typeof(TMovement).FullName}"),
            };

            return (TMovement)domain;
        }
    }
}
