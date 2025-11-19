using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DentalDrill.CRM.Extensions;
using DentalDrill.CRM.Models;
using DentalDrill.CRM.Models.ViewModels;
using DevGuild.AspNetCore.Controllers.Mvc.Crud;
using DevGuild.AspNetCore.Controllers.Mvc.Filters;
using DevGuild.AspNetCore.Controllers.Mvc.KendoUI.ActionHandlers;
using DevGuild.AspNetCore.Services.Data;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DentalDrill.CRM.Controllers
{
    [Authorize(Roles = ApplicationRoles.Combined.Staff)]
    public class ClientHandpiecesController : Controller
    {
        private readonly IRepository repository;

        public ClientHandpiecesController(IEntityControllerServices controllerServices, IRepository repository)
        {
            this.repository = repository;

            this.ReadHandpiecesHandler = new TelerikCrudAjaxDependentReadActionHandler<Guid, Handpiece, Guid, ClientHandpiece, HandpieceHistoryItemViewModel>(this, controllerServices, new DefaultDependentEntityPermissionsValidator<Handpiece, ClientHandpiece>(controllerServices.PermissionsHub, null, null, null, null));
            this.ReadHandpiecesHandler.Overrides.QuerySingleParentEntity = this.QuerySingleParentEntity;
            this.ReadHandpiecesHandler.Overrides.PreprocessRequest = this.PreprocessRequest;
            this.ReadHandpiecesHandler.Overrides.PrepareReadQuery = this.PrepareReadQuery;
            this.ReadHandpiecesHandler.Overrides.ConvertEntityToViewModel = this.ConvertEntityToViewModel;
        }

        protected TelerikCrudAjaxDependentReadActionHandler<Guid, Handpiece, Guid, ClientHandpiece, HandpieceHistoryItemViewModel> ReadHandpiecesHandler { get; }

        public async Task<IActionResult> MergeTool(Guid clientId)
        {
            var client = await this.repository.QueryWithoutTracking<Client>().SingleOrDefaultAsync(x => x.Id == clientId);
            if (client == null)
            {
                return this.NotFound();
            }

            return this.View(client);
        }

        [AjaxPost]
        public async Task<IActionResult> ReadExisting(Guid? clientId, [DataSourceRequest] DataSourceRequest request)
        {
            if (clientId == null)
            {
                return this.NotFound();
            }

            var client = await this.repository.QueryWithoutTracking<Client>().SingleOrDefaultAsync(x => x.Id == clientId);
            if (client == null)
            {
                return this.NotFound();
            }

            var query = this.repository.QueryWithoutTracking<ClientHandpiece>()
                .Include(x => x.Handpieces).ThenInclude(x => x.Job)
                .Include(x => x.Handpieces).ThenInclude(x => x.ServiceLevel)
                .Include(x => x.Components)
                .Where(x => x.ClientId == client.Id)
                .OrderBy(x => x.Brand)
                .ThenBy(x => x.Model)
                .ThenBy(x => x.Serial);

            var items = await query.ToListAsync();
            var itemsTransformed = items.Select(x => new ClientHandpieceExistingViewModel
            {
                Id = x.Id,
                Type = ClientHandpieceExistingType.Normal,
                Brand = x.Brand,
                Model = x.Model,
                Serial = x.Serial,
                MainText = x.Brand + " " + x.Model + " S/N " + x.Serial,
                ComponentsText = x.ComponentsText,
                Components = x.Components
                    .OrderBy(y => y.OrderNo)
                    .Select(y => new ClientHandpieceComponentExistingViewModel
                    {
                        Id = y.Id,
                        OrderNo = y.OrderNo,
                        Brand = y.Brand,
                        Model = y.Model,
                        Serial = y.Serial,
                    }).ToList(),
                LastRepairDate = x.Handpieces.OrderByDescending(y => y.Job.Received).FirstOrDefault()?.Job.Received,
                LastRepairStatus = x.Handpieces.OrderByDescending(y => y.Job.Received).FirstOrDefault()?.HandpieceStatus.ToDisplayString(),
                LastRepairServiceLevelName = x.Handpieces.OrderByDescending(y => y.Job.Received).FirstOrDefault()?.ServiceLevel?.Name,
            }).ToList();

            itemsTransformed.Insert(0, new ClientHandpieceExistingViewModel
            {
                Id = new Guid("00000000-0000-0000-0000-000000000000"),
                Type = ClientHandpieceExistingType.NewHandpiece,
                Brand = String.Empty,
                Model = String.Empty,
                Serial = String.Empty,
                MainText = "New Handpiece",
                ComponentsText = String.Empty,
                Components = new List<ClientHandpieceComponentExistingViewModel>(),
                LastRepairDate = null,
                LastRepairStatus = String.Empty,
                LastRepairServiceLevelName = String.Empty,
            });

            var result = await itemsTransformed.ToDataSourceResultAsync(request);
            return this.Json(result);
        }

        [AjaxPost]
        public Task<IActionResult> ReadHandpieces(Guid clientHandpieceId, [DataSourceRequest] DataSourceRequest request) => this.ReadHandpiecesHandler.Read(clientHandpieceId, request);

        [AjaxPost]
        public async Task<IActionResult> MoveToNew(Guid handpieceId)
        {
            var handpiece = await this.repository.Query<Handpiece>()
                .Include(x => x.Job)
                .Include(x => x.Components)
                .SingleOrDefaultAsync(x => x.Id == handpieceId);
            var sourceClientHandpiece = await this.repository.Query<ClientHandpiece>()
                .Include(x => x.Components)
                .SingleOrDefaultAsync(x => x.Id == handpiece.ClientHandpieceId);
            var remainingHandpieces = await this.repository.Query<Handpiece>()
                .Include(x => x.Job)
                .Include(x => x.Components)
                .Where(x => x.ClientHandpieceId == sourceClientHandpiece.Id && x.Id != handpieceId)
                .OrderByDescending(x => x.Job.Received)
                .ToListAsync();

            if (remainingHandpieces.Count == 0)
            {
                return this.Json(new { Success = false });
            }

            handpiece.ClientHandpiece = new ClientHandpiece
            {
                ClientId = handpiece.Job.ClientId,
                Components = new List<ClientHandpieceComponent>(),
            };

            handpiece.ClientHandpiece.UpdateFromHandpiece(handpiece);
            sourceClientHandpiece.UpdateFromHandpiece(remainingHandpieces.First());

            await this.repository.SaveChangesAsync();
            return this.Json(new { Success = true });
        }

        [AjaxPost]
        public async Task<IActionResult> MoveToExisting(Guid handpieceId, Guid clientHandpieceId)
        {
            var handpiece = await this.repository.Query<Handpiece>()
                .Include(x => x.Job)
                .Include(x => x.Components)
                .SingleOrDefaultAsync(x => x.Id == handpieceId);
            var sourceClientHandpiece = await this.repository.Query<ClientHandpiece>()
                .Include(x => x.Components)
                .SingleOrDefaultAsync(x => x.Id == handpiece.ClientHandpieceId);
            var remainingHandpieces = await this.repository.Query<Handpiece>()
                .Include(x => x.Job)
                .Include(x => x.Components)
                .Where(x => x.ClientHandpieceId == sourceClientHandpiece.Id && x.Id != handpieceId)
                .OrderByDescending(x => x.Job.Received)
                .ToListAsync();

            var destinationClientHandpiece = await this.repository.Query<ClientHandpiece>()
                .Include(x => x.Components)
                .SingleOrDefaultAsync(x => x.Id == clientHandpieceId);
            var destinationHandpieces = await this.repository.Query<Handpiece>()
                .Include(x => x.Job)
                .Include(x => x.Components)
                .Where(x => x.ClientHandpieceId == destinationClientHandpiece.Id)
                .OrderByDescending(x => x.Job.Received)
                .ToListAsync();

            handpiece.ClientHandpieceId = destinationClientHandpiece.Id;
            if (destinationHandpieces.Count == 0 || destinationHandpieces.First().Job.Received < handpiece.Job.Received)
            {
                destinationClientHandpiece.UpdateFromHandpiece(handpiece);
            }

            var refreshAll = false;
            if (remainingHandpieces.Count == 0)
            {
                await this.repository.DeleteAsync(sourceClientHandpiece);
                refreshAll = true;
            }
            else
            {
                sourceClientHandpiece.UpdateFromHandpiece(remainingHandpieces.First());
            }

            await this.repository.SaveChangesAsync();
            return this.Json(new { Success = true, RefreshAll = refreshAll });
        }

        private Task<ClientHandpiece> QuerySingleParentEntity(Guid id)
        {
            return this.repository.Query<ClientHandpiece>().SingleOrDefaultAsync(x => x.Id == id);
        }

        private Task PreprocessRequest(DataSourceRequest request)
        {
            request.RenameFields(new Dictionary<String, String>
            {
                ["JobNumber"] = "Job.Number",
                ["JobReceived"] = "Job.Received",
            });

            return Task.CompletedTask;
        }

        private Task<IQueryable<Handpiece>> PrepareReadQuery(ClientHandpiece parent)
        {
            var query = this.repository.Query<Handpiece>("Job", "Creator", "EstimatedBy", "RepairedBy")
                .Where(x => x.Job.ClientId == parent.ClientId && x.ClientHandpieceId == parent.Id);

            return Task.FromResult(query);
        }

        private HandpieceHistoryItemViewModel ConvertEntityToViewModel(ClientHandpiece parent, Handpiece entity, String[] allowedProperties)
        {
            return new HandpieceHistoryItemViewModel
            {
                Id = entity.Id,
                JobNumber = entity.Job.JobNumber,
                JobReceived = entity.Job.Received,
                CompletedOn = entity.CompletedOn,
                DiagnosticReport = entity.DiagnosticReport,
                Rating = entity.Rating,
                TechnicianName = entity.EstimatedById != null ? entity.EstimatedBy.FirstName + " " + entity.EstimatedBy.LastName : String.Empty,
                CostOfRepair = entity.CostOfRepair ?? 0m,
            };
        }
    }
}
