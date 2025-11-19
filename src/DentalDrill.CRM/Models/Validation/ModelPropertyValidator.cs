using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DentalDrill.CRM.Models.Validation
{
    public abstract class ModelPropertyValidator<T, TProperty> : IModelValidator<T>
    {
        private readonly Expression<Func<T, TProperty>> expression;

        protected ModelPropertyValidator(Expression<Func<T, TProperty>> expression)
        {
            this.expression = expression;
        }

        public Task<ModelValidationResult> ValidateAsync(T model)
        {
            var func = this.expression.Compile();
            var propertyName = ((MemberExpression)this.expression.Body).Member.Name;

            return this.ValidatePropertyAsync(model, propertyName, func(model));
        }

        protected abstract Task<ModelValidationResult> ValidatePropertyAsync(T model, String propertyName, TProperty propertyValue);
    }
}