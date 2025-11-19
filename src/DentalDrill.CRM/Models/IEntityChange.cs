using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DentalDrill.CRM.Models
{
    public interface IEntityChange<TEntity, TEntityKey>
    {
        Guid Id { get; set; }

        TEntityKey EntityId { get; set; }

        TEntity Entity { get; set; }

        DateTime ChangedOn { get; set; }

        Guid ChangedById { get; set; }

        Employee ChangedBy { get; set; }

        ChangeAction Action { get; set; }
    }
}
