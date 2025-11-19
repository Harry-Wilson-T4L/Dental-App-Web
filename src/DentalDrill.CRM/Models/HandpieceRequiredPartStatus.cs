namespace DentalDrill.CRM.Models
{
    public enum HandpieceRequiredPartStatus
    {
        /// <summary>The status cannot be determined.</summary>
        Unknown,

        /// <summary>The required part is not available and no order in progress.</summary>
        Waiting,

        /// <summary>The required part is not available and order has been requested.</summary>
        WaitingRequested,

        /// <summary>The required part is not available and order has been approved.</summary>
        WaitingApproved,

        /// <summary>The required part is not available and order has been ordered.</summary>
        WaitingOrdered,

        /// <summary>The required part is available and was allocated.</summary>
        Allocated,

        /// <summary>The required part is consumed.</summary>
        Completed,

        /// <summary>The repair was cancelled and required part is no longer required.</summary>
        Cancelled,
    }
}
