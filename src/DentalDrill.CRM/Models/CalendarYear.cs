using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DentalDrill.CRM.Models
{
    public class CalendarYear
    {
        public Guid Id { get; set; }

        public Int32 Year { get; set; }

        public DateTimeOffset RangeStart { get; set; }

        public DateTimeOffset RangeEnd { get; set; }
    }
}
