using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DentalDrill.CRM.Models
{
    public class EmailTemplate
    {
        public Guid Id { get; set; }

        public String Content { get; set; }

        public T DeserializeContent<T>()
        {
            return JsonConvert.DeserializeObject<T>(this.Content);
        }

        public void SerializeContent<T>(T value)
        {
            this.Content = JsonConvert.SerializeObject(value);
        }
    }
}
