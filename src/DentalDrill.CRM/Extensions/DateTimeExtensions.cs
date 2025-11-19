using System;

namespace DentalDrill.CRM.Extensions
{
    public static class DateTimeExtensions
    {
        public static Int32 GetMonthIndex(this DateTime dateTime)
        {
            return dateTime.Year * 12 + (dateTime.Month - 1);
        }
    }
}
