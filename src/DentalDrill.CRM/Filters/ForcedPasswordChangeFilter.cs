using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DentalDrill.CRM.Controllers;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DentalDrill.CRM.Filters
{
    public class ForcedPasswordChangeFilter : IAsyncActionFilter
    {
        public Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (this.HasMustChangeFlag(context) && !this.IsChangePasswordAction(context))
            {
                context.Result = new RedirectToActionResult("ChangePassword", "Account", new { });
                return Task.CompletedTask;
            }

            return next();
        }

        private Boolean HasMustChangeFlag(ActionExecutingContext context)
        {
            var principal = context.HttpContext.User;
            if (principal.Identity.IsAuthenticated)
            {
                var claim = principal.FindFirst("MustChangePasswordAtNextLogin");
                if (claim != null && claim.Value == "true")
                {
                    return true;
                }
            }

            return false;
        }

        private Boolean IsChangePasswordAction(ActionExecutingContext context)
        {
            if (context.ActionDescriptor is ControllerActionDescriptor controllerAction)
            {
                if (this.IsActionMatch(controllerAction, "ChangePassword", "Account"))
                {
                    return true;
                }

                if (this.IsActionMatch(controllerAction, "Logout", "Account"))
                {
                    return true;
                }
            }

            return false;
        }

        private Boolean IsActionMatch(ControllerActionDescriptor controllerAction, [AspMvcAction] String actionName, [AspMvcController] String controllerName)
        {
            return controllerAction.ControllerName.Equals(controllerName, StringComparison.InvariantCultureIgnoreCase) &&
                   controllerAction.ActionName.Equals(actionName, StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
