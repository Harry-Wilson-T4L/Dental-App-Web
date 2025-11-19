using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DentalDrill.CRM.Services
{
    public class SuspendService : ISuspendService
    {
        private readonly ConcurrentDictionary<String, Counter> counters = new ConcurrentDictionary<String, Counter>();

        public Boolean Test(String key)
        {
            var counter = this.GetCounter(key);
            return counter.Test();
        }

        public void SuspendFor(String key, Int32 iterations)
        {
            var counter = this.GetCounter(key);
            counter.Update(iterations);
        }

        private Counter GetCounter(String key)
        {
            return this.counters.GetOrAdd(key, x => new Counter());
        }

        private class Counter
        {
            private readonly Object lockObject = new Object();
            private Int32 value = 0;

            public Boolean Test()
            {
                lock (this.lockObject)
                {
                    if (this.value == 0)
                    {
                        return true;
                    }

                    this.value--;
                    return false;
                }
            }

            public void Update(Int32 value)
            {
                lock (this.lockObject)
                {
                    this.value = value;
                }
            }
        }
    }
}
