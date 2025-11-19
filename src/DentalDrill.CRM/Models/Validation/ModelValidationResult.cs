using System;
using System.Collections.Generic;
using System.Linq;

namespace DentalDrill.CRM.Models.Validation
{
    public class ModelValidationResult
    {
        private readonly Boolean isValid;
        private readonly List<ModelValidationError> errors;

        private ModelValidationResult(Boolean isValid, IEnumerable<ModelValidationError> errors)
        {
            this.isValid = isValid;
            this.errors = errors?.ToList() ?? new List<ModelValidationError>();
        }

        public Boolean IsValid => this.isValid;

        public IReadOnlyList<ModelValidationError> Errors => this.errors;

        public static ModelValidationResult Valid()
        {
            return new ModelValidationResult(true, null);
        }

        public static ModelValidationResult Invalid(params ModelValidationError[] errors)
        {
            return new ModelValidationResult(false, errors);
        }

        public static ModelValidationResult FromErrorsList(IReadOnlyList<ModelValidationError> errors)
        {
            if (errors.Count == 0)
            {
                return new ModelValidationResult(true, null);
            }
            else
            {
                return new ModelValidationResult(false, errors);
            }
        }
    }
}
