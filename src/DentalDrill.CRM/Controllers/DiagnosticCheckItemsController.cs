using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using DentalDrill.CRM.Extensions;
using DentalDrill.CRM.Models;
using DentalDrill.CRM.Models.ViewModels;
using DentalDrill.CRM.Services;
using DevGuild.AspNetCore.Controllers.Mvc.Crud;
using DevGuild.AspNetCore.Controllers.Mvc.KendoUI.ActionHandlers;
using DevGuild.AspNetCore.Controllers.Mvc.KendoUI.Base;
using DevGuild.AspNetCore.Services.Data;
using DevGuild.AspNetCore.Services.Permissions.Annotations;
using Kendo.Mvc;
using Kendo.Mvc.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DentalDrill.CRM.Controllers
{
    [PermissionsManager("Type", "/Domain/DiagnosticCheckItem")]
    public class DiagnosticCheckItemsController : BaseTelerikFullInlineCrudController<Guid, DiagnosticCheckItem, DiagnosticCheckItemIndexModel, DiagnosticCheckItemViewModel>
    {
        public DiagnosticCheckItemsController(IEntityControllerServices controllerServices)
            : base(controllerServices)
        {
            this.IndexHandler.Overrides.InitializeIndexViewModel = this.InitializeIndexViewModel;

            this.ReadHandler.Overrides.PreprocessRequest = this.PreprocessRequest;
            this.ReadHandler.Overrides.PrepareReadQuery = this.PrepareReadQuery;
            this.ReadHandler.Overrides.ConvertEntityToViewModel = this.ConvertEntityToViewModel;

            this.CreateHandler.Overrides.InitializeNewEntity = this.InitializeNewEntity;
            this.CreateHandler.Overrides.ConvertEntityToViewModel = entity => Task.FromResult(this.ConvertEntityToViewModel(entity, null));

            this.UpdateHandler.Overrides.QuerySingleEntity = this.QuerySingleEntity;
            this.UpdateHandler.Overrides.UpdateExistingEntity = this.UpdateExistingEntity;
            this.UpdateHandler.Overrides.ConvertEntityToViewModel = (entity) => Task.FromResult(this.ConvertEntityToViewModel(entity, null));

            this.DestroyHandler.Overrides.QuerySingleEntity = this.QuerySingleEntity;
        }

        private async Task InitializeIndexViewModel(DiagnosticCheckItemIndexModel model)
        {
            model.Types = await this.Repository.Query<DiagnosticCheckType>().ToListAsync();
            model.Access = await this.ControllerServices.ServiceProvider.GetService<UserEntityResolver>().GetEmployeeAccessAsync();
        }

        private Task PreprocessRequest(DataSourceRequest request)
        {
            if (request.Filters != null)
            {
                ProcessFilterCollection(request.Filters);
            }

            void ProcessFilterCollection(IList<IFilterDescriptor> filters)
            {
                for (var i = 0; i < filters.Count; i++)
                {
                    var filter = filters[i];
                    switch (filter)
                    {
                        case FilterDescriptor single:
                            if (single.Member == "Types" && single.Operator == FilterOperator.IsEqualTo)
                            {
                                filters[i] = new ContainItemFilterDescriptor<DiagnosticCheckItem, DiagnosticCheckItemType, Guid>(Guid.Parse(single.Value?.ToString() ?? String.Empty), x => x.TypeId, x => x.Types);
                            }

                            break;
                        case CompositeFilterDescriptor composite:
                            ProcessFilterCollection(composite.FilterDescriptors);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException(nameof(filter));
                    }
                }
            }

            return Task.CompletedTask;
        }

        private Task<IQueryable<DiagnosticCheckItem>> PrepareReadQuery()
        {
            var query = this.Repository.Query<DiagnosticCheckItem>("Types");
            query.Where(x => x.Types.Any(y => y.TypeId == Guid.Empty));
            return Task.FromResult(query);
        }

        private DiagnosticCheckItemViewModel ConvertEntityToViewModel(DiagnosticCheckItem item, String[] allowedProperties)
        {
            return new DiagnosticCheckItemViewModel
            {
                Id = item.Id,
                Types = item.Types.Select(x => x.TypeId).ToList(),
                Name = item.Name,
            };
        }

        private Task<DiagnosticCheckItem> QuerySingleEntity(Guid id)
        {
            return this.Repository.Query<DiagnosticCheckItem>("Types").SingleOrDefaultAsync(x => x.Id == id);
        }

        private async Task InitializeNewEntity(DiagnosticCheckItem entity, DiagnosticCheckItemViewModel model)
        {
            var maxOrderNo = await this.Repository.QueryWithoutTracking<DiagnosticCheckItemType>()
                .GroupBy(x => x.TypeId)
                .Select(x => new { Id = x.Key, MaxOrderNo = x.Max(y => y.OrderNo) })
                .ToListAsync();
            var nextOrderNo = maxOrderNo.ToDictionary(x => x.Id, x => x.MaxOrderNo + 1);
            var allTypes = await this.Repository.QueryWithoutTracking<DiagnosticCheckType>().ToListAsync();
            foreach (var type in allTypes)
            {
                if (!nextOrderNo.ContainsKey(type.Id))
                {
                    nextOrderNo.Add(type.Id, 1);
                }
            }

            entity.Name = model.Name;
            entity.Types = new Collection<DiagnosticCheckItemType>();
            entity.Types.SyncWithCollection(model.Types, (e, id) => e.TypeId == id, typeId => new DiagnosticCheckItemType { TypeId = typeId, OrderNo = nextOrderNo[typeId]++ });
        }

        private async Task UpdateExistingEntity(DiagnosticCheckItem entity, DiagnosticCheckItemViewModel model)
        {
            var maxOrderNo = await this.Repository.QueryWithoutTracking<DiagnosticCheckItemType>()
                .GroupBy(x => x.TypeId)
                .Select(x => new { Id = x.Key, MaxOrderNo = x.Max(y => y.OrderNo) })
                .ToListAsync();
            var nextOrderNo = maxOrderNo.ToDictionary(x => x.Id, x => x.MaxOrderNo + 1);
            var allTypes = await this.Repository.QueryWithoutTracking<DiagnosticCheckType>().ToListAsync();
            foreach (var type in allTypes)
            {
                if (!nextOrderNo.ContainsKey(type.Id))
                {
                    nextOrderNo.Add(type.Id, 1);
                }
            }

            entity.Name = model.Name;
            entity.Types.SyncWithCollection(model.Types, (e, id) => e.TypeId == id, typeId => new DiagnosticCheckItemType { ItemId = entity.Id, TypeId = typeId, OrderNo = nextOrderNo[typeId]++ });
        }

        private class ContainItemFilterDescriptor<TParent, TCollectionItem, TCollectionItemId> : IFilterDescriptor
        {
            public ContainItemFilterDescriptor(TCollectionItemId id, Expression<Func<TCollectionItem, TCollectionItemId>> idSelector, Expression<Func<TParent, ICollection<TCollectionItem>>> collectionSelector)
            {
                this.Id = id;
                this.IdSelector = idSelector;
                this.CollectionSelector = collectionSelector;
            }

            public TCollectionItemId Id { get; }

            public Expression<Func<TCollectionItem, TCollectionItemId>> IdSelector { get; }

            public Expression<Func<TParent, ICollection<TCollectionItem>>> CollectionSelector { get; }

            public Expression CreateFilterExpression(Expression instance)
            {
                if (this.CollectionSelector.Body is not MemberExpression collectionMemberExpression || collectionMemberExpression.Member is not PropertyInfo collectionProperty)
                {
                    throw new NotSupportedException("Non property expression is not supported");
                }

                if (this.IdSelector.Body is not MemberExpression idSelectorExpression || idSelectorExpression.Member is not PropertyInfo idProperty)
                {
                    throw new NotSupportedException("Non property expression is not supported");
                }

                var itemParameter = Expression.Parameter(typeof(TCollectionItem), "z");
                var itemIdExpression = Expression.Property(itemParameter, idProperty);
                var comparedIdExpression = Expression.Constant(this.Id, typeof(TCollectionItemId));

                var anyPredicateExpression = Expression.Lambda<Func<TCollectionItem, Boolean>>(
                    Expression.Equal(itemIdExpression, comparedIdExpression),
                    itemParameter);

                var instanceCollection = Expression.Property(instance, collectionProperty);
                var methodAny = typeof(System.Linq.Enumerable)
                    .GetMethods(BindingFlags.Public | BindingFlags.Static).Single(x => x.Name == "Any" && x.GetParameters().Length == 2)
                    .MakeGenericMethod(typeof(TCollectionItem));

                var methodAnyCall = Expression.Call(null, methodAny, instanceCollection, anyPredicateExpression);
                return methodAnyCall;
            }
        }
    }
}