using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using DentalDrill.SmartFreight.Client.Models;

namespace DentalDrill.SmartFreight.Client
{
    public class SmartFreightClient : ISmartFreightClient
    {
        private readonly SmartFreightClientOptions options;
        private readonly SFOv1 client;

        public SmartFreightClient(SmartFreightClientOptions options)
        {
            this.options = options;
            this.client = this.CreateClient();
        }

        public async Task<Consignment> CalculateRateAsync(String reference, Consignment consignment)
        {
            var response = await this.client.CalculateRateAsync(this.options.Id, this.options.Password, reference, consignment.SerializeToString());
            return Consignment.DeserializeFromString(response);
        }

        public async Task<(Consignment Result, String Request, String Response)> CalculateRateExAsync(String reference, Consignment consignment)
        {
            var request = consignment.SerializeToString();
            var response = await this.client.CalculateRateAsync(this.options.Id, this.options.Password, reference, request);
            return (Consignment.DeserializeFromString(response), request, response);
        }

        public async Task<Consignment> ImportAsync(String reference, Consignment consignment)
        {
            var response = await this.client.ImportAsync(this.options.Id, this.options.Password, reference, consignment.SerializeToString());
            return Consignment.DeserializeFromString(response);
        }

        public async Task<(Consignment Result, String Request, String Response)> ImportExAsync(String reference, Consignment consignment)
        {
            var request = consignment.SerializeToString();
            var response = await this.client.ImportAsync(this.options.Id, this.options.Password, reference, request);
            return (Consignment.DeserializeFromString(response), request, response);
        }

        private SFOv1 CreateClient()
        {
            if (this.options.ServiceUri.StartsWith("https://"))
            {
                return new SFOv1Client(new BasicHttpsBinding(), new EndpointAddress(this.options.ServiceUri));
            }
            else if (this.options.ServiceUri.StartsWith("http://"))
            {
                return new SFOv1Client(new BasicHttpBinding(), new EndpointAddress(this.options.ServiceUri));
            }
            else
            {
                throw new InvalidOperationException("Service url format is not supported");
            }
        }
    }
}
