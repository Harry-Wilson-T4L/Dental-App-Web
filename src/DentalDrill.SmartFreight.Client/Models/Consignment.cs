using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace DentalDrill.SmartFreight.Client.Models
{
    public class Consignment
    {
        public DateTime? Date { get; set; }

        public String Prefix { get; set; }

        public String Number { get; set; }

        public ConsignmentReturnType? ReturnConsignment { get; set; }

        public Receiver Receiver { get; set; }

        public Sender Sender { get; set; }

        public ConsignmentChargeTo? ChargeTo { get; set; }

        public Carrier Carrier { get; set; }

        public ThirdParty ThirdParty { get; set; }

        public RegisteredOwner RegisteredOwner { get; set; }

        public ReturnToSender ReturnToSender { get; set; }

        public Boolean? WebPrint { get; set; }

        public List<FreightLine> Lines { get; set; }

        public SpecialInstructions SpecialInstructions { get; set; }

        public String TrackingId { get; set; }

        public ConsignmentTotals Totals { get; set; }

        public static Consignment DeserializeFromXml(XElement xml)
        {
            var consignment = new Consignment
            {
                Date = xml.Element("condate").ParseAsDateTime(),
                Prefix = xml.Element("conprfx")?.Value,
                Number = xml.Element("conno")?.Value,
                Receiver = Receiver.ReadFromXml(xml),
                Sender = Sender.ReadFromXml(xml),
                ChargeTo = xml.Element("chargeto").ParseEnum<ConsignmentChargeTo>(
                    ("S", ConsignmentChargeTo.Sender),
                    ("R", ConsignmentChargeTo.Receiver),
                    ("T", ConsignmentChargeTo.ThirdParty)),
                Carrier = Carrier.ReadFromXml(xml),
                ThirdParty = ThirdParty.ReadFromXml(xml),
                RegisteredOwner = RegisteredOwner.ReadFromXml(xml),
                ReturnToSender = ReturnToSender.ReadFromXml(xml),
                TrackingId = xml.Element("trackingid")?.Value,
                Totals = ConsignmentTotals.ReadFromXml(xml),
            };

            return consignment;
        }

        public static Consignment DeserializeFromString(String xmlContent)
        {
            var xml = XElement.Parse(xmlContent);
            return Consignment.DeserializeFromXml(xml);
        }

        public XElement SerializeToXml()
        {
            var root = new XElement("connote");

            root.TryAddDateElement("condate", this.Date);
            root.TryAddStringElement("conprfx", this.Prefix);
            root.TryAddStringElement("conno", this.Number);

            if (this.ReturnConsignment.HasValue)
            {
                switch (this.ReturnConsignment.Value)
                {
                    case ConsignmentReturnType.Return:
                        root.Add(new XElement("returnconnote", "Y"));
                        break;
                    case ConsignmentReturnType.Outgoing:
                        root.Add(new XElement("returnconnote", "A"));
                        break;
                }
            }
            
            this.Receiver?.AppendToXml(root);
            this.Sender?.AppendToXml(root);

            if (this.ChargeTo.HasValue)
            {
                switch (this.ChargeTo.Value)
                {
                    case ConsignmentChargeTo.Sender:
                        root.Add(new XElement("chargeto", "S"));
                        break;
                    case ConsignmentChargeTo.Receiver:
                        root.Add(new XElement("chargeto", "R"));
                        break;
                    case ConsignmentChargeTo.ThirdParty:
                        root.Add(new XElement("chargeto", "T"));
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            this.Carrier?.AppendToXml(root);
            this.ThirdParty?.AppendToXml(root);
            this.RegisteredOwner?.AppendToXml(root);
            this.ReturnToSender?.AppendToXml(root);

            if (this.WebPrint.HasValue)
            {
                root.Add(new XElement("webprint", this.WebPrint.Value ? "Yes" : "No"));
            }

            if (this.Lines != null)
            {
                foreach (var line in this.Lines)
                {
                    var lineNode = new XElement("freightlinedetails");
                    line.AppendToXml(lineNode);
                    root.Add(lineNode);
                }
            }

            this.SpecialInstructions?.AppendToXml(root);

            return root;
        }

        public String SerializeToString()
        {
            return this.SerializeToXml().ToString();
        }
    }
}
