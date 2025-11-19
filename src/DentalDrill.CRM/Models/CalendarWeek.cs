using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DentalDrill.CRM.Models
{
    public class CalendarWeek
    {
        public Guid Id { get; set; }

        public Guid YearId { get; set; }

        public CalendarYear Year { get; set; }

        public Int32 Week { get; set; }

        public DateTimeOffset RangeStart { get; set; }

        public DateTimeOffset RangeEnd { get; set; }
    }
}
