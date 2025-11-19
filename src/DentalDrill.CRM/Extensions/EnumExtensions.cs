using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace DentalDrill.CRM.Extensions
{
    public static class EnumExtensions
    {
        public static Boolean IsOneOf<T>(this T value, T possibleValue1)
            where T : Enum
        {
            return value.Equals(possibleValue1);
        }

        public static Boolean IsOneOf<T>(this T value, T possibleValue1, T possibleValue2)
            where T : Enum
        {
            return value.Equals(possibleValue1) || value.Equals(possibleValue2);
        }

        public static Boolean IsOneOf<T>(this T value, T possibleValue1, T possibleValue2, T possibleValue3)
            where T : Enum
        {
            return value.Equals(possibleValue1) || value.Equals(possibleValue2) || value.Equals(possibleValue3);
        }

        public static Boolean IsOneOf<T>(this T value, T possibleValue1, T possibleValue2, T possibleValue3, T possibleValue4)
            where T : Enum
        {
            return value.Equals(possibleValue1) || value.Equals(possibleValue2) || value.Equals(possibleValue3) || value.Equals(possibleValue4);
        }

        public static Boolean IsOneOf<T>(this T value, T possibleValue1, T possibleValue2, T possibleValue3, T possibleValue4, T possibleValue5)
            where T : Enum
        {
            return value.Equals(possibleValue1) || value.Equals(possibleValue2) || value.Equals(possibleValue3) || value.Equals(possibleValue4) || value.Equals(possibleValue5);
        }

        public static Boolean IsOneOf<T>(this T value, params T[] possibleValues)
            where T : Enum
        {
            foreach (var possibleValue in possibleValues)
            {
                if (value.Equals(possibleValue))
                {
                    return true;
                }
            }

            return false;
        }

        public static Boolean IsNotOneOf<T>(this T value, T invalidValue1)
            where T : Enum
        {
            return !value.Equals(invalidValue1);
        }

        public static Boolean IsNotOneOf<T>(this T value, T invalidValue1, T invalidValue2)
            where T : Enum
        {
            return !value.Equals(invalidValue1) && !value.Equals(invalidValue2);
        }

        public static Boolean IsNotOneOf<T>(this T value, T invalidValue1, T invalidValue2, T invalidValue3)
            where T : Enum
        {
            return !value.Equals(invalidValue1) && !value.Equals(invalidValue2) && !value.Equals(invalidValue3);
        }

        public static Boolean IsNotOneOf<T>(this T value, T invalidValue1, T invalidValue2, T invalidValue3, T invalidValue4)
            where T : Enum
        {
            return !value.Equals(invalidValue1) && !value.Equals(invalidValue2) && !value.Equals(invalidValue3) && !value.Equals(invalidValue4);
        }

        public static Boolean IsNotOneOf<T>(this T value, T invalidValue1, T invalidValue2, T invalidValue3, T invalidValue4, T invalidValue5)
            where T : Enum
        {
            return !value.Equals(invalidValue1) && !value.Equals(invalidValue2) && !value.Equals(invalidValue3) && !value.Equals(invalidValue4) && !value.Equals(invalidValue5);
        }

        public static Boolean IsNotOneOf<T>(this T value, params T[] invalidValues)
            where T : Enum
        {
            foreach (var invalidValue in invalidValues)
            {
                if (value.Equals(invalidValues))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
