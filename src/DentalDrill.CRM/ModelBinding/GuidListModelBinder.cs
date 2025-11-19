using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevGuild.AspNetCore.Contracts;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DentalDrill.CRM.ModelBinding
{
    public class GuidListModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            Ensure.Argument.NotNull(bindingContext, nameof(bindingContext));

            var modelName = bindingContext.BinderModelName;
            if (String.IsNullOrEmpty(modelName))
            {
                modelName = bindingContext.ModelName;
            }

            var valueProviderResult = bindingContext.ValueProvider.GetValue(modelName);
            if (valueProviderResult == ValueProviderResult.None)
            {
                return Task.CompletedTask;
            }

            bindingContext.ModelState.SetModelValue(modelName, valueProviderResult);

            var rawValues = valueProviderResult.Values
                .ToArray()
                .Select(x => x != null ? x.Split(',', StringSplitOptions.RemoveEmptyEntries) : new String[0])
                .SelectMany(x => x)
                .ToArray();

            var parsedValues = rawValues.Select(x => Guid.TryParse(x, out var result) ? (Guid?)result : null).Where(x => x.HasValue).Select(x => x.Value).ToList();

            bindingContext.Result = ModelBindingResult.Success(parsedValues);
            return Task.CompletedTask;
        }
    }
}
