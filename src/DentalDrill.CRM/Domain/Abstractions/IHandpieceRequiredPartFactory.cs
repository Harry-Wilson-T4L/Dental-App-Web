using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DentalDrill.CRM.Models;

namespace DentalDrill.CRM.Domain.Abstractions
{
    public interface IHandpieceRequiredPartFactory
    {
        IHandpieceRequiredPart Create(HandpieceRequiredPart part, IHandpiece handpiece, IInventorySKU sku);
    }
}
