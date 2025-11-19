using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DentalDrill.CRM.Domain.Abstractions;
using DentalDrill.CRM.Models;
using DentalDrill.CRM.Models.ViewModels;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DentalDrill.CRM.Controllers
{
    [Authorize(Roles = ApplicationRoles.Combined.Administrator)]
    public class MaintenanceReminderController : Controller
    {
        private readonly IClientManager clientManager;

        public MaintenanceReminderController(IClientManager clientManager)
        {
            this.clientManager = clientManager;
        }

        public async Task<IActionResult> Index()
        {
            return this.View();
        }

        public async Task<IActionResult> Pending()
        {
            return this.View();
        }

        public async Task<IActionResult> ReadPending([DataSourceRequest] DataSourceRequest request)
        {
            var reminders = await this.clientManager.RepairHistory.GetPendingRemindersAsync();
            var items = new List<ClientRepairedItemViewModel>();
            foreach (var reminder in reminders)
            {
                foreach (var item in reminder.ItemsUpForService)
                {
                    items.Add(new ClientRepairedItemViewModel
                    {
                        Id = item.Id,
                        ClientId = reminder.Client.Id,
                        ClientName = reminder.Client.Entity.FullName,
                        ClientEmail = reminder.Client.Entity.Email,
                        Brand = item.Brand,
                        Model = item.Model,
                        Serial = item.Serial,
                        LastRepair = item.LastRepair.Number,
                        LastRepairUrl = this.Url.Action("Edit", "Handpieces", new { id = item.LastRepair.Id }),
                        LastRepairDate = item.LastRepair.CompletedOn ?? item.LastRepair.RepairedOn ?? item.LastRepair.ApprovedOn ?? item.LastRepair.EstimatedOn ?? item.LastRepair.ReceivedOn,
                        LastRepairStatus = item.LastRepair.Status,
                        RemindersLastDateTime = item.RemindersLastDateTime,
                        RemindersCount = item.RemindersCount,
                        TotalRemindersCount = item.TotalRemindersCount,
                        Status = item.Status,
                    });
                }
            }

            var result = await items.ToDataSourceResultAsync(request);
            return this.Json(result);
        }

        public async Task<IActionResult> ReadRecent([DataSourceRequest] DataSourceRequest request)
        {
            var reminders = await this.clientManager.RepairHistory.GetAllHistoryAsync();
            var items = new List<ClientRepairedItemViewModel>();
            foreach (var reminder in reminders)
            {
                foreach (var item in reminder.ItemsUpForService)
                {
                    items.Add(new ClientRepairedItemViewModel
                    {
                        Id = item.Id,
                        ClientId = reminder.Client.Id,
                        ClientName = reminder.Client.Entity.FullName,
                        ClientEmail = reminder.Client.Entity.Email,
                        Brand = item.Brand,
                        Model = item.Model,
                        Serial = item.Serial,
                        LastRepair = item.LastRepair.Number,
                        LastRepairUrl = this.Url.Action("Edit", "Handpieces", new { id = item.LastRepair.Id }),
                        LastRepairDate = item.LastRepair.CompletedOn ?? item.LastRepair.RepairedOn ?? item.LastRepair.ApprovedOn ?? item.LastRepair.EstimatedOn ?? item.LastRepair.ReceivedOn,
                        LastRepairStatus = item.LastRepair.Status,
                        RemindersLastDateTime = item.RemindersLastDateTime,
                        RemindersCount = item.RemindersCount,
                        TotalRemindersCount = item.TotalRemindersCount,
                        Status = item.Status,
                    });
                }
            }

            var result = await items.ToDataSourceResultAsync(request);
            return this.Json(result);
        }
    }
}
