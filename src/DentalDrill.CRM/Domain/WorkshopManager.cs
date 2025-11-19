using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DentalDrill.CRM.Domain.Abstractions;
using DentalDrill.CRM.Models;
using DevGuild.AspNetCore.Services.Data;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace DentalDrill.CRM.Domain
{
    public class WorkshopManager : IWorkshopManager
    {
        private readonly IRepository repository;
        private readonly IWorkshopFactory factory;

        public WorkshopManager(IRepository repository, IWorkshopFactory factory)
        {
            this.repository = repository;
            this.factory = factory;
        }

        public async Task<IReadOnlyList<IWorkshop>> ListAllAsync()
        {
            var entities = await this.repository.Query<Workshop>()
                .OrderBy(x => x.OrderNo)
                .ToListAsync();

            return entities.Select(x => this.factory.Create(x)).ToList();
        }

        public async Task<IReadOnlyList<IWorkshop>> ListAllAsync(IEmployeeAccess access, Func<IEmployeeAccessWorkshop, Boolean> predicate)
        {
            var accessibleWorkshops = access.Workshops.GetAvailable(predicate);
            var entities = await this.repository.Query<Workshop>()
                .Where(x => accessibleWorkshops.Contains(x.Id))
                .OrderBy(x => x.OrderNo)
                .ToListAsync();

            return entities.Select(x => this.factory.Create(x)).ToList();
        }

        public async Task<IReadOnlyList<IWorkshop>> ListActiveAsync([CanBeNull] IReadOnlyList<Guid> extra = null)
        {
            var entities = await this.repository.Query<Workshop>()
                .Where(extra == null
                    ? x => x.DeletionStatus == DeletionStatus.Normal
                    : x => x.DeletionStatus == DeletionStatus.Normal || extra.Contains(x.Id))
                .OrderBy(x => x.OrderNo)
                .ToListAsync();

            return entities.Select(x => this.factory.Create(x)).ToList();
        }

        public async Task<IReadOnlyList<IWorkshop>> ListActiveAsync(IEmployeeAccess access, Func<IEmployeeAccessWorkshop, Boolean> predicate, IReadOnlyList<Guid> extra = null)
        {
            var accessibleWorkshops = access.Workshops.GetAvailable(predicate);
            var entities = await this.repository.Query<Workshop>()
                .Where(x => accessibleWorkshops.Contains(x.Id))
                .Where(extra == null
                    ? x => x.DeletionStatus == DeletionStatus.Normal
                    : x => x.DeletionStatus == DeletionStatus.Normal || extra.Contains(x.Id))
                .OrderBy(x => x.OrderNo)
                .ToListAsync();

            return entities.Select(x => this.factory.Create(x)).ToList();
        }

        public async Task<IReadOnlyDictionary<Guid, IWorkshop>> LoadResolverAsync()
        {
            var entities = await this.repository.Query<Workshop>()
                .OrderBy(x => x.OrderNo)
                .ToListAsync();

            return entities.Select(x => this.factory.Create(x)).ToDictionary(x => x.Id);
        }

        public async Task<IWorkshop> GetSydneyAsync()
        {
            var id = new Guid("3a3fd5ae-b473-4846-8eec-64f7830a9408");
            var entity = await this.repository.Query<Workshop>().SingleOrDefaultAsync(x => x.Id == id);
            if (entity == null)
            {
                throw new InvalidOperationException("Sydney workshop not found");
            }

            return this.factory.Create(entity);
        }

        public async Task<IWorkshop> GetByIdAsync(Guid id)
        {
            var entity = await this.repository.Query<Workshop>().SingleOrDefaultAsync(x => x.Id == id);
            if (entity == null)
            {
                return null;
            }

            return this.factory.Create(entity);
        }

        public async Task<IWorkshop> GetActiveByIdAsync(Guid id)
        {
            var entity = await this.repository.Query<Workshop>().SingleOrDefaultAsync(x => x.Id == id && x.DeletionStatus == DeletionStatus.Normal);
            if (entity == null)
            {
                return null;
            }

            return this.factory.Create(entity);
        }
    }
}
