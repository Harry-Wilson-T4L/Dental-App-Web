using System;
using System.Linq;
using System.Threading.Tasks;
using DentalDrill.CRM.Domain.Abstractions;
using DentalDrill.CRM.Models;
using DentalDrill.CRM.Models.ViewModels;
using DevGuild.AspNetCore.Controllers.Mvc.Filters;
using DevGuild.AspNetCore.Services.Data;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DentalDrill.CRM.Controllers
{
    [Authorize(Roles = ApplicationRoles.Combined.Staff)]
    public class ClientRepairHistoryController : Controller
    {
        private readonly IClientManager clientManager;
        private readonly IRepository repository;

        public ClientRepairHistoryController(IClientManager clientManager, IRepository repository)
        {
            this.clientManager = clientManager;
            this.repository = repository;
        }

        [AjaxPost]
        public async Task<IActionResult> Read(Guid parentId, [DataSourceRequest] DataSourceRequest request)
        {
            var client = await this.clientManager.GetByIdAsync(parentId);
            if (client == null)
            {
                return this.NotFound();
            }

            var repairHistory = await client.RepairedHistory.GetAllAsync();
            var repairHistoryViewModel = repairHistory.Select(x => new ClientRepairedItemViewModel
            {
                Id = x.Id,
                ClientId = client.Id,
                ClientName = client.Entity.FullName,
                ClientEmail = client.Entity.Email,
                Brand = x.Brand,
                Model = x.Model,
                Serial = x.Serial,
                LastRepair = x.LastRepair.Number,
                LastRepairUrl = this.Url.Action("Edit", "Handpieces", new { id = x.LastRepair.Id }),
                LastRepairDate = x.LastRepair.CompletedOn ?? x.LastRepair.RepairedOn ?? x.LastRepair.ApprovedOn ?? x.LastRepair.EstimatedOn ?? x.LastRepair.ReceivedOn,
                LastRepairStatus = x.LastRepair.Status,
                RemindersLastDateTime = x.RemindersLastDateTime,
                RemindersCount = x.RemindersCount,
                TotalRemindersCount = x.TotalRemindersCount,
                Status = x.Status,
            }).ToList();

            var response = await repairHistoryViewModel.ToDataSourceResultAsync(request);
            return this.Json(response);
        }

        [AjaxPost]
        public async Task<IActionResult> Toggle(Guid parentId, [FromBody] ClientRepairedItemToggleModel model)
        {
            var client = await this.clientManager.GetByIdAsync(parentId);
            if (client == null)
            {
                return this.NotFound();
            }

            var item = await client.RepairedHistory.GetAsync(model.ClientHandpieceId);
            if (item == null)
            {
                return this.NotFound();
            }

            if (model.Disable)
            {
                await item.DisableAsync();
            }
            else
            {
                await item.EnableAsync();
            }

            await this.repository.SaveChangesAsync();
            return this.Json(new { });
        }

        [AjaxPost]
        public async Task<IActionResult> ResetCount(Guid parentId, [FromBody] ClientRepairedItemResetModel model)
        {
            var client = await this.clientManager.GetByIdAsync(parentId);
            if (client == null)
            {
                return this.NotFound();
            }

            var item = await client.RepairedHistory.GetAsync(model.ClientHandpieceId);
            if (item == null)
            {
                return this.NotFound();
            }

            await item.ResetRemindersCountAsync();
            await this.repository.SaveChangesAsync();
            return this.Json(new { });
        }
    }
}
