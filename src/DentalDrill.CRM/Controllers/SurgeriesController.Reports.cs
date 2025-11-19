using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DentalDrill.CRM.Domain;
using DentalDrill.CRM.Models.ViewModels;
using DentalDrill.CRM.Models.ViewModels.Reports.Surgery;
using DevGuild.AspNetCore.Controllers.Mvc.Filters;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DentalDrill.CRM.Controllers
{
    public partial class SurgeriesController
    {
        [AjaxPost]
        public async Task<IActionResult> ReadReportBrands(String parentId, SurgeryReportBrandsFilter filterModel, [DataSourceRequest] DataSourceRequest request)
        {
            var surgery = await ClientDomainModel.GetByUrlIdAsync(this.ControllerServices.ServiceProvider, parentId);
            if (surgery == null)
            {
                return this.NotFound();
            }

            await this.ClientPermissionsValidator.DemandCanDetailsAsync(surgery.Entity);
            var report = await surgery.Reports.PrepareBrandsReport(filterModel.From, filterModel.To);

            switch (filterModel.DateAggregate)
            {
                case ReportDateAggregate.EntirePeriod:
                {
                    var result = await report.GetBrandsForEntirePeriodAsync();
                    return this.Json(await result.ToDataSourceResultAsync(request));
                }

                case ReportDateAggregate.Yearly:
                case ReportDateAggregate.Quarterly:
                case ReportDateAggregate.Monthly:
                case ReportDateAggregate.Weekly:
                case ReportDateAggregate.Daily:
                {
                    var result = await report.GetBrandsAggregatedAsync(filterModel.DateAggregate);
                    return this.Json(await result.ToDataSourceResultAsync(request));
                }

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        [AjaxPost]
        public async Task<IActionResult> ReadReportBrandsByModel(String parentId, SurgeryReportBrandsFilter filterModel, [DataSourceRequest] DataSourceRequest request)
        {
            var surgery = await ClientDomainModel.GetByUrlIdAsync(this.ControllerServices.ServiceProvider, parentId);
            if (surgery == null)
            {
                return this.NotFound();
            }

            await this.ClientPermissionsValidator.DemandCanDetailsAsync(surgery.Entity);
            var report = await surgery.Reports.PrepareBrandsReport(filterModel.From, filterModel.To);

            switch (filterModel.DateAggregate)
            {
                case ReportDateAggregate.EntirePeriod:
                {
                    var result = await report.GetBrandsAndModelsForEntirePeriodAsync();
                    return this.Json(await result.ToDataSourceResultAsync(request));
                }

                case ReportDateAggregate.Yearly:
                case ReportDateAggregate.Quarterly:
                case ReportDateAggregate.Monthly:
                case ReportDateAggregate.Weekly:
                case ReportDateAggregate.Daily:
                {
                    var result = await report.GetBrandsAndModelsAggregatedAsync(filterModel.DateAggregate);
                    return this.Json(await result.ToDataSourceResultAsync(request));
                }

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        [AjaxPost]
        public async Task<IActionResult> ReadReportStatuses(String parentId, SurgeryReportStatusesFilter filterModel, [DataSourceRequest] DataSourceRequest request)
        {
            var surgery = await ClientDomainModel.GetByUrlIdAsync(this.ControllerServices.ServiceProvider, parentId);
            if (surgery == null)
            {
                return this.NotFound();
            }

            await this.ClientPermissionsValidator.DemandCanDetailsAsync(surgery.Entity);
            var report = await surgery.Reports.PrepareStatusesReport(filterModel.From, filterModel.To);
            var result = await report.GetBrandsForEntirePeriodAsync();

            return this.Json(await result.ToDataSourceResultAsync(request));
        }
    }
}
