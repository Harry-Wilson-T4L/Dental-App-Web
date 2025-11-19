using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DentalDrill.CRM.Models;
using DentalDrill.CRM.Models.ViewModels;
using DentalDrill.CRM.Services;
using DevGuild.AspNetCore.Controllers.Mvc.Crud;
using DevGuild.AspNetCore.Controllers.Mvc.KendoUI.Base;
using DevGuild.AspNetCore.Services.Data;
using DevGuild.AspNetCore.Services.Permissions.Annotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace DentalDrill.CRM.Controllers
{
    [PermissionsManager("Type", "/Domain/ServiceLevel")]
    [PermissionsManager("Entity", "/Domain/ServiceLevel/Entities/{entity}")]
    public class ServiceLevelsController : BaseTelerikFullInlineCrudController<Guid, ServiceLevel, EmptyEmployeeIndexViewModel, ServiceLevelViewModel>
    {
        public ServiceLevelsController(IEntityControllerServices controllerServices)
            : base(controllerServices)
        {
            this.IndexHandler.Overrides.InitializeIndexViewModel = this.InitializeIndexViewModel;

            this.ReadHandler.Overrides.PrepareReadQuery = this.PrepareReadQuery;
        }

        private async Task InitializeIndexViewModel(EmptyEmployeeIndexViewModel model)
        {
            model.Access = await this.ControllerServices.ServiceProvider.GetService<UserEntityResolver>().GetEmployeeAccessAsync();
        }

        private Task<IQueryable<ServiceLevel>> PrepareReadQuery()
        {
            var query = this.Repository.Query<ServiceLevel>();
            return Task.FromResult(query);
        }
    }
}