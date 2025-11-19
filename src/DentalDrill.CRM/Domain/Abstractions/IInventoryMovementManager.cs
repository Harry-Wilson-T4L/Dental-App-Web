using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DentalDrill.CRM.Domain.Abstractions
{
    public interface IInventoryMovementManager
    {
        Task<IInventoryMovement> GetById(Guid id);

        Task<TMovement> GetById<TMovement>(Guid id)
            where TMovement : class, IInventoryMovement;
    }
}
