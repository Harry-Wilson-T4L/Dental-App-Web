using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kendo.Mvc.UI.Fluent;

namespace DentalDrill.CRM.Extensions
{
    public static class GridBuilderExtensions
    {
        public static GridBuilder<T> When<T>(this GridBuilder<T> builder, Boolean condition, Func<GridBuilder<T>, GridBuilder<T>> configuration)
            where T : class
        {
            if (condition)
            {
                return configuration(builder);
            }
            else
            {
                return builder;
            }
        }
    }
}
