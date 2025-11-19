using System;
using System.Threading.Tasks;
using DentalDrill.CRM.Domain.Abstractions;
using DentalDrill.CRM.Models;
using Microsoft.Extensions.DependencyInjection;

namespace DentalDrill.CRM.Domain
{
    public class ClientDomainModel : IClient
    {
        private readonly Guid id;

        private readonly Lazy<IClientFeedbackCollection> feedback;
        private readonly Lazy<IClientReports> reports;
        private readonly Lazy<IClientRepairedHistory> repairedHistory;

        private Client client;

        public ClientDomainModel(Client client, IClientInternalsFactory internalsFactory)
        {
            this.id = client.Id;
            this.feedback = new Lazy<IClientFeedbackCollection>(() => internalsFactory.CreateFeedbackCollection(this));
            this.reports = new Lazy<IClientReports>(() => internalsFactory.CreateReports(this));
            this.repairedHistory = new Lazy<IClientRepairedHistory>(() => internalsFactory.CreateRepairedHistory(this));

            this.client = client;
        }

        public Guid Id => this.id;

        public Client Entity => this.client;

        public IClientFeedbackCollection Feedback => this.feedback.Value;

        public IClientReports Reports => this.reports.Value;

        public IClientRepairedHistory RepairedHistory => this.repairedHistory.Value;

        public static Task<IClient> GetByIdAsync(IServiceProvider serviceProvider, Guid id)
        {
            return serviceProvider.GetRequiredService<IClientManager>().GetByIdAsync(id);
        }

        public static Task<IClient> GetByUrlIdAsync(IServiceProvider serviceProvider, String urlId)
        {
            return serviceProvider.GetRequiredService<IClientManager>().GetByUrlIdAsync(urlId);
        }
    }
}
