using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DentalDrill.CRM.Models.ViewModels
{
    public class CorporateSurgeryViewModel
    {
        public Guid Id { get; set; }

        public Corporate Corporate { get; set; }

        public CorporateAppearance Appearance { get; set; }

        public List<HandpieceModelInfo> Models { get; set; }

        public List<Client> Clients { get; set; }

        public Int32 ReceivedItems { get; set; }

        public Int32 BeingEstimatedItems { get; set; }

        public Int32 WaitingForApprovalItems { get; set; }

        public Int32 EstimateSentItems { get; set; }

        public Int32 BeingRepairedItems { get; set; }

        public Int32 ReadyForReturnItems { get; set; }

        public Int32 UnrepairedItems { get; set; }

        public Int32 SentCompleteItems { get; set; }
    }
}
