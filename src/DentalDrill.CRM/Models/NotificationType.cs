using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DentalDrill.CRM.Models
{
    public enum NotificationType
    {
        /// <summary>
        /// The unknown type.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// The job is created and need to be estimated by workshop.
        /// Trigger: Job Created
        /// Scope: Workshop
        /// Recipient: *
        /// </summary>
        JobCreated = 1,

        /// <summary>
        /// The job estimation is complete and need to be approved.
        /// Trigger: Job Status: BeingEstimated -> WaitingForApproval
        /// Scope: Office
        /// Recipient: Creator
        /// </summary>
        JobEstimated = 2,

        /// <summary>
        /// The job is approved and repair can start.
        /// Trigger: Job Status: WaitingForApproval -> BeingRepaired
        /// Scope: Workshop
        /// Recipient: *
        /// </summary>
        JobApprovedForRepair = 3,

        /// <summary>
        /// The job repair is complete.
        /// Trigger: Job Status: BeingRepaired -> ReadyToReturn
        /// Scope: Office
        /// Recipient: *
        /// </summary>
        JobRepairComplete = 4,

        /// <summary>
        /// A handpiece store order is created.
        /// Trigger: Handpiece Store Order Created
        /// Scope: Office
        /// Recipient: *
        /// </summary>
        HandpieceStoreOrderCreated = 5,
    }
}
