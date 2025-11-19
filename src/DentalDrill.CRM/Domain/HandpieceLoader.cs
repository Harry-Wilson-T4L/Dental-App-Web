using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DentalDrill.CRM.Domain.Abstractions;
using DentalDrill.CRM.Models;

namespace DentalDrill.CRM.Domain
{
    public class HandpieceLoader : DomainLoaderBase, IHandpieceLoader
    {
        private readonly IHandpieceFactory factory;
        private readonly IJobManager jobManager;
        private readonly IJobLoader jobLoader;
        private readonly IClientHandpieceManager clientHandpieceManager;
        private readonly IClientHandpieceLoader clientHandpieceLoader;

        public HandpieceLoader(
            IHandpieceFactory factory,
            IJobManager jobManager,
            IJobLoader jobLoader,
            IClientHandpieceManager clientHandpieceManager,
            IClientHandpieceLoader clientHandpieceLoader)
        {
            this.factory = factory;
            this.jobManager = jobManager;
            this.jobLoader = jobLoader;
            this.clientHandpieceManager = clientHandpieceManager;
            this.clientHandpieceLoader = clientHandpieceLoader;
        }

        public async Task<IHandpiece> LoadOneAsync(
            Handpiece entity,
            IReadOnlyDictionary<Guid, IWorkshop> workshopsResolver = null,
            IReadOnlyDictionary<Guid, IClient> clientsResolver = null,
            IReadOnlyDictionary<String, IJobType> jobTypesResolver = null,
            IReadOnlyDictionary<Guid, IJob> jobResolver = null,
            IReadOnlyDictionary<Guid, IClientHandpiece> clientHandpieceResolver = null)
        {
            var job = await DomainLoaderBase.ResolveRequiredDependencyWithLoader(
                entity.JobId,
                entity.Job,
                jobResolver,
                key => this.jobManager.GetByIdAsync(entity.JobId),
                e => this.jobLoader.LoadOneAsync(
                    e,
                    workshopsResolver,
                    clientsResolver,
                    jobTypesResolver));

            var clientHandpiece = await DomainLoaderBase.ResolveRequiredDependencyWithLoader(
                entity.ClientHandpieceId,
                entity.ClientHandpiece,
                clientHandpieceResolver,
                key => this.clientHandpieceManager.GetByIdAsync(entity.ClientHandpieceId),
                e => this.clientHandpieceLoader.LoadOneAsync(
                    e,
                    clientsResolver));

            return this.factory.Create(entity, job, clientHandpiece);
        }

        public async Task<IReadOnlyList<IHandpiece>> LoadListAsync(
            IReadOnlyList<Handpiece> entities,
            IReadOnlyDictionary<Guid, IWorkshop> workshopsResolver = null,
            IReadOnlyDictionary<Guid, IClient> clientsResolver = null,
            IReadOnlyDictionary<String, IJobType> jobTypesResolver = null,
            IReadOnlyDictionary<Guid, IJob> jobResolver = null,
            IReadOnlyDictionary<Guid, IClientHandpiece> clientHandpieceResolver = null)
        {
            jobResolver = await DomainLoaderBase.PrepareResolverWithLoader(
                entities.Select(x => new KeyValuePair<Guid, Job>(x.JobId, x.Job)).ToList(),
                jobResolver,
                () => this.jobManager.LoadResolverAsync(),
                e => this.jobLoader.LoadListAsync(e, workshopsResolver, clientsResolver, jobTypesResolver),
                job => job.Id);

            clientHandpieceResolver = await DomainLoaderBase.PrepareResolverWithLoader(
                entities.Select(x => new KeyValuePair<Guid, ClientHandpiece>(x.ClientHandpieceId, x.ClientHandpiece)).ToList(),
                clientHandpieceResolver,
                () => this.clientHandpieceManager.LoadResolverAsync(),
                e => this.clientHandpieceLoader.LoadListAsync(e, clientsResolver),
                clientHandpiece => clientHandpiece.Id);

            return entities.Select(x => this.factory.Create(x, jobResolver[x.JobId], clientHandpieceResolver[x.ClientHandpieceId])).ToList();
        }
    }
}
