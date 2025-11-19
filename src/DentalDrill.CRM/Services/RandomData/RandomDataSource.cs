using System;

namespace DentalDrill.CRM.Services.RandomData
{
    public class RandomDataSource : IRandomDataSource
    {
        private readonly Random rng = new Random();

        public Int32 GenerateInt32()
        {
            return this.rng.Next();
        }

        public Double GenerateDouble()
        {
            return this.rng.NextDouble();
        }
    }
}
