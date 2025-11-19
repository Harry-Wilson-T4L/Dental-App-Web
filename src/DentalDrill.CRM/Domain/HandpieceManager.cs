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
    public class HandpieceManager : IHandpieceManager
    {
        private readonly IRepository repository;
        private readonly IHandpieceLoader loader;

        public HandpieceManager(IRepository repository, IHandpieceLoader loader)
        {
            this.repository = repository;
            this.loader = loader;
        }

        public async Task<IHandpiece> GetByIdAsync(Guid id)
        {
            var handpiece = await this.repository.QueryWithoutTracking<Handpiece>()
                .Include(x => x.Job)
                .Include(x => x.Job.Client)
                .Include(x => x.Job.JobType)
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
                .SingleOrDefaultAsync(predicate);
            if (handpiece == null)
            {
                return null;
            }

            return await this.loader.LoadOneAsync(handpiece);
        }

        public async Task<IReadOnlyList<IHandpiece>> FindManyAsync(Expression<Func<Handpiece, Boolean>> predicate)
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
