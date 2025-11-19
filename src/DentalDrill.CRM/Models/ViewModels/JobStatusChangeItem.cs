using System;

namespace DentalDrill.CRM.Models.ViewModels
{
    public class JobStatusChangeItem
    {
        public Guid Id { get; set; }

        public HandpieceStatus Status { get; set; }
    }
}