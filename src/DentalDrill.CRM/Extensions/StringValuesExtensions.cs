using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Primitives;

namespace DentalDrill.CRM.Extensions
{
    public static class StringValuesExtensions
    {
        public static Boolean AsBooleanOrDefault(this StringValues values, Boolean defaultValue = false)
        {
            if (values.Count == 0)
            {
                return defaultValue;
            }

            var firstValue = values[0];
            return Boolean.Parse(firstValue);
        }

        public static Int32 AsInt32OrDefault(this StringValues values, Int32 defaultValue = 0)
        {
            if (values.Count == 0)
            {
                return defaultValue;
            }

            var firstValue = values[0];
            return Int32.Parse(firstValue);
        }

        public static String AsStringOrDefault(this StringValues values, String defaultValues = null)
        {
            if (values.Count == 0)
            {
                return defaultValues;
            }

            return values[0];
        }
    }
}
