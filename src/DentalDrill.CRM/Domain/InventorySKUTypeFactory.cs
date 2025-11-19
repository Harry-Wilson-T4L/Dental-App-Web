using DentalDrill.CRM.Domain.Abstractions;
using DentalDrill.CRM.Models;
using DevGuild.AspNetCore.Services.Data;

namespace DentalDrill.CRM.Domain
{
    public class InventorySKUTypeFactory : IInventorySKUTypeFactory
    {
        private readonly IRepository repository;

        public InventorySKUTypeFactory(IRepository repository)
        {
            this.repository = repository;
        }

        public IInventorySKUType Create(InventorySKUType entity)
        {
            return new InventorySKUTypeDomainModel(entity, this.repository);
        }
    }
}
