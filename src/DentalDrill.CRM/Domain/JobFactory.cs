using System;
using DentalDrill.CRM.Domain.Abstractions;
using DentalDrill.CRM.Models;
using DevGuild.AspNetCore.Services.Data;
using Microsoft.Extensions.DependencyInjection;

namespace DentalDrill.CRM.Domain
{
    public class JobFactory : IJobFactory
    {
        private readonly IServiceProvider serviceProvider;

        public JobFactory(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public IJob Create(Job entity, IWorkshop workshop, IClient client, IJobType jobType)
        {
            return new JobDomainModel(
                entity,
                workshop,
                client,
                jobType,
                this.serviceProvider.GetRequiredService<IRepository>(),
                this.serviceProvider.GetRequiredService<IHandpieceLoader>());
        }
    }
}
