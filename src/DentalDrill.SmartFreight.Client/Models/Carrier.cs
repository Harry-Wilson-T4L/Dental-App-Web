using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace DentalDrill.SmartFreight.Client.Models
{
    public class Carrier
    {
        public Boolean? LeastCost { get; set; }

        public String Name { get; set; }

        public String AccountNumber { get; set; }

        public String Service { get; set; }

        public static Carrier Automatic()
        {
            return new Carrier
            {
                Name = "[Automatic]"
            };
        }

        internal void AppendToXml(XElement parent)
        {
            if (this.LeastCost.HasValue)
            {
                parent.Add(new XElement("applyleastcost", this.LeastCost.Value ? "Yes" : "No"));
            }

            if (!String.IsNullOrEmpty(this.Name))
            {
                parent.Add(new XElement("carriername", this.Name));
            }

            if (!String.IsNullOrEmpty(this.AccountNumber))
            {
                parent.Add(new XElement("accno", this.AccountNumber));
            }

            if (!String.IsNullOrEmpty(this.Service))
            {
                parent.Add(new XElement("service", this.Service));
            }
        }

        internal static Carrier ReadFromXml(XElement node)
        {
            if (node == null)
            {
                return null;
            }

            var carrier = new Carrier
            {
                Name = node.Element("carriername")?.Value,
                AccountNumber = node.Element("accno")?.Value,
                Service = node.Element("service")?.Value,
            };

            if (carrier.Name == null && carrier.AccountNumber == null && carrier.Service == null)
            {
                return null;
            }

            return carrier;
        }
    }
}
