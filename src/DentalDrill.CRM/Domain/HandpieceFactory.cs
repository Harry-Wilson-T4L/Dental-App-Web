using System;
using DentalDrill.CRM.Domain.Abstractions;
using DentalDrill.CRM.Models;
using DentalDrill.CRM.Services;
using DevGuild.AspNetCore.Services.Data;
using Microsoft.Extensions.DependencyInjection;

namespace DentalDrill.CRM.Domain
{
    public class HandpieceFactory : IHandpieceFactory
    {
        private readonly IServiceProvider serviceProvider;

        public HandpieceFactory(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public IHandpiece Create(Handpiece entity, IJob job, IClientHandpiece clientHandpiece)
        {
            return new HandpieceDomainModel(
                entity,
                job,
                clientHandpiece,
                this.serviceProvider.GetRequiredService<IRepository>(),
                this.serviceProvider.GetRequiredService<IHandpieceRequiredPartManager>(),
                this.serviceProvider.GetRequiredService<IChangeTrackingService<Handpiece, HandpieceChange>>());
        }
    }
}
