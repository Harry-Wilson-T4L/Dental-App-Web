using System;
using System.Collections.Generic;

namespace DentalDrill.CRM.Models.Validation
{
    public class ModelValidatorCollection<T>
    {
        private readonly List<IModelValidator<T>> validators = new List<IModelValidator<T>>();
        private Boolean isSealed = false;

        public IReadOnlyList<IModelValidator<T>> Validators => this.validators;

        public void AddValidator(IModelValidator<T> validator)
        {
            if (this.isSealed)
            {
                throw new InvalidOperationException();
            }

            this.validators.Add(validator);
        }
    }
}