using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DentalDrill.CRM.Services.Csv
{
    public class CsvHeader
    {
        private readonly IReadOnlyList<String> columnNames;
        private readonly IReadOnlyDictionary<String, Int32> columnOrdinals;

        public CsvHeader(IEnumerable<String> columns)
        {
            this.columnNames = columns.ToList();
            this.columnOrdinals = this.columnNames.Select((x, i) => new { Index = i, Name = x }).ToDictionary(x => x.Name, x => x.Index);
        }

        public String GetName(Int32 index)
        {
            return this.columnNames[index];
        }

        public Int32 GetOrdinal(String name)
        {
            return this.columnOrdinals[name];
        }
    }
}
