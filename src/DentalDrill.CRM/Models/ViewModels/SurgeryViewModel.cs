using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DentalDrill.CRM.Models.ViewModels
{
    public class SurgeryViewModel
    {
        public Guid Id { get; set; }

        public Client Client { get; set; }

        public ClientAppearance Appearance { get; set; }

        public List<HandpieceModelInfo> Models { get; set; }

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
