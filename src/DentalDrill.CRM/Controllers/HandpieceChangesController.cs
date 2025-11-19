using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DentalDrill.CRM.Models;
using DentalDrill.CRM.Models.Permissions;
using DentalDrill.CRM.Models.ViewModels;
using DentalDrill.CRM.Services;
using DevGuild.AspNetCore.Controllers.Mvc.Crud;
using DevGuild.AspNetCore.Controllers.Mvc.Filters;
using DevGuild.AspNetCore.Controllers.Mvc.KendoUI.ActionHandlers;
using DevGuild.AspNetCore.Services.Data;
using Kendo.Mvc;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;

namespace DentalDrill.CRM.Controllers
{
    [Authorize(Roles = ApplicationRoles.Combined.Staff)]
    public class HandpieceChangesController : Controller
    {
        private readonly IEntityControllerServices controllerServices;
        private readonly RepairWorkflowService repairWorkflowService;

        private IDictionary<Guid, Employee> employees;
        private IDictionary<Guid, ReturnEstimate> returnEstimates;
        private IDictionary<Guid, ServiceLevel> serviceLevels;
        private PropertiesAccessControlList<Handpiece> acl;

        public HandpieceChangesController(IEntityControllerServices controllerServices, RepairWorkflowService repairWorkflowService)
        {
            this.controllerServices = controllerServices;
            this.repairWorkflowService = repairWorkflowService;

            this.IndexHandler = new TelerikCrudDependentIndexActionHandler<Guid, HandpieceChange, Guid, Handpiece, HandpieceChangeIndexViewModel>(
                this,
                controllerServices,
                new DefaultDependentEntityPermissionsValidator<HandpieceChange, Handpiece>(controllerServices.PermissionsHub, null, null, null, null));

            this.ReadHandler = new TelerikCrudAjaxDependentReadIntermediateActionHandler<Guid, HandpieceChange, Guid, Handpiece, HandpieceChangeItemViewModel, HandpieceChangeItemViewModel>(
                this,
                controllerServices,
                new DefaultDependentEntityPermissionsValidator<HandpieceChange, Handpiece>(controllerServices.PermissionsHub, null, null, null, null));

            this.IndexHandler.Overrides.InitializeIndexViewModel = this.InitializeIndexViewModel;

            this.ReadHandler.Overrides.QuerySingleParentEntity = this.QuerySingleParentEntity;
            this.ReadHandler.Overrides.PrepareReadQuery = this.PrepareReadQuery;
            this.ReadHandler.Overrides.FinalizeQueryPreparation = this.FinalizeQueryPreparation;
            this.ReadHandler.Overrides.ConvertEntityToViewModel = this.ConvertEntityToViewModel;
        }

        protected IRepository Repository => this.controllerServices.Repository;

        protected TelerikCrudDependentIndexActionHandler<Guid, HandpieceChange, Guid, Handpiece, HandpieceChangeIndexViewModel> IndexHandler { get; }

        protected TelerikCrudAjaxDependentReadIntermediateActionHandler<Guid, HandpieceChange, Guid, Handpiece, HandpieceChangeItemViewModel, HandpieceChangeItemViewModel> ReadHandler { get; }

        [AjaxGet]
        public Task<IActionResult> Index(Guid parentId) => this.IndexHandler.Index(parentId);

        [AjaxPost]
        public Task<IActionResult> Read(Guid parentId, [DataSourceRequest] DataSourceRequest request) => this.ReadHandler.Read(parentId, request);

        private Task InitializeIndexViewModel(HandpieceChangeIndexViewModel model, Handpiece entity)
        {
            model.Parent = entity;
            return Task.CompletedTask;
        }

        private Task<Handpiece> QuerySingleParentEntity(Guid id)
        {
            return this.Repository.Query<Handpiece>()
                .Include(x => x.Job)
                .SingleOrDefaultAsync(x => x.Id == id);
        }

        private async Task<IQueryable<HandpieceChange>> PrepareReadQuery(Handpiece parent)
        {
            IQueryable<HandpieceChange> query = this.Repository.Query<HandpieceChange>()
                .Where(x => x.HandpieceId == parent.Id)
                .Where(x => ((x.OldStatus == null && x.NewStatus != null) || (x.NewStatus == null && x.OldStatus != null) || (x.OldStatus != x.NewStatus)) ||
                            (x.OldContent == null && x.NewContent != null) || (x.OldContent != null && x.NewContent == null) || (x.OldContent != x.NewContent))
                .OrderByDescending(x => x.ChangedOn);

            this.employees = (await this.Repository.Query<Employee>().ToListAsync()).ToDictionary(x => x.Id);
            this.returnEstimates = (await this.Repository.Query<ReturnEstimate>().ToListAsync()).ToDictionary(x => x.Id);
            this.serviceLevels = (await this.Repository.Query<ServiceLevel>().ToListAsync()).ToDictionary(x => x.Id);
            this.acl = await this.repairWorkflowService.GetPropertiesAccessControlAsync(parent);

            return query;
        }

