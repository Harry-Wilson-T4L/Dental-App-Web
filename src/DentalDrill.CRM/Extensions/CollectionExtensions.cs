using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevGuild.AspNetCore.Contracts;

namespace DentalDrill.CRM.Extensions
{
    public static class CollectionExtensions
    {
        public static void SyncWithCollection<TFirst, TSecond>(this ICollection<TFirst> original, ICollection<TSecond> updated, Func<TFirst, TSecond, Boolean> comparer, Func<TSecond, TFirst> mapper)
        {
            Ensure.Argument.NotNull(original, nameof(original));

            if (updated != null && updated.Count > 0)
            {
                var removing = original.Where(x => !updated.Any(y => comparer(x, y))).ToList();
                var adding = updated.Where(x => !original.Any(y => comparer(y, x))).ToList();

                foreach (var removed in removing)
                {
                    original.Remove(removed);
                }

                foreach (var added in adding)
                {
                    original.Add(mapper(added));
                }
            }
            else
            {
                foreach (var removed in original.ToList())
                {
                    original.Remove(removed);
                }
            }
        }

        public static async Task SyncWithCollectionAsync<TFirst, TSecond>(this ICollection<TFirst> original, ICollection<TSecond> updated, Func<TFirst, TSecond, Boolean> comparer, Func<TSecond, Task<TFirst>> mapper)
        {
            Ensure.Argument.NotNull(original, nameof(original));

            if (updated != null && updated.Count > 0)
            {
                var removing = original.Where(x => !updated.Any(y => comparer(x, y))).ToList();
                var adding = updated.Where(x => !original.Any(y => comparer(y, x))).ToList();

                foreach (var removed in removing)
                {
                    original.Remove(removed);
                }

                foreach (var added in adding)
                {
                    original.Add(await mapper(added));
                }
            }
            else
            {
                foreach (var removed in original.ToList())
                {
                    original.Remove(removed);
                }
            }
        }
    }
}
