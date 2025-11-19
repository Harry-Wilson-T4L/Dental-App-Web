using System;
using DentalDrill.CRM.Domain.Abstractions;
using DevGuild.AspNetCore.Services.Data;
using Microsoft.Extensions.DependencyInjection;

namespace DentalDrill.CRM.Domain
{
    public class ClientInternalsFactory : IClientInternalsFactory
    {
        private readonly IServiceProvider serviceProvider;

        public ClientInternalsFactory(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public IClientFeedbackCollection CreateFeedbackCollection(IClient client)
        {
            return new ClientFeedbackCollectionDomainModel(this.serviceProvider, client);
        }

        public IClientReports CreateReports(IClient client)
        {
            return new ClientReportsDomainModel(this.serviceProvider, client);
        }

        public IClientRepairedHistory CreateRepairedHistory(IClient client)
        {
            return new ClientRepairedHistory(
                client,
                this.serviceProvider.GetRequiredService<IRepository>(),
                this.serviceProvider.GetRequiredService<IClientRepairedItemFactory>(),
                this.serviceProvider.GetRequiredService<IHandpieceLoader>());
        }
    }
}
