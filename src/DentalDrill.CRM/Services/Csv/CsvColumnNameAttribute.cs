using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DentalDrill.CRM.Services.Csv
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class CsvColumnNameAttribute : Attribute
    {
        public CsvColumnNameAttribute(String name)
        {
            this.Name = name;
        }

        public String Name { get; }
    }
}
