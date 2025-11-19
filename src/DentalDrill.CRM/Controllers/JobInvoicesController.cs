using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DentalDrill.CRM.Domain.Abstractions;
using DentalDrill.CRM.Extensions;
using DentalDrill.CRM.Models;
using DentalDrill.CRM.Models.Permissions;
using DentalDrill.CRM.Models.ViewModels;
using DentalDrill.CRM.Services;
using DentalDrill.CRM.Services.Permissions;
using DevGuild.AspNetCore.Controllers.Mvc.Crud;
using DevGuild.AspNetCore.Controllers.Mvc.KendoUI.Base;
using DevGuild.AspNetCore.Controls.HybridForms;
using DevGuild.AspNetCore.Services.Data;
using DevGuild.AspNetCore.Services.Permissions;
using DevGuild.AspNetCore.Services.Storage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DentalDrill.CRM.Controllers
{
    [Authorize(Roles = ApplicationRoles.Combined.Staff)]
    public class JobInvoicesController : BaseTelerikIndexlessDependentBasicCrudController<Guid, JobInvoice, Guid, Job, JobInvoiceReadModel, JobInvoice, JobInvoiceCreateModel, Object, JobInvoice>
    {
        private readonly IStorageHub storageHub;
        private readonly UserEntityResolver userResolver;

        public JobInvoicesController(IEntityControllerServices controllerServices, IStorageHub storageHub, UserEntityResolver userResolver)
            : base(controllerServices)
        {
            this.storageHub = storageHub;
            this.userResolver = userResolver;
            this.ReadHandler.Overrides.PrepareReadQuery = this.PrepareReadQuery;
            this.ReadHandler.Overrides.ConvertEntityToViewModel = this.ConvertEntityToViewModel;

            this.CreateHandler.Overrides.QuerySingleParentEntity = this.QuerySingleParentEntity;
            this.CreateHandler.Overrides.InitializeCreateModel = this.InitializeCreateModel;
            this.CreateHandler.Overrides.InitializeNewEntity = this.InitializeNewEntity;
            this.CreateHandler.Overrides.GetCreateSuccessResult = this.GetCreateSuccessResult;

            this.DeleteHandler.Overrides.QuerySingleEntity = this.QuerySingleEntity;
            this.DeleteHandler.Overrides.GetDeleteSuccessResult = this.GetDeleteSuccessResult;
        }

        public async Task<IActionResult> Download(Guid id)
        {
            var entity = await this.Repository.Query<JobInvoice>("File").SingleOrDefaultAsync(x => x.Id == id);
            if (entity == null)
            {
                return this.NotFound();
            }

            var container = this.storageHub.GetContainer(entity.File.Container);
            var path = entity.File.GetContainerPath();

            return this.File(await container.GetFileContentAsync(path), "application/pdf", $"{entity.File.OriginalName}.{entity.File.Extension}");
        }

        [NonAction]
        public override Task<IActionResult> Details(Guid id)
        {
            throw new NotSupportedException();
        }

        [NonAction]
        public override Task<IActionResult> Edit(Guid id)
        {
            throw new NotSupportedException();
        }

        [NonAction]
        public override Task<IActionResult> Edit(Guid id, Object model)
        {
            throw new NotSupportedException();
        }

        protected override IEntityPermissionsValidator<JobInvoice> GetEntityPermissionsValidator()
        {
            return new CustomPermissionValidator(
                this.ControllerServices.PermissionsHub,
                this.ControllerServices.ServiceProvider.GetRequiredService<UserEntityResolver>());
        }

        protected override IDependentEntityPermissionsValidator<JobInvoice, Job> GetDependentEntityPermissionsValidator()
        {
            return new CustomPermissionValidator(
                this.ControllerServices.PermissionsHub,
                this.ControllerServices.ServiceProvider.GetRequiredService<UserEntityResolver>());
        }

        private Task<IQueryable<JobInvoice>> PrepareReadQuery(Job parent)
        {
            var query = this.Repository.QueryWithoutTracking<JobInvoice>("Job", "File", "Employee").Where(x => x.JobId == parent.Id);
            return Task.FromResult(query);
        }

        private JobInvoiceReadModel ConvertEntityToViewModel(Job parent, JobInvoice entity, String[] allowedProperties)
        {
            return new JobInvoiceReadModel
            {
                Id = entity.Id,
                JobId = entity.JobId,
                FullInvoiceNumber = $"{entity.Job.JobNumber}-{entity.InvoiceNumber}",
                FileName = entity.File.OriginalName + "." + entity.File.Extension,
                CreatedOn = DateTime.SpecifyKind(entity.CreatedOn, DateTimeKind.Utc),
                EmployeeName = entity.Employee.FirstName + " " + entity.Employee.LastName,
            };
        }

        private Task<JobInvoice> QuerySingleEntity(Guid id)
        {
            return this.Repository.Query<JobInvoice>("Job", "File", "Employee").SingleOrDefaultAsync(x => x.Id == id);
        }

        private Task<Job> QuerySingleParentEntity(Guid id)
        {
            return this.Repository.Query<Job>("Invoices").SingleOrDefaultAsync(x => x.Id == id);
        }

        private Task InitializeCreateModel(Job parent, JobInvoiceCreateModel model, Boolean initial)
        {
            model.Parent = parent;
            return Task.CompletedTask;
        }

        private async Task InitializeNewEntity(Job parent, JobInvoice entity, JobInvoiceCreateModel model)
        {
            var employee = (await this.userResolver.ResolveCurrentUserEntity()) as Employee;

            entity.FileId = model.FileId ?? throw new InvalidOperationException();
            entity.CreatedOn = DateTime.UtcNow;
            entity.EmployeeId = employee.Id;
            entity.InvoiceNumber = parent.Invoices.Any() ? parent.Invoices.Max(x => x.InvoiceNumber) + 1 : 1;
        }

        private Task<IActionResult> GetCreateSuccessResult(Job parent, JobInvoice entity, JobInvoiceCreateModel model, Dictionary<String, Object> additionalData)
        {
            return this.HybridFormResultAsync("JobInvoicesCreate", this.RedirectToAction("Edit", "Jobs", new { id = parent.Id, Tab = "Invoices" }));
        }

        private Task<IActionResult> GetDeleteSuccessResult(JobInvoice entity, Dictionary<String, Object> additionalData)
        {
            return this.HybridFormResultAsync("JobInvoicesDelete", this.RedirectToAction("Edit", "Jobs", new { id = entity.JobId, Tab = "Invoices" }));
        }

        private class CustomPermissionValidator : DependentPlusEntityPermissionsValidatorBase<JobInvoice, Job>
        {
            private readonly UserEntityResolver userEntityResolver;
            private IEmployeeAccess access;

            public CustomPermissionValidator(IPermissionsHub permissionsHub, UserEntityResolver userEntityResolver)
                : base(permissionsHub)
            {
                this.userEntityResolver = userEntityResolver;
            }

            public override async Task<Boolean> CanIndexAsync()
            {
                var employeeAccess = await this.ResolveAccessAsync();
                return employeeAccess.Workshops
                    .GetAll()
                    .Any(x => x.JobTypes
                        .GetAll()
                        .Any(y => y.CanReadJobComponent(JobEntityComponent.Invoice)));
            }

            public override async Task<Boolean> CanIndexAsync(Job parentEntity)
            {
                var employeeAccess = await this.ResolveAccessAsync();
                return employeeAccess
                    .Workshops[parentEntity.WorkshopId]
                    .JobTypes[parentEntity.JobTypeId]
                    .CanReadJobComponent(JobEntityComponent.Invoice);
            }

            public override async Task<Boolean> CanCreateAsync()
            {
                var employeeAccess = await this.ResolveAccessAsync();
                return employeeAccess.Workshops
                    .GetAll()
                    .Any(x => x.JobTypes
                        .GetAll()
                        .Any(y => y.CanWriteJobComponent(JobEntityComponent.Invoice)));
            }

            public override async Task<Boolean> CanCreateAsync(Job parentEntity)
            {
                var employeeAccess = await this.ResolveAccessAsync();
                return employeeAccess
                    .Workshops[parentEntity.WorkshopId]
                    .JobTypes[parentEntity.JobTypeId]
                    .CanWriteJobComponent(JobEntityComponent.Invoice);
            }

            public override async Task<Boolean> CanDetailsAsync(JobInvoice entity)
            {
                var employeeAccess = await this.ResolveAccessAsync();
                return employeeAccess
                    .Workshops[entity.Job.WorkshopId]
                    .JobTypes[entity.Job.JobTypeId]
                    .CanReadJobComponent(JobEntityComponent.Invoice);
            }

            public override async Task<Boolean> CanEditAsync(JobInvoice entity)
            {
                var employeeAccess = await this.ResolveAccessAsync();
                return employeeAccess
                    .Workshops[entity.Job.WorkshopId]
                    .JobTypes[entity.Job.JobTypeId]
                    .CanWriteJobComponent(JobEntityComponent.Invoice);
            }

            public override async Task<Boolean> CanDeleteAsync(JobInvoice entity)
            {
                var employeeAccess = await this.ResolveAccessAsync();
                return employeeAccess
                    .Workshops[entity.Job.WorkshopId]
                    .JobTypes[entity.Job.JobTypeId]
                    .CanWriteJobComponent(JobEntityComponent.Invoice);
            }

            public override async Task<IQueryable<JobInvoice>> RequireReadAccessAsync(IQueryable<JobInvoice> query)
            {
                var employeeAccess = await this.ResolveAccessAsync();
                var predicates = new List<Expression<Func<JobInvoice, Boolean>>>();
                foreach (var workshop in employeeAccess.Workshops.GetAll())
                {
                    foreach (var jobType in workshop.JobTypes.GetAll())
                    {
                        if (jobType.CanReadJobComponent(JobEntityComponent.Invoice))
                        {
                            predicates.Add(x => x.Job.WorkshopId == workshop.WorkshopId && x.Job.JobTypeId == jobType.JobTypeId);
                        }
                    }
                }

                if (predicates.Count == 0)
                {
                    return query.Where(x => false);
                }

                return query.WhereOneOf(predicates);
            }

            public override async Task<IQueryable<JobInvoice>> RequireReadAccessAsync(Job parent, IQueryable<JobInvoice> query)
            {
                var employeeAccess = await this.ResolveAccessAsync();
                if (employeeAccess.Workshops[parent.WorkshopId].JobTypes[parent.JobTypeId].CanReadJobComponent(JobEntityComponent.Invoice))
                {
                    return query;
                }

                return query.Where(x => false);
            }

            private Task<IEmployeeAccess> ResolveAccessAsync()
            {
                if (this.access != null)
                {
                    return Task.FromResult(this.access);
                }

                async Task<IEmployeeAccess> LoadAccessAsync()
                {
                    this.access = await this.userEntityResolver.GetEmployeeAccessAsync();
                    return this.access;
                }

                return LoadAccessAsync();
            }
        }
    }
}
