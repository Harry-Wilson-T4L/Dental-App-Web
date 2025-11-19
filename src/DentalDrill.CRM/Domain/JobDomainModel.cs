using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DentalDrill.CRM.Domain.Abstractions;
using DentalDrill.CRM.Models;
using DevGuild.AspNetCore.Services.Data;
using Microsoft.EntityFrameworkCore;

namespace DentalDrill.CRM.Domain
{
    public class JobDomainModel : IJob
    {
        private readonly Job job;
        private readonly IWorkshop workshop;
        private readonly IClient client;
        private readonly IJobType jobType;

        private readonly IRepository repository;

        private readonly Lazy<IHandpieceCollection> handpieces;

        public JobDomainModel(
            Job job,
            IWorkshop workshop,
            IClient client,
            IJobType jobType,
            IRepository repository,
            IHandpieceLoader handpieceLoader)
        {
            this.job = job;
            this.workshop = workshop;
            this.client = client;
            this.jobType = jobType;

            this.repository = repository;

            this.handpieces = new Lazy<IHandpieceCollection>(() => new HandpieceCollection(this, repository, handpieceLoader));
        }

        public Guid Id => this.job.Id;

        public IWorkshop Workshop => this.workshop;

        public IClient Client => this.client;

        public IJobType JobType => this.jobType;

        public Int64 JobNumber => this.job.JobNumber;

        public DateTime Received => this.job.Received;

        public String Comment => this.job.Comment;

        public JobStatus Status => this.job.Status;

        public Boolean HasWarning => this.job.HasWarning;

        public String ApprovedBy => this.job.ApprovedBy;

        public DateTime? ApprovedOn => this.job.ApprovedOn;

        public JobRatePlan RatePlan => this.job.RatePlan;

        public IHandpieceCollection Handpieces => this.handpieces.Value;

        public async Task ChangeClientAsync(IClient client)
        {
            var destinationClientHandpieces = await this.repository.Query<ClientHandpiece>()
                .Include(x => x.Components)
                .Where(x => x.ClientId == client.Id)
                .ToListAsync();

            var movedHandpieces = await this.repository.Query<Handpiece>()
                .Include(x => x.Components)
                .Where(x => x.JobId == this.Id)
                .ToListAsync();

            foreach (var movedHandpiece in movedHandpieces)
            {
                var match = destinationClientHandpieces.Where(x => x.MatchesHandpiece(movedHandpiece)).MinBy(x => x.Id);
                if (match == null)
                {
                    match = new ClientHandpiece
                    {
                        ClientId = client.Id,
                        Components = new List<ClientHandpieceComponent>(),
                    };

                    match.UpdateFromHandpiece(movedHandpiece);

                    await this.repository.InsertAsync(match);
                    destinationClientHandpieces.Add(match);
                    movedHandpiece.ClientHandpieceId = match.Id;
                }
                else
                {
                    movedHandpiece.ClientHandpieceId = match.Id;
                }
            }

            this.job.ClientId = client.Id;
            await this.repository.SaveChangesAsync();

            var orphanedClientHandpieces = await this.repository.Query<ClientHandpiece>()
                .Include(x => x.Handpieces)
                .Where(x => x.ClientId == this.Client.Id && x.Handpieces.Count == 0)
                .ToListAsync();
            foreach (var orphanedClientHandpiece in orphanedClientHandpieces)
            {
                await this.repository.DeleteAsync(orphanedClientHandpiece);
            }

            await this.repository.SaveChangesAsync();
        }

        public async Task ChangeWorkshopAsync(IWorkshop workshop)
        {
            var savedParts = new List<(Guid HandpieceId, IInventorySKU SKU, Decimal Quantity)>();

            var allHandpieces = await this.Handpieces.FindManyAsync();
            foreach (var handpiece in allHandpieces)
            {
                var allParts = await handpiece.Parts.GetRequiredPartsAsync();
                foreach (var part in allParts)
                {
                    savedParts.Add((handpiece.Id, part.SKU, part.Quantity));
                    await part.DeleteAsync();
                }
            }

            foreach (var sku in savedParts.Select(x => x.SKU).DistinctBy(x => x.Id))
            {
                await sku.TryProcessMovementsChangesAsync(this.Workshop);
            }

            await this.repository.SaveChangesAsync();

            this.job.WorkshopId = workshop.Id;
            await this.repository.SaveChangesAsync();

            var migratedHandpieces = await this.Handpieces.FindManyAsync();
            foreach (var handpiece in migratedHandpieces)
            {
                foreach (var savedPart in savedParts.Where(x => x.HandpieceId == handpiece.Id))
                {
                    await handpiece.Parts.AddRequiredPartAsync(savedPart.SKU, savedPart.Quantity);
                }
            }

            foreach (var sku in savedParts.Select(x => x.SKU).DistinctBy(x => x.Id))
            {
                await sku.TryProcessMovementsChangesAsync(workshop);
            }

            await this.repository.SaveChangesAsync();
        }
    }
}
