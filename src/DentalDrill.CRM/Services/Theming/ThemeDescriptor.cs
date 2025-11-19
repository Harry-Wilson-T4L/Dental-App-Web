using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DentalDrill.CRM.Services.Theming
{
    public class ThemeDescriptor
    {
        public ThemeDescriptor(String key, String name)
        {
            this.Key = key;
            this.Name = name;
        }

        public String Key { get; }

        public String Name { get; }
    }
}
