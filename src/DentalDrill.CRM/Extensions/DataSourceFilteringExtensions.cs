using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kendo.Mvc;
using Kendo.Mvc.UI;

namespace DentalDrill.CRM.Extensions
{
    public static class DataSourceFilteringExtensions
    {
        public static void ChangeFilterValueType<TSource, TTarget>(this DataSourceRequest request, String fieldName, Func<TSource, TTarget> converter)
        {
            request.RemapFilterFields(new Dictionary<String, Action<FilterDescriptor>>
            {
                [fieldName] = descriptor =>
                {
                    if (descriptor.Value is TSource source)
                    {
                        descriptor.Value = converter(source);
                    }
                },
            });
        }

        public static void RenameFields(this DataSourceRequest request, IDictionary<String, String> renames)
        {
            request.RemapFilterFields(renames.Select(x => new
            {
                Key = x.Key,
                Value = (Action<FilterDescriptor>)(descriptor => descriptor.Member = x.Value),
            }).ToDictionary(x => x.Key, x => x.Value));
            request.RemapSortFields(renames.Select(x => new
            {
                Key = x.Key,
                Value = (Action<SortDescriptor>)(descriptor => descriptor.Member = x.Value),
            }).ToDictionary(x => x.Key, x => x.Value));
        }

        public static void RemapFilterFields(this DataSourceRequest request, IDictionary<String, Action<FilterDescriptor>> actions)
        {
            void ProcessFilters(IEnumerable<IFilterDescriptor> filters)
            {
                foreach (var filter in filters)
                {
                    switch (filter)
                    {
                        case FilterDescriptor normal:
                            if (actions.TryGetValue(normal.Member, out var action))
                            {
                                action(normal);
                            }

                            break;
                        case CompositeFilterDescriptor composite:
                            ProcessFilters(composite.FilterDescriptors);
                            break;
                    }
                }
            }

            ProcessFilters(request.Filters);
        }

        public static void RemapSortFields(this DataSourceRequest request, IDictionary<String, Action<SortDescriptor>> actions)
        {
            foreach (var sort in request.Sorts)
            {
                if (actions.TryGetValue(sort.Member, out var action))
                {
                    action(sort);
                }
            }
        }

        public static void ReplaceFilterOfFiled(this DataSourceRequest request, String fieldName, FilterOperator filterOperator, Func<FilterDescriptor, IFilterDescriptor> replacement)
        {
            if (request.Filters != null)
            {
                ProcessFilterCollection(request.Filters);
            }

            void ProcessFilterCollection(IList<IFilterDescriptor> filters)
            {
                for (var i = 0; i < filters.Count; i++)
                {
                    var filter = filters[i];
                    switch (filter)
                    {
                        case FilterDescriptor single:
                            if (single.Member == fieldName && single.Operator == filterOperator)
                            {
                                filters[i] = replacement(single);
                            }

                            break;

                        case CompositeFilterDescriptor composite:
                            ProcessFilterCollection(composite.FilterDescriptors);
                            break;
                    }
                }
            }
        }
    }
}
