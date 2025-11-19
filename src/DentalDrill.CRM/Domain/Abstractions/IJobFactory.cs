using DentalDrill.CRM.Models;

namespace DentalDrill.CRM.Domain.Abstractions
{
    public interface IJobFactory
    {
        IJob Create(Job entity, IWorkshop workshop, IClient client, IJobType jobType);
    }
}
