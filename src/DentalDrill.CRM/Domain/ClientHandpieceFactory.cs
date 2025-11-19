using DentalDrill.CRM.Domain.Abstractions;
using DentalDrill.CRM.Models;

namespace DentalDrill.CRM.Domain
{
    public class ClientHandpieceFactory : IClientHandpieceFactory
    {
        public IClientHandpiece Create(ClientHandpiece dataEntity, IClient client)
        {
            return new ClientHandpieceDomainModel(dataEntity, client);
        }
    }
}
