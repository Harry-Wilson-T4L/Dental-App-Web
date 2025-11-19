using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DentalDrill.CRM.Models;
using DentalDrill.CRM.Models.ViewModels.CorporatePricingServiceLevels;
using DentalDrill.CRM.Services;
using DevGuild.AspNetCore.Controllers.Mvc.Crud;
using DevGuild.AspNetCore.Controllers.Mvc.KendoUI.Base;
using DevGuild.AspNetCore.Services.Data;
using DevGuild.AspNetCore.Services.Permissions.Annotations;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DentalDrill.CRM.Controllers
{
    [Authorize(Roles = ApplicationRoles.Combined.Staff)]
    [PermissionsManager("Type", "/Domain/CorporatePricing")]
    [PermissionsManager("Entity", "/Domain/CorporatePricing/Entities/{entity}")]
    public class CorporatePricingServiceLevelsController : BaseTelerikFullInlineCrudController<Guid, ServiceLevel, CorporatePricingServiceLevelIndexModel, CorporatePricingServiceLevelViewModel>
    {
        private List<CorporatePricingCategory> readCategories;

        public CorporatePricingServiceLevelsController(IEntityControllerServices controllerServices)
            : base(controllerServices)
        {
            this.IndexHandler.Overrides.InitializeIndexViewModel = this.InitializeIndexViewModel;

            this.ReadHandler.Overrides.PrepareReadQuery = this.PrepareReadQuery;
            this.ReadHandler.Overrides.ConvertEntityToViewModel = this.ConvertEntityToViewModel;

            this.UpdateHandler.Overrides.QuerySingleEntity = this.QuerySingleEntityForUpdate;
            this.UpdateHandler.Overrides.UpdateExistingEntity = this.UpdateExistingEntity;
            this.UpdateHandler.Overrides.ConvertEntityToViewModel = this.ConvertUpdatedEntityToViewModel;
        }

        public override Task<IActionResult> Create(DataSourceRequest request, CorporatePricingServiceLevelViewModel model)
        {
            throw new NotSupportedException();
        }

        public override Task<IActionResult> Destroy(DataSourceRequest request, CorporatePricingServiceLevelViewModel model)
        {
            throw new NotSupportedException();
        }

        private async Task InitializeIndexViewModel(CorporatePricingServiceLevelIndexModel model)
        {
            model.Categories = await this.Repository.QueryWithoutTracking<CorporatePricingCategory>().OrderBy(x => x.OrderNo).ToListAsync();
            model.Access = await this.ControllerServices.ServiceProvider.GetService<UserEntityResolver>().GetEmployeeAccessAsync();
        }

        private async Task<IQueryable<ServiceLevel>> PrepareReadQuery()
        {
            this.readCategories = await this.Repository.QueryWithoutTracking<CorporatePricingCategory>().OrderBy(x => x.OrderNo).ToListAsync();
            IQueryable<ServiceLevel> query = this.Repository.Query<ServiceLevel>()
                .Include(x => x.CorporatePricing);

            return query;
        }

        private CorporatePricingServiceLevelViewModel ConvertEntityToViewModel(ServiceLevel entity, String[] allowedProperties)
        {
            return new CorporatePricingServiceLevelViewModel
            {
                Id = entity.Id,
                ServiceLevelName = entity.Name,
                CategoriesPricing = this.readCategories.ToDictionary(
                    keySelector: x => x.Id,
                    elementSelector: x => entity.CorporatePricing.SingleOrDefault(y => y.CategoryId == x.Id)?.CostOfRepair),
            };
        }

        private Task<ServiceLevel> QuerySingleEntityForUpdate(Guid id)
        {
            return this.Repository.Query<ServiceLevel>()
                .Include(x => x.CorporatePricing)
                .ThenInclude(x => x.Category)
                .SingleOrDefaultAsync(x => x.Id == id);
        }

        private async Task UpdateExistingEntity(ServiceLevel entity, CorporatePricingServiceLevelViewModel model)
        {
            var categories = await this.Repository.QueryWithoutTracking<CorporatePricingCategory>().OrderBy(x => x.OrderNo).ToListAsync();
            foreach (var category in categories)
            {
                var localValue = entity.CorporatePricing.SingleOrDefault(x => x.CategoryId == category.Id);
                if (model.CategoriesPricing.TryGetValue(category.Id, out var value) && value.HasValue)
                {
                    // Value is present
                    if (localValue != null)
                    {
                        localValue.CostOfRepair = value.Value;
                    }
                    else
                    {
                        entity.CorporatePricing.Add(new CorporatePricingServiceLevel
                        {
                            CategoryId = category.Id,
                            CostOfRepair = value.Value,
                        });
                    }
                }
                else
                {
                    // Value is missing
                    if (localValue != null)
                    {
                        entity.CorporatePricing.Remove(localValue);
                    }
                }
            }
        }

        private async Task<CorporatePricingServiceLevelViewModel> ConvertUpdatedEntityToViewModel(ServiceLevel entity)
        {
            this.readCategories = await this.Repository.QueryWithoutTracking<CorporatePricingCategory>().OrderBy(x => x.OrderNo).ToListAsync();
            return this.ConvertEntityToViewModel(entity, null);
        }
    }
}
