using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;

namespace DentalDrill.CRM.Models.ViewModels.Identity.Manage
{
    public class ExternalLoginsViewModel
    {
        public IList<UserLoginInfo> CurrentLogins { get; set; }

        public IList<AuthenticationScheme> OtherLogins { get; set; }

        public Boolean ShowRemoveButton { get; set; }

        public String StatusMessage { get; set; }
    }
}
