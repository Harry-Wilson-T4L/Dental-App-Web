using DentalDrill.CRM.Domain.Abstractions;
using DentalDrill.CRM.Models;

namespace DentalDrill.CRM.Domain
{
    public class WorkshopFactory : IWorkshopFactory
    {
        public IWorkshop Create(Workshop entity)
        {
            return new WorkshopDomainModel(entity);
        }
    }
}
