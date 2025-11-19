using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace DentalDrill.CRM.Views.Manage
{
    public static class ManageNavPages
    {
        public static String ActivePageKey => "ActivePage";

        public static String Index => "Index";

        public static String ChangePassword => "ChangePassword";

        public static String ExternalLogins => "ExternalLogins";

        public static String TwoFactorAuthentication => "TwoFactorAuthentication";

        public static String Appearance => "Appearance";

        public static String Notifications => "Notifications";

        public static String IndexNavClass(ViewContext viewContext) => ManageNavPages.PageNavClass(viewContext, Index);

        public static String ChangePasswordNavClass(ViewContext viewContext) => ManageNavPages.PageNavClass(viewContext, ChangePassword);

        public static String ExternalLoginsNavClass(ViewContext viewContext) => ManageNavPages.PageNavClass(viewContext, ExternalLogins);

        public static String TwoFactorAuthenticationNavClass(ViewContext viewContext) => ManageNavPages.PageNavClass(viewContext, TwoFactorAuthentication);

        public static String AppearanceNavClass(ViewContext viewContext) => ManageNavPages.PageNavClass(viewContext, Appearance);

        public static String NotificationsNavClass(ViewContext viewContext) => ManageNavPages.PageNavClass(viewContext, Notifications);

        public static String PageNavClass(ViewContext viewContext, String page)
        {
            var activePage = viewContext.ViewData["ActivePage"] as String;
            return string.Equals(activePage, page, StringComparison.OrdinalIgnoreCase) ? "active" : null;
        }

        public static void AddActivePage(this ViewDataDictionary viewData, String activePage) => viewData[ActivePageKey] = activePage;
    }
}
