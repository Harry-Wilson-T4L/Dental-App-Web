using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace DentalDrill.CRM.Extensions.ModelBinding
{
    public static class PolymorphicModelBinderExtensions
    {
        public static MvcOptions AddPolymorphicModelBinder<TBase>(this MvcOptions options, Action<PolymorphicModelBinderOptionsBuilder<TBase>> polymorphicOptions)
        {
            var polymorphicOptionsBuilder = new PolymorphicModelBinderOptionsBuilder<TBase>();
            polymorphicOptions(polymorphicOptionsBuilder);
            var (baseType, subtypes) = polymorphicOptionsBuilder.GetResult();

            var bindingProvider = new PolymorphicModelBinderProvider(baseType, subtypes);

            options.ModelBinderProviders.Insert(0, bindingProvider);
            return options;
        }
    }
}
