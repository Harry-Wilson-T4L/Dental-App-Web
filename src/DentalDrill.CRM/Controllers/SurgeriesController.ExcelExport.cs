using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DentalDrill.CRM.Domain;
using DentalDrill.CRM.Models.ViewModels.Reports.Surgery;
using DentalDrill.CRM.Services.ExcelExporter;
using Microsoft.AspNetCore.Mvc;

namespace DentalDrill.CRM.Controllers
{
    public partial class SurgeriesController
    {
        public async Task<IActionResult> ExportBrandsToExcel(String parentId, [FromQuery(Name = "")] SurgeryReportBrandsFilter filter, String reportFields)
        {
            var surgery = await ClientDomainModel.GetByUrlIdAsync(this.ControllerServices.ServiceProvider, parentId);
            if (surgery == null)
            {
                return this.NotFound();
            }

            await this.ClientPermissionsValidator.DemandCanDetailsAsync(surgery.Entity);
            var report = await surgery.Reports.PrepareBrandsReport(filter.From, filter.To);
            var reportFieldsHashSet = new HashSet<String>(reportFields.Split(","));

            var exporter = new SurgeryBrandsReportExcelExporter(surgery, report, filter.From, filter.To, filter.DateAggregate, reportFieldsHashSet);
            var bytes = await exporter.ExportAsBytes();
            return this.File(
                fileContents: bytes,
                contentType: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                fileDownloadName: $"{parentId}-Brands-{filter.From:yyyyMMdd}-{filter.To:yyyyMMdd}-{filter.DateAggregate}.xlsx");
        }

        public async Task<IActionResult> ExportStatusesToExcel(String parentId, [FromQuery(Name = "")] SurgeryReportStatusesFilter filter)
        {
            var surgery = await ClientDomainModel.GetByUrlIdAsync(this.ControllerServices.ServiceProvider, parentId);
            if (surgery == null)
            {
                return this.NotFound();
            }

            await this.ClientPermissionsValidator.DemandCanDetailsAsync(surgery.Entity);
            var report = await surgery.Reports.PrepareStatusesReport(filter.From, filter.To);

            var exporter = new SurgeryStatusesReportExcelExporter(surgery, report, filter.From, filter.To);
            var bytes = await exporter.ExportAsBytes();
            return this.File(
                fileContents: bytes,
                contentType: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                fileDownloadName: $"{parentId}-Statuses-{filter.From:yyyyMMdd}-{filter.To:yyyyMMdd}.xlsx");
        }
    }
}
