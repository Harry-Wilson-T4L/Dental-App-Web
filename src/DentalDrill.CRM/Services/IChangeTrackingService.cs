using System;
using System.Threading.Tasks;

namespace DentalDrill.CRM.Services
{
    public interface IChangeTrackingService<in TEntity, TEntityChange>
        where TEntity : class
        where TEntityChange : class
    {
        Task TrackCreatedEntityAsync(TEntity entity, Boolean useCurrentRepository = false);

        Task<TEntityChange> CaptureEntityForUpdate(TEntity entity);

        Task TrackModifyEntityAsync(TEntity entity, TEntityChange capturedChange, Boolean useCurrentRepository = false);

        Task TrackDeleteEntityAsync(TEntity entity, TEntityChange capturedChange, Boolean useCurrentRepository = false);
    }
}