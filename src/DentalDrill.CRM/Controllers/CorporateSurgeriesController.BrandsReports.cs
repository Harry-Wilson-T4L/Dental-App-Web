using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DentalDrill.CRM.Domain;
using DentalDrill.CRM.Models;
using DentalDrill.CRM.Models.ViewModels;
using DentalDrill.CRM.Models.ViewModels.Reports.Corporate;
using DentalDrill.CRM.Services.ExcelExporter;
using DevGuild.AspNetCore.Controllers.Mvc.Filters;
using DevGuild.AspNetCore.Services.Data;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DentalDrill.CRM.Controllers
{
    public partial class CorporateSurgeriesController
    {
        public async Task<IActionResult> ExportBrandsToExcel(String parentId, [FromQuery(Name = "")] CorporateSurgeryReportBrandsFilter filter, String reportFields)
        {
            var corporate = await CorporateDomainModel.GetByUrlIdAsync(this.ControllerServices.ServiceProvider, parentId);
            if (corporate == null)
            {
                return this.NotFound();
            }

            // For now - always include all clients
            filter.Clients = (await this.Repository.Query<Client>().Where(x => x.CorporateId == corporate.Id).ToListAsync()).Select(x => x.Id).ToArray();

            await this.CorporatePermissionsValidator.DemandCanDetailsAsync(corporate.Entity);
            var report = await corporate.Reports.PrepareBrandsReport(filter.From, filter.To, filter.Clients);
            var reportFieldsHashSet = new HashSet<String>(reportFields.Split(","));
            var exporter = new CorporateBrandsReportExcelExporter(corporate, report, filter.From, filter.To, filter.DateAggregate, reportFieldsHashSet);
            var bytes = await exporter.ExportAsBytes();
            return this.File(
                fileContents: bytes,
                contentType: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                fileDownloadName: $"{parentId}-Brands-{filter.From:yyyyMMdd}-{filter.To:yyyyMMdd}-{filter.DateAggregate}.xlsx");
        }

        [AjaxPost]
        public async Task<IActionResult> ReadReportBrands(String parentId, CorporateSurgeryReportBrandsFilter filterModel, [DataSourceRequest] DataSourceRequest request)
        {
            var corporate = await CorporateDomainModel.GetByUrlIdAsync(this.ControllerServices.ServiceProvider, parentId);
            if (corporate == null)
            {
                return this.NotFound();
            }

            await this.CorporatePermissionsValidator.DemandCanDetailsAsync(corporate.Entity);
            var report = await corporate.Reports.PrepareBrandsReport(filterModel.From, filterModel.To, filterModel.Clients);

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
        public async Task<IActionResult> ReadReportBrandsByModel(String parentId, CorporateSurgeryReportBrandsFilter filterModel, [DataSourceRequest] DataSourceRequest request)
        {
            var corporate = await CorporateDomainModel.GetByUrlIdAsync(this.ControllerServices.ServiceProvider, parentId);
            if (corporate == null)
            {
                return this.NotFound();
            }

            await this.CorporatePermissionsValidator.DemandCanDetailsAsync(corporate.Entity);
            var report = await corporate.Reports.PrepareBrandsReport(filterModel.From, filterModel.To, filterModel.Clients);

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
        public async Task<IActionResult> ReadReportBrandsBySurgeries(String parentId, CorporateSurgeryReportBrandsFilter filterModel, [DataSourceRequest] DataSourceRequest request)
        {
            var corporate = await CorporateDomainModel.GetByUrlIdAsync(this.ControllerServices.ServiceProvider, parentId);
            if (corporate == null)
            {
                return this.NotFound();
            }

            await this.CorporatePermissionsValidator.DemandCanDetailsAsync(corporate.Entity);
            var report = await corporate.Reports.PrepareBrandsReport(filterModel.From, filterModel.To, filterModel.Clients);

            switch (filterModel.DateAggregate)
            {
                case ReportDateAggregate.EntirePeriod:
                {
                    var result = await report.GetBrandsAndSurgeriesForEntirePeriodAsync();
                    return this.Json(await result.ToDataSourceResultAsync(request));
                }

                case ReportDateAggregate.Yearly:
                case ReportDateAggregate.Quarterly:
                case ReportDateAggregate.Monthly:
                case ReportDateAggregate.Weekly:
                case ReportDateAggregate.Daily:
                {
                    var result = await report.GetBrandsAndSurgeriesAggregatedAsync(filterModel.DateAggregate);
                    return this.Json(await result.ToDataSourceResultAsync(request));
                }

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
