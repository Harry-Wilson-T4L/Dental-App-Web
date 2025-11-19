using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace DentalDrill.CRM.Extensions.ModelBinding
{
    public class PolymorphicModelBinder : IModelBinder
    {
        private readonly IReadOnlyDictionary<String, (Type Type, ModelMetadata Metadata, IModelBinder Binder)> binders;

        public PolymorphicModelBinder(IReadOnlyDictionary<String, (Type Type, ModelMetadata Metadata, IModelBinder Binder)> binders)
        {
            this.binders = binders;
        }

        public async Task BindModelAsync(ModelBindingContext bindingContext)
        {
            var modelTypeName = ModelNames.CreatePropertyModelName(bindingContext.ModelName, "__Type");
            var modelTypeValue = bindingContext.ValueProvider.GetValue(modelTypeName).FirstValue;
            if (String.IsNullOrEmpty(modelTypeValue))
            {
                bindingContext.Result = ModelBindingResult.Failed();
                return;
            }

            if (!this.binders.TryGetValue(modelTypeValue, out var binder))
            {
                bindingContext.Result = ModelBindingResult.Failed();
                return;
            }

            var newBindingContext = DefaultModelBindingContext.CreateBindingContext(
                actionContext: bindingContext.ActionContext,
                valueProvider: bindingContext.ValueProvider,
                metadata: binder.Metadata,
                bindingInfo: null,
                modelName: bindingContext.ModelName);

            await binder.Binder.BindModelAsync(newBindingContext);
            bindingContext.Result = newBindingContext.Result;
            if (bindingContext.Result.IsModelSet)
            {
                bindingContext.ValidationState[bindingContext.Result] = new ValidationStateEntry
                {
                    Metadata = binder.Metadata,
                };
            }
        }
    }
}
