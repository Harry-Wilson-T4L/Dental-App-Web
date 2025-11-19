using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DentalDrill.CRM.Models;
using DentalDrill.CRM.Models.ViewModels;
using DentalDrill.CRM.Services;
using DevGuild.AspNetCore.Controllers.Mvc.Crud;
using DevGuild.AspNetCore.Controllers.Mvc.KendoUI.Base;
using DevGuild.AspNetCore.Services.Permissions.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace DentalDrill.CRM.Controllers
{
    [PermissionsManager("Type", "/Domain/ProblemDescriptionOption")]
    [PermissionsManager("Entity", "/Domain/ProblemDescriptionOption/Entities/{entity}")]
    public class ProblemDescriptionOptionsController : BaseTelerikFullInlineCrudController<Guid, ProblemDescriptionOption, EmptyEmployeeIndexViewModel, ProblemDescriptionOptionViewModel>
    {
        public ProblemDescriptionOptionsController(IEntityControllerServices controllerServices)
            : base(controllerServices)
        {
            this.IndexHandler.Overrides.InitializeIndexViewModel = this.InitializeIndexViewModel;
        }

        private async Task InitializeIndexViewModel(EmptyEmployeeIndexViewModel model)
        {
            model.Access = await this.ControllerServices.ServiceProvider.GetService<UserEntityResolver>().GetEmployeeAccessAsync();
        }
    }
}
