using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace DentalDrill.SmartFreight.Client.Models
{
    public class ThirdParty
    {
        public String AccountNumber { get; set; }

        public String Name { get; set; }

        public Boolean? AdHoc { get; set; }

        public AddressDetails Address { get; set; }

        internal void AppendToXml(XElement parent)
        {
            if (!String.IsNullOrEmpty(this.AccountNumber))
            {
                parent.Add(new XElement("tpyaccno", this.AccountNumber));
            }

            if (!String.IsNullOrEmpty(this.Name))
            {
                parent.Add(new XElement("tpyname", this.Name));
            }

            if (this.AdHoc.HasValue)
            {
                parent.Add(new XElement("tpyadhoc", this.AdHoc.Value ? "Yes" : "No"));
            }

            if (this.Address != null)
            {
                var addressNode = new XElement("tpyaddr");
                this.Address.AppendToXml(addressNode);
                parent.Add(addressNode);
            }
        }

        internal static ThirdParty ReadFromXml(XElement node)
        {
            if (node == null)
            {
                return null;
            }

            var thirdParty = new ThirdParty
            {
                AccountNumber = node.Element("tpyaccno")?.Value,
                Name = node.Element("tpyname")?.Value,
                AdHoc = Primitives.ParseBooleanValue(node.Element("tpyadhoc")?.Value),
                Address = AddressDetails.ReadFromXml(node.Element("tpyaddr")),
            };

            if (thirdParty.AccountNumber == null &&
                thirdParty.Name == null &&
                thirdParty.AdHoc == null &&
                thirdParty.Address == null)
            {
                return null;
            }

            return thirdParty;
        }
    }
}
