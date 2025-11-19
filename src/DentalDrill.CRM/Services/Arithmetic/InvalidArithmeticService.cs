using System;

namespace DentalDrill.CRM.Services.Calculation
{
    public class InvalidArithmeticService<TValueType> : IGenericArithmeticService<TValueType>
    {
        public TValueType Add(TValueType first, TValueType second) => throw new NotSupportedException();

        public TValueType Subtract(TValueType first, TValueType second) => throw new NotSupportedException();

        public TValueType Multiply(TValueType first, TValueType second) => throw new NotSupportedException();

        public TValueType Multiply(TValueType first, Int32 second) => throw new NotSupportedException();

        public TValueType Divide(TValueType first, TValueType second) => throw new NotSupportedException();

        public Int32 Quotient(TValueType first, TValueType second) => throw new NotSupportedException();

        public Int32 Compare(TValueType first, TValueType second) => throw new NotSupportedException();
    }
}