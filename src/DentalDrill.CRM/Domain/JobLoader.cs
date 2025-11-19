using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DentalDrill.CRM.Domain.Abstractions;
using DentalDrill.CRM.Models;
using JetBrains.Annotations;

namespace DentalDrill.CRM.Domain
{
    public class JobLoader : DomainLoaderBase, IJobLoader
    {
        private readonly IJobFactory factory;
        private readonly IWorkshopManager workshopManager;
        private readonly IWorkshopFactory workshopFactory;
        private readonly IClientManager clientManager;
        private readonly IClientFactory clientFactory;
        private readonly IJobTypeManager jobTypeManager;
        private readonly IJobTypeFactory jobTypeFactory;

        public JobLoader(
            IJobFactory factory,
            IWorkshopManager workshopManager,
            IWorkshopFactory workshopFactory,
            IClientManager clientManager,
            IClientFactory clientFactory,
            IJobTypeManager jobTypeManager,
            IJobTypeFactory jobTypeFactory)
        {
            this.factory = factory;
            this.workshopManager = workshopManager;
            this.workshopFactory = workshopFactory;
            this.clientManager = clientManager;
            this.clientFactory = clientFactory;
            this.jobTypeManager = jobTypeManager;
            this.jobTypeFactory = jobTypeFactory;
        }

        public async Task<IJob> LoadOneAsync(
            Job entity,
            IReadOnlyDictionary<Guid, IWorkshop> workshopsResolver = null,
            IReadOnlyDictionary<Guid, IClient> clientsResolver = null,
            IReadOnlyDictionary<String, IJobType> jobTypesResolver = null)
        {
            var workshop = await DomainLoaderBase.ResolveRequiredDependency(
                entity.WorkshopId,
                entity.Workshop,
                workshopsResolver,
                key => this.workshopManager.GetByIdAsync(key),
                e => this.workshopFactory.Create(e));

            var client = await DomainLoaderBase.ResolveRequiredDependency(
                entity.ClientId,
                entity.Client,
                clientsResolver,
                key => this.clientManager.GetByIdAsync(key),
                e => this.clientFactory.Create(e));

            var jobType = await DomainLoaderBase.ResolveRequiredDependency(
                entity.JobTypeId,
                entity.JobType,
                jobTypesResolver,
                key => this.jobTypeManager.GetByIdAsync(key),
                e => this.jobTypeFactory.Create(e));

            return this.factory.Create(entity, workshop, client, jobType);
        }

        public async Task<IReadOnlyList<IJob>> LoadListAsync(
            IReadOnlyList<Job> entities,
            IReadOnlyDictionary<Guid, IWorkshop> workshopsResolver = null,
            IReadOnlyDictionary<Guid, IClient> clientsResolver = null,
            IReadOnlyDictionary<String, IJobType> jobTypesResolver = null)
        {
            workshopsResolver = await DomainLoaderBase.PrepareResolver(
                entities.Select(x => new KeyValuePair<Guid, Workshop>(x.WorkshopId, x.Workshop)).ToList(),
                workshopsResolver,
                () => this.workshopManager.LoadResolverAsync(),
                e => this.workshopFactory.Create(e));

            clientsResolver = await DomainLoaderBase.PrepareResolver(
                entities.Select(x => new KeyValuePair<Guid, Client>(x.ClientId, x.Client)).ToList(),
                clientsResolver,
                () => this.clientManager.LoadResolverAsync(),
                e => this.clientFactory.Create(e));

            jobTypesResolver = await DomainLoaderBase.PrepareResolver(
                entities.Select(x => new KeyValuePair<String, JobType>(x.JobTypeId, x.JobType)).ToList(),
                jobTypesResolver,
                () => this.jobTypeManager.LoadResolverAsync(),
                e => this.jobTypeFactory.Create(e));

            return entities.Select(x => this.factory.Create(x, workshopsResolver[x.WorkshopId], clientsResolver[x.ClientId], jobTypesResolver[x.JobTypeId])).ToList();
        }
    }
}
