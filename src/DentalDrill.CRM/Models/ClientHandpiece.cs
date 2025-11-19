using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace DentalDrill.CRM.Models
{
    public class ClientHandpiece
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid ClientId { get; set; }

        public Client Client { get; set; }

        [Required]
        [MaxLength(100)]
        public String Brand { get; set; }

        [Required]
        [MaxLength(200)]
        public String Model { get; set; }

        [Required]
        [MaxLength(200)]
        public String Serial { get; set; }

        public String ComponentsText { get; set; }

        public Boolean RemindersDisabled { get; set; }

        public Int32 RemindersRecentCount { get; set; }

        public Int32 RemindersTotalCount { get; set; }

        public Guid? RemindersLastHandpieceId { get; set; }

        public DateTime? RemindersLastDateTime { get; set; }

        public ICollection<ClientHandpieceComponent> Components { get; set; }

        public ICollection<Handpiece> Handpieces { get; set; }

        public Boolean MatchesHandpiece(Handpiece handpiece)
        {
            if (this.Components == null || handpiece.Components == null)
            {
                throw new InvalidOperationException("Components were not loaded");
            }

            if (this.Brand != handpiece.Brand ||
                this.Model != handpiece.MakeAndModel ||
                this.Serial != handpiece.Serial)
            {
                return false;
            }

            var thisComponents = this.Components.OrderBy(x => x.OrderNo).ToList();
            var otherComponents = handpiece.Components.OrderBy(x => x.OrderNo).ToList();

            if (thisComponents.Count != otherComponents.Count)
            {
                return false;
            }

            for (var i = 0; i < thisComponents.Count; i++)
            {
                if (thisComponents[i].Brand != otherComponents[i].Brand ||
                    thisComponents[i].Model != otherComponents[i].Model ||
                    thisComponents[i].Serial != otherComponents[i].Serial)
                {
                    return false;
                }
            }

            return true;
        }

        public void UpdateFromHandpiece(Handpiece handpiece)
        {
            this.Brand = handpiece.Brand;
            this.Model = handpiece.MakeAndModel;
            this.Serial = handpiece.Serial;
            this.ComponentsText = handpiece.ComponentsText;

            var thisComponents = this.Components.OrderBy(x => x.OrderNo).ToList();
            var otherComponents = handpiece.Components.OrderBy(x => x.OrderNo).ToList();

            var minLength = Math.Min(thisComponents.Count, otherComponents.Count);
            var maxOrder = 0;
            for (var i = 0; i < minLength; i++)
            {
                thisComponents[i].Brand = otherComponents[i].Brand;
                thisComponents[i].Model = otherComponents[i].Model;
                thisComponents[i].Serial = otherComponents[i].Serial;
                maxOrder = Math.Max(maxOrder, thisComponents[i].OrderNo);
            }

            for (var i = minLength; i < thisComponents.Count; i++)
            {
                this.Components.Remove(thisComponents[i]);
            }

            for (var i = minLength; i < otherComponents.Count; i++)
            {
                this.Components.Add(new ClientHandpieceComponent
                {
                    Brand = otherComponents[i].Brand,
                    Model = otherComponents[i].Model,
                    Serial = otherComponents[i].Serial,
                    OrderNo = ++maxOrder,
                });
            }
        }
    }
}
