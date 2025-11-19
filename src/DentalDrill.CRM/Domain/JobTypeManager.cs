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
    public class JobTypeManager : IJobTypeManager
    {
        private readonly IRepository repository;
        private readonly IJobTypeFactory factory;

        public JobTypeManager(
            IRepository repository,
            IJobTypeFactory factory)
        {
            this.repository = repository;
            this.factory = factory;
        }

        public async Task<IReadOnlyList<IJobType>> ListAsync()
        {
            var entities = await this.repository.Query<JobType>().OrderBy(x => x.Name).ToListAsync();
            return entities.Select(x => this.factory.Create(x)).ToList();
        }

        public async Task<IReadOnlyDictionary<String, IJobType>> LoadResolverAsync()
        {
            var entities = await this.repository.Query<JobType>().OrderBy(x => x.Name).ToListAsync();
            return entities.Select(x => this.factory.Create(x)).ToDictionary(x => x.Id, StringComparer.InvariantCultureIgnoreCase);
        }

        public async Task<IJobType> GetByIdAsync(String id)
        {
            var entity = await this.repository.Query<JobType>().SingleOrDefaultAsync(x => x.Id == id);
            if (entity == null)
            {
                return null;
            }

            return this.factory.Create(entity);
        }
    }
}
