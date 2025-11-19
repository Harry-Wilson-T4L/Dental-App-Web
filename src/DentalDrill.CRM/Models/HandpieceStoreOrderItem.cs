using System;

namespace DentalDrill.CRM.Models
{
    public class HandpieceStoreOrderItem
    {
        public Guid Id { get; set; }

        public Guid OrderId { get; set; }

        public HandpieceStoreOrder Order { get; set; }

        public Guid ListingId { get; set; }

        public HandpieceStoreListing Listing { get; set; }
    }
}