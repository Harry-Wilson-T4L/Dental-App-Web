using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace DentalDrill.CRM.Services.Csv
{
    public class CsvEntry
    {
        private readonly CsvHeader header;
        private readonly IReadOnlyList<String> values;

        public CsvEntry(CsvHeader header, IEnumerable<String> values)
        {
            this.header = header;
            this.values = values.ToList();
        }

        public String this[Int32 index] => this.values[index];

        public String this[String name] => this.values[this.header.GetOrdinal(name)];

        public T ParseAs<T>()
        {
            var type = typeof(T);
            var properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(x => x.CanRead && x.CanWrite && x.PropertyType == typeof(String))
                .ToList();

            var constructor = type.GetConstructor(new Type[] { });
            var instance = (T)constructor.Invoke(new Object[] { });
            foreach (var property in properties)
            {
                var columnName = property.Name;
                if (property.GetCustomAttribute<CsvColumnIgnoreAttribute>() != null)
                {
                    continue;
                }

                var columnAttribute = property.GetCustomAttribute<CsvColumnNameAttribute>();
                if (columnAttribute != null)
                {
                    columnName = columnAttribute.Name;
                }

                property.SetValue(instance, this[columnName]);
            }

            return instance;
        }
    }
}
