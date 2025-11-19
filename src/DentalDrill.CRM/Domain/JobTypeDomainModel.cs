using System;
using DentalDrill.CRM.Domain.Abstractions;
using DentalDrill.CRM.Models;

namespace DentalDrill.CRM.Domain
{
    public class JobTypeDomainModel : IJobType
    {
        private readonly JobType jobType;

        public JobTypeDomainModel(JobType jobType)
        {
            this.jobType = jobType;
        }

        public String Id => this.jobType.Id;

        public String Name => this.jobType.Name;

        public String ShortName => this.jobType.ShortName;
    }
}
