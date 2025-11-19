using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DentalDrill.CRM.Extensions.ModelBinding
{
    public class PolymorphicModelBinderOptionsBuilder<TBase>
    {
        private readonly Type baseType;
        private readonly Dictionary<String, Type> subtypes;

        public PolymorphicModelBinderOptionsBuilder()
        {
            this.baseType = typeof(TBase);
            this.subtypes = new Dictionary<String, Type>();
        }

        public PolymorphicModelBinderOptionsBuilder<TBase> AddDerived<TDerived>()
            where TDerived : TBase
        {
            return this.AddDerived<TDerived>(typeof(TDerived).Name);
        }

        public PolymorphicModelBinderOptionsBuilder<TBase> AddDerived<TDerived>(String name)
            where TDerived : TBase
        {
            this.subtypes.Add(name, typeof(TDerived));
            return this;
        }

        public (Type Type, IReadOnlyDictionary<String, Type> Subtypes) GetResult()
        {
            return (this.baseType, this.subtypes);
        }
    }
}
