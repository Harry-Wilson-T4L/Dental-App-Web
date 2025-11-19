using System;

namespace DentalDrill.CRM.Emails
{
    public interface IManageNotificationsLinkEmail
    {
        Boolean ManageNotificationsEnabled { get; set; }

        String ManageNotificationsLink { get; set; }
    }
}
