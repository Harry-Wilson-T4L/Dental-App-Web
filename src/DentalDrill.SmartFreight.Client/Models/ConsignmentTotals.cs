using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace DentalDrill.SmartFreight.Client.Models
{
    public class ConsignmentTotals
    {
        public Decimal TotalCost { get;set; }

        public Decimal TotalCostPlusMarkup { get; set; }

        public Decimal TotalWeight { get; set; }

        public Decimal TotalVolume { get;set; }

        public static ConsignmentTotals ReadFromXml(XElement node)
        {
            if (node == null)
            {
                return null;
            }

            var totalCost = node.Element("totcost").ParseAsDecimal();
            if (totalCost == null)
            {
                return null;
            }

            return new ConsignmentTotals
            {
                TotalCost = totalCost ?? 0,
                TotalCostPlusMarkup = node.Element("totcostplusmrkup").ParseAsDecimal() ?? 0,
                TotalVolume = node.Element("totcube").ParseAsDecimal() ?? 0,
                TotalWeight = node.Element("totwgt").ParseAsDecimal() ?? 0,
            };
        }
    }
}
