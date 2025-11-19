using System;

namespace DentalDrill.CRM.Services.RandomData
{
    public abstract class RandomValueDescriptor<TValueType>
    {
        protected RandomValueDescriptor()
        {
        }

        protected RandomValueDescriptor(Double weight)
        {
            this.Weight = weight;
        }

        public Double Weight { get; set; }
    }
}
