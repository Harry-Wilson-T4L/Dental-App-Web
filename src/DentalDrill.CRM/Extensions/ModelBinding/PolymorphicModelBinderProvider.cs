using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DentalDrill.CRM.Extensions.ModelBinding
{
    public class PolymorphicModelBinderProvider : IModelBinderProvider
    {
        private readonly Type baseType;
        private readonly IReadOnlyDictionary<String, Type> subtypes;

        public PolymorphicModelBinderProvider(Type baseType, IReadOnlyDictionary<String, Type> subtypes)
        {
            this.baseType = baseType;
            this.subtypes = subtypes;
        }

        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context.Metadata.ModelType != this.baseType)
            {
                return null;
            }

            var binders = new Dictionary<String, (Type Type, ModelMetadata Metadata, IModelBinder Binder)>();
            foreach (var subtype in this.subtypes)
            {
                var subtypeMetadata = context.MetadataProvider.GetMetadataForType(subtype.Value);
                var subtypeBinder = context.CreateBinder(subtypeMetadata);

                binders.Add(subtype.Key, (subtype.Value, subtypeMetadata, subtypeBinder));
            }

            return new PolymorphicModelBinder(binders);
        }
    }
}
