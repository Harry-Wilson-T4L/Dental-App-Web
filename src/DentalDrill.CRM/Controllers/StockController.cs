using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DentalDrill.CRM.Models;
using DentalDrill.CRM.Models.ViewModels;
using DentalDrill.CRM.Services;
using DevGuild.AspNetCore.Controllers.Mvc.Crud;
using DevGuild.AspNetCore.Controllers.Mvc.Filters;
using DevGuild.AspNetCore.Services.Data;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DentalDrill.CRM.Controllers
{
    [Authorize(Roles = ApplicationRoles.Combined.Staff)]
    public class StockController : Controller
    {
        private readonly IEntityControllerServices controllerServices;
        private readonly CalendarService calendarService;
        private readonly ILogger logger;

        public StockController(IEntityControllerServices controllerServices, CalendarService calendarService, ILogger<StockController> logger)
        {
            this.controllerServices = controllerServices;
            this.calendarService = calendarService;
            this.logger = logger;
        }

        protected IRepository Repository => this.controllerServices.Repository;

        public Task<IActionResult> Index()
        {
            return Task.FromResult<IActionResult>(this.View());
        }

        public async Task<IActionResult> Restock()
        {
            var week = await this.calendarService.GetCurrentWeekAsync();
            var model = new StockControlByWeekViewModel
            {
                Week = week,
                PreviousWeek = await this.calendarService.GetPreviousWeekAsync(week),
                NextWeek = await this.calendarService.GetNextWeekAsync(week),
            };

            return this.View(model);
        }

        [AjaxPost]
        public async Task<IActionResult> ReadHandpiecesBeingRepaired([DataSourceRequest] DataSourceRequest request)
        {
            var handpieces = this.Repository.Query<Handpiece>()
                .Where(x => x.PartsOutOfStock != HandpiecePartsStockStatus.InStock)
                .Where(x => (x.HandpieceStatus == HandpieceStatus.BeingRepaired || x.HandpieceStatus == HandpieceStatus.WaitingForParts))
                .Select(x => new
                {
                    Id = x.Id,
                    JobId = x.JobId,
                    JobNumber = x.Job.JobNumber,
                    Brand = x.Brand,
                    Model = x.MakeAndModel,
                    ServiceLevel = x.ServiceLevelId != null ? x.ServiceLevel.Name : "N/A",
                    PartsComment = x.PartsComment,
                    Amount = x.CostOfRepair,
                    Ordered = x.PartsOrdered,
                });

            var result = await handpieces.ToDataSourceResultAsync(request);
            return this.Json(result);
        }

        [AjaxPost]
        public async Task<IActionResult> ReadHandpiecesWaitingApproval([DataSourceRequest] DataSourceRequest request)
        {
            var handpieces = this.Repository.Query<Handpiece>()
                .Where(x => x.PartsOutOfStock != HandpiecePartsStockStatus.InStock)
                .Where(x => x.HandpieceStatus == HandpieceStatus.WaitingForApproval || x.HandpieceStatus == HandpieceStatus.EstimateSent || x.HandpieceStatus == HandpieceStatus.NeedsReApproval)
                .Select(x => new
                {
                    Id = x.Id,
                    JobId = x.JobId,
                    JobNumber = x.Job.JobNumber,
                    Brand = x.Brand,
                    Model = x.MakeAndModel,
                    ServiceLevel = x.ServiceLevelId != null ? x.ServiceLevel.Name : "N/A",
                    PartsComment = x.PartsComment,
                    Amount = x.CostOfRepair,
                    Ordered = x.PartsOrdered,
                });

            var result = await handpieces.ToDataSourceResultAsync(request);
            return this.Json(result);
        }

        [AjaxPost]
        public async Task<IActionResult> ReadHandpiecesWithPartsOut([DataSourceRequest] DataSourceRequest request, Guid weekId)
        {
            var week = await this.calendarService.GetWeekAsync(weekId);
            if (week == null)
            {
                return this.NotFound();
            }

            var entries = this.Repository.Query<StockControlEntry>()
                .Where(x => x.WeekId == week.Id && x.Handpiece.PartsOrdered == false)
                .Select(x => new
                {
                    Id = x.Id,
                    CompletedAt = x.CompletedAt,
                    JobId = x.Handpiece.JobId,
                    JobNumber = x.Handpiece.Job.JobNumber,
                    HandpieceId = x.HandpieceId,
                    Brand = x.Handpiece.Brand,
                    Model = x.Handpiece.MakeAndModel,
                    ServiceLevel = x.Handpiece.ServiceLevelId != null ? x.Handpiece.ServiceLevel.Name : "N/A",
                    PartsComment = x.Handpiece.PartsComment,
                    Amount = x.Handpiece.CostOfRepair,
                    Ordered = x.Status == StockControlEntryStatus.Ordered,
                    Ignored = x.Status == StockControlEntryStatus.Ignored,
                });

            var result = await entries.ToDataSourceResultAsync(request, x => new
            {
                Id = x.Id,
                CompletedAt = x.CompletedAt.UtcDateTime,
                JobId = x.JobId,
                JobNumber = x.JobNumber,
                HandpieceId = x.HandpieceId,
                Brand = x.Brand,
                Model = x.Model,
                ServiceLevel = x.ServiceLevel,
                PartsComment = x.PartsComment,
                Amount = x.Amount,
                Ordered = x.Ordered,
                Ignored = x.Ignored,
            });
            return this.Json(result);
        }

        [HttpPost]
        [Authorize(Roles = ApplicationRoles.Combined.Administrator)]
        public async Task<IActionResult> UpdateOrderedStatus(Guid id, Boolean ordered)
        {
            var handpiece = await this.Repository.Query<Handpiece>().SingleOrDefaultAsync(x => x.Id == id);
            if (handpiece == null)
            {
                return this.NotFound();
            }

            this.logger.LogInformation($"Changing ordered status of Handpiece('{handpiece.Id}') to {ordered}");
            handpiece.PartsOrdered = ordered;
            await this.Repository.UpdateAsync(handpiece);
            await this.Repository.SaveChangesAsync();

            return this.NoContent();
        }

        [HttpPost]
        [Authorize(Roles = ApplicationRoles.Combined.Administrator)]
        public async Task<IActionResult> UpdateStatus(Guid id, StockControlEntryStatus status, Guid? weekId)
        {
            var currentWeek = await this.calendarService.GetCurrentWeekAsync();
            var selectedWeek = weekId.HasValue ? await this.calendarService.GetWeekAsync(weekId.Value) : null;
            var entry = await this.Repository.Query<StockControlEntry>("Week", "Handpiece", "Handpiece.Job", "Handpiece.ServiceLevel")
                .SingleOrDefaultAsync(x => x.Id == id);

            if (entry == null)
            {
                return this.NotFound();
            }

            switch (status)
            {
                case StockControlEntryStatus.Active:
                    entry.Status = status;
                    if (selectedWeek != null)
                    {
                        entry.WeekId = selectedWeek.Id;
                    }
                    else if (entry.Week.RangeStart < currentWeek.RangeStart)
                    {
                        entry.WeekId = currentWeek.Id;
                    }

                    if (entry.Handpiece.PartsRestocked)
                    {
                        entry.Handpiece.PartsRestocked = false;
                    }

                    break;
                case StockControlEntryStatus.Ignored:
                    entry.Status = status;
                    if (selectedWeek != null)
                    {
                        entry.WeekId = selectedWeek.Id;
                    }

                    if (entry.Handpiece.PartsRestocked)
                    {
                        entry.Handpiece.PartsRestocked = false;
                    }

                    break;
                case StockControlEntryStatus.Ordered:
                    entry.Status = status;
                    if (selectedWeek != null)
                    {
                        entry.WeekId = selectedWeek.Id;
                    }

                    if (!entry.Handpiece.PartsRestocked)
                    {
                        entry.Handpiece.PartsRestocked = true;
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(status), status, null);
            }

            this.logger.LogInformation($"Changing status of StockControlEntry('{entry.Id}') to {status}");
            await this.Repository.UpdateAsync(entry);
            await this.Repository.SaveChangesAsync();
            return this.Json(new
            {
                Id = entry.Id,
                CompletedAt = entry.CompletedAt.UtcDateTime,
                JobId = entry.Handpiece.JobId,
                JobNumber = entry.Handpiece.Job.JobNumber,
                HandpieceId = entry.HandpieceId,
                Brand = entry.Handpiece.Brand,
                Model = entry.Handpiece.MakeAndModel,
                ServiceLevel = entry.Handpiece.ServiceLevelId != null ? entry.Handpiece.ServiceLevel.Name : "N/A",
                PartsComment = entry.Handpiece.PartsComment,
                Amount = entry.Handpiece.CostOfRepair,
                Ordered = entry.Status == StockControlEntryStatus.Ordered,
                Ignored = entry.Status == StockControlEntryStatus.Ignored,
            });
        }
    }
}
