using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DentalDrill.CRM.Domain.Abstractions;
using DentalDrill.CRM.Models;
using DevGuild.AspNetCore.Services.Data;
using Microsoft.EntityFrameworkCore;

namespace DentalDrill.CRM.Domain
{
    public class InventoryMovementManager : IInventoryMovementManager
    {
        private readonly IRepository repository;
        private readonly IInventoryMovementFactory factory;
        private readonly IWorkshopManager workshopManager;

        public InventoryMovementManager(IRepository repository, IInventoryMovementFactory factory, IWorkshopManager workshopManager)
        {
            this.repository = repository;
            this.factory = factory;
            this.workshopManager = workshopManager;
        }

        public async Task<IInventoryMovement> GetById(Guid id)
        {
            var entity = await this.repository.QueryWithoutTracking<InventoryMovement>().SingleOrDefaultAsync(x => x.Id == id);
            if (entity == null)
            {
                return null;
            }

            var workshop = await this.workshopManager.GetByIdAsync(entity.WorkshopId);
            return this.factory.Create(entity, workshop);
        }

        public async Task<TMovement> GetById<TMovement>(Guid id)
            where TMovement : class, IInventoryMovement
        {
            var entity = await this.repository.QueryWithoutTracking<InventoryMovement>().SingleOrDefaultAsync(x => x.Id == id);
            if (entity == null)
            {
                return null;
            }

            var workshop = await this.workshopManager.GetByIdAsync(entity.WorkshopId);
            return this.factory.Create<TMovement>(entity, workshop);
        }
    }
}
