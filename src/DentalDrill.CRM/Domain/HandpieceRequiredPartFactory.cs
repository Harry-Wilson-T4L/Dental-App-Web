using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DentalDrill.CRM.Domain.Abstractions;
using DentalDrill.CRM.Models;
using DevGuild.AspNetCore.Services.Data;

namespace DentalDrill.CRM.Domain
{
    public class HandpieceRequiredPartFactory : IHandpieceRequiredPartFactory
    {
        private readonly IRepository repository;

        public HandpieceRequiredPartFactory(IRepository repository)
        {
            this.repository = repository;
        }

        public IHandpieceRequiredPart Create(HandpieceRequiredPart part, IHandpiece handpiece, IInventorySKU sku)
        {
            return new HandpieceRequiredPartDomainModel(part, handpiece, sku, this.repository);
        }
    }
}
