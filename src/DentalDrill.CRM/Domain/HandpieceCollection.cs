using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DentalDrill.CRM.Domain.Abstractions;
using DentalDrill.CRM.Models;
using DevGuild.AspNetCore.Services.Data;
using Microsoft.EntityFrameworkCore;

namespace DentalDrill.CRM.Domain
{
    public class HandpieceCollection : IHandpieceCollection
    {
        private readonly IJob job;

        private readonly IRepository repository;
        private readonly IHandpieceLoader loader;

        public HandpieceCollection(IJob job, IRepository repository, IHandpieceLoader loader)
        {
            this.job = job;
            this.repository = repository;
            this.loader = loader;
        }

        public async Task<IHandpiece> GetByIdAsync(Guid id)
        {
            var handpiece = await this.repository.QueryWithoutTracking<Handpiece>()
                .Include(x => x.Job)
                .Include(x => x.Job.Client)
                .Include(x => x.Job.JobType)
                .Where(x => x.JobId == this.job.Id)
                .SingleOrDefaultAsync(x => x.Id == id);
            if (handpiece == null)
            {
                return null;
            }

            return await this.loader.LoadOneAsync(handpiece);
        }

        public async Task<IHandpiece> FindOneAsync(Expression<Func<Handpiece, Boolean>> predicate)
        {
            var handpiece = await this.repository.QueryWithoutTracking<Handpiece>()
                .Include(x => x.Job)
                .Include(x => x.Job.Client)
                .Include(x => x.Job.JobType)
                .Where(x => x.JobId == this.job.Id)
                .SingleOrDefaultAsync(predicate);
            if (handpiece == null)
            {
                return null;
            }

            return await this.loader.LoadOneAsync(handpiece);
        }

        public async Task<IReadOnlyList<IHandpiece>> FindManyAsync(Expression<Func<Handpiece, Boolean>> predicate = null)
        {
            var handpieces = await this.repository.QueryWithoutTracking<Handpiece>()
                .Include(x => x.Job)
                .Include(x => x.Job.Client)
                .Include(x => x.Job.JobType)
                .Where(predicate ?? (x => true))
                .ToListAsync();

            return await this.loader.LoadListAsync(handpieces);
        }
    }
}
