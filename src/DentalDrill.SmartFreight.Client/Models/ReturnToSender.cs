using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace DentalDrill.SmartFreight.Client.Models
{
    public class ReturnToSender
    {
        public String Name { get; set; }

        public AddressDetails Address { get; set; }

        internal void AppendToXml(XElement parent)
        {
            if (!String.IsNullOrEmpty(this.Name))
            {
                parent.Add(new XElement("rtsname", this.Name));
            }

            if (this.Address != null)
            {
                var addressNode = new XElement("rtsaddr");
                this.Address.AppendToXml(addressNode);
                parent.Add(addressNode);
            }
        }

        internal static ReturnToSender ReadFromXml(XElement node)
        {
            if (node == null)
            {
                return null;
            }

            var rts = new ReturnToSender
            {
                Name = node.Element("rtsname")?.Value,
                Address = AddressDetails.ReadFromXml(node.Element("rtsaddr")),
            };

            if (rts.Name == null && rts.Address == null)
            {
                return null;
            }

            return rts;
        }
    }
}
