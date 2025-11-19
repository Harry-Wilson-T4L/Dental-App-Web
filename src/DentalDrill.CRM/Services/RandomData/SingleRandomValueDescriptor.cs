using System;

namespace DentalDrill.CRM.Services.RandomData
{
    public class SingleRandomValueDescriptor<TValueType> : RandomValueDescriptor<TValueType>
    {
        public SingleRandomValueDescriptor()
        {
        }

        public SingleRandomValueDescriptor(TValueType value, Double weight)
            : base(weight)
        {
            this.Value = value;
        }

        public TValueType Value { get; set; }
    }
}