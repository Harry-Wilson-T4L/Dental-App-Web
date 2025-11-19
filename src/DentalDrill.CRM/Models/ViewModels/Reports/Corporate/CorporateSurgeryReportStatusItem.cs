using System;

namespace DentalDrill.CRM.Models.ViewModels.Reports.Corporate
{
    public class CorporateSurgeryReportStatusItem
    {
        public Guid ClientId { get; set; }

        public String ClientName { get; set; }

        public Int32 StatusReceived { get; set; }

        public Int32 StatusBeingEstimated { get; set; }

        public Int32 StatusWaitingForApproval { get; set; }

        public Int32 StatusEstimateSent { get; set; }

        public Int32 StatusBeingRepaired { get; set; }

        public Int32 StatusReadyToReturn { get; set; }

        public Int32 StatusSentComplete { get; set; }

        public Int32 StatusCancelled { get; set; }

        public Int32 StatusAny { get; set; }
    }
}