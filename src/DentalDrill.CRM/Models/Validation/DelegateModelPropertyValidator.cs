using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DentalDrill.CRM.Models.Validation
{
    public class DelegateModelPropertyValidator<T, TProperty> : ModelPropertyValidator<T, TProperty>
    {
        private readonly ValidatorDelegate validationDelegate;

        public DelegateModelPropertyValidator(Expression<Func<T, TProperty>> expression, ValidatorDelegate validationDelegate)
            : base(expression)
        {
            this.validationDelegate = validationDelegate;
        }

        public delegate Task<ModelValidationResult> ValidatorDelegate(T model, String propertyName, TProperty propertyValue);

        protected override Task<ModelValidationResult> ValidatePropertyAsync(T model, String propertyName, TProperty propertyValue)
        {
            return this.validationDelegate(model, propertyName, propertyValue);
        }
    }
}