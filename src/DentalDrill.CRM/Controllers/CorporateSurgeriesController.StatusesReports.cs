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
        public async Task<IActionResult> ExportStatusesToExcel(String parentId, [FromQuery(Name = "")] CorporateSurgeryReportStatusesFilter filter)
        {
            var corporate = await CorporateDomainModel.GetByUrlIdAsync(this.ControllerServices.ServiceProvider, parentId);
            if (corporate == null)
            {
                return this.NotFound();
            }

            // For now - always include all clients
            filter.Clients = (await this.Repository.Query<Client>().Where(x => x.CorporateId == corporate.Id).ToListAsync()).Select(x => x.Id).ToArray();

            await this.CorporatePermissionsValidator.DemandCanDetailsAsync(corporate.Entity);
            var report = await corporate.Reports.PrepareStatusesReport(filter.From, filter.To, filter.Clients);
            var exporter = new CorporateStatusesReportExcelExporter(corporate, report);
            var bytes = await exporter.ExportAsBytes();
            return this.File(
                fileContents: bytes,
                contentType: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                fileDownloadName: $"{parentId}-Statuses-{filter.From:yyyyMMdd}-{filter.To:yyyyMMdd}.xlsx");
        }

        [AjaxPost]
        public async Task<IActionResult> ReadReportStatuses(String parentId, CorporateSurgeryReportStatusesFilter filterModel, [DataSourceRequest] DataSourceRequest request)
        {
            var corporate = await CorporateDomainModel.GetByUrlIdAsync(this.ControllerServices.ServiceProvider, parentId);
            if (corporate == null)
            {
                return this.NotFound();
            }

            await this.CorporatePermissionsValidator.DemandCanDetailsAsync(corporate.Entity);
            var report = await corporate.Reports.PrepareStatusesReport(filterModel.From, filterModel.To, filterModel.Clients);
            var result = await report.GetSurgeriesForEntirePeriodAsync();

            return this.Json(await result.ToDataSourceResultAsync(request));
        }
    }
}
