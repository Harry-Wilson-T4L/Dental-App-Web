using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DentalDrill.CRM.Data;
using DentalDrill.CRM.Models.ViewModels;
using DentalDrill.CRM.Models.ViewModels.Reports.Corporate;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DentalDrill.CRM.Domain.Reporting
{
    public class CorporateBrandsReportsDomainModel
    {
        private readonly IServiceProvider serviceProvider;
        private readonly CorporateDomainModel corporate;

        private readonly DateTime from;
        private readonly DateTime to;
        private readonly IReadOnlyList<Guid> clients;

        private IQueryable<CorporateSurgeryReportSurgeryItem> query;

        public CorporateBrandsReportsDomainModel(IServiceProvider serviceProvider, CorporateDomainModel corporate, DateTime from, DateTime to, IReadOnlyList<Guid> clients)
        {
            this.serviceProvider = serviceProvider;
            this.corporate = corporate;
            this.from = from;
            this.to = to;
            this.clients = clients;
        }

        public Task PrepareQuery()
        {
            var dbContext = this.serviceProvider.GetService<ApplicationDbContext>();
            var queryText = new StringBuilder();
            queryText.Append($@"select
  [Jobs].[ClientId] as [ClientId],
  [Clients].[Name] as [ClientName],
  [Handpieces].[Brand] as [Brand],
  [Handpieces].[MakeAndModel] as [Model],
  datepart(year, [Jobs].[Received]) as [Year],
  datepart(quarter, [Jobs].[Received]) as [Quarter],
  datepart(month, [Jobs].[Received]) as [Month],
  datepart(week, [Jobs].[Received]) as [Week],
  [Jobs].[Received] as [Date],
  COALESCE([Handpieces].[CostOfRepair], 0.00) as [Cost],
  CAST([Handpieces].[Rating] as decimal(18,2)) as [Rating],
  IIF ([Handpieces].[HandpieceStatus] = 71, 1, 0) as [Unrepaired],
  1 as [Handpieces]
from [Handpieces]
  inner join [Jobs] on [Handpieces].[JobId] = [Jobs].[Id]
  inner join [Clients] on [Jobs].[ClientId] = [Clients].[Id]
where [Clients].[CorporateId] = @corporateId
  and [Jobs].[Received] between @from and @to
  and [Jobs].[ClientId] in ({String.Join(", ", this.clients.Select((x, i) => $"@client{i}"))})");

            var queryParameters = new List<Object>();
            queryParameters.Add(new SqlParameter("corporateId", SqlDbType.UniqueIdentifier) { Value = this.corporate.Id });
            queryParameters.Add(new SqlParameter("from", SqlDbType.Date) { Value = this.from });
            queryParameters.Add(new SqlParameter("to", SqlDbType.Date) { Value = this.to });
            for (var i = 0; i < this.clients.Count; i++)
            {
                queryParameters.Add(new SqlParameter($"client{i}", SqlDbType.UniqueIdentifier) { Value = this.clients[i] });
            }

            this.query = dbContext.Set<CorporateSurgeryReportSurgeryItem>().FromSqlRaw(queryText.ToString(), queryParameters.ToArray());
            return Task.CompletedTask;
        }

        public async Task<List<CorporateSurgeryReportSurgeryEntireItem>> GetSurgeriesForEntirePeriodAsync()
        {
            if (this.query == null)
            {
                await this.PrepareQuery();
            }

            var items = await this.query
                .GroupBy(x => new { x.ClientId, x.ClientName })
                .Select(x => new
                {
                    ClientId = x.Key.ClientId,
                    ClientName = x.Key.ClientName,
                    RatingAverage = x.Average(y => y.Rating),
                    CostSum = x.Sum(y => y.Cost),
                    CostAverage = x.Average(y => y.Cost),
                    HandpiecesCount = x.Sum(y => y.Handpieces),
                    UnrepairedCount = x.Sum(y => y.Unrepaired),
                })
                .ToListAsync();

            return items
                .Select(x => new CorporateSurgeryReportSurgeryEntireItem
                {
                    ClientId = x.ClientId,
                    ClientName = x.ClientName,
                    RatingAverage = x.RatingAverage,
                    CostSum = x.CostSum,
                    CostAverage = x.CostAverage,
                    UnrepairedPercent = (Decimal)x.UnrepairedCount / x.HandpiecesCount,
                    HandpiecesCount = x.HandpiecesCount,
                })
                .ToList();
        }

        public async Task<List<CorporateSurgeryReportSurgeryAggregateItem>> GetSurgeriesAggregatedAsync(ReportDateAggregate dateAggregate)
        {
            if (this.query == null)
            {
                await this.PrepareQuery();
            }

            switch (dateAggregate)
            {
                case ReportDateAggregate.Yearly:
                {
                    var items = await this.query
                        .GroupBy(x => new { x.ClientId, x.ClientName, x.Year })
                        .Select(x => new
                        {
                            ClientId = x.Key.ClientId,
                            ClientName = x.Key.ClientName,
                            Year = x.Key.Year,
                            RatingAverage = x.Average(y => y.Rating),
                            CostSum = x.Sum(y => y.Cost),
                            CostAverage = x.Average(y => y.Cost),
                            HandpiecesCount = x.Sum(y => y.Handpieces),
                            UnrepairedCount = x.Sum(y => y.Unrepaired),
                        })
                        .ToListAsync();

                    return items.GroupBy(x => new { x.ClientId, x.ClientName })
                        .Select(x => new CorporateSurgeryReportSurgeryAggregateItem
                        {
                            ClientId = x.Key.ClientId,
                            ClientName = x.Key.ClientName,
                            RatingAverage = x.ToDictionary(y => y.Year.ToString(), y => y.RatingAverage),
                            CostSum = x.ToDictionary(y => y.Year.ToString(), y => y.CostSum),
                            CostAverage = x.ToDictionary(y => y.Year.ToString(), y => y.CostAverage),
                            UnrepairedPercent = x.ToDictionary(y => y.Year.ToString(), y => (Decimal)y.UnrepairedCount / y.HandpiecesCount),
                            HandpiecesCount = x.ToDictionary(y => y.Year.ToString(), y => y.HandpiecesCount),
                        })
                        .ToList();
                }

                case ReportDateAggregate.Quarterly:
                {
                    var items = await this.query
                        .GroupBy(x => new { x.ClientId, x.ClientName, x.Year, x.Quarter })
                        .Select(x => new
                        {
                            ClientId = x.Key.ClientId,
                            ClientName = x.Key.ClientName,
                            Year = x.Key.Year,
                            Quarter = x.Key.Quarter,
                            RatingAverage = x.Average(y => y.Rating),
                            CostSum = x.Sum(y => y.Cost),
                            CostAverage = x.Average(y => y.Cost),
                            HandpiecesCount = x.Sum(y => y.Handpieces),
                            UnrepairedCount = x.Sum(y => y.Unrepaired),
                        })
                        .ToListAsync();

                    return items.GroupBy(x => new { x.ClientId, x.ClientName })
                        .Select(x => new CorporateSurgeryReportSurgeryAggregateItem
                        {
                            ClientId = x.Key.ClientId,
                            ClientName = x.Key.ClientName,
                            RatingAverage = x.ToDictionary(y => y.Year.ToString() + "-Q" + y.Quarter.ToString(), y => y.RatingAverage),
                            CostSum = x.ToDictionary(y => y.Year.ToString() + "-Q" + y.Quarter.ToString(), y => y.CostSum),
                            CostAverage = x.ToDictionary(y => y.Year.ToString() + "-Q" + y.Quarter.ToString(), y => y.CostAverage),
                            UnrepairedPercent = x.ToDictionary(y => y.Year.ToString() + "-Q" + y.Quarter.ToString(), y => (Decimal)y.UnrepairedCount / y.HandpiecesCount),
                            HandpiecesCount = x.ToDictionary(y => y.Year.ToString() + "-Q" + y.Quarter.ToString(), y => y.HandpiecesCount),
                        })
                        .ToList();
                }

                case ReportDateAggregate.Monthly:
                {
                    var items = await this.query
                        .GroupBy(x => new { x.ClientId, x.ClientName, x.Year, x.Quarter, x.Month })
                        .Select(x => new
                        {
                            ClientId = x.Key.ClientId,
                            ClientName = x.Key.ClientName,
                            Year = x.Key.Year,
                            Quarter = x.Key.Quarter,
                            Month = x.Key.Month,
                            RatingAverage = x.Average(y => y.Rating),
                            CostSum = x.Sum(y => y.Cost),
                            CostAverage = x.Average(y => y.Cost),
                            HandpiecesCount = x.Sum(y => y.Handpieces),
                            UnrepairedCount = x.Sum(y => y.Unrepaired),
                        })
                        .ToListAsync();

                    return items.GroupBy(x => new { x.ClientId, x.ClientName })
                        .Select(x => new CorporateSurgeryReportSurgeryAggregateItem
                        {
                            ClientId = x.Key.ClientId,
                            ClientName = x.Key.ClientName,
                            RatingAverage = x.ToDictionary(y => y.Year.ToString() + "-" + y.Month.ToString(), y => y.RatingAverage),
                            CostSum = x.ToDictionary(y => y.Year.ToString() + "-" + y.Month.ToString(), y => y.CostSum),
                            CostAverage = x.ToDictionary(y => y.Year.ToString() + "-" + y.Month.ToString(), y => y.CostAverage),
                            UnrepairedPercent = x.ToDictionary(y => y.Year.ToString() + "-" + y.Month.ToString(), y => (Decimal)y.UnrepairedCount / y.HandpiecesCount),
                            HandpiecesCount = x.ToDictionary(y => y.Year.ToString() + "-" + y.Month.ToString(), y => y.HandpiecesCount),
                        })
                        .ToList();
                }

                case ReportDateAggregate.Weekly:
                {
                    var items = await this.query
                        .GroupBy(x => new { x.ClientId, x.ClientName, x.Year, x.Week })
                        .Select(x => new
                        {
                            ClientId = x.Key.ClientId,
                            ClientName = x.Key.ClientName,
                            Year = x.Key.Year,
                            Week = x.Key.Week,
                            RatingAverage = x.Average(y => y.Rating),
                            CostSum = x.Sum(y => y.Cost),
                            CostAverage = x.Average(y => y.Cost),
                            HandpiecesCount = x.Sum(y => y.Handpieces),
                            UnrepairedCount = x.Sum(y => y.Unrepaired),
                        })
                        .ToListAsync();

                    return items.GroupBy(x => new { x.ClientId, x.ClientName })
                        .Select(x => new CorporateSurgeryReportSurgeryAggregateItem
                        {
                            ClientId = x.Key.ClientId,
                            ClientName = x.Key.ClientName,
                            RatingAverage = x.ToDictionary(y => y.Year.ToString() + "-W" + y.Week.ToString(), y => y.RatingAverage),
                            CostSum = x.ToDictionary(y => y.Year.ToString() + "-W" + y.Week.ToString(), y => y.CostSum),
                            CostAverage = x.ToDictionary(y => y.Year.ToString() + "-W" + y.Week.ToString(), y => y.CostAverage),
                            UnrepairedPercent = x.ToDictionary(y => y.Year.ToString() + "-W" + y.Week.ToString(), y => (Decimal)y.UnrepairedCount / y.HandpiecesCount),
                            HandpiecesCount = x.ToDictionary(y => y.Year.ToString() + "-W" + y.Week.ToString(), y => y.HandpiecesCount),
                        })
                        .ToList();
                }

                case ReportDateAggregate.Daily:
                {
                    var items = await this.query
                        .GroupBy(x => new { x.ClientId, x.ClientName, x.Year, x.Quarter, x.Month, x.Date })
                        .Select(x => new
                        {
                            ClientId = x.Key.ClientId,
                            ClientName = x.Key.ClientName,
                            Year = x.Key.Year,
                            Quarter = x.Key.Quarter,
                            Month = x.Key.Month,
                            Date = x.Key.Date,
                            RatingAverage = x.Average(y => y.Rating),
                            CostSum = x.Sum(y => y.Cost),
                            CostAverage = x.Average(y => y.Cost),
                            HandpiecesCount = x.Sum(y => y.Handpieces),
                            UnrepairedCount = x.Sum(y => y.Unrepaired),
                        })
                        .ToListAsync();

                    return items.GroupBy(x => new { x.ClientId, x.ClientName })
                        .Select(x => new CorporateSurgeryReportSurgeryAggregateItem
                        {
                            ClientId = x.Key.ClientId,
                            ClientName = x.Key.ClientName,
                            RatingAverage = x.ToDictionary(y => y.Date.ToString("yyyy-MM-dd"), y => y.RatingAverage),
                            CostSum = x.ToDictionary(y => y.Date.ToString("yyyy-MM-dd"), y => y.CostSum),
                            CostAverage = x.ToDictionary(y => y.Date.ToString("yyyy-MM-dd"), y => y.CostAverage),
                            UnrepairedPercent = x.ToDictionary(y => y.Date.ToString("yyyy-MM-dd"), y => (Decimal)y.UnrepairedCount / y.HandpiecesCount),
                            HandpiecesCount = x.ToDictionary(y => y.Date.ToString("yyyy-MM-dd"), y => y.HandpiecesCount),
                        })
                        .ToList();
                }

                default:
                    throw new ArgumentOutOfRangeException(nameof(dateAggregate), dateAggregate, null);
            }
        }

        public async Task<List<CorporateSurgeryReportSurgeryModelEntireItem>> GetSurgeriesAndModelsForEntirePeriodAsync()
        {
            if (this.query == null)
            {
                await this.PrepareQuery();
            }

            var items = await this.query
                .GroupBy(x => new { x.ClientId, x.ClientName, x.Brand, x.Model })
                .Select(x => new
                {
                    ClientId = x.Key.ClientId,
                    ClientName = x.Key.ClientName,
                    Brand = x.Key.Brand,
                    Model = x.Key.Model,
                    RatingAverage = Math.Round(x.Average(y => y.Rating), 2),
                    CostSum = x.Sum(y => y.Cost),
                    CostAverage = x.Average(y => y.Cost),
                    HandpiecesCount = x.Sum(y => y.Handpieces),
                    UnrepairedCount = x.Sum(y => y.Unrepaired),
                })
                .ToListAsync();

            return items
                .Select(x => new CorporateSurgeryReportSurgeryModelEntireItem
                {
                    ClientId = x.ClientId,
                    ClientName = x.ClientName,
                    Brand = x.Brand,
                    Model = x.Model,
                    RatingAverage = x.RatingAverage,
                    CostSum = x.CostSum,
                    CostAverage = x.CostAverage,
                    HandpiecesCount = x.HandpiecesCount,
                    UnrepairedPercent = (Decimal)x.UnrepairedCount / x.HandpiecesCount,
                })
                .ToList();
        }

        public async Task<List<CorporateSurgeryReportSurgeryModelAggregateItem>> GetSurgeriesAndModelsAggregatedAsync(ReportDateAggregate dateAggregate)
        {
            if (this.query == null)
            {
                await this.PrepareQuery();
            }

            switch (dateAggregate)
            {
                case ReportDateAggregate.Yearly:
                {
                    var items = await this.query
                        .GroupBy(x => new { x.ClientId, x.ClientName, x.Brand, x.Model, x.Year })
                        .Select(x => new
                        {
                            ClientId = x.Key.ClientId,
                            ClientName = x.Key.ClientName,
                            Brand = x.Key.Brand,
                            Model = x.Key.Model,
                            Year = x.Key.Year,
                            RatingAverage = x.Average(y => y.Rating),
                            CostSum = x.Sum(y => y.Cost),
                            CostAverage = x.Average(y => y.Cost),
                            HandpiecesCount = x.Sum(y => y.Handpieces),
                            UnrepairedCount = x.Sum(y => y.Unrepaired),
                        })
                        .ToListAsync();

                    return items.GroupBy(x => new { x.ClientId, x.ClientName, x.Brand, x.Model })
                        .Select(x => new CorporateSurgeryReportSurgeryModelAggregateItem
                        {
                            ClientId = x.Key.ClientId,
                            ClientName = x.Key.ClientName,
                            Brand = x.Key.Brand,
                            Model = x.Key.Model,
                            RatingAverage = x.ToDictionary(y => y.Year.ToString(), y => y.RatingAverage),
                            CostSum = x.ToDictionary(y => y.Year.ToString(), y => y.CostSum),
                            CostAverage = x.ToDictionary(y => y.Year.ToString(), y => y.CostAverage),
                            UnrepairedPercent = x.ToDictionary(y => y.Year.ToString(), y => (Decimal)y.UnrepairedCount / y.HandpiecesCount),
                            HandpiecesCount = x.ToDictionary(y => y.Year.ToString(), y => y.HandpiecesCount),
                        })
                        .ToList();
                }

                case ReportDateAggregate.Quarterly:
                {
                    var items = await this.query
                        .GroupBy(x => new { x.ClientId, x.ClientName, x.Brand, x.Model, x.Year, x.Quarter })
                        .Select(x => new
                        {
                            ClientId = x.Key.ClientId,
                            ClientName = x.Key.ClientName,
                            Brand = x.Key.Brand,
                            Model = x.Key.Model,
                            Year = x.Key.Year,
                            Quarter = x.Key.Quarter,
                            RatingAverage = x.Average(y => y.Rating),
                            CostSum = x.Sum(y => y.Cost),
                            CostAverage = x.Average(y => y.Cost),
                            HandpiecesCount = x.Sum(y => y.Handpieces),
                            UnrepairedCount = x.Sum(y => y.Unrepaired),
                        })
                        .ToListAsync();

                    return items.GroupBy(x => new { x.ClientId, x.ClientName, x.Brand, x.Model })
                        .Select(x => new CorporateSurgeryReportSurgeryModelAggregateItem
                        {
                            ClientId = x.Key.ClientId,
                            ClientName = x.Key.ClientName,
                            Brand = x.Key.Brand,
                            Model = x.Key.Model,
                            RatingAverage = x.ToDictionary(y => y.Year.ToString() + "-Q" + y.Quarter.ToString(), y => y.RatingAverage),
                            CostSum = x.ToDictionary(y => y.Year.ToString() + "-Q" + y.Quarter.ToString(), y => y.CostSum),
                            CostAverage = x.ToDictionary(y => y.Year.ToString() + "-Q" + y.Quarter.ToString(), y => y.CostAverage),
                            UnrepairedPercent = x.ToDictionary(y => y.Year.ToString() + "-Q" + y.Quarter.ToString(), y => (Decimal)y.UnrepairedCount / y.HandpiecesCount),
                            HandpiecesCount = x.ToDictionary(y => y.Year.ToString() + "-Q" + y.Quarter.ToString(), y => y.HandpiecesCount),
                        })
                        .ToList();
                }

                case ReportDateAggregate.Monthly:
                {
                    var items = await this.query
                        .GroupBy(x => new { x.ClientId, x.ClientName, x.Brand, x.Model, x.Year, x.Quarter, x.Month })
                        .Select(x => new
                        {
                            ClientId = x.Key.ClientId,
                            ClientName = x.Key.ClientName,
                            Brand = x.Key.Brand,
                            Model = x.Key.Model,
                            Year = x.Key.Year,
                            Quarter = x.Key.Quarter,
                            Month = x.Key.Month,
                            RatingAverage = x.Average(y => y.Rating),
                            CostSum = x.Sum(y => y.Cost),
                            CostAverage = x.Average(y => y.Cost),
                            HandpiecesCount = x.Sum(y => y.Handpieces),
                            UnrepairedCount = x.Sum(y => y.Unrepaired),
                        })
                        .ToListAsync();

                    return items.GroupBy(x => new { x.ClientId, x.ClientName, x.Brand, x.Model })
                        .Select(x => new CorporateSurgeryReportSurgeryModelAggregateItem
                        {
                            ClientId = x.Key.ClientId,
                            ClientName = x.Key.ClientName,
                            Brand = x.Key.Brand,
                            Model = x.Key.Model,
                            RatingAverage = x.ToDictionary(y => y.Year.ToString() + "-" + y.Month.ToString(), y => y.RatingAverage),
                            CostSum = x.ToDictionary(y => y.Year.ToString() + "-" + y.Month.ToString(), y => y.CostSum),
                            CostAverage = x.ToDictionary(y => y.Year.ToString() + "-" + y.Month.ToString(), y => y.CostAverage),
                            UnrepairedPercent = x.ToDictionary(y => y.Year.ToString() + "-" + y.Month.ToString(), y => (Decimal)y.UnrepairedCount / y.HandpiecesCount),
                            HandpiecesCount = x.ToDictionary(y => y.Year.ToString() + "-" + y.Month.ToString(), y => y.HandpiecesCount),
                        })
                        .ToList();
                }

                case ReportDateAggregate.Weekly:
                {
                    var items = await this.query
                        .GroupBy(x => new { x.ClientId, x.ClientName, x.Brand, x.Model, x.Year, x.Week })
                        .Select(x => new
                        {
                            ClientId = x.Key.ClientId,
                            ClientName = x.Key.ClientName,
                            Brand = x.Key.Brand,
                            Model = x.Key.Model,
                            Year = x.Key.Year,
                            Week = x.Key.Week,
                            RatingAverage = x.Average(y => y.Rating),
                            CostSum = x.Sum(y => y.Cost),
                            CostAverage = x.Average(y => y.Cost),
                            HandpiecesCount = x.Sum(y => y.Handpieces),
                            UnrepairedCount = x.Sum(y => y.Unrepaired),
                        })
                        .ToListAsync();

                    return items.GroupBy(x => new { x.ClientId, x.ClientName, x.Brand, x.Model })
                        .Select(x => new CorporateSurgeryReportSurgeryModelAggregateItem
                        {
                            ClientId = x.Key.ClientId,
                            ClientName = x.Key.ClientName,
                            Brand = x.Key.Brand,
                            Model = x.Key.Model,
                            RatingAverage = x.ToDictionary(y => y.Year.ToString() + "-W" + y.Week.ToString(), y => y.RatingAverage),
                            CostSum = x.ToDictionary(y => y.Year.ToString() + "-W" + y.Week.ToString(), y => y.CostSum),
                            CostAverage = x.ToDictionary(y => y.Year.ToString() + "-W" + y.Week.ToString(), y => y.CostAverage),
                            UnrepairedPercent = x.ToDictionary(y => y.Year.ToString() + "-W" + y.Week.ToString(), y => (Decimal)y.UnrepairedCount / y.HandpiecesCount),
                            HandpiecesCount = x.ToDictionary(y => y.Year.ToString() + "-W" + y.Week.ToString(), y => y.HandpiecesCount),
                        })
                        .ToList();
                }

                case ReportDateAggregate.Daily:
                {
                    var items = await this.query
                        .GroupBy(x => new { x.ClientId, x.ClientName, x.Brand, x.Model, x.Year, x.Quarter, x.Month, x.Date })
                        .Select(x => new
                        {
                            ClientId = x.Key.ClientId,
                            ClientName = x.Key.ClientName,
                            Brand = x.Key.Brand,
                            Model = x.Key.Model,
                            Year = x.Key.Year,
                            Quarter = x.Key.Quarter,
                            Month = x.Key.Month,
                            Date = x.Key.Date,
                            RatingAverage = x.Average(y => y.Rating),
                            CostSum = x.Sum(y => y.Cost),
                            CostAverage = x.Average(y => y.Cost),
                            HandpiecesCount = x.Sum(y => y.Handpieces),
                            UnrepairedCount = x.Sum(y => y.Unrepaired),
                        })
                        .ToListAsync();

                    return items.GroupBy(x => new { x.ClientId, x.ClientName, x.Brand, x.Model })
                        .Select(x => new CorporateSurgeryReportSurgeryModelAggregateItem
                        {
                            ClientId = x.Key.ClientId,
                            ClientName = x.Key.ClientName,
                            Brand = x.Key.Brand,
                            Model = x.Key.Model,
                            RatingAverage = x.ToDictionary(y => y.Date.ToString("yyyy-MM-dd"), y => y.RatingAverage),
                            CostSum = x.ToDictionary(y => y.Date.ToString("yyyy-MM-dd"), y => y.CostSum),
                            CostAverage = x.ToDictionary(y => y.Date.ToString("yyyy-MM-dd"), y => y.CostAverage),
                            UnrepairedPercent = x.ToDictionary(y => y.Date.ToString("yyyy-MM-dd"), y => (Decimal)y.UnrepairedCount / y.HandpiecesCount),
                            HandpiecesCount = x.ToDictionary(y => y.Date.ToString("yyyy-MM-dd"), y => y.HandpiecesCount),
                        })
                        .ToList();
                }

                default:
                    throw new ArgumentOutOfRangeException(nameof(dateAggregate), dateAggregate, null);
            }
        }

        public async Task<List<CorporateSurgeryReportSurgeryBrandEntireItem>> GetSurgeriesAndBrandsForEntirePeriodAsync()
        {
            if (this.query == null)
            {
                await this.PrepareQuery();
            }

            var items = await this.query
                .GroupBy(x => new { x.ClientId, x.ClientName, x.Brand })
                .Select(x => new
                {
                    ClientId = x.Key.ClientId,
                    ClientName = x.Key.ClientName,
                    Brand = x.Key.Brand,
                    RatingAverage = Math.Round(x.Average(y => y.Rating), 2),
                    CostSum = x.Sum(y => y.Cost),
                    CostAverage = x.Average(y => y.Cost),
                    HandpiecesCount = x.Sum(y => y.Handpieces),
                    UnrepairedCount = x.Sum(y => y.Unrepaired),
                })
                .ToListAsync();

            return items
                .Select(x => new CorporateSurgeryReportSurgeryBrandEntireItem
                {
                    ClientId = x.ClientId,
                    ClientName = x.ClientName,
                    Brand = x.Brand,
                    RatingAverage = x.RatingAverage,
                    CostSum = x.CostSum,
                    CostAverage = x.CostAverage,
                    HandpiecesCount = x.HandpiecesCount,
                    UnrepairedPercent = (Decimal)x.UnrepairedCount / x.HandpiecesCount,
                })
                .ToList();
        }

        public async Task<List<CorporateSurgeryReportSurgeryBrandAggregateItem>> GetSurgeriesAndBrandsAggregatedAsync(ReportDateAggregate dateAggregate)
        {
            if (this.query == null)
            {
                await this.PrepareQuery();
            }

            switch (dateAggregate)
            {
                case ReportDateAggregate.Yearly:
                {
                    var items = await this.query
                        .GroupBy(x => new { x.ClientId, x.ClientName, x.Brand, x.Year })
                        .Select(x => new
                        {
                            ClientId = x.Key.ClientId,
                            ClientName = x.Key.ClientName,
                            Brand = x.Key.Brand,
                            Year = x.Key.Year,
                            RatingAverage = x.Average(y => y.Rating),
                            CostSum = x.Sum(y => y.Cost),
                            CostAverage = x.Average(y => y.Cost),
                            HandpiecesCount = x.Sum(y => y.Handpieces),
                            UnrepairedCount = x.Sum(y => y.Unrepaired),
                        })
                        .ToListAsync();

                    return items.GroupBy(x => new { x.ClientId, x.ClientName, x.Brand })
                        .Select(x => new CorporateSurgeryReportSurgeryBrandAggregateItem
                        {
                            ClientId = x.Key.ClientId,
                            ClientName = x.Key.ClientName,
                            Brand = x.Key.Brand,
                            RatingAverage = x.ToDictionary(y => y.Year.ToString(), y => y.RatingAverage),
                            CostSum = x.ToDictionary(y => y.Year.ToString(), y => y.CostSum),
                            CostAverage = x.ToDictionary(y => y.Year.ToString(), y => y.CostAverage),
                            UnrepairedPercent = x.ToDictionary(y => y.Year.ToString(), y => (Decimal)y.UnrepairedCount / y.HandpiecesCount),
                            HandpiecesCount = x.ToDictionary(y => y.Year.ToString(), y => y.HandpiecesCount),
                        })
                        .ToList();
                }

                case ReportDateAggregate.Quarterly:
                {
                    var items = await this.query
                        .GroupBy(x => new { x.ClientId, x.ClientName, x.Brand, x.Year, x.Quarter })
                        .Select(x => new
                        {
                            ClientId = x.Key.ClientId,
                            ClientName = x.Key.ClientName,
                            Brand = x.Key.Brand,
                            Year = x.Key.Year,
                            Quarter = x.Key.Quarter,
                            RatingAverage = x.Average(y => y.Rating),
                            CostSum = x.Sum(y => y.Cost),
                            CostAverage = x.Average(y => y.Cost),
                            HandpiecesCount = x.Sum(y => y.Handpieces),
                            UnrepairedCount = x.Sum(y => y.Unrepaired),
                        })
                        .ToListAsync();

                    return items.GroupBy(x => new { x.ClientId, x.ClientName, x.Brand })
                        .Select(x => new CorporateSurgeryReportSurgeryBrandAggregateItem
                        {
                            ClientId = x.Key.ClientId,
                            ClientName = x.Key.ClientName,
                            Brand = x.Key.Brand,
                            RatingAverage = x.ToDictionary(y => y.Year.ToString() + "-Q" + y.Quarter.ToString(), y => y.RatingAverage),
                            CostSum = x.ToDictionary(y => y.Year.ToString() + "-Q" + y.Quarter.ToString(), y => y.CostSum),
                            CostAverage = x.ToDictionary(y => y.Year.ToString() + "-Q" + y.Quarter.ToString(), y => y.CostAverage),
                            UnrepairedPercent = x.ToDictionary(y => y.Year.ToString() + "-Q" + y.Quarter.ToString(), y => (Decimal)y.UnrepairedCount / y.HandpiecesCount),
                            HandpiecesCount = x.ToDictionary(y => y.Year.ToString() + "-Q" + y.Quarter.ToString(), y => y.HandpiecesCount),
                        })
                        .ToList();
                }

                case ReportDateAggregate.Monthly:
                {
                    var items = await this.query
                        .GroupBy(x => new { x.ClientId, x.ClientName, x.Brand, x.Year, x.Quarter, x.Month })
                        .Select(x => new
                        {
                            ClientId = x.Key.ClientId,
                            ClientName = x.Key.ClientName,
                            Brand = x.Key.Brand,
                            Year = x.Key.Year,
                            Quarter = x.Key.Quarter,
                            Month = x.Key.Month,
                            RatingAverage = x.Average(y => y.Rating),
                            CostSum = x.Sum(y => y.Cost),
                            CostAverage = x.Average(y => y.Cost),
                            HandpiecesCount = x.Sum(y => y.Handpieces),
                            UnrepairedCount = x.Sum(y => y.Unrepaired),
                        })
                        .ToListAsync();

                    return items.GroupBy(x => new { x.ClientId, x.ClientName, x.Brand })
                        .Select(x => new CorporateSurgeryReportSurgeryBrandAggregateItem
                        {
                            ClientId = x.Key.ClientId,
                            ClientName = x.Key.ClientName,
                            Brand = x.Key.Brand,
                            RatingAverage = x.ToDictionary(y => y.Year.ToString() + "-" + y.Month.ToString(), y => y.RatingAverage),
                            CostSum = x.ToDictionary(y => y.Year.ToString() + "-" + y.Month.ToString(), y => y.CostSum),
                            CostAverage = x.ToDictionary(y => y.Year.ToString() + "-" + y.Month.ToString(), y => y.CostAverage),
                            UnrepairedPercent = x.ToDictionary(y => y.Year.ToString() + "-" + y.Month.ToString(), y => (Decimal)y.UnrepairedCount / y.HandpiecesCount),
                            HandpiecesCount = x.ToDictionary(y => y.Year.ToString() + "-" + y.Month.ToString(), y => y.HandpiecesCount),
                        })
                        .ToList();
                }

                case ReportDateAggregate.Weekly:
                {
                    var items = await this.query
                        .GroupBy(x => new { x.ClientId, x.ClientName, x.Brand, x.Year, x.Week })
                        .Select(x => new
                        {
                            ClientId = x.Key.ClientId,
                            ClientName = x.Key.ClientName,
                            Brand = x.Key.Brand,
                            Year = x.Key.Year,
                            Week = x.Key.Week,
                            RatingAverage = x.Average(y => y.Rating),
                            CostSum = x.Sum(y => y.Cost),
                            CostAverage = x.Average(y => y.Cost),
                            HandpiecesCount = x.Sum(y => y.Handpieces),
                            UnrepairedCount = x.Sum(y => y.Unrepaired),
                        })
                        .ToListAsync();

                    return items.GroupBy(x => new { x.ClientId, x.ClientName, x.Brand })
                        .Select(x => new CorporateSurgeryReportSurgeryBrandAggregateItem
                        {
                            ClientId = x.Key.ClientId,
                            ClientName = x.Key.ClientName,
                            Brand = x.Key.Brand,
                            RatingAverage = x.ToDictionary(y => y.Year.ToString() + "-W" + y.Week.ToString(), y => y.RatingAverage),
                            CostSum = x.ToDictionary(y => y.Year.ToString() + "-W" + y.Week.ToString(), y => y.CostSum),
                            CostAverage = x.ToDictionary(y => y.Year.ToString() + "-W" + y.Week.ToString(), y => y.CostAverage),
                            UnrepairedPercent = x.ToDictionary(y => y.Year.ToString() + "-W" + y.Week.ToString(), y => (Decimal)y.UnrepairedCount / y.HandpiecesCount),
                            HandpiecesCount = x.ToDictionary(y => y.Year.ToString() + "-W" + y.Week.ToString(), y => y.HandpiecesCount),
                        })
                        .ToList();
                }

                case ReportDateAggregate.Daily:
                {
                    var items = await this.query
                        .GroupBy(x => new { x.ClientId, x.ClientName, x.Brand, x.Year, x.Quarter, x.Month, x.Date })
                        .Select(x => new
                        {
                            ClientId = x.Key.ClientId,
                            ClientName = x.Key.ClientName,
                            Brand = x.Key.Brand,
                            Year = x.Key.Year,
                            Quarter = x.Key.Quarter,
                            Month = x.Key.Month,
                            Date = x.Key.Date,
                            RatingAverage = x.Average(y => y.Rating),
                            CostSum = x.Sum(y => y.Cost),
                            CostAverage = x.Average(y => y.Cost),
                            HandpiecesCount = x.Sum(y => y.Handpieces),
                            UnrepairedCount = x.Sum(y => y.Unrepaired),
                        })
                        .ToListAsync();

                    return items.GroupBy(x => new { x.ClientId, x.ClientName, x.Brand })
                        .Select(x => new CorporateSurgeryReportSurgeryBrandAggregateItem
                        {
                            ClientId = x.Key.ClientId,
                            ClientName = x.Key.ClientName,
                            Brand = x.Key.Brand,
                            RatingAverage = x.ToDictionary(y => y.Date.ToString("yyyy-MM-dd"), y => y.RatingAverage),
                            CostSum = x.ToDictionary(y => y.Date.ToString("yyyy-MM-dd"), y => y.CostSum),
                            CostAverage = x.ToDictionary(y => y.Date.ToString("yyyy-MM-dd"), y => y.CostAverage),
                            UnrepairedPercent = x.ToDictionary(y => y.Date.ToString("yyyy-MM-dd"), y => (Decimal)y.UnrepairedCount / y.HandpiecesCount),
                            HandpiecesCount = x.ToDictionary(y => y.Date.ToString("yyyy-MM-dd"), y => y.HandpiecesCount),
                        })
                        .ToList();
                }

                default:
                    throw new ArgumentOutOfRangeException(nameof(dateAggregate), dateAggregate, null);
            }
        }

        public async Task<List<CorporateSurgeryReportBrandEntireItem>> GetBrandsForEntirePeriodAsync()
        {
            if (this.query == null)
            {
                await this.PrepareQuery();
            }

            var items = await this.query
                .GroupBy(x => new { x.Brand })
                .Select(x => new
                {
                    Brand = x.Key.Brand,
                    RatingAverage = x.Average(y => y.Rating),
                    CostSum = x.Sum(y => y.Cost),
                    CostAverage = x.Average(y => y.Cost),
                    HandpiecesCount = x.Sum(y => y.Handpieces),
                    UnrepairedCount = x.Sum(y => y.Unrepaired),
                })
                .ToListAsync();

            return items
                .Select(x => new CorporateSurgeryReportBrandEntireItem
                {
                    Brand = x.Brand,
                    RatingAverage = x.RatingAverage,
                    CostSum = x.CostSum,
                    CostAverage = x.CostAverage,
                    UnrepairedPercent = (Decimal)x.UnrepairedCount / x.HandpiecesCount,
                    HandpiecesCount = x.HandpiecesCount,
                })
                .ToList();
        }

        public async Task<List<CorporateSurgeryReportBrandAggregateItem>> GetBrandsAggregatedAsync(ReportDateAggregate dateAggregate)
        {
            if (this.query == null)
            {
                await this.PrepareQuery();
            }

            switch (dateAggregate)
            {
                case ReportDateAggregate.Yearly:
                {
                    var items = await this.query
                        .GroupBy(x => new { x.Brand, x.Year })
                        .Select(x => new
                        {
                            Brand = x.Key.Brand,
                            Year = x.Key.Year,
                            RatingAverage = x.Average(y => y.Rating),
                            CostSum = x.Sum(y => y.Cost),
                            CostAverage = x.Average(y => y.Cost),
                            HandpiecesCount = x.Sum(y => y.Handpieces),
                            UnrepairedCount = x.Sum(y => y.Unrepaired),
                        })
                        .ToListAsync();

                    return items.GroupBy(x => new { x.Brand })
                        .Select(x => new CorporateSurgeryReportBrandAggregateItem
                        {
                            Brand = x.Key.Brand,
                            RatingAverage = x.ToDictionary(y => y.Year.ToString(), y => y.RatingAverage),
                            CostSum = x.ToDictionary(y => y.Year.ToString(), y => y.CostSum),
                            CostAverage = x.ToDictionary(y => y.Year.ToString(), y => y.CostAverage),
                            UnrepairedPercent = x.ToDictionary(y => y.Year.ToString(), y => (Decimal)y.UnrepairedCount / y.HandpiecesCount),
                            HandpiecesCount = x.ToDictionary(y => y.Year.ToString(), y => y.HandpiecesCount),
                        })
                        .ToList();
                }

                case ReportDateAggregate.Quarterly:
                {
                    var items = await this.query
                        .GroupBy(x => new { x.Brand, x.Year, x.Quarter })
                        .Select(x => new
                        {
                            Brand = x.Key.Brand,
                            Year = x.Key.Year,
                            Quarter = x.Key.Quarter,
                            RatingAverage = x.Average(y => y.Rating),
                            CostSum = x.Sum(y => y.Cost),
                            CostAverage = x.Average(y => y.Cost),
                            HandpiecesCount = x.Sum(y => y.Handpieces),
                            UnrepairedCount = x.Sum(y => y.Unrepaired),
                        })
                        .ToListAsync();

                    return items.GroupBy(x => new { x.Brand })
                        .Select(x => new CorporateSurgeryReportBrandAggregateItem
                        {
                            Brand = x.Key.Brand,
                            RatingAverage = x.ToDictionary(y => y.Year.ToString() + "-Q" + y.Quarter.ToString(), y => y.RatingAverage),
                            CostSum = x.ToDictionary(y => y.Year.ToString() + "-Q" + y.Quarter.ToString(), y => y.CostSum),
                            CostAverage = x.ToDictionary(y => y.Year.ToString() + "-Q" + y.Quarter.ToString(), y => y.CostAverage),
                            UnrepairedPercent = x.ToDictionary(y => y.Year.ToString() + "-Q" + y.Quarter.ToString(), y => (Decimal)y.UnrepairedCount / y.HandpiecesCount),
                            HandpiecesCount = x.ToDictionary(y => y.Year.ToString() + "-Q" + y.Quarter.ToString(), y => y.HandpiecesCount),
                        })
                        .ToList();
                }

                case ReportDateAggregate.Monthly:
                {
                    var items = await this.query
                        .GroupBy(x => new { x.Brand, x.Year, x.Quarter, x.Month })
                        .Select(x => new
                        {
                            Brand = x.Key.Brand,
                            Year = x.Key.Year,
                            Quarter = x.Key.Quarter,
                            Month = x.Key.Month,
                            RatingAverage = x.Average(y => y.Rating),
                            CostSum = x.Sum(y => y.Cost),
                            CostAverage = x.Average(y => y.Cost),
                            HandpiecesCount = x.Sum(y => y.Handpieces),
                            UnrepairedCount = x.Sum(y => y.Unrepaired),
                        })
                        .ToListAsync();

                    return items.GroupBy(x => new { x.Brand })
                        .Select(x => new CorporateSurgeryReportBrandAggregateItem
                        {
                            Brand = x.Key.Brand,
                            RatingAverage = x.ToDictionary(y => y.Year.ToString() + "-" + y.Month.ToString(), y => y.RatingAverage),
                            CostSum = x.ToDictionary(y => y.Year.ToString() + "-" + y.Month.ToString(), y => y.CostSum),
                            CostAverage = x.ToDictionary(y => y.Year.ToString() + "-" + y.Month.ToString(), y => y.CostAverage),
                            UnrepairedPercent = x.ToDictionary(y => y.Year.ToString() + "-" + y.Month.ToString(), y => (Decimal)y.UnrepairedCount / y.HandpiecesCount),
                            HandpiecesCount = x.ToDictionary(y => y.Year.ToString() + "-" + y.Month.ToString(), y => y.HandpiecesCount),
                        })
                        .ToList();
                }

                case ReportDateAggregate.Weekly:
                {
                    var items = await this.query
                        .GroupBy(x => new { x.Brand, x.Year, x.Week })
                        .Select(x => new
                        {
                            Brand = x.Key.Brand,
                            Year = x.Key.Year,
                            Week = x.Key.Week,
                            RatingAverage = x.Average(y => y.Rating),
                            CostSum = x.Sum(y => y.Cost),
                            CostAverage = x.Average(y => y.Cost),
                            HandpiecesCount = x.Sum(y => y.Handpieces),
                            UnrepairedCount = x.Sum(y => y.Unrepaired),
                        })
                        .ToListAsync();

                    return items.GroupBy(x => new { x.Brand })
                        .Select(x => new CorporateSurgeryReportBrandAggregateItem
                        {
                            Brand = x.Key.Brand,
                            RatingAverage = x.ToDictionary(y => y.Year.ToString() + "-W" + y.Week.ToString(), y => y.RatingAverage),
                            CostSum = x.ToDictionary(y => y.Year.ToString() + "-W" + y.Week.ToString(), y => y.CostSum),
                            CostAverage = x.ToDictionary(y => y.Year.ToString() + "-W" + y.Week.ToString(), y => y.CostAverage),
                            UnrepairedPercent = x.ToDictionary(y => y.Year.ToString() + "-W" + y.Week.ToString(), y => (Decimal)y.UnrepairedCount / y.HandpiecesCount),
                            HandpiecesCount = x.ToDictionary(y => y.Year.ToString() + "-W" + y.Week.ToString(), y => y.HandpiecesCount),
                        })
                        .ToList();
                }

                case ReportDateAggregate.Daily:
                {
                    var items = await this.query
                        .GroupBy(x => new { x.Brand, x.Year, x.Quarter, x.Month, x.Date })
                        .Select(x => new
                        {
                            Brand = x.Key.Brand,
                            Year = x.Key.Year,
                            Quarter = x.Key.Quarter,
                            Month = x.Key.Month,
                            Date = x.Key.Date,
                            RatingAverage = x.Average(y => y.Rating),
                            CostSum = x.Sum(y => y.Cost),
                            CostAverage = x.Average(y => y.Cost),
                            HandpiecesCount = x.Sum(y => y.Handpieces),
                            UnrepairedCount = x.Sum(y => y.Unrepaired),
                        })
                        .ToListAsync();

                    return items.GroupBy(x => new { x.Brand })
                        .Select(x => new CorporateSurgeryReportBrandAggregateItem
                        {
                            Brand = x.Key.Brand,
                            RatingAverage = x.ToDictionary(y => y.Date.ToString("yyyy-MM-dd"), y => y.RatingAverage),
                            CostSum = x.ToDictionary(y => y.Date.ToString("yyyy-MM-dd"), y => y.CostSum),
                            CostAverage = x.ToDictionary(y => y.Date.ToString("yyyy-MM-dd"), y => y.CostAverage),
                            UnrepairedPercent = x.ToDictionary(y => y.Date.ToString("yyyy-MM-dd"), y => (Decimal)y.UnrepairedCount / y.HandpiecesCount),
                            HandpiecesCount = x.ToDictionary(y => y.Date.ToString("yyyy-MM-dd"), y => y.HandpiecesCount),
                        })
                        .ToList();
                }

                default:
                    throw new ArgumentOutOfRangeException(nameof(dateAggregate), dateAggregate, null);
            }
        }

        public async Task<List<CorporateSurgeryReportBrandModelEntireItem>> GetBrandsAndModelsForEntirePeriodAsync()
        {
            if (this.query == null)
            {
                await this.PrepareQuery();
            }

            var items = await this.query
                .GroupBy(x => new { x.Brand, x.Model })
                .Select(x => new
                {
                    Brand = x.Key.Brand,
                    Model = x.Key.Model,
                    RatingAverage = Math.Round(x.Average(y => y.Rating), 2),
                    CostSum = x.Sum(y => y.Cost),
                    CostAverage = x.Average(y => y.Cost),
                    HandpiecesCount = x.Sum(y => y.Handpieces),
                    UnrepairedCount = x.Sum(y => y.Unrepaired),
                })
                .ToListAsync();

            return items
                .Select(x => new CorporateSurgeryReportBrandModelEntireItem
                {
                    Brand = x.Brand,
                    Model = x.Model,
                    RatingAverage = x.RatingAverage,
                    CostSum = x.CostSum,
                    CostAverage = x.CostAverage,
                    HandpiecesCount = x.HandpiecesCount,
                    UnrepairedPercent = (Decimal)x.UnrepairedCount / x.HandpiecesCount,
                })
                .ToList();
        }

        public async Task<List<CorporateSurgeryReportBrandModelAggregateItem>> GetBrandsAndModelsAggregatedAsync(ReportDateAggregate dateAggregate)
        {
            if (this.query == null)
            {
                await this.PrepareQuery();
            }

            switch (dateAggregate)
            {
                case ReportDateAggregate.Yearly:
                {
                    var items = await this.query
                        .GroupBy(x => new { x.Brand, x.Model, x.Year })
                        .Select(x => new
                        {
                            Brand = x.Key.Brand,
                            Model = x.Key.Model,
                            Year = x.Key.Year,
                            RatingAverage = x.Average(y => y.Rating),
                            CostSum = x.Sum(y => y.Cost),
                            CostAverage = x.Average(y => y.Cost),
                            HandpiecesCount = x.Sum(y => y.Handpieces),
                            UnrepairedCount = x.Sum(y => y.Unrepaired),
                        })
                        .ToListAsync();

                    return items.GroupBy(x => new { x.Brand, x.Model })
                        .Select(x => new CorporateSurgeryReportBrandModelAggregateItem
                        {
                            Brand = x.Key.Brand,
                            Model = x.Key.Model,
                            RatingAverage = x.ToDictionary(y => y.Year.ToString(), y => y.RatingAverage),
                            CostSum = x.ToDictionary(y => y.Year.ToString(), y => y.CostSum),
                            CostAverage = x.ToDictionary(y => y.Year.ToString(), y => y.CostAverage),
                            UnrepairedPercent = x.ToDictionary(y => y.Year.ToString(), y => (Decimal)y.UnrepairedCount / y.HandpiecesCount),
                            HandpiecesCount = x.ToDictionary(y => y.Year.ToString(), y => y.HandpiecesCount),
                        })
                        .ToList();
                }

                case ReportDateAggregate.Quarterly:
                {
                    var items = await this.query
                        .GroupBy(x => new { x.Brand, x.Model, x.Year, x.Quarter })
                        .Select(x => new
                        {
                            Brand = x.Key.Brand,
                            Model = x.Key.Model,
                            Year = x.Key.Year,
                            Quarter = x.Key.Quarter,
                            RatingAverage = x.Average(y => y.Rating),
                            CostSum = x.Sum(y => y.Cost),
                            CostAverage = x.Average(y => y.Cost),
                            HandpiecesCount = x.Sum(y => y.Handpieces),
                            UnrepairedCount = x.Sum(y => y.Unrepaired),
                        })
                        .ToListAsync();

                    return items.GroupBy(x => new { x.Brand, x.Model })
                        .Select(x => new CorporateSurgeryReportBrandModelAggregateItem
                        {
                            Brand = x.Key.Brand,
                            Model = x.Key.Model,
                            RatingAverage = x.ToDictionary(y => y.Year.ToString() + "-Q" + y.Quarter.ToString(), y => y.RatingAverage),
                            CostSum = x.ToDictionary(y => y.Year.ToString() + "-Q" + y.Quarter.ToString(), y => y.CostSum),
                            CostAverage = x.ToDictionary(y => y.Year.ToString() + "-Q" + y.Quarter.ToString(), y => y.CostAverage),
                            UnrepairedPercent = x.ToDictionary(y => y.Year.ToString() + "-Q" + y.Quarter.ToString(), y => (Decimal)y.UnrepairedCount / y.HandpiecesCount),
                            HandpiecesCount = x.ToDictionary(y => y.Year.ToString() + "-Q" + y.Quarter.ToString(), y => y.HandpiecesCount),
                        })
                        .ToList();
                }

                case ReportDateAggregate.Monthly:
                {
                    var items = await this.query
                        .GroupBy(x => new { x.Brand, x.Model, x.Year, x.Quarter, x.Month })
                        .Select(x => new
                        {
                            Brand = x.Key.Brand,
                            Model = x.Key.Model,
                            Year = x.Key.Year,
                            Quarter = x.Key.Quarter,
                            Month = x.Key.Month,
                            RatingAverage = x.Average(y => y.Rating),
                            CostSum = x.Sum(y => y.Cost),
                            CostAverage = x.Average(y => y.Cost),
                            HandpiecesCount = x.Sum(y => y.Handpieces),
                            UnrepairedCount = x.Sum(y => y.Unrepaired),
                        })
                        .ToListAsync();

                    return items.GroupBy(x => new { x.Brand, x.Model })
                        .Select(x => new CorporateSurgeryReportBrandModelAggregateItem
                        {
                            Brand = x.Key.Brand,
                            Model = x.Key.Model,
                            RatingAverage = x.ToDictionary(y => y.Year.ToString() + "-" + y.Month.ToString(), y => y.RatingAverage),
                            CostSum = x.ToDictionary(y => y.Year.ToString() + "-" + y.Month.ToString(), y => y.CostSum),
                            CostAverage = x.ToDictionary(y => y.Year.ToString() + "-" + y.Month.ToString(), y => y.CostAverage),
                            UnrepairedPercent = x.ToDictionary(y => y.Year.ToString() + "-" + y.Month.ToString(), y => (Decimal)y.UnrepairedCount / y.HandpiecesCount),
                            HandpiecesCount = x.ToDictionary(y => y.Year.ToString() + "-" + y.Month.ToString(), y => y.HandpiecesCount),
                        })
                        .ToList();
                }

                case ReportDateAggregate.Weekly:
                {
                    var items = await this.query
                        .GroupBy(x => new { x.Brand, x.Model, x.Year, x.Week })
                        .Select(x => new
                        {
                            Brand = x.Key.Brand,
                            Model = x.Key.Model,
                            Year = x.Key.Year,
                            Week = x.Key.Week,
                            RatingAverage = x.Average(y => y.Rating),
                            CostSum = x.Sum(y => y.Cost),
                            CostAverage = x.Average(y => y.Cost),
                            HandpiecesCount = x.Sum(y => y.Handpieces),
                            UnrepairedCount = x.Sum(y => y.Unrepaired),
                        })
                        .ToListAsync();

                    return items.GroupBy(x => new { x.Brand, x.Model })
                        .Select(x => new CorporateSurgeryReportBrandModelAggregateItem
                        {
                            Brand = x.Key.Brand,
                            Model = x.Key.Model,
                            RatingAverage = x.ToDictionary(y => y.Year.ToString() + "-W" + y.Week.ToString(), y => y.RatingAverage),
                            CostSum = x.ToDictionary(y => y.Year.ToString() + "-W" + y.Week.ToString(), y => y.CostSum),
                            CostAverage = x.ToDictionary(y => y.Year.ToString() + "-W" + y.Week.ToString(), y => y.CostAverage),
                            UnrepairedPercent = x.ToDictionary(y => y.Year.ToString() + "-W" + y.Week.ToString(), y => (Decimal)y.UnrepairedCount / y.HandpiecesCount),
                            HandpiecesCount = x.ToDictionary(y => y.Year.ToString() + "-W" + y.Week.ToString(), y => y.HandpiecesCount),
                        })
                        .ToList();
                }

                case ReportDateAggregate.Daily:
                {
                    var items = await this.query
                        .GroupBy(x => new { x.Brand, x.Model, x.Year, x.Quarter, x.Month, x.Date })
                        .Select(x => new
                        {
                            Brand = x.Key.Brand,
                            Model = x.Key.Model,
                            Year = x.Key.Year,
                            Quarter = x.Key.Quarter,
                            Month = x.Key.Month,
                            Date = x.Key.Date,
                            RatingAverage = x.Average(y => y.Rating),
                            CostSum = x.Sum(y => y.Cost),
                            CostAverage = x.Average(y => y.Cost),
                            HandpiecesCount = x.Sum(y => y.Handpieces),
                            UnrepairedCount = x.Sum(y => y.Unrepaired),
                        })
                        .ToListAsync();

                    return items.GroupBy(x => new { x.Brand, x.Model })
                        .Select(x => new CorporateSurgeryReportBrandModelAggregateItem
                        {
                            Brand = x.Key.Brand,
                            Model = x.Key.Model,
                            RatingAverage = x.ToDictionary(y => y.Date.ToString("yyyy-MM-dd"), y => y.RatingAverage),
                            CostSum = x.ToDictionary(y => y.Date.ToString("yyyy-MM-dd"), y => y.CostSum),
                            CostAverage = x.ToDictionary(y => y.Date.ToString("yyyy-MM-dd"), y => y.CostAverage),
                            UnrepairedPercent = x.ToDictionary(y => y.Date.ToString("yyyy-MM-dd"), y => (Decimal)y.UnrepairedCount / y.HandpiecesCount),
                            HandpiecesCount = x.ToDictionary(y => y.Date.ToString("yyyy-MM-dd"), y => y.HandpiecesCount),
                        })
                        .ToList();
                }

                default:
                    throw new ArgumentOutOfRangeException(nameof(dateAggregate), dateAggregate, null);
            }
        }

        public async Task<List<CorporateSurgeryReportBrandSurgeryEntireItem>> GetBrandsAndSurgeriesForEntirePeriodAsync()
        {
            if (this.query == null)
            {
                await this.PrepareQuery();
            }

            var items = await this.query
                .GroupBy(x => new { x.Brand, x.ClientId, x.ClientName })
                .Select(x => new
                {
                    Brand = x.Key.Brand,
                    ClientId = x.Key.ClientId,
                    ClientName = x.Key.ClientName,
                    RatingAverage = Math.Round(x.Average(y => y.Rating), 2),
                    CostSum = x.Sum(y => y.Cost),
                    CostAverage = x.Average(y => y.Cost),
                    HandpiecesCount = x.Sum(y => y.Handpieces),
                    UnrepairedCount = x.Sum(y => y.Unrepaired),
                })
                .ToListAsync();

            return items
                .Select(x => new CorporateSurgeryReportBrandSurgeryEntireItem
                {
                    Brand = x.Brand,
                    ClientId = x.ClientId,
                    ClientName = x.ClientName,
                    RatingAverage = x.RatingAverage,
                    CostSum = x.CostSum,
                    CostAverage = x.CostAverage,
                    HandpiecesCount = x.HandpiecesCount,
                    UnrepairedPercent = (Decimal)x.UnrepairedCount / x.HandpiecesCount,
                })
                .ToList();
        }

        public async Task<List<CorporateSurgeryReportBrandSurgeryAggregateItem>> GetBrandsAndSurgeriesAggregatedAsync(ReportDateAggregate dateAggregate)
        {
            if (this.query == null)
            {
                await this.PrepareQuery();
            }

            switch (dateAggregate)
            {
                case ReportDateAggregate.Yearly:
                {
                    var items = await this.query
                        .GroupBy(x => new { x.Brand, x.ClientId, x.ClientName, x.Year })
                        .Select(x => new
                        {
                            Brand = x.Key.Brand,
                            ClientId = x.Key.ClientId,
                            ClientName = x.Key.ClientName,
                            Year = x.Key.Year,
                            RatingAverage = x.Average(y => y.Rating),
                            CostSum = x.Sum(y => y.Cost),
                            CostAverage = x.Average(y => y.Cost),
                            HandpiecesCount = x.Sum(y => y.Handpieces),
                            UnrepairedCount = x.Sum(y => y.Unrepaired),
                        })
                        .ToListAsync();

                    return items.GroupBy(x => new { x.Brand, x.ClientId, x.ClientName })
                        .Select(x => new CorporateSurgeryReportBrandSurgeryAggregateItem
                        {
                            ClientId = x.Key.ClientId,
                            ClientName = x.Key.ClientName,
                            Brand = x.Key.Brand,
                            RatingAverage = x.ToDictionary(y => y.Year.ToString(), y => y.RatingAverage),
                            CostSum = x.ToDictionary(y => y.Year.ToString(), y => y.CostSum),
                            CostAverage = x.ToDictionary(y => y.Year.ToString(), y => y.CostAverage),
                            UnrepairedPercent = x.ToDictionary(y => y.Year.ToString(), y => (Decimal)y.UnrepairedCount / y.HandpiecesCount),
                            HandpiecesCount = x.ToDictionary(y => y.Year.ToString(), y => y.HandpiecesCount),
                        })
                        .ToList();
                }

                case ReportDateAggregate.Quarterly:
                {
                    var items = await this.query
                        .GroupBy(x => new { x.ClientId, x.ClientName, x.Brand, x.Year, x.Quarter })
                        .Select(x => new
                        {
                            Brand = x.Key.Brand,
                            ClientId = x.Key.ClientId,
                            ClientName = x.Key.ClientName,
                            Year = x.Key.Year,
                            Quarter = x.Key.Quarter,
                            RatingAverage = x.Average(y => y.Rating),
                            CostSum = x.Sum(y => y.Cost),
                            CostAverage = x.Average(y => y.Cost),
                            HandpiecesCount = x.Sum(y => y.Handpieces),
                            UnrepairedCount = x.Sum(y => y.Unrepaired),
                        })
                        .ToListAsync();

                    return items.GroupBy(x => new { x.ClientId, x.ClientName, x.Brand })
                        .Select(x => new CorporateSurgeryReportBrandSurgeryAggregateItem
                        {
                            Brand = x.Key.Brand,
                            ClientId = x.Key.ClientId,
                            ClientName = x.Key.ClientName,
                            RatingAverage = x.ToDictionary(y => y.Year.ToString() + "-Q" + y.Quarter.ToString(), y => y.RatingAverage),
                            CostSum = x.ToDictionary(y => y.Year.ToString() + "-Q" + y.Quarter.ToString(), y => y.CostSum),
                            CostAverage = x.ToDictionary(y => y.Year.ToString() + "-Q" + y.Quarter.ToString(), y => y.CostAverage),
                            UnrepairedPercent = x.ToDictionary(y => y.Year.ToString() + "-Q" + y.Quarter.ToString(), y => (Decimal)y.UnrepairedCount / y.HandpiecesCount),
                            HandpiecesCount = x.ToDictionary(y => y.Year.ToString() + "-Q" + y.Quarter.ToString(), y => y.HandpiecesCount),
                        })
                        .ToList();
                }

                case ReportDateAggregate.Monthly:
                {
                    var items = await this.query
                        .GroupBy(x => new { x.ClientId, x.ClientName, x.Brand, x.Year, x.Quarter, x.Month })
                        .Select(x => new
                        {
                            Brand = x.Key.Brand,
                            ClientId = x.Key.ClientId,
                            ClientName = x.Key.ClientName,
                            Year = x.Key.Year,
                            Quarter = x.Key.Quarter,
                            Month = x.Key.Month,
                            RatingAverage = x.Average(y => y.Rating),
                            CostSum = x.Sum(y => y.Cost),
                            CostAverage = x.Average(y => y.Cost),
                            HandpiecesCount = x.Sum(y => y.Handpieces),
                            UnrepairedCount = x.Sum(y => y.Unrepaired),
                        })
                        .ToListAsync();

                    return items.GroupBy(x => new { x.Brand, x.ClientId, x.ClientName })
                        .Select(x => new CorporateSurgeryReportBrandSurgeryAggregateItem
                        {
                            ClientId = x.Key.ClientId,
                            ClientName = x.Key.ClientName,
                            Brand = x.Key.Brand,
                            RatingAverage = x.ToDictionary(y => y.Year.ToString() + "-" + y.Month.ToString(), y => y.RatingAverage),
                            CostSum = x.ToDictionary(y => y.Year.ToString() + "-" + y.Month.ToString(), y => y.CostSum),
                            CostAverage = x.ToDictionary(y => y.Year.ToString() + "-" + y.Month.ToString(), y => y.CostAverage),
                            UnrepairedPercent = x.ToDictionary(y => y.Year.ToString() + "-" + y.Month.ToString(), y => (Decimal)y.UnrepairedCount / y.HandpiecesCount),
                            HandpiecesCount = x.ToDictionary(y => y.Year.ToString() + "-" + y.Month.ToString(), y => y.HandpiecesCount),
                        })
                        .ToList();
                }

                case ReportDateAggregate.Weekly:
                {
                    var items = await this.query
                        .GroupBy(x => new { x.Brand, x.ClientId, x.ClientName, x.Year, x.Week })
                        .Select(x => new
                        {
                            Brand = x.Key.Brand,
                            ClientId = x.Key.ClientId,
                            ClientName = x.Key.ClientName,
                            Year = x.Key.Year,
                            Week = x.Key.Week,
                            RatingAverage = x.Average(y => y.Rating),
                            CostSum = x.Sum(y => y.Cost),
                            CostAverage = x.Average(y => y.Cost),
                            HandpiecesCount = x.Sum(y => y.Handpieces),
                            UnrepairedCount = x.Sum(y => y.Unrepaired),
                        })
                        .ToListAsync();

                    return items.GroupBy(x => new { x.Brand, x.ClientId, x.ClientName })
                        .Select(x => new CorporateSurgeryReportBrandSurgeryAggregateItem
                        {
                            ClientId = x.Key.ClientId,
                            ClientName = x.Key.ClientName,
                            Brand = x.Key.Brand,
                            RatingAverage = x.ToDictionary(y => y.Year.ToString() + "-W" + y.Week.ToString(), y => y.RatingAverage),
                            CostSum = x.ToDictionary(y => y.Year.ToString() + "-W" + y.Week.ToString(), y => y.CostSum),
                            CostAverage = x.ToDictionary(y => y.Year.ToString() + "-W" + y.Week.ToString(), y => y.CostAverage),
                            UnrepairedPercent = x.ToDictionary(y => y.Year.ToString() + "-W" + y.Week.ToString(), y => (Decimal)y.UnrepairedCount / y.HandpiecesCount),
                            HandpiecesCount = x.ToDictionary(y => y.Year.ToString() + "-W" + y.Week.ToString(), y => y.HandpiecesCount),
                        })
                        .ToList();
                }

                case ReportDateAggregate.Daily:
                {
                    var items = await this.query
                        .GroupBy(x => new { x.ClientId, x.ClientName, x.Brand, x.Year, x.Quarter, x.Month, x.Date })
                        .Select(x => new
                        {
                            Brand = x.Key.Brand,
                            ClientId = x.Key.ClientId,
                            ClientName = x.Key.ClientName,
                            Year = x.Key.Year,
                            Quarter = x.Key.Quarter,
                            Month = x.Key.Month,
                            Date = x.Key.Date,
                            RatingAverage = x.Average(y => y.Rating),
                            CostSum = x.Sum(y => y.Cost),
                            CostAverage = x.Average(y => y.Cost),
                            HandpiecesCount = x.Sum(y => y.Handpieces),
                            UnrepairedCount = x.Sum(y => y.Unrepaired),
                        })
                        .ToListAsync();

                    return items.GroupBy(x => new { x.Brand, x.ClientId, x.ClientName })
                        .Select(x => new CorporateSurgeryReportBrandSurgeryAggregateItem
                        {
                            Brand = x.Key.Brand,
                            ClientId = x.Key.ClientId,
                            ClientName = x.Key.ClientName,
                            RatingAverage = x.ToDictionary(y => y.Date.ToString("yyyy-MM-dd"), y => y.RatingAverage),
                            CostSum = x.ToDictionary(y => y.Date.ToString("yyyy-MM-dd"), y => y.CostSum),
                            CostAverage = x.ToDictionary(y => y.Date.ToString("yyyy-MM-dd"), y => y.CostAverage),
                            UnrepairedPercent = x.ToDictionary(y => y.Date.ToString("yyyy-MM-dd"), y => (Decimal)y.UnrepairedCount / y.HandpiecesCount),
                            HandpiecesCount = x.ToDictionary(y => y.Date.ToString("yyyy-MM-dd"), y => y.HandpiecesCount),
                        })
                        .ToList();
                }

                default:
                    throw new ArgumentOutOfRangeException(nameof(dateAggregate), dateAggregate, null);
            }
        }
    }
}
