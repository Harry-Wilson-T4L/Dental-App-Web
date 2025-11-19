using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DentalDrill.CRM.Models;
using DevGuild.AspNetCore.Services.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DentalDrill.CRM.Domain
{
    public class CorporateDomainModel
    {
        private readonly IServiceProvider serviceProvider;
        private readonly Guid id;

        private Corporate corporate;

        private CorporateReportsDomainModel reports;

        private CorporateDomainModel(IServiceProvider serviceProvider, Corporate corporate)
        {
            this.serviceProvider = serviceProvider;
            this.id = corporate.Id;

            this.corporate = corporate;
        }

        public Guid Id => this.id;

        public Corporate Entity => this.corporate;

        public CorporateReportsDomainModel Reports => this.reports ?? (this.reports = new CorporateReportsDomainModel(this.serviceProvider, this));

        public static async Task<CorporateDomainModel> GetByIdAsync(IServiceProvider serviceProvider, Guid id)
        {
            var repository = serviceProvider.GetService<IRepository>();
            var corporate = await repository.Query<Corporate>().SingleOrDefaultAsync(x => x.Id == id);
            if (corporate == null)
            {
                return null;
            }

            return new CorporateDomainModel(serviceProvider, corporate);
        }

        public static async Task<CorporateDomainModel> GetByUrlIdAsync(IServiceProvider serviceProvider, String urlId)
        {
            var repository = serviceProvider.GetService<IRepository>();
            var corporate = await repository.Query<Corporate>().SingleOrDefaultAsync(x => x.UrlPath == urlId);
            if (corporate == null)
            {
                return null;
            }

            return new CorporateDomainModel(serviceProvider, corporate);
        }
    }
}
