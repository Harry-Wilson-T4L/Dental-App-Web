using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DentalDrill.CRM.Services
{
    public interface ISuspendService
    {
        Boolean Test(String key);

        void SuspendFor(String key, Int32 iterations);
    }
}
