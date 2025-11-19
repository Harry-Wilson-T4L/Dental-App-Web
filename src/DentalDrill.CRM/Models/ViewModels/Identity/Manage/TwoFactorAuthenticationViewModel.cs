using System;

namespace DentalDrill.CRM.Models.ViewModels.Identity.Manage
{
    public class TwoFactorAuthenticationViewModel
    {
        public Boolean HasAuthenticator { get; set; }

        public Int32 RecoveryCodesLeft { get; set; }

        public Boolean Is2faEnabled { get; set; }
    }
}
