using System.Threading.Tasks;

namespace DentalDrill.CRM.Models.Validation
{
    public interface IModelValidator<T>
    {
        Task<ModelValidationResult> ValidateAsync(T model);
    }
}