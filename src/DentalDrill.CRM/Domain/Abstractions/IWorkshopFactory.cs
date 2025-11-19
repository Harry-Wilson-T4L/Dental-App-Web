using DentalDrill.CRM.Models;

namespace DentalDrill.CRM.Domain.Abstractions
{
    public interface IWorkshopFactory
    {
        IWorkshop Create(Workshop entity);
    }
}
