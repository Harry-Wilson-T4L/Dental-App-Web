using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace DentalDrill.SmartFreight.Client.Models
{
    public static class SerializationExtensions
    {
        public static void TryAddStringElement(this XElement parent, String name, String value)
        {
            if (!String.IsNullOrEmpty(value))
            {
                parent.Add(new XElement(name, value));
            }
        }

        public static void TryAddDateElement(this XElement parent, String name, DateTime? value)
        {
            if (value.HasValue)
            {
                parent.Add(new XElement(name, value.Value.ToString("dd/MM/yyyy")));
            }
        }
    }
}