        private Task<IQueryable<HandpieceChangeItemViewModel>> FinalizeQueryPreparation(Handpiece parent, IQueryable<HandpieceChange> query)
        {
            var newQuery = query.Select(x => new HandpieceChangeItemViewModel
            {
                Id = x.Id,
                ChangedOn = x.ChangedOn,
                ChangedByName = x.ChangedBy.FirstName + " " + x.ChangedBy.LastName,
                OldStatus = x.OldStatus,
                NewStatus = x.NewStatus,
                OldContent = x.OldContent,
                NewContent = x.NewContent,
            });

            return Task.FromResult(newQuery);
        }

        private HandpieceChangeItemViewModel ConvertEntityToViewModel(Handpiece parent, HandpieceChangeItemViewModel entity, String[] allowedProperties)
        {
            entity.ChangedOn = DateTime.SpecifyKind(entity.ChangedOn, DateTimeKind.Utc);
            entity.OldStatusName = entity.OldStatus.HasValue ? entity.OldStatus.Value.ToDisplayString() : String.Empty;
            entity.NewStatusName = entity.NewStatus.HasValue ? entity.NewStatus.Value.ToDisplayString() : String.Empty;

            var oldContent = this.DecodeChangeDetails(entity.OldContent);
            var newContent = this.DecodeChangeDetails(entity.NewContent);

            var oldFields = new StringBuilder();
            var newFields = new StringBuilder();

            void EmitFieldChange(String aclPropertyName, String name, Func<HandpieceChangeDetails, String> expression)
            {
                var oldValue = expression(oldContent) ?? String.Empty;
                var newValue = expression(newContent) ?? String.Empty;
                if (oldValue != newValue && (aclPropertyName == null || this.acl.For(aclPropertyName).CanDisplay))
                {
                    oldFields.AppendLine($"{name}: {oldValue}");
                    newFields.AppendLine($"{name}: {newValue}");
                }
            }

            EmitFieldChange("Brand", "Brand", x => x.Brand);
            EmitFieldChange("MakeAndModel", "Model", x => x.MakeAndModel);
            EmitFieldChange("Serial", "Serial", x => x.Serial);
            EmitFieldChange("Serial", "Extra components", x => x.Components);
            EmitFieldChange(null, "Creator", x => x.Creator);
            EmitFieldChange("EstimatedById", "Estimated by", x => x.EstimatedBy);
            EmitFieldChange(null, "Repaired by", x => x.RepairedBy);
            EmitFieldChange("ProblemDescription", "Problem description", x => x.ProblemDescription);
            EmitFieldChange("DiagnosticReport", "Diagnostic report", x => x.DiagnosticReport);
            EmitFieldChange("CostOfRepair", "Cost of repair", x => x.CostOfRepair);
            EmitFieldChange("Parts", "Parts", x => x.Parts);
            EmitFieldChange("Parts", "Required parts", x => x.PartsRequired);
            EmitFieldChange("PartsOutOfStock", "Parts out of stock", x => x.PartsOutOfStock);
            EmitFieldChange("Rating", "Rating", x => x.Rating);
            EmitFieldChange("InternalComment", "Internal comment", x => x.InternalComment);
            EmitFieldChange("PublicComment", "Public comment", x => x.PublicComment);
            EmitFieldChange("ReturnById", "Return by", x => x.ReturnBy);
            EmitFieldChange("ServiceLevelId", "Service level", x => x.ServiceLevel);
            EmitFieldChange("SpeedType", "Speed type", x => x.SpeedType);
            EmitFieldChange("Speed", "Speed", x => x.Speed);

            entity.OldContent = null;
            entity.NewContent = null;
            entity.OldFields = oldFields.ToString();
            entity.NewFields = newFields.ToString();

            return entity;
        }

