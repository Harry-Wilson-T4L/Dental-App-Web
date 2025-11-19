using DentalDrill.CRM.Models;

namespace DentalDrill.CRM.Domain.Abstractions
{
    public interface IClientHandpieceFactory
    {
        IClientHandpiece Create(ClientHandpiece dataEntity, IClient client);
    }
}
