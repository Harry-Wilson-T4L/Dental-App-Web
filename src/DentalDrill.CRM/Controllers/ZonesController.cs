using System;
using System.Linq;
using System.Threading.Tasks;
using DentalDrill.CRM.Models;
using DentalDrill.CRM.Models.ViewModels;
using DentalDrill.CRM.Services;
using DevGuild.AspNetCore.Controllers.Mvc.Crud;
using DevGuild.AspNetCore.Controllers.Mvc.KendoUI.Base;
using DevGuild.AspNetCore.Services.Data;
using DevGuild.AspNetCore.Services.Permissions.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace DentalDrill.CRM.Controllers
{
    [PermissionsManager("Type", "/Domain/Zone")]
    [PermissionsManager("Entity", "/Domain/Zone/Entities/{entity}")]
    public class ZonesController : BaseTelerikFullInlineCrudController<Guid, Zone, EmptyEmployeeIndexViewModel, ZoneViewModel>
    {
        public ZonesController(IEntityControllerServices controllerServices)
            : base(controllerServices)
        {
            this.IndexHandler.Overrides.InitializeIndexViewModel = this.InitializeIndexViewModel;

            this.ReadHandler.Overrides.PrepareReadQuery = this.PrepareReadQuery;
        }

        private async Task InitializeIndexViewModel(EmptyEmployeeIndexViewModel model)
        {
            model.Access = await this.ControllerServices.ServiceProvider.GetService<UserEntityResolver>().GetEmployeeAccessAsync();
        }

        private Task<IQueryable<Zone>> PrepareReadQuery()
        {
            var query = this.Repository.Query<Zone>();
            return Task.FromResult(query);
        }
    }
}