using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace DentalDrill.CRM.Models.Ephemeral
{
    public class ClientHandpiece
    {
        public String Id { get; set; }

        public Guid ClientId { get; set; }

        public String Brand { get; set; }

        public String MakeAndModel { get; set; }

        public String Serial { get; set; }

        public ICollection<Handpiece> Repairs { get; set; }
    }
}
