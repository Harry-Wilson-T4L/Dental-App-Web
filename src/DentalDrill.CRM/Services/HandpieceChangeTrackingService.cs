using System;
using System.Linq;
using System.Threading.Tasks;
using DentalDrill.CRM.Models;
using DevGuild.AspNetCore.Services.Data;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace DentalDrill.CRM.Services
{
    public class HandpieceChangeTrackingService : ChangeTrackingService<Handpiece, Guid, HandpieceChange>
    {
        public HandpieceChangeTrackingService(IRepositoryFactory repositoryFactory, UserEntityResolver userResolver, IRepository repository)
            : base(repositoryFactory, userResolver, repository)
        {
        }

        protected override Task FillOldVersion(HandpieceChange change, Handpiece entity)
        {
            change.OldStatus = entity.HandpieceStatus;
            change.OldContent = this.CaptureContent(entity);
            return Task.CompletedTask;
        }

        protected override Task FillNewVersion(HandpieceChange change, Handpiece entity)
        {
            change.NewStatus = entity.HandpieceStatus;
            change.NewContent = this.CaptureContent(entity);
            return Task.CompletedTask;
        }

        protected override async Task<Handpiece> ReloadEntity(Handpiece entity)
        {
            var reloaded = await this.Repository.QueryWithoutTracking<Handpiece>()
                .Include(x => x.SelectedDiagnosticCheckItems)
                .ThenInclude(x => x.Item)
                .Include(x => x.PartsRequired)
                .ThenInclude(x => x.SKU)
                .Include(x => x.Components)
                .SingleOrDefaultAsync(x => x.Id == entity.Id);

            if (reloaded == null)
            {
                throw new InvalidOperationException("Unable to reload the entity");
            }

            return reloaded;
        }

        protected override Task<HandpieceChange> CreateChange(Handpiece entity)
        {
            var change = new HandpieceChange
            {
                HandpieceId = entity.Id,
            };

            return Task.FromResult(change);
        }

        private String CaptureContent(Handpiece handpiece)
        {
            return JsonConvert.SerializeObject(new
            {
                Id = handpiece.Id,
                JobPosition = handpiece.JobPosition,

                HandpieceStatus = handpiece.HandpieceStatus,
                Brand = handpiece.Brand,
                MakeAndModel = handpiece.MakeAndModel,
                Serial = handpiece.Serial,
                CreatorId = handpiece.CreatorId,
                EstimatedById = handpiece.EstimatedById,
                RepairedById = handpiece.RepairedById,
                ProblemDescription = handpiece.ProblemDescription,

                Components = handpiece.Components
                    .OrderBy(x => x.OrderNo)
                    .Select(x => new
                    {
                        x.Brand,
                        x.Model,
                        x.Serial,
                    })
                    .ToArray(),
                ComponentsText = handpiece.ComponentsText,

                DiagnosticReport = handpiece.DiagnosticReport,
                DiagnosticOther = handpiece.DiagnosticOther,
                SelectedDiagnosticCheckItems = handpiece.SelectedDiagnosticCheckItems
                    .OrderBy(x => x.OrderNo)
                    .Select(diagnostic => new
                    {
                        diagnostic.Item.Id,
                        diagnostic.Item.Name,
                    }).ToArray(),

                CostOfRepair = handpiece.CostOfRepair,
                Parts = handpiece.Parts,
                PartsRequired = handpiece.PartsRequired.Select(pr => new
                {
                    SKU = new
                    {
                        Id = pr.SKU.Id,
                        Name = pr.SKU.Name,
                    },
                    Quantity = pr.Quantity,
                }),
                PartsOutOfStock = handpiece.PartsOutOfStock,
                Rating = handpiece.Rating,

                InternalComment = handpiece.InternalComment,
                PublicComment = handpiece.PublicComment,
                ReturnById = handpiece.ReturnById,
                ServiceLevelId = handpiece.ServiceLevelId,
                SpeedType = handpiece.SpeedType,
                Speed = handpiece.Speed,
            });
        }
    }
}