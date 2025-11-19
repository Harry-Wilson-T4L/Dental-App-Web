using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace DentalDrill.SmartFreight.Client.Models
{
    public class Sender
    {
        public String Name { get; set; }

        public String EmailAddress { get; set; }

        public String PhoneNumber { get; set; }

        public AddressDetails Address { get; set; }

        internal void AppendToXml(XElement parent)
        {
            if (!String.IsNullOrEmpty(this.Name))
            {
                parent.Add(new XElement("sendname", this.Name));
            }

            if (!String.IsNullOrEmpty(this.EmailAddress))
            {
                parent.Add(new XElement("sendemail", this.EmailAddress));
            }

            if (!String.IsNullOrEmpty(this.PhoneNumber))
            {
                parent.Add(new XElement("sendph", this.PhoneNumber));
            }

            if (this.Address != null)
            {
                var addressNode = new XElement("sendaddr");
                this.Address.AppendToXml(addressNode);
                parent.Add(addressNode);
            }
        }

        internal static Sender ReadFromXml(XElement node)
        {
            if (node == null)
            {
                return null;
            }

            var sender = new Sender
            {
                Name = node.Element("sendname")?.Value,
                PhoneNumber = node.Element("sendph")?.Value,
                EmailAddress = node.Element("sendemail")?.Value,
                Address = AddressDetails.ReadFromXml(node.Element("sendaddr")),
            };

            if (sender.Name == null && sender.PhoneNumber == null && sender.EmailAddress == null && sender.Address == null)
            {
                return null;
            }

            return sender;
        }
    }
}
