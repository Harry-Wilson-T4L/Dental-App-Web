using System;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DentalDrill.CRM.Models.ViewModels.Identity.Account
{
    public class AccountNotificationsViewModel
    {
        [BindNever]
        public Client Client { get; set; }

        public String Token { get; set; }

        public Boolean AllowManualEmails { get; set; }

        public Boolean AllowHandpieceNotifications { get; set; }

        public Boolean AllowFeedbackRequests { get; set; }

        public Boolean AllowMaintenanceReminders { get; set; }
    }
}
