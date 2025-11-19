using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DentalDrill.CRM.Domain.Abstractions;
using DentalDrill.CRM.Models;
using DevGuild.AspNetCore.Services.Data;
using Microsoft.EntityFrameworkCore;

namespace DentalDrill.CRM.Domain
{
    public class JobManager : IJobManager
    {
        private readonly IRepository repository;
        private readonly IJobLoader loader;

        public JobManager(IRepository repository, IJobLoader loader)
        {
            this.repository = repository;
            this.loader = loader;
        }

        public async Task<IReadOnlyList<IJob>> ListAsync()
        {
            var jobs = await this.repository.Query<Job>()
                .Include(x => x.Workshop)
                .Include(x => x.Client)
                .Include(x => x.JobType)
                .ToListAsync();

            return await this.loader.LoadListAsync(jobs);
        }

        public async Task<IReadOnlyDictionary<Guid, IJob>> LoadResolverAsync()
        {
            var list = await this.ListAsync();
            return list.ToDictionary(x => x.Id);
        }

        public async Task<IJob> GetByIdAsync(Guid id)
        {
            var entity = await this.repository.Query<Job>()
                .Include(x => x.Workshop)
                .Include(x => x.Client)
                .Include(x => x.JobType)
                .SingleOrDefaultAsync(x => x.Id == id);
            if (entity == null)
            {
                return null;
            }

            return await this.loader.LoadOneAsync(entity);
        }
    }
}
