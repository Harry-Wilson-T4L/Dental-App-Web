using DentalDrill.CRM.Domain.Abstractions;
using DentalDrill.CRM.Models;

namespace DentalDrill.CRM.Domain
{
    public class JobTypeFactory : IJobTypeFactory
    {
        public IJobType Create(JobType entity)
        {
            return new JobTypeDomainModel(entity);
        }
    }
}
