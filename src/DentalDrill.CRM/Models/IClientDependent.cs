using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DentalDrill.CRM.Models
{
    public interface IClientDependent
    {
        Guid ClientId { get; set; }

        Client Client { get; set; }
    }
}
