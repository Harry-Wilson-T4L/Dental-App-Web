using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DentalDrill.CRM.Services
{
    public class DateTimeService : IDateTimeService
    {
        public DateTimeOffset CurrentTime => DateTimeOffset.Now;

        public DateTime CurrentUtcTime => DateTime.UtcNow;
    }
}
