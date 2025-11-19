using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DentalDrill.CRM.Models
{
    public interface ISoftDelete
    {
        DeletionStatus DeletionStatus { get; set; }

        DateTime? DeletionDate { get; set; }
    }
}
