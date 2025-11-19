using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DentalDrill.CRM.Data;
using DentalDrill.CRM.Domain.Abstractions;
using DentalDrill.CRM.Domain.Abstractions.Reporting;
using DentalDrill.CRM.Models.ViewModels;
using DentalDrill.CRM.Models.ViewModels.Reports.Surgery;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DentalDrill.CRM.Domain.Reporting
{
    public class ClientBrandsReportsDomainModel : IClientBrandsReports
    {
        private readonly IServiceProvider serviceProvider;
        private readonly IClient client;

        private readonly DateTime from;
        private readonly DateTime to;

        private IQueryable<SurgeryReportSurgeryItem> query;

        public ClientBrandsReportsDomainModel(IServiceProvider serviceProvider, IClient client, DateTime from, DateTime to)
        {
            this.serviceProvider = serviceProvider;
            this.client = client;
            this.from = from;
            this.to = to;
        }

        public Task PrepareQuery()
        {
            var dbContext = this.serviceProvider.GetService<ApplicationDbContext>();
            var queryText = new StringBuilder();
            queryText.Append($@"select
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
where [Jobs].[ClientId] = @clientId
  and [Jobs].[Received] between @from and @to");

            var queryParameters = new List<Object>();
            queryParameters.Add(new SqlParameter("clientId", SqlDbType.UniqueIdentifier) { Value = this.client.Id });
            queryParameters.Add(new SqlParameter("from", SqlDbType.Date) { Value = this.from });
            queryParameters.Add(new SqlParameter("to", SqlDbType.Date) { Value = this.to });

            this.query = dbContext.Set<SurgeryReportSurgeryItem>().FromSqlRaw(queryText.ToString(), queryParameters.ToArray());
            return Task.CompletedTask;
        }

        public async Task<List<SurgeryReportBrandEntireItem>> GetBrandsForEntirePeriodAsync()
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
                .Select(x => new SurgeryReportBrandEntireItem
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

        public Task<List<SurgeryReportBrandAggregateItem>> GetBrandsAggregatedAsync(ReportDateAggregate dateAggregate)
        {
            switch (dateAggregate)
            {
                case ReportDateAggregate.Yearly:
                    return this.GetBrandsAggregatedYearlyAsync();
                case ReportDateAggregate.Quarterly:
                    return this.GetBrandsAggregatedQuarterlyAsync();
                case ReportDateAggregate.Monthly:
                    return this.GetBrandsAggregatedMonthlyAsync();
                case ReportDateAggregate.Weekly:
                    return this.GetBrandsAggregatedWeeklyAsync();
                case ReportDateAggregate.Daily:
                    return this.GetBrandsAggregatedDailyAsync();
                default:
                    throw new ArgumentOutOfRangeException(nameof(dateAggregate), dateAggregate, null);
            }
        }

        public async Task<List<SurgeryReportBrandAggregateItem>> GetBrandsAggregatedYearlyAsync()
        {
            if (this.query == null)
            {
                await this.PrepareQuery();
            }

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
                .Select(x => new SurgeryReportBrandAggregateItem
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

        public async Task<List<SurgeryReportBrandAggregateItem>> GetBrandsAggregatedQuarterlyAsync()
        {
            if (this.query == null)
            {
                await this.PrepareQuery();
            }

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
                .Select(x => new SurgeryReportBrandAggregateItem
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

        public async Task<List<SurgeryReportBrandAggregateItem>> GetBrandsAggregatedMonthlyAsync()
        {
            if (this.query == null)
            {
                await this.PrepareQuery();
            }

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
                .Select(x => new SurgeryReportBrandAggregateItem
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

        public async Task<List<SurgeryReportBrandAggregateItem>> GetBrandsAggregatedWeeklyAsync()
        {
            if (this.query == null)
            {
                await this.PrepareQuery();
            }

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
                .Select(x => new SurgeryReportBrandAggregateItem
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

        public async Task<List<SurgeryReportBrandAggregateItem>> GetBrandsAggregatedDailyAsync()
        {
            if (this.query == null)
            {
                await this.PrepareQuery();
            }

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
                .Select(x => new SurgeryReportBrandAggregateItem
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

        public async Task<List<SurgeryReportBrandModelEntireItem>> GetBrandsAndModelsForEntirePeriodAsync()
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
                .Select(x => new SurgeryReportBrandModelEntireItem
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

        public Task<List<SurgeryReportBrandModelAggregateItem>> GetBrandsAndModelsAggregatedAsync(ReportDateAggregate dateAggregate)
        {
            switch (dateAggregate)
            {
                case ReportDateAggregate.Yearly:
                    return this.GetBrandsAndModelsAggregatedYearlyAsync();
                case ReportDateAggregate.Quarterly:
                    return this.GetBrandsAndModelsAggregatedQuarterlyAsync();
                case ReportDateAggregate.Monthly:
                    return this.GetBrandsAndModelsAggregatedMonthlyAsync();
                case ReportDateAggregate.Weekly:
                    return this.GetBrandsAndModelsAggregatedMonthlyAsync();
                case ReportDateAggregate.Daily:
                    return this.GetBrandsAndModelsAggregatedDailyAsync();
                default:
                    throw new ArgumentOutOfRangeException(nameof(dateAggregate), dateAggregate, null);
            }
        }

        public async Task<List<SurgeryReportBrandModelAggregateItem>> GetBrandsAndModelsAggregatedYearlyAsync()
        {
            if (this.query == null)
            {
                await this.PrepareQuery();
            }

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
                .Select(x => new SurgeryReportBrandModelAggregateItem
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

        public async Task<List<SurgeryReportBrandModelAggregateItem>> GetBrandsAndModelsAggregatedQuarterlyAsync()
        {
            if (this.query == null)
            {
                await this.PrepareQuery();
            }

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
                .Select(x => new SurgeryReportBrandModelAggregateItem
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

        public async Task<List<SurgeryReportBrandModelAggregateItem>> GetBrandsAndModelsAggregatedMonthlyAsync()
        {
            if (this.query == null)
            {
                await this.PrepareQuery();
            }

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
                .Select(x => new SurgeryReportBrandModelAggregateItem
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

        public async Task<List<SurgeryReportBrandModelAggregateItem>> GetBrandsAndModelsAggregatedWeeklyAsync()
        {
            if (this.query == null)
            {
                await this.PrepareQuery();
            }

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
                .Select(x => new SurgeryReportBrandModelAggregateItem
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

        public async Task<List<SurgeryReportBrandModelAggregateItem>> GetBrandsAndModelsAggregatedDailyAsync()
        {
            if (this.query == null)
            {
                await this.PrepareQuery();
            }

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
                .Select(x => new SurgeryReportBrandModelAggregateItem
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
    }
}
