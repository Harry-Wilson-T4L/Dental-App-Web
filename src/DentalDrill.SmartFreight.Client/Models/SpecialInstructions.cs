using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace DentalDrill.SmartFreight.Client.Models
{
    public class SpecialInstructions
    {
        public String Full { get; set; }

        public String Line1 { get; set; }

        public String Line2 { get; set; }

        public String Line3 { get; set; }

        public void AppendToXml(XElement node)
        {
            if (!String.IsNullOrEmpty(this.Full))
            {
                node.Add(new XElement("fullspins", this.Full));
                return;
            }

            var spins = new XElement("spins");

            spins.TryAddStringElement("sp1", this.Line1);
            spins.TryAddStringElement("sp2", this.Line2);
            spins.TryAddStringElement("sp3", this.Line3);

            node.Add(spins);
        }
    }
}
