using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Xml.Linq;

namespace DentalDrill.SmartFreight.Client.Models
{
    internal static class Primitives
    {
        public static Boolean? ParseBooleanValue(String value)
        {
            if (String.IsNullOrEmpty(value))
            {
                return null;
            }

            if (String.Equals(value, "Yes", StringComparison.InvariantCultureIgnoreCase) ||
                String.Equals(value, "Y", StringComparison.InvariantCultureIgnoreCase) ||
                String.Equals(value, "True", StringComparison.InvariantCultureIgnoreCase) ||
                String.Equals(value, "1") ||
                String.Equals(value, "-1"))
            {
                return true;
            }

            if (String.Equals(value, "No", StringComparison.InvariantCultureIgnoreCase) ||
                String.Equals(value, "N", StringComparison.InvariantCultureIgnoreCase) ||
                String.Equals(value, "False", StringComparison.InvariantCultureIgnoreCase) ||
                String.Equals(value, "0"))
            {
                return false;
            }

            return null;
        }

        public static DateTime? ParseAsDateTime(this XElement node)
        {
            if (node == null)
            {
                return null;
            }

            var stringValue = node.Value;
            if (String.IsNullOrEmpty(stringValue))
            {
                return null;
            }

            if (DateTime.TryParseExact(stringValue, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var dateValue))
            {
                return dateValue;
            }

            return null;
        }

        public static Decimal? ParseAsDecimal(this XElement node)
        {
            if (node == null)
            {
                return null;
            }

            var stringValue = node.Value;
            if (String.IsNullOrEmpty(stringValue))
            {
                return null;
            }

            if (Decimal.TryParse(stringValue, NumberStyles.Any, CultureInfo.InvariantCulture, out var decimalValue))
            {
                return decimalValue;
            }

            return null;
        }

        public static T? ParseEnum<T>(this XElement node, params (String Name, T Value)[] valuesMap)
            where T : struct
        {
            if (node == null)
            {
                return null;
            }

            var stringValue = node.Value;
            if (String.IsNullOrEmpty(stringValue))
            {
                return null;
            }

            foreach (var valuePair in valuesMap)
            {
                if (String.Equals(valuePair.Name, stringValue, StringComparison.InvariantCultureIgnoreCase))
                {
                    return valuePair.Value;
                }
            }

            return null;
        }
    }
}
