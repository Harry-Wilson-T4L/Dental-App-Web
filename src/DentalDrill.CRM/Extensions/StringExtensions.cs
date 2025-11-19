using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DentalDrill.CRM.Extensions
{
    public class StringExtensions
    {
        public static String CollapseSpaces(String value)
        {
            String prev;
            do
            {
                prev = value;
                value = prev.Replace("  ", " ");
            }
            while (prev != value);

            return value;
        }
    }
}
