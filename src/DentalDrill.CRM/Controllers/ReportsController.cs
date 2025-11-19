using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DentalDrill.CRM.Domain;
using DentalDrill.CRM.Models;
using DentalDrill.CRM.Models.Permissions;
using DentalDrill.CRM.Models.ViewModels;
using DentalDrill.CRM.Models.ViewModels.Reports.Global;
using DentalDrill.CRM.Services;
using DevGuild.AspNetCore.Controllers.Mvc.Filters;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace DentalDrill.CRM.Controllers
{
    [Authorize(Roles = ApplicationRoles.Combined.Staff)]
    public class ReportsController : Controller
    {
        private readonly IServiceProvider serviceProvider;

        public ReportsController(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public async Task<IActionResult> Index()
        {
            if (!(await this.serviceProvider.GetRequiredService<UserEntityResolver>().GetEmployeeAccessAsync()).Global.CanReadComponent(GlobalComponent.Report))
            {
                return this.NotFound();
            }

            var model = new GlobalReportsViewModel
            {
                DateRanges = CreateReportDateRanges(),
                DefaultDateRange = ReportDateRange.YearUpTo(DateTime.UtcNow.Date, "Year"),
            };

            return this.View(model);

            List<List<ReportDateRange>> CreateReportDateRanges()
            {
                var today = DateTime.UtcNow.Date;
                return new List<List<ReportDateRange>>
                {
                    new List<ReportDateRange>
                    {
                        ReportDateRange.SingleDay(today, "Today"),
                        ReportDateRange.SingleWeekUpTo(today, "Week"),
                    },
                    new List<ReportDateRange>
                    {
                        ReportDateRange.SpecificNumberOfPreviousDays(today, 30, "30 days"),
                        ReportDateRange.SingleMonthUpTo(today, today.ToString("MMMM")),
                        ReportDateRange.SingleMonth(today.Year, today.Month - 1, today.AddMonths(-1).ToString("MMMM")),
                        ReportDateRange.SingleMonth(today.Year, today.Month - 2, today.AddMonths(-2).ToString("MMMM")),
                    },
                    new List<ReportDateRange>
                    {
                        ReportDateRange.QuarterUpTo(today, "Quarter"),
                        ReportDateRange.YearUpTo(today, "Year"),
                    },
                };
            }
        }

        [AjaxPost]
        public async Task<IActionResult> ReadReportHandpieces(GlobalHandpiecesReportFilter filterModel, [DataSourceRequest] DataSourceRequest request)
        {
            if (!(await this.serviceProvider.GetRequiredService<UserEntityResolver>().GetEmployeeAccessAsync()).Global.CanReadComponent(GlobalComponent.Report))
            {
                return this.NotFound();
            }

            var global = await GlobalDomainModel.InitializeAsync(this.serviceProvider);

            var report = await global.Reports.PrepareHandpiecesReport(filterModel.From, filterModel.To, filterModel.Fields);

            switch (filterModel.DateAggregate)
            {
                case ReportDateAggregate.EntirePeriod:
                {
                    var result = await report.GetForEntirePeriodAsync();
                    return this.Json(await result.ToDataSourceResultAsync(request));
                }

                case ReportDateAggregate.Yearly:
                case ReportDateAggregate.Quarterly:
                case ReportDateAggregate.Monthly:
                case ReportDateAggregate.Weekly:
                case ReportDateAggregate.Daily:
                {
                    var result = await report.GetAggregatedAsync(filterModel.DateAggregate);
                    return this.Json(await result.ToDataSourceResultAsync(request));
                }

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        [AjaxPost]
        public async Task<IActionResult> ReadReportTechWarranty(GlobalTechWarrantyReportFilter filterModel, [DataSourceRequest] DataSourceRequest request)
        {
            if (!(await this.serviceProvider.GetRequiredService<UserEntityResolver>().GetEmployeeAccessAsync()).Global.CanReadComponent(GlobalComponent.Report))
            {
                return this.NotFound();
            }

            var global = await GlobalDomainModel.InitializeAsync(this.serviceProvider);
            var report = await global.Reports.PrepareTechWarrantyReport(filterModel.From, filterModel.To);
            var result = await report.GetDataAsync();
            return this.Json(await result.ToDataSourceResultAsync(request));
        }

        public async Task<IActionResult> ReadReportBatchReturns(GlobalBatchResultReportFilter filterModel, [DataSourceRequest] DataSourceRequest request)
        {
            if (!(await this.serviceProvider.GetRequiredService<UserEntityResolver>().GetEmployeeAccessAsync()).Global.CanReadComponent(GlobalComponent.Report))
            {
                return this.NotFound();
            }

            var global = await GlobalDomainModel.InitializeAsync(this.serviceProvider);
            var report = await global.Reports.PrepareBatchResultReport(filterModel.From, filterModel.To);
            var result = await report.GetDataAsync();
            return this.Json(await result.ToDataSourceResultAsync(request));
        }
    }
}
