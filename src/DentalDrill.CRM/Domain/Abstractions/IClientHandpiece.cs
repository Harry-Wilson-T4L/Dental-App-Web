using System;
using System.Collections.Generic;

namespace DentalDrill.CRM.Domain.Abstractions
{
    public interface IClientHandpiece
    {
        Guid Id { get; }

        IClient Client { get; }

        String Brand { get; }

        String Model { get; }

        String Serial { get; }

        IReadOnlyList<IClientHandpieceComponent> Components { get; }

        String ComponentsText { get; }
    }
}
