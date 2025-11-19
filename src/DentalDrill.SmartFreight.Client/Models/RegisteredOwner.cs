using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace DentalDrill.SmartFreight.Client.Models
{
    public class RegisteredOwner
    {
        public String Name { get; set; }

        public AddressDetails Address { get; set; }

        internal void AppendToXml(XElement parent)
        {
            if (!String.IsNullOrEmpty(this.Name))
            {
                parent.Add(new XElement("regname", this.Name));
            }

            if (this.Address != null)
            {
                var addressNode = new XElement("regaddr");
                this.Address.AppendToXml(addressNode);
                parent.Add(addressNode);
            }
        }

        internal static RegisteredOwner ReadFromXml(XElement node)
        {
            if (node == null)
            {
                return null;
            }

            var owner = new RegisteredOwner
            {
                Name = node.Element("regname")?.Value,
                Address = AddressDetails.ReadFromXml(node.Element("regaddr")),
            };

            if (owner.Name == null && owner.Address == null)
            {
                return null;
            }

            return owner;
        }
    }
}
