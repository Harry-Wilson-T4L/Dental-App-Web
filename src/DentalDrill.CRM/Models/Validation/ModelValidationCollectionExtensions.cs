using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DentalDrill.CRM.Models.Validation
{
    public static class ModelValidationCollectionExtensions
    {
        public static ModelValidatorCollection<T> Custom<T>(this ModelValidatorCollection<T> collection, Func<T, Boolean> condition, String propertyName, String errorMessage)
        {
            var validator = new DelegateModelValidator<T>(model =>
            {
                if (!condition(model))
                {
                    return Task.FromResult(ModelValidationResult.Invalid(new ModelValidationError(propertyName, errorMessage)));
                }

                return Task.FromResult(ModelValidationResult.Valid());
            });
            collection.AddValidator(validator);
            return collection;
        }

        public static ModelValidatorCollection<T> Custom<T, TProperty>(this ModelValidatorCollection<T> collection, Expression<Func<T, TProperty>> expression, Func<TProperty, Boolean> condition, String errorMessage)
        {
            var validator = new DelegateModelPropertyValidator<T, TProperty>(expression, (model, propertyName, propertyValue) =>
            {
                if (!condition(propertyValue))
                {
                    return Task.FromResult(ModelValidationResult.Invalid(new ModelValidationError(propertyName, errorMessage)));
                }

                return Task.FromResult(ModelValidationResult.Valid());
            });
            collection.AddValidator(validator);
            return collection;
        }

        public static ModelValidatorCollection<T> CustomAsync<T>(this ModelValidatorCollection<T> collection, Func<T, Task<Boolean>> asyncCondition, String propertyName, String errorMessage)
        {
            var validator = new DelegateModelValidator<T>(async model =>
            {
                if (!await asyncCondition(model))
                {
                    return ModelValidationResult.Invalid(new ModelValidationError(propertyName, errorMessage));
                }

                return ModelValidationResult.Valid();
            });

            collection.AddValidator(validator);
            return collection;
        }

        public static ModelValidatorCollection<T> Fact<T>(this ModelValidatorCollection<T> collection, Boolean fact, String propertyName, String errorMessage)
        {
            if (!fact)
            {
                var validator = new DelegateModelValidator<T>(model => Task.FromResult(ModelValidationResult.Invalid(new ModelValidationError(propertyName, errorMessage))));
                collection.AddValidator(validator);
            }

            return collection;
        }

        public static ModelValidatorCollection<T> NotNull<T, TProperty>(this ModelValidatorCollection<T> collection, Expression<Func<T, TProperty>> expression, String errorMessage)
        {
            return collection.Custom(expression, x => x != null, errorMessage);
        }

        public static ModelValidatorCollection<T> NotNullOrEmpty<T>(this ModelValidatorCollection<T> collection, Expression<Func<T, String>> expression, String errorMessage)
        {
            return collection.Custom(expression, x => !String.IsNullOrEmpty(x), errorMessage);
        }

        public static ModelValidatorCollection<T> EqualTo<T, TProperty>(this ModelValidatorCollection<T> collection, Expression<Func<T, TProperty>> expression, TProperty minimumValue, String errorMessage)
            where TProperty : IComparable<TProperty>
        {
            return collection.Custom(expression, x => x.CompareTo(minimumValue) == 0, errorMessage);
        }

        public static ModelValidatorCollection<T> NotEqualTo<T, TProperty>(this ModelValidatorCollection<T> collection, Expression<Func<T, TProperty>> expression, TProperty minimumValue, String errorMessage)
            where TProperty : IComparable<TProperty>
        {
            return collection.Custom(expression, x => x.CompareTo(minimumValue) != 0, errorMessage);
        }

        public static ModelValidatorCollection<T> GreaterThan<T, TProperty>(this ModelValidatorCollection<T> collection, Expression<Func<T, TProperty>> expression, TProperty minimumValue, String errorMessage)
            where TProperty : IComparable<TProperty>
        {
            return collection.Custom(expression, x => x.CompareTo(minimumValue) > 0, errorMessage);
        }

        public static ModelValidatorCollection<T> GreaterThanOrEqualTo<T, TProperty>(this ModelValidatorCollection<T> collection, Expression<Func<T, TProperty>> expression, TProperty minimumValue, String errorMessage)
            where TProperty : IComparable<TProperty>
        {
            return collection.Custom(expression, x => x.CompareTo(minimumValue) >= 0, errorMessage);
        }

        public static ModelValidatorCollection<T> LessThan<T, TProperty>(this ModelValidatorCollection<T> collection, Expression<Func<T, TProperty>> expression, TProperty minimumValue, String errorMessage)
            where TProperty : IComparable<TProperty>
        {
            return collection.Custom(expression, x => x.CompareTo(minimumValue) < 0, errorMessage);
        }

        public static ModelValidatorCollection<T> LessThanOrEqualTo<T, TProperty>(this ModelValidatorCollection<T> collection, Expression<Func<T, TProperty>> expression, TProperty minimumValue, String errorMessage)
            where TProperty : IComparable<TProperty>
        {
            return collection.Custom(expression, x => x.CompareTo(minimumValue) <= 0, errorMessage);
        }

        public static ModelValidatorCollection<T> Require<T, TProperty>(this ModelValidatorCollection<T> collection, Expression<Func<T, TProperty>> expression)
        {
            var propertyType = typeof(TProperty);
            if (propertyType == typeof(String))
            {
                return collection.Custom(expression, x => !String.IsNullOrEmpty(x as String), "Field is required");
            }
            else if (propertyType.IsValueType)
            {
                if (propertyType.IsConstructedGenericType)
                {
                    var genericType = propertyType.GetGenericTypeDefinition();
                    if (genericType == typeof(Nullable<>))
                    {
                        return collection.NotNull(expression, "Field is required");
                    }
                }
            }
            else
            {
                return collection.NotNull(expression, "Field is required");
            }

            return collection;
        }
    }
}