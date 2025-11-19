using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace DentalDrill.CRM.Domain
{
    public abstract class DomainLoaderBase
    {
        protected static Task<TDomainEntity> ResolveRequiredDependency<TKey, TDomainEntity, TDataEntity>(
            TKey key,
            [CanBeNull] TDataEntity dataEntity,
            [CanBeNull] IReadOnlyDictionary<TKey, TDomainEntity> preloaded,
            Func<TKey, Task<TDomainEntity>> domainGetter,
            Func<TDataEntity, TDomainEntity> domainFactory)
            where TKey : IEquatable<TKey>
        {
            if (preloaded != null && preloaded.TryGetValue(key, out var preloadedEntity))
            {
                return Task.FromResult(preloadedEntity);
            }

            if (dataEntity != null)
            {
                return Task.FromResult(domainFactory(dataEntity));
            }

            return domainGetter(key);
        }

        protected static Task<TDomainEntity> ResolveRequiredDependencyWithLoader<TKey, TDomainEntity, TDataEntity>(
            TKey key,
            [CanBeNull] TDataEntity dataEntity,
            [CanBeNull] IReadOnlyDictionary<TKey, TDomainEntity> preloaded,
            Func<TKey, Task<TDomainEntity>> domainGetter,
            Func<TDataEntity, Task<TDomainEntity>> domainLoader)
            where TKey : IEquatable<TKey>
        {
            if (preloaded != null && preloaded.TryGetValue(key, out var preloadedEntity))
            {
                return Task.FromResult(preloadedEntity);
            }

            if (dataEntity != null)
            {
                return domainLoader(dataEntity);
            }

            return domainGetter(key);
        }

        protected static Task<IReadOnlyDictionary<TKey, TDomainEntity>> PrepareResolver<TKey, TDomainEntity, TDataEntity>(
            IReadOnlyList<KeyValuePair<TKey, TDataEntity>> entitiesData,
            [CanBeNull] IReadOnlyDictionary<TKey, TDomainEntity> preloaded,
            Func<Task<IReadOnlyDictionary<TKey, TDomainEntity>>> domainResolverLoader,
            Func<TDataEntity, TDomainEntity> domainFactory)
            where TKey : IEquatable<TKey>
        {
            var requiredIds = entitiesData.Select(x => x.Key).Distinct().ToList();
            var missingIds = preloaded == null ? requiredIds : requiredIds.Where(x => !preloaded.ContainsKey(x)).ToList();
            if (missingIds.Count == 0)
            {
                return Task.FromResult(preloaded ?? new Dictionary<TKey, TDomainEntity>());
            }

            var resolver = preloaded?.ToDictionary(x => x.Key, x => x.Value) ?? new Dictionary<TKey, TDomainEntity>();
            foreach (var missingId in missingIds)
            {
                var includedEntity = entitiesData.Where(x => x.Key.Equals(missingId) && x.Value != null).Take(1).ToList();
                if (includedEntity.Count == 0)
                {
                    return domainResolverLoader();
                }

                resolver.Add(missingId, domainFactory(includedEntity[0].Value));
            }

            return Task.FromResult<IReadOnlyDictionary<TKey, TDomainEntity>>(resolver);
        }

        protected static Task<IReadOnlyDictionary<TKey, TDomainEntity>> PrepareResolverWithLoader<TKey, TDomainEntity, TDataEntity>(
            IReadOnlyList<KeyValuePair<TKey, TDataEntity>> entitiesData,
            [CanBeNull] IReadOnlyDictionary<TKey, TDomainEntity> preloaded,
            Func<Task<IReadOnlyDictionary<TKey, TDomainEntity>>> domainResolverLoader,
            Func<IReadOnlyList<TDataEntity>, Task<IReadOnlyList<TDomainEntity>>> domainLoader,
            Func<TDomainEntity, TKey> domainKeySelector)
            where TKey : IEquatable<TKey>
        {
            var requiredIds = entitiesData.Select(x => x.Key).Distinct().ToList();
            var missingIds = preloaded == null ? requiredIds : requiredIds.Where(x => !preloaded.ContainsKey(x)).ToList();
            if (missingIds.Count == 0)
            {
                return Task.FromResult(preloaded ?? new Dictionary<TKey, TDomainEntity>());
            }

            var missingEntities = missingIds.Select(x => new { Id = x, Entity = entitiesData.Where(y => y.Key.Equals(x) && y.Value != null).Select(y => y.Value).FirstOrDefault() }).ToList();
            if (missingEntities.All(x => x.Entity != null))
            {
                return MergeLoadedAndPreloadedAsync(preloaded, domainLoader(missingEntities.Select(x => x.Entity).ToList()));
            }
            else
            {
                return domainResolverLoader();
            }

            async Task<IReadOnlyDictionary<TKey, TDomainEntity>> MergeLoadedAndPreloadedAsync(IReadOnlyDictionary<TKey, TDomainEntity> preloaded, Task<IReadOnlyList<TDomainEntity>> loadingTask)
            {
                var resolver = preloaded?.ToDictionary(x => x.Key, x => x.Value) ?? new Dictionary<TKey, TDomainEntity>();
                var loaded = await loadingTask;
                foreach (var loadedEntity in loaded)
                {
                    resolver.Add(domainKeySelector(loadedEntity), loadedEntity);
                }

                return resolver;
            }
        }
    }
}
