using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DentalDrill.SmartFreight.Client;

namespace DentalDrill.CRM.Services
{
    public class PickupRequestSmartFreightOptions
    {
        public Boolean Enabled { get; set; }

        public Boolean Testing { get; set; }

        public String SenderEmail { get; set; }

        public String ServiceUri { get; set; }

        public String Id { get; set; }

        public String Password { get; set; }

        public SmartFreightClientOptions CreateClientOptions()
        {
            return new SmartFreightClientOptions
            {
                Id = this.Id,
                Password = this.Password,
                ServiceUri = this.ServiceUri,
            };
        }
    }
}
