using System;

namespace DentalDrill.CRM.Domain.Abstractions
{
    public interface IClientHandpieceComponent
    {
        String Brand { get; }

        String Model { get; }

        String Serial { get; }
    }
}
