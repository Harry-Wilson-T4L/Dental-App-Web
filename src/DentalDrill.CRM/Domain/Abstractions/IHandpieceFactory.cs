
using DentalDrill.CRM.Models;

namespace DentalDrill.CRM.Domain.Abstractions
{
    public interface IHandpieceFactory
    {
        IHandpiece Create(Handpiece entity, IJob job, IClientHandpiece clientHandpiece);
    }
}
