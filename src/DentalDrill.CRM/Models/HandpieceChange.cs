using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DentalDrill.CRM.Models
{
    public class HandpieceChange : IEntityChange<Handpiece, Guid>
    {
        public Guid Id { get; set; }

        public Guid HandpieceId { get; set; }

        public Handpiece Handpiece { get; set; }

        public DateTime ChangedOn { get; set; }

        public Guid ChangedById { get; set; }

        public Employee ChangedBy { get; set; }

        public ChangeAction Action { get; set; }

        public HandpieceStatus? OldStatus { get; set; }

        public HandpieceStatus? NewStatus { get; set; }

        public String OldContent { get; set; }

        public String NewContent { get; set; }

        Guid IEntityChange<Handpiece, Guid>.EntityId
        {
            get => this.HandpieceId;
            set => this.HandpieceId = value;
        }

        Handpiece IEntityChange<Handpiece, Guid>.Entity
        {
            get => this.Handpiece;
            set => this.Handpiece = value;
        }
    }
}
