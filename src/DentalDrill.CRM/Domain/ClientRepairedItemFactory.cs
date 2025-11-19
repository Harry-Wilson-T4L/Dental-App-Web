using System;
using System.Collections.Generic;
using DentalDrill.CRM.Domain.Abstractions;
using DentalDrill.CRM.Models;
using DentalDrill.CRM.Services;
using DevGuild.AspNetCore.Services.Data;
using Microsoft.Extensions.DependencyInjection;

namespace DentalDrill.CRM.Domain
{
    public class ClientRepairedItemFactory : IClientRepairedItemFactory
    {
        private readonly IServiceProvider serviceProvider;

        public ClientRepairedItemFactory(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public IClientRepairedItem Create(IClient client, ClientHandpiece dataEntity, IReadOnlyList<IHandpiece> handpieces)
        {
            return new ClientRepairedItem(
                client,
                dataEntity,
                handpieces,
                this.serviceProvider.GetRequiredService<IDateTimeService>(),
                this.serviceProvider.GetRequiredService<IRepository>());
        }
    }
}
