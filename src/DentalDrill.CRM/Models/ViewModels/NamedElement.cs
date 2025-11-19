using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DentalDrill.CRM.Models.ViewModels
{
    public class NamedElement<TKey>
    {
        public TKey Id { get; set; }

        public String Name { get; set; }
    }
}
