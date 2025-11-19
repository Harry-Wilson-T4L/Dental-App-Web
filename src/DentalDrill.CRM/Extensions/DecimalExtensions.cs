using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DentalDrill.CRM.Extensions
{
    public static class DecimalExtensions
    {
        public static Boolean Equals(this Decimal value, Decimal other, Decimal precision)
        {
            return Math.Abs(value - other) < precision;
        }

        public static Boolean NotEquals(this Decimal value, Decimal other, Decimal precision)
        {
            return !value.Equals(other, precision);
        }

        public static Boolean GreaterThan(this Decimal value, Decimal other, Decimal precision)
        {
            return value.NotEquals(other, precision) && value > other;
        }

        public static Boolean GreaterThanOrEqual(this Decimal value, Decimal other, Decimal precision)
        {
            return value.Equals(other, precision) || value > other;
        }

        public static Boolean LessThan(this Decimal value, Decimal other, Decimal precision)
        {
            return value.NotEquals(other, precision) && value < other;
        }

        public static Boolean LessThanOrEqual(this Decimal value, Decimal other, Decimal precision)
        {
            return value.Equals(other, precision)  || value < other;
        }
    }
}
