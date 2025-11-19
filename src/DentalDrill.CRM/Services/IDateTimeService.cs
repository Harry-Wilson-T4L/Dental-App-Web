using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DentalDrill.CRM.Services
{
    public interface IDateTimeService
    {
        DateTimeOffset CurrentTime { get; }

        DateTime CurrentUtcTime { get; }
    }
}
