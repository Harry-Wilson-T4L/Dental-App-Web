using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DentalDrill.CRM.Models.ViewModels
{
    public class CallbackReadModel
    {
        public Guid Id { get; set; }

        public DateTime? CallDateTime { get; set; }

        public Guid ClientId { get; set; }

        public String ClientName { get; set; }

        public String Note { get; set; }
    }
}
