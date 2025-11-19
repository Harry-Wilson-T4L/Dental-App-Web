using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DentalDrill.CRM.Models;
using DentalDrill.CRM.Models.ViewModels;
using DentalDrill.CRM.Services;
using DevGuild.AspNetCore.Controllers.Mvc.Crud;
using DevGuild.AspNetCore.Controllers.Mvc.Crud.ActionHandlers;
using DevGuild.AspNetCore.Controllers.Mvc.KendoUI.Base;
using DevGuild.AspNetCore.Controls.HybridForms;
using DevGuild.AspNetCore.Services.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DentalDrill.CRM.Controllers
{
    [Authorize(Roles = ApplicationRoles.Combined.Staff)]
    public class ClientInvoicesController : BaseTelerikIndexlessDependentBasicCrudController<Guid, JobInvoice, Guid, Client, ClientInvoiceReadModel, Object, ClientInvoiceCreateModel, Object, Object>
    {
        private readonly UserEntityResolver userResolver;

        public ClientInvoicesController(IEntityControllerServices controllerServices, UserEntityResolver userResolver)
            : base(controllerServices)
        {
            this.userResolver = userResolver;
            this.ReadHandler.Overrides.PrepareReadQuery = this.PrepareReadQuery;
            this.ReadHandler.Overrides.ConvertEntityToViewModel = this.ConvertEntityToViewModel;

            this.CreateHandler.Overrides.InitializeCreateModel = this.InitializeCreateModel;
            this.CreateHandler.Overrides.InitializeNewEntityParent = this.InitializeNewEntityParent;
            this.CreateHandler.Overrides.InitializeNewEntity = this.InitializeNewEntity;
            this.CreateHandler.Overrides.GetCreateSuccessResult = this.GetCreateSuccessResult;
        }

        private Task<IQueryable<JobInvoice>> PrepareReadQuery(Client parent)
        {
            var query = this.ControllerServices.Repository.QueryWithoutTracking<JobInvoice>()
                .Include(x => x.Job).ThenInclude(x => x.Client)
                .Include(x => x.Job).ThenInclude(x => x.JobType)
                .Include(x => x.File)
                .Include(x => x.Employee)
                .Where(x => x.Job.ClientId == parent.Id);
            return Task.FromResult(query);
        }

        private ClientInvoiceReadModel ConvertEntityToViewModel(Client parent, JobInvoice entity, String[] allowedProperties)
        {
            return new ClientInvoiceReadModel
            {
                Id = entity.Id,
                JobId = entity.JobId,
                JobTypeName = entity.Job.JobType.Name,
                FullInvoiceNumber = $"{entity.Job.JobNumber}-{entity.InvoiceNumber}",
                FileName = entity.File.OriginalName + "." + entity.File.Extension,
                CreatedOn = entity.CreatedOn,
                EmployeeName = entity.Employee.FirstName + " " + entity.Employee.LastName,
            };
        }

        private async Task InitializeCreateModel(Client parent, ClientInvoiceCreateModel model, Boolean initial)
        {
            model.Parent = parent;
            model.Jobs = await this.ControllerServices.Repository.QueryWithoutTracking<Job>()
                .Include(x => x.JobType)
                .Where(x => x.ClientId == parent.Id)
                .OrderByDescending(x => x.Received)
                .ThenByDescending(x => x.JobNumber)
                .ToListAsync();
        }

        private Task InitializeNewEntityParent(Client parent, JobInvoice entity, ClientInvoiceCreateModel model)
        {
            return Task.CompletedTask;
        }

        private async Task InitializeNewEntity(Client parent, JobInvoice entity, ClientInvoiceCreateModel model)
        {
            var employee = (await this.userResolver.ResolveCurrentUserEntity()) as Employee;
            var job = await this.ControllerServices.Repository.QueryWithoutTracking<Job>().SingleAsync(x => x.Id == model.JobId);
            var lastInvoice = await this.ControllerServices.Repository.QueryWithoutTracking<JobInvoice>()
                .Where(x => x.JobId == job.Id)
                .OrderByDescending(x => x.InvoiceNumber)
                .FirstOrDefaultAsync();

            entity.JobId = job.Id;
            entity.FileId = model.FileId ?? throw new InvalidOperationException();
            entity.CreatedOn = DateTime.UtcNow;
            entity.EmployeeId = employee.Id;
            entity.InvoiceNumber = lastInvoice?.InvoiceNumber + 1 ?? 1;
        }

        private Task<IActionResult> GetCreateSuccessResult(Client parent, JobInvoice entity, ClientInvoiceCreateModel model, Dictionary<String, Object> additionalData)
        {
            return this.HybridFormResultAsync("ClientInvoicesCreate", this.RedirectToAction("Details", "Clients", new { id = parent.Id, Tab = "Invoices" }));
        }
    }
}
