using System;
using System.Linq;
using System.Threading.Tasks;
using DentalDrill.CRM.Models;
using DevGuild.AspNetCore.Services.Data;
using Kendo.Mvc.Extensions;
using Microsoft.EntityFrameworkCore;

namespace DentalDrill.CRM.Services.GenericFlags
{
    public class GenericFlagsService
    {
        private readonly IRepository repository;

        public GenericFlagsService(IRepository repository)
        {
            this.repository = repository;
        }

        public async Task<GenericFlagState> GetFlagStateAsync(String flagId)
        {
            var flag = await this.repository.Query<GenericFlag>().SingleOrDefaultAsync(x => x.Id == flagId);
            return flag?.State ?? GenericFlagState.NotSet;
        }

        public async Task SetFlagStateAsync(String flagId, GenericFlagState state)
        {
            var flag = await this.repository.Query<GenericFlag>().SingleOrDefaultAsync(x => x.Id == flagId);
            if (flag != null)
            {
                flag.State = state;
                await this.repository.UpdateAsync(flag);
            }
            else
            {
                flag = new GenericFlag { Id = flagId, State = state };
                await this.repository.InsertAsync(flag);
            }
        }

        public async Task ClearFlagsByPrefixAsync(String prefix)
        {
            var matchingFlags = await this.repository.Query<GenericFlag>()
                .Where(x => x.Id.StartsWith(prefix))
                .ToListAsync();

            foreach (var flag in matchingFlags)
            {
                await this.repository.DeleteAsync(flag);
            }

            await this.repository.SaveChangesAsync();
        }
    }
}