        private HandpieceChangeDetails DecodeChangeDetails(String content)
        {
            if (String.IsNullOrEmpty(content))
            {
                return new HandpieceChangeDetails();
            }

            try
            {
                var json = JObject.Parse(content);
                return new HandpieceChangeDetails
                {
                    Brand = TryReadStringProperty(json, "Brand"),
                    MakeAndModel = TryReadStringProperty(json, "MakeAndModel"),
                    Serial = TryReadStringProperty(json, "Serial"),
                    Components = TryReadCustom(json, "Components", token =>
                    {
                        if (token is not JArray array)
                        {
                            return null;
                        }

                        return String.Join("; ", array.Select(x =>
                        {
                            try
                            {
                                return $"{x["Brand"]} {x["Model"]} S/N {x["Serial"]}";
                            }
                            catch
                            {
                                return String.Empty;
                            }
                        }));
                    }),
                    Creator = TryReadGuidPropertyAndResolve(json, "CreatorId", this.employees)?.GetFullName(),
                    EstimatedBy = TryReadGuidPropertyAndResolve(json, "EstimatedBy", this.employees)?.GetFullName(),
                    RepairedBy = TryReadGuidPropertyAndResolve(json, "RepairedBy", this.employees)?.GetFullName(),
                    ProblemDescription = TryReadStringProperty(json, "ProblemDescription"),
                    DiagnosticReport = TryReadStringProperty(json, "DiagnosticReport"),
                    CostOfRepair = TryReadDecimalProperty(json, "CostOfRepair"),
                    Parts = TryReadStringProperty(json, "Parts"),
                    PartsRequired = TryReadCustom(json, "PartsRequired", token =>
                    {
                        if (token is not JArray array)
                        {
                            return null;
                        }

                        return String.Join("; ", array.Select(x =>
                        {
                            try
                            {
                                return $"{x["Quantity"]}x {x["SKU"]["Name"]}";
                            }
                            catch
                            {
                                return String.Empty;
                            }
                        }));
                    }),
                    PartsOutOfStock = TryReadCustom(json, "PartsOutOfStock", token => token.Type switch
                    {
                        JTokenType.Boolean => token.Value<Boolean>() switch
                        {
                            false => "No",
                            true => "Yes",
                        },
                        JTokenType.Integer => token.Value<Int32>() switch
                        {
                            0 => "No",
                            1 => "Yes",
                            2 => "Partial",
                            _ => token.Value<Int32>().ToString(),
                        },
                        JTokenType.String => token.Value<String>(),
                        _ => String.Empty,
                    }),
                    Rating = TryReadInt32Property(json, "Rating"),
                    InternalComment = TryReadStringProperty(json, "InternalComment"),
                    PublicComment = TryReadStringProperty(json, "PublicComment"),
                    ReturnBy = TryReadGuidPropertyAndResolve(json, "ReturnById", this.returnEstimates)?.Name,
                    ServiceLevel = TryReadGuidPropertyAndResolve(json, "ServiceLevelId", this.serviceLevels)?.Name,
                    SpeedType = TryReadEnumProperty<HandpieceSpeed>(json, "SpeedType"),
                    Speed = TryReadInt32Property(json, "Speed"),
                };
            }
            catch
            {
                return new HandpieceChangeDetails();
            }

            T TryReadGuidPropertyAndResolve<T>(JObject obj, String name, IDictionary<Guid, T> map) => TryReadCustomValue<String, T>(obj, name, x => map[Guid.Parse(x)]);

            String TryReadStringProperty(JObject obj, String name) => TryReadCustomValue<String, String>(obj, name, x => x);

            String TryReadBooleanProperty(JObject obj, String name)
            {
                switch (TryReadCustomValue<Boolean, Boolean?>(obj, name, x => (Boolean?)x))
                {
                    case true:
                        return "Yes";
                    case false:
                        return "No";
                    default:
                        return String.Empty;
                }
            }

            String TryReadDecimalProperty(JObject obj, String name) => TryReadCustomValue<Decimal, String>(obj, name, x => x.ToString(CultureInfo.InvariantCulture)) ?? String.Empty;

            String TryReadInt32Property(JObject obj, String name) => TryReadCustomValue<Int32, String>(obj, name, x => x.ToString()) ?? String.Empty;

            String TryReadEnumProperty<TEnum>(JObject obj, String name) => TryReadCustomValue<Int32, String>(obj, name, x => Enum.GetName(typeof(TEnum), x));

            TResult TryReadCustomValue<TValue, TResult>(JObject obj, String name, Func<TValue, TResult> resultFormatter)
            {
                try
                {
                    if (obj.ContainsKey(name) && obj[name].Type != JTokenType.Null)
                    {
                        var value = obj[name].Value<TValue>();
                        return resultFormatter(value);
                    }

                    return default(TResult);
                }
                catch
                {
                    return default(TResult);
                }
            }

            String TryReadCustom(JObject obj, String name, Func<JToken, String> formatter)
            {
                if (obj.ContainsKey(name) && obj[name].Type != JTokenType.Null)
                {
                    return formatter(obj[name]);
                }

                return null;
            }
        }
    }
}
