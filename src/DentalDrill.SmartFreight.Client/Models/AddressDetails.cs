using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace DentalDrill.SmartFreight.Client.Models
{
    public class AddressDetails
    {
        /// <summary>
        /// Gets or sets the address line 1. Normally used for Lvl, Suite, Flr.
        /// </summary>
        /// <value>
        /// The address line 1.
        /// </value>
        public String AddressLine1 { get; set; }

        /// <summary>
        /// Gets or sets the address line 2. Street address.
        /// </summary>
        /// <value>
        /// The address line 2.
        /// </value>
        public String AddressLine2 { get; set; }

        /// <summary>
        /// Gets or sets the town/suburb.
        /// </summary>
        /// <value>
        /// The town/suburb.
        /// </value>
        public String Town { get; set; }

        /// <summary>
        /// Gets or sets the state.
        /// </summary>
        /// <value>
        /// The state.
        /// </value>
        public String State { get; set; }

        /// <summary>
        /// Gets or sets the postcode.
        /// </summary>
        /// <value>
        /// The postcode.
        /// </value>
        public String Postcode { get; set; }

        /// <summary>
        /// Gets or sets the country.
        /// </summary>
        /// <value>
        /// The country.
        /// </value>
        public String Country { get; set; }

        public static AddressDetails AustralianAddress(String line1, String line2, String suburb, String state, String postCode)
        {
            return new AddressDetails
            {
                AddressLine1 = line1,
                AddressLine2 = line2,
                Town = suburb,
                State = state,
                Postcode = postCode,
                Country = "Australia",
            };
        }

        public static AddressDetails NewZealandAddress(String line1, String line2, String town, String locality, String island)
        {
            return new AddressDetails
            {
                AddressLine1 = line1,
                AddressLine2 = line2,
                Town = town,
                State = locality,
                Postcode = island,
                Country = "New Zealand",
            };
        }

        internal void AppendToXml(XElement parent)
        {
            if (!String.IsNullOrEmpty(this.AddressLine1))
            {
                parent.Add(new XElement("add1", this.AddressLine1));
            }

            if (!String.IsNullOrEmpty(this.AddressLine2))
            {
                parent.Add(new XElement("add2", this.AddressLine2));
            }

            if (!String.IsNullOrEmpty(this.Town))
            {
                parent.Add(new XElement("add3", this.Town));
            }

            if (!String.IsNullOrEmpty(this.State))
            {
                parent.Add(new XElement("add4", this.State));
            }

            if (!String.IsNullOrEmpty(this.Postcode))
            {
                parent.Add(new XElement("add5", this.Postcode));
            }

            if (!String.IsNullOrEmpty(this.Country))
            {
                parent.Add(new XElement("add6", this.Country));
            }
        }

        internal static AddressDetails ReadFromXml(XElement node)
        {
            if (node == null)
            {
                return null;
            }

            var address = new AddressDetails
            {
                AddressLine1 = node.Element("add1")?.Value,
                AddressLine2 = node.Element("add2")?.Value,
                Town = node.Element("add3")?.Value,
                State = node.Element("add4")?.Value,
                Postcode = node.Element("add5")?.Value,
                Country = node.Element("add6")?.Value,
            };

            if (address.AddressLine1 == null && address.AddressLine2 == null && address.Town == null && address.State == null && address.Postcode == null && address.Country == null)
            {
                return null;
            }

            return address;
        }
    }
}
