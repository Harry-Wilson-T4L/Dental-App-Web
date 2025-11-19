using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DentalDrill.CRM.Services.Calculation
{
    public interface IGenericArithmeticService<TValueType>
    {
        TValueType Add(TValueType first, TValueType second);

        TValueType Subtract(TValueType first, TValueType second);

        TValueType Multiply(TValueType first, TValueType second);

        TValueType Multiply(TValueType first, Int32 second);

        TValueType Divide(TValueType first, TValueType second);

        Int32 Quotient(TValueType first, TValueType second);

        Int32 Compare(TValueType first, TValueType second);
    }
}
