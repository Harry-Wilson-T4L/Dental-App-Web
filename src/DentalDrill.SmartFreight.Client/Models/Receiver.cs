using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml.Linq;

namespace DentalDrill.SmartFreight.Client.Models
{
    public class Receiver
    {
        public String AccountNumber { get; set; }

        public String Name { get; set; }

        public String PhoneNumber { get; set; }

        public String ContactName { get; set; }

        public String FaxNumber { get; set; }

        public String MobileNumber { get; set; }

        public String EmailAddress { get; set; }

        public Boolean? IsAdHoc { get; set; }

        public AddressDetails Address { get; set; }

        internal void AppendToXml(XElement parent)
        {
            parent.Add(new XElement("recaccno", this.AccountNumber));
            parent.Add(new XElement("recname", this.Name));

            if (!String.IsNullOrEmpty(this.PhoneNumber))
            {
                parent.Add(new XElement("recph", this.PhoneNumber));
            }

            if (!String.IsNullOrEmpty(this.ContactName))
            {
                parent.Add(new XElement("reccontact", this.ContactName));
            }

            if (!String.IsNullOrEmpty(this.FaxNumber))
            {
                parent.Add(new XElement("recfax", this.FaxNumber));
            }

            if (!String.IsNullOrEmpty(this.MobileNumber))
            {
                parent.Add(new XElement("recmobile", this.MobileNumber));
            }

            if (!String.IsNullOrEmpty(this.EmailAddress))
            {
                parent.Add(new XElement("recemail", this.EmailAddress));
            }

            if (this.IsAdHoc.HasValue)
            {
                parent.Add(new XElement("adhocrec", this.IsAdHoc.Value ? "Yes" : "No"));
            }

            if (this.Address != null)
            {
                var addressNode = new XElement("recaddr");
                this.Address.AppendToXml(addressNode);
                parent.Add(addressNode);
            }
        }

        internal static Receiver ReadFromXml(XElement node)
        {
            if (node == null)
            {
                return null;
            }

            var receiver = new Receiver
            {
                AccountNumber = node.Element("recaccno")?.Value,
                Name = node.Element("recname")?.Value,
                PhoneNumber = node.Element("recph")?.Value,
                ContactName = node.Element("reccontact")?.Value,
                FaxNumber = node.Element("recfax")?.Value,
                MobileNumber = node.Element("recmobile")?.Value,
                EmailAddress = node.Element("recemail")?.Value,
                IsAdHoc = Primitives.ParseBooleanValue(node.Element("adhocrec")?.Value),
                Address = AddressDetails.ReadFromXml(node.Element("recaddr"))
            };

            if (receiver.AccountNumber == null &&
                receiver.Name == null &&
                receiver.PhoneNumber == null &&
                receiver.ContactName == null &&
                receiver.FaxNumber == null &&
                receiver.MobileNumber == null &&
                receiver.EmailAddress == null &&
                receiver.IsAdHoc == null &&
                receiver.Address == null)
            {
                return null;
            }

            return receiver;
        }
    }
}
