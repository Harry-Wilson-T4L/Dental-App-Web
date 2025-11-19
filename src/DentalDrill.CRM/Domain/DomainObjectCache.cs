using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DentalDrill.CRM.Domain
{
    public class DomainObjectCache<TKey, TAbstraction>
    {
        private readonly Dictionary<TKey, TAbstraction> cacheStore = new Dictionary<TKey, TAbstraction>();

        public Task<TAbstraction> TryGetCachedValue(TKey key, Func<TKey, Task<TAbstraction>> factory)
        {
            if (this.cacheStore.TryGetValue(key, out var cached))
            {
                return Task.FromResult(cached);
            }

            return CreateAndCache();

            async Task<TAbstraction> CreateAndCache()
            {
                var created = await factory(key);
                this.cacheStore.Add(key, created);

                return created;
            }
        }
    }
}
