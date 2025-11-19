using System;
using DentalDrill.CRM.Domain.Abstractions;
using DentalDrill.CRM.Models;
using Microsoft.Extensions.DependencyInjection;

namespace DentalDrill.CRM.Domain
{
    public class ClientFactory : IClientFactory
    {
        private readonly IServiceProvider serviceProvider;

        public ClientFactory(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public IClient Create(Client entity)
        {
            return new ClientDomainModel(entity, this.serviceProvider.GetRequiredService<IClientInternalsFactory>());
        }
    }
}
