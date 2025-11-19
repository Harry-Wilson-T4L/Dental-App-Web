using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace DentalDrill.CRM.Extensions
{
    public static class ControllerExtensions
    {
        public static LocalRedirectResult LocalRedirectOrDefault(this ControllerBase controller, String localUrl)
        {
            if (String.IsNullOrEmpty(localUrl) || !controller.Url.IsLocalUrl(localUrl))
            {
                return controller.LocalRedirect(controller.Url.Content("~/"));
            }
            else
            {
                return controller.LocalRedirect(localUrl);
            }
        }
    }
}
