using System;

namespace DentalDrill.CRM.Models
{
    public class InventoryMovementLinkedHandpiece
    {
        public Guid Id { get; set; }

        public Guid JobId { get; set; }

        public Job Job { get; set; }

        public Int64 JobNumber { get; set; }

        public JobStatus JobStatus { get; set; }

        public Guid HandpieceId { get; set; }

        public Handpiece Handpiece { get; set; }

        public String HandpieceNumber { get; set; }

        public Guid ClientId { get; set; }

        public Client Client { get; set; }

        public String ClientFullName { get; set; }

        public HandpieceStatus HandpieceStatus { get; set; }
    }
}
