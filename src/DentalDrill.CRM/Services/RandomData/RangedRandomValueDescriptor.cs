using System;

namespace DentalDrill.CRM.Services.RandomData
{
    public class RangedRandomValueDescriptor<TValueType> : RandomValueDescriptor<TValueType>
    {
        public RangedRandomValueDescriptor()
        {
        }

        public RangedRandomValueDescriptor(TValueType minValue, TValueType maxValue, TValueType step, Double weight)
            : base(weight)
        {
            this.MinValue = minValue;
            this.MaxValue = maxValue;
            this.Step = step;
        }

        public TValueType MinValue { get; set; }

        public TValueType MaxValue { get; set; }

        public TValueType Step { get; set; }
    }
}