using DentalDrill.CRM.Models;

namespace DentalDrill.CRM.Domain.Abstractions
{
    public interface IJobTypeFactory
    {
        IJobType Create(JobType entity);
    }
}
