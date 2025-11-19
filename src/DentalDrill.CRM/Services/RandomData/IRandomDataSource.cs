using System;

namespace DentalDrill.CRM.Services.RandomData
{
    public interface IRandomDataSource
    {
        Int32 GenerateInt32();

        Double GenerateDouble();
    }
}