using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using String = System.String;

namespace DentalDrill.CRM.Models.Validation
{
    public static class ModelValidationResultExtensions
    {
        public static void MergeToModelState(this ModelValidationResult result, ModelStateDictionary modelState, String prefix)
        {
            if (result.IsValid)
            {
                return;
            }

            foreach (var error in result.Errors)
            {
                modelState.AddModelError(MergeName(prefix, error.Field), error.Error);
            }

            String MergeName(String prefix, String property)
            {
                if (String.IsNullOrEmpty(prefix))
                {
                    return property;
                }

                if (String.IsNullOrEmpty(property))
                {
                    return prefix;
                }

                if (property.StartsWith("["))
                {
                    return prefix + property;
                }

                return prefix + "." + property;
            }
        }
    }
}
