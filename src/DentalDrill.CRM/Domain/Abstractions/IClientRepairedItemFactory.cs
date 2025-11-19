using System;
using System.Collections.Generic;
using DentalDrill.CRM.Models;

namespace DentalDrill.CRM.Domain.Abstractions
{
    public interface IClientRepairedItemFactory
    {
        IClientRepairedItem Create(IClient client, ClientHandpiece dataEntity, IReadOnlyList<IHandpiece> handpieces);
    }
}
