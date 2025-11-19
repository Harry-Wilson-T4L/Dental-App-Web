using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DentalDrill.CRM.Domain.Abstractions;
using DentalDrill.CRM.Models;
using DentalDrill.CRM.Services;
using DevGuild.AspNetCore.Services.Data;
using Microsoft.EntityFrameworkCore;

namespace DentalDrill.CRM.Domain
{
    public class ClientManager : IClientManager
    {
        private readonly IRepository repository;
        private readonly IClientFactory factory;

        private readonly Lazy<ClientRepairedHistoryManager> repairHistory;

        public ClientManager(IRepository repository, IClientFactory factory, IClientRepairedHistoryReminderFactory reminderFactory, IDateTimeService dateTimeService)
        {
            this.repository = repository;
            this.factory = factory;

            this.repairHistory = new Lazy<ClientRepairedHistoryManager>(() => new ClientRepairedHistoryManager(this, this.repository, reminderFactory, dateTimeService));
        }

        public IClientRepairedHistoryManager RepairHistory => this.repairHistory.Value;

        public async Task<IReadOnlyList<IClient>> ListFilteredAsync(Expression<Func<Client, Boolean>> filter)
        {
            var entities = await this.repository.Query<Client>().Where(filter).ToListAsync();
            var result = new List<IClient>();
            foreach (var entity in entities)
            {
                result.Add(this.factory.Create(entity));
            }

            return result;
        }

        public async Task<IReadOnlyDictionary<Guid, IClient>> LoadResolverAsync()
        {
            var entities = await this.repository.Query<Client>().ToListAsync();
            return entities.Select(x => this.factory.Create(x)).ToDictionary(x => x.Id);
        }

        public async Task<IClient> GetByIdAsync(Guid id)
        {
            var entity = await this.repository.Query<Client>().SingleOrDefaultAsync(x => x.Id == id);
            if (entity == null)
            {
                return null;
            }

            return this.factory.Create(entity);
        }

        public async Task<IClient> GetByUrlIdAsync(String urlId)
        {
            var entity = await this.repository.Query<Client>().SingleOrDefaultAsync(x => x.UrlPath == urlId);
            if (entity == null)
            {
                return null;
            }

            return this.factory.Create(entity);
        }
    }
}
