using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DentalDrill.CRM.Domain.Abstractions;
using DentalDrill.CRM.Models;
using DentalDrill.CRM.Services;
using DevGuild.AspNetCore.Services.Data;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace DentalDrill.CRM.Domain
{
    public abstract class InventoryMovementDomainModelBase : IInventoryMovement
    {
        private readonly IWorkshop workshop;

        private readonly IRepository repository;
        private readonly IInventorySKUManager inventorySKUManager;
        private readonly UserEntityResolver userResolver;
        private readonly IDateTimeService dateTimeService;

        private InventoryMovement entity;

        protected InventoryMovementDomainModelBase(
            InventoryMovement entity,
            IWorkshop workshop,
            IRepository repository,
            IInventorySKUManager inventorySKUManager,
            UserEntityResolver userResolver,
            IDateTimeService dateTimeService)
        {
            this.repository = repository;
            this.inventorySKUManager = inventorySKUManager;
            this.userResolver = userResolver;
            this.dateTimeService = dateTimeService;

            this.entity = entity;
            this.workshop = workshop;
        }

        public Guid Id => this.entity.Id;

        public IWorkshop Workshop => this.workshop;

        public Guid SKUId => this.entity.SKUId;

        public InventoryMovementDirection Direction => this.entity.Direction;

        public InventoryMovementType Type => this.entity.Type;

        public InventoryMovementStatus Status => this.entity.Status;

        public Decimal Quantity => this.entity.Quantity;

        public Decimal QuantityAbsolute => this.entity.QuantityAbsolute;

        public Decimal? Price => this.entity.Price;

        public DateTime CreatedOn => this.entity.CreatedOn;

        public DateTime? CompletedOn => this.entity.CompletedOn;

        public String Comment => this.entity.Comment;

        protected InventoryMovement Entity => this.entity;

        protected IRepository Repository => this.repository;

        protected IInventorySKUManager InventorySKUManager => this.inventorySKUManager;

        public async Task RefreshAsync()
        {
            var newEntity = await this.repository.QueryWithoutTracking<InventoryMovement>().SingleOrDefaultAsync(x => x.Id == this.entity.Id);
            if (newEntity == null)
            {
                throw new InvalidOperationException("Entity is missing after refresh");
            }

            if (newEntity.Type != this.entity.Type)
            {
                throw new InvalidOperationException("Entity has different type");
            }

            this.entity = newEntity;
        }

        public Task<IInventorySKU> GetSKUAsync()
        {
            return this.inventorySKUManager.GetByIdAsync(this.SKUId);
        }

        public async Task<IReadOnlyList<InventoryMovementChange>> GetChangesAsync()
        {
            var changes = await this.repository.QueryWithoutTracking<InventoryMovementChange>()
                .Include(x => x.ChangedBy)
                .Where(x => x.MovementId == this.Id)
                .OrderBy(x => x.ChangedOn)
                .ToListAsync();

            return changes;
        }

        public async Task TrackCreation()
        {
            var trackedChange = await this.PrepareTrackChangeAsync(ChangeAction.Create, null);
            await this.TrackChangeAsync(trackedChange, this.entity);
        }

        protected async Task<InventoryMovementChange> PrepareTrackChangeAsync(ChangeAction action, InventoryMovement entity)
        {
            var user = await this.userResolver.ResolveCurrentUserEntity();
            if (!(user is Employee employee))
            {
                throw new InvalidOperationException("User is not an Employee");
            }

            var change = new InventoryMovementChange
            {
                Id = Guid.NewGuid(),
                SKUId = this.SKUId,
                MovementId = this.Id,
                ChangedOn = this.dateTimeService.CurrentUtcTime,
                ChangedById = employee.Id,
                Action = action,
            };

            if (action == ChangeAction.Modify || action == ChangeAction.Delete)
            {
                this.FillOldChangeInfo(change, entity);
            }

            return change;
        }

        protected async Task TrackChangeAsync(InventoryMovementChange change, InventoryMovement entity)
        {
            if (change.Action == ChangeAction.Create || change.Action == ChangeAction.Modify)
            {
                this.FillNewChangeInfo(change, entity);
            }

            await this.Repository.InsertAsync(change);
        }

        private void FillOldChangeInfo(InventoryMovementChange change, InventoryMovement entity)
        {
            change.OldStatus = entity.Status;
            change.OldQuantity = entity.Quantity;
            change.OldPrice = entity.Price;
            change.OldComment = entity.Comment;
            change.OldContent = JsonConvert.SerializeObject(new
            {
                Direction = entity.Direction,
                Type = entity.Type,
                Status = entity.Status,
                Quantity = entity.Quantity,
                QuantityAbsolute = entity.QuantityAbsolute,
                Comment = entity.Comment,
                Price = entity.Price,
            });
        }

        private void FillNewChangeInfo(InventoryMovementChange change, InventoryMovement entity)
        {
            change.NewStatus = entity.Status;
            change.NewQuantity = entity.Quantity;
            change.NewPrice = entity.Price;
            change.NewComment = entity.Comment;
            change.NewContent = JsonConvert.SerializeObject(new
            {
                Direction = entity.Direction,
                Type = entity.Type,
                Status = entity.Status,
                Quantity = entity.Quantity,
                QuantityAbsolute = entity.QuantityAbsolute,
                Comment = entity.Comment,
                Price = entity.Price,
            });
        }
    }
}
