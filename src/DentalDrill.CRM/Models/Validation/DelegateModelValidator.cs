using System.Threading.Tasks;

namespace DentalDrill.CRM.Models.Validation
{
    public class DelegateModelValidator<T> : IModelValidator<T>
    {
        private readonly ValidatorDelegate validatorDelegate;

        public DelegateModelValidator(ValidatorDelegate validatorDelegate)
        {
            this.validatorDelegate = validatorDelegate;
        }

        public delegate Task<ModelValidationResult> ValidatorDelegate(T model);

        public Task<ModelValidationResult> ValidateAsync(T model)
        {
            return this.validatorDelegate(model);
        }
    }
}