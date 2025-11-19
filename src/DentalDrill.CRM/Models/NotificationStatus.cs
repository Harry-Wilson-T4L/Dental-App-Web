using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DentalDrill.CRM.Models
{
    public enum NotificationStatus
    {
        /// <summary>
        /// The notification is not read yet.
        /// </summary>
        Unread,

        /// <summary>
        /// The notification is read but not resolved yet.
        /// </summary>
        Read,

        /// <summary>
        /// The notification is resolved.
        /// </summary>
        Resolved,
    }
}
