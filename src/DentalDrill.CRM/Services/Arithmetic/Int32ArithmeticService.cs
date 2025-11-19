using System;

namespace DentalDrill.CRM.Services.Calculation
{
    public class Int32ArithmeticService : IGenericArithmeticService<Int32>
    {
        public Int32 Add(Int32 first, Int32 second) => first + second;

        public Int32 Subtract(Int32 first, Int32 second) => first - second;

        public Int32 Multiply(Int32 first, Int32 second) => first * second;

        public Int32 Divide(Int32 first, Int32 second) => first / second;

        public Int32 Quotient(Int32 first, Int32 second) => first / second;

        public Int32 Compare(Int32 first, Int32 second) => first.CompareTo(second);
    }
}