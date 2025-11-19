using System;

namespace DentalDrill.CRM.Models.Validation
{
    public class ModelValidationError
    {
        private readonly String field;
        private readonly String error;

        public ModelValidationError(String field, String error)
        {
            this.field = field;
            this.error = error;
        }

        public String Field => this.field;

        public String Error => this.error;
    }
}