using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DentalDrill.CRM.Extensions;
using DentalDrill.CRM.Models;
using DentalDrill.CRM.Models.ViewModels;
using DentalDrill.CRM.Services;
using DevGuild.AspNetCore.Controllers.Mvc.Crud;
using DevGuild.AspNetCore.Controllers.Mvc.KendoUI.Base;
using DevGuild.AspNetCore.Services.Data;
using DevGuild.AspNetCore.Services.Permissions.Annotations;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DentalDrill.CRM.Controllers
{
    [PermissionsManager("Type", "/Domain/CalendarWeek")]
    [PermissionsManager("Entity", "/Domain/CalendarWeek/Entities/{entity}")]
    public class CalendarController : BaseTelerikFullInlineCrudController<Guid, CalendarWeek, EmptyIndexViewModel, CalendarWeekViewModel>
    {
        private readonly CalendarService calendarService;

        public CalendarController(IEntityControllerServices controllerServices, CalendarService calendarService)
            : base(controllerServices)
        {
            this.calendarService = calendarService;

            this.ReadHandler.Overrides.PreprocessRequest = this.PreprocessRequest;
            this.ReadHandler.Overrides.PrepareReadQuery = this.PrepareReadQuery;
            this.ReadHandler.Overrides.ConvertEntityToViewModel = this.ConvertEntityToViewModel;
        }

        public override Task<IActionResult> Create(DataSourceRequest request, CalendarWeekViewModel model) => throw new NotSupportedException();

        public override Task<IActionResult> Update(DataSourceRequest request, CalendarWeekViewModel model) => throw new NotSupportedException();

        public override Task<IActionResult> Destroy(DataSourceRequest request, CalendarWeekViewModel model) => throw new NotSupportedException();

        [HttpPost]
        public async Task<IActionResult> PreviousWeek(Guid id)
        {
            var week = await this.calendarService.GetWeekAsync(id);
            if (week == null)
            {
                return this.NotFound();
            }

            var previousWeek = await this.calendarService.GetPreviousWeekAsync(week);
            if (previousWeek == null)
            {
                return this.Json(null);
            }

            return this.Json(new
            {
                weekId = previousWeek.Id,
                weekName = $"Week {previousWeek.Week} ({previousWeek.RangeStart:MMM dd} - {previousWeek.RangeEnd.AddDays(-1):MMM dd})",
                hasPrevious = await this.calendarService.GetPreviousWeekAsync(previousWeek) != null,
                hasNext = true,
            });
        }

        [HttpPost]
        public async Task<IActionResult> NextWeek(Guid id)
        {
            var week = await this.calendarService.GetWeekAsync(id);
            if (week == null)
            {
                return this.NotFound();
            }

            var nextWeek = await this.calendarService.GetNextWeekAsync(week);
            if (nextWeek == null)
            {
                return this.Json(null);
            }

            return this.Json(new
            {
                weekId = nextWeek.Id,
                weekName = $"Week {nextWeek.Week} ({nextWeek.RangeStart:MMM dd} - {nextWeek.RangeEnd.AddDays(-1):MMM dd})",
                hasPrevious = true,
                hasNext = await this.calendarService.GetNextWeekAsync(nextWeek) != null,
            });
        }

        private Task PreprocessRequest(DataSourceRequest request)
        {
            request.RenameFields(new Dictionary<String, String>
            {
                ["Year"] = "Year.Year",
            });

            return Task.CompletedTask;
        }

        private Task<IQueryable<CalendarWeek>> PrepareReadQuery()
        {
            return Task.FromResult(this.Repository.Query<CalendarWeek>("Year"));
        }

        private CalendarWeekViewModel ConvertEntityToViewModel(CalendarWeek entity, String[] allowedProperties)
        {
            return new CalendarWeekViewModel
            {
                Id = entity.Id,
                Year = entity.Year.Year,
                Week = entity.Week,
                RangeStart = entity.RangeStart.UtcDateTime,
                RangeEnd = entity.RangeEnd.UtcDateTime,
            };
        }
    }
}
