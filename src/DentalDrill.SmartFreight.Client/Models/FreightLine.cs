using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace DentalDrill.SmartFreight.Client.Models
{
    public class FreightLine
    {
        public String Reference { get; set; }

        public String Description { get; set; }

        public Int32 Amount { get; set; }

        public String CountryManufacture { get; set; }

        public String GoodsDescription { get; set; }

        public Int32? Pieces { get; set; }

        public Decimal? Value { get; set; }

        public Decimal? Weight { get; set; }

        public Decimal? Volume { get; set; }

        public Int32? Length { get; set; }

        public Int32? Height { get; set; }

        public Int32? Width { get; set; }

        public Int32? ExpectedAmount { get; set; }

        public Int32? ExpectedWeight { get; set; }

        public Decimal? ExpectedVolume { get; set; }

        public String YourItemDesc { get; set; }

        public String Category { get; set; }

        public String HSCode { get; set; }

        internal void AppendToXml(XElement parent)
        {
            if (!String.IsNullOrEmpty(this.Reference))
            {
                parent.Add(new XElement("ref", this.Reference));
            }

            if (!String.IsNullOrEmpty(this.Description))
            {
                parent.Add(new XElement("desc", this.Description));
            }

            parent.Add(new XElement("amt", this.Amount));

            if (!String.IsNullOrEmpty(this.CountryManufacture))
            {
                parent.Add(new XElement("countrymanufacture", this.CountryManufacture));
            }

            if (!String.IsNullOrEmpty(this.GoodsDescription))
            {
                parent.Add(new XElement("goods", this.GoodsDescription));
            }

            if (this.Pieces.HasValue)
            {
                parent.Add(new XElement("pieces", this.Pieces.Value));
            }

            if (this.Value.HasValue)
            {
                parent.Add(new XElement("val", this.Value.Value));
            }

            if (this.Weight.HasValue)
            {
                parent.Add(new XElement("wgt", this.Weight.Value));
            }

            if (this.Volume.HasValue)
            {
                parent.Add(new XElement("cube", this.Volume.Value));
            }

            if (this.Length.HasValue)
            {
                parent.Add(new XElement("len", this.Length.Value));
            }

            if (this.Height.HasValue)
            {
                parent.Add(new XElement("hgt", this.Height.Value));
            }

            if (this.Width.HasValue)
            {
                parent.Add(new XElement("wdt", this.Width.Value));
            }

            if (this.ExpectedAmount.HasValue)
            {
                parent.Add(new XElement("expectedamt", this.ExpectedAmount.Value));
            }

            if (this.ExpectedWeight.HasValue)
            {
                parent.Add(new XElement("expectedwgt", this.ExpectedWeight.Value));
            }

            if (this.ExpectedVolume.HasValue)
            {
                parent.Add(new XElement("expectedcube", this.ExpectedVolume.Value));
            }

            if (!String.IsNullOrEmpty(this.YourItemDesc))
            {
                parent.Add(new XElement("youritemdesc", this.YourItemDesc));
            }

            if (!String.IsNullOrEmpty(this.Category))
            {
                parent.Add(new XElement("cat", this.Category));
            }

            if (!String.IsNullOrEmpty(this.HSCode))
            {
                parent.Add(new XElement("hscode", this.HSCode));
            }
        }
    }
}
