using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DentalDrill.CRM.Models;
using DevGuild.AspNetCore.Services.Data;

namespace DentalDrill.CRM.Services
{
    public abstract class ChangeTrackingService<TEntity, TEntityKey, TEntityChange>
        : IChangeTrackingService<TEntity, TEntityChange>, IDisposable
        where TEntity : class
        where TEntityChange : class, IEntityChange<TEntity, TEntityKey>
    {
        protected ChangeTrackingService(IRepositoryFactory repositoryFactory, UserEntityResolver userResolver, IRepository repository)
        {
            this.RepositoryFactory = repositoryFactory;
            this.UserResolver = userResolver;
            this.Repository = repository;
        }

        protected IRepositoryFactory RepositoryFactory { get; }

        protected IRepository Repository { get; }

        protected UserEntityResolver UserResolver { get; }

        public async Task TrackCreatedEntityAsync(TEntity entity, Boolean useCurrentRepository = false)
        {
            var reloaded = await this.ReloadEntity(entity);
            var change = await this.CreateChange(reloaded);

            change.Action = ChangeAction.Create;
            await this.SetChangeInformation(change);
            await this.FillNewVersion(change, reloaded);

            await this.SaveEntityChange(change, useCurrentRepository);
        }

        public async Task<TEntityChange> CaptureEntityForUpdate(TEntity entity)
        {
            var reloaded = await this.ReloadEntity(entity);
            var change = await this.CreateChange(reloaded);

            await this.FillOldVersion(change, reloaded);
            return change;
        }

        public async Task TrackModifyEntityAsync(TEntity entity, TEntityChange capturedChange, Boolean useCurrentRepository = false)
        {
            var reloaded = await this.ReloadEntity(entity);

            capturedChange.Action = ChangeAction.Modify;
            await this.SetChangeInformation(capturedChange);
            await this.FillNewVersion(capturedChange, reloaded);
            await this.SaveEntityChange(capturedChange, useCurrentRepository);
        }

        public async Task TrackDeleteEntityAsync(TEntity entity, TEntityChange capturedChange, Boolean useCurrentRepository = false)
        {
            capturedChange.Action = ChangeAction.Delete;
            await this.SetChangeInformation(capturedChange);
            await this.SaveEntityChange(capturedChange, useCurrentRepository);
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected abstract Task FillOldVersion(TEntityChange change, TEntity entity);

        protected abstract Task FillNewVersion(TEntityChange change, TEntity entity);

        protected abstract Task<TEntity> ReloadEntity(TEntity entity);

        protected abstract Task<TEntityChange> CreateChange(TEntity entity);

        protected virtual async Task SetChangeInformation(TEntityChange change)
        {
            var user = await this.UserResolver.ResolveCurrentUserEntity();
            if (!(user is Employee employee))
            {
                throw new InvalidOperationException("User is not an Employee");
            }

            change.ChangedOn = DateTime.UtcNow;
            change.ChangedById = employee.Id;
        }

        protected virtual async Task SaveEntityChange(TEntityChange change, Boolean useCurrentRepository)
        {
            if (useCurrentRepository)
            {
                await this.Repository.InsertAsync(change);
            }
            else
            {
                using (var repository = this.RepositoryFactory.CreateRepository())
                {
                    await repository.InsertAsync(change);
                    await repository.SaveChangesAsync();
                }
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.Repository?.Dispose();
            }
        }
    }
}
