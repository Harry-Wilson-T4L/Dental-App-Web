using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DentalDrill.CRM.Data;
using DentalDrill.CRM.Models;
using DentalDrill.CRM.Models.ViewModels;
using DentalDrill.CRM.Models.ViewModels.Reports.Global;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

#pragma warning disable SA1025 // Code should not contain multiple whitespace in a row - better readability here
namespace DentalDrill.CRM.Domain.Reporting
{
    public class GlobalHandpiecesReportsDomainModel
    {
        private readonly IServiceProvider serviceProvider;

        private readonly DateTime from;
        private readonly DateTime to;
        private readonly HashSet<String> groupingFields;

        private IQueryable<GlobalHandpiecesReportItem> query;

        public GlobalHandpiecesReportsDomainModel(IServiceProvider serviceProvider, DateTime from, DateTime to, IEnumerable<String> groupingFields)
        {
            this.serviceProvider = serviceProvider;

            this.from = from;
            this.to = to;
            this.groupingFields = groupingFields.ToHashSet(StringComparer.InvariantCultureIgnoreCase);
        }

        public Task PrepareQuery()
        {
            var dbContext = this.serviceProvider.GetService<ApplicationDbContext>();
            var queryText = new StringBuilder();
            queryText.Append(@"select
  cast('00000000-0000-0000-0000-0000000000000' as uniqueidentifier) as [NullId],
  [Jobs].[ClientId] as [ClientId],
  [Clients].[Name] as [ClientName],
  [Handpieces].[Brand] as [Brand],
  [Handpieces].[MakeAndModel] as [Model],
  [ServiceLevels].[Name] as [ServiceLevelName],
  [RepairedBy].[FirstName] + ' ' + [RepairedBy].[LastName] as [RepairedByName],
  [Handpieces].[HandpieceStatus] as [HandpieceStatus],
  datepart(year, [Jobs].[Received]) as [Year],
  datepart(quarter, [Jobs].[Received]) as [Quarter],
  datepart(month, [Jobs].[Received]) as [Month],
  datepart(week, [Jobs].[Received]) as [Week],
  [Jobs].[Received] as [Date],
  coalesce([Handpieces].[CostOfRepair], 0.00) as [Cost],
  cast([Handpieces].[Rating] as decimal(18,2)) as [Rating],
  iif([Handpieces].[HandpieceStatus] = 71, 1, 0) as [Unrepaired],
  iif([Handpieces].[HandpieceStatus] = 51, 1, 0) as [ReturnUnrepaired],
  1 as [Handpieces],
  datediff(day, [Jobs].[Received], [CompletedOn]) as [Turnaround],
  iif([ServiceLevels].[Name] = 'Warranty', 1, 0) as [Warranty]
from [Handpieces]
  inner join [Jobs] on [Handpieces].[JobId] = [Jobs].[Id]
  inner join [Clients] on [Jobs].[ClientId] = [Clients].[Id]
  left join [ServiceLevels] on [Handpieces].[ServiceLevelId] = [ServiceLevels].[Id]
  left join [Employees] [RepairedBy] on [Handpieces].[RepairedById] = [RepairedBy].[Id]
where [Jobs].[Received] between @from and @to
  and [Clients].[HideJobs] = 0");

            var queryParameters = new List<Object>();
            queryParameters.Add(new SqlParameter("from", SqlDbType.Date) { Value = this.from });
            queryParameters.Add(new SqlParameter("to", SqlDbType.Date) { Value = this.to });

            this.query = dbContext.Set<GlobalHandpiecesReportItem>().FromSqlRaw(queryText.ToString(), queryParameters.ToArray());
            return Task.CompletedTask;
        }

        public async Task<List<GlobalHandpiecesReportItem>> GetRawDataAsync()
        {
            if (this.query == null)
            {
                await this.PrepareQuery();
            }

            var items = await this.query.ToListAsync();
            return items;
        }

        public async Task<List<GlobalHandpiecesReportEntireItem>> GetForEntirePeriodAsync()
        {
            if (this.query == null)
            {
                await this.PrepareQuery();
            }

            var groupedFields = new
            {
                Client = this.groupingFields.Contains("Client"),
                ServiceLevel = this.groupingFields.Contains("ServiceLevel"),
                Brand = this.groupingFields.Contains("Brand"),
                Model = this.groupingFields.Contains("Model"),
                RepairedBy = this.groupingFields.Contains("RepairedBy"),
                HandpieceStatus = this.groupingFields.Contains("HandpieceStatus"),
            };

            var items = await this.query
                .GroupBy(x => new
                {
                    ClientId                = groupedFields.Client ? x.ClientId : x.NullId,
                    ClientName              = groupedFields.Client ? x.ClientName : String.Empty,
                    ServiceLevelName        = groupedFields.ServiceLevel ? x.ServiceLevelName : String.Empty,
                    Brand                   = groupedFields.Brand ? x.Brand : String.Empty,
                    Model                   = groupedFields.Model ? x.Model : String.Empty,
                    RepairedByName          = groupedFields.RepairedBy ? x.RepairedByName : String.Empty,
                    HandpieceStatus         = groupedFields.HandpieceStatus ? x.HandpieceStatus : HandpieceStatus.None,
                })
                .Select(x => new
                {
                    ClientId                = x.Key.ClientId,
                    ClientName              = x.Key.ClientName,
                    ServiceLevelName        = x.Key.ServiceLevelName,
                    Brand                   = x.Key.Brand,
                    Model                   = x.Key.Model,
                    RepairedByName          = x.Key.RepairedByName,
                    HandpieceStatus         = x.Key.HandpieceStatus,
                    RatingAverage           = x.Average(y => y.Rating),
                    CostSum                 = x.Sum(y => y.Cost),
                    CostAverage             = x.Average(y => y.Cost),
                    HandpiecesCount         = x.Sum(y => y.Handpieces),
                    UnrepairedCount         = x.Sum(y => y.Unrepaired),
                    ReturnUnrepairedCount   = x.Sum(y => y.ReturnUnrepaired),
                    TurnaroundAverage       = x.Average(y => y.Turnaround),
                    WarrantyCount           = x.Sum(y => y.Warranty),
                })
                .ToListAsync();

            return items
                .Select(x => new GlobalHandpiecesReportEntireItem
                {
                    ClientId                = x.ClientId,
                    ClientName              = x.ClientName,
                    ServiceLevelName        = x.ServiceLevelName,
                    Brand                   = x.Brand,
                    Model                   = x.Model,
                    RepairedByName          = x.RepairedByName,
                    HandpieceStatus         = x.HandpieceStatus,
                    RatingAverage           = x.RatingAverage,
                    CostSum                 = x.CostSum,
                    CostAverage             = x.CostAverage,
                    UnrepairedPercent       = (Decimal)x.UnrepairedCount / x.HandpiecesCount,
                    ReturnUnrepairedPercent = (Decimal)x.ReturnUnrepairedCount / x.HandpiecesCount,
                    HandpiecesCount         = x.HandpiecesCount,
                    TurnaroundAverage       = x.TurnaroundAverage,
                    WarrantyCount           = x.WarrantyCount,
                })
                .ToList();
        }

        public async Task<List<GlobalHandpiecesReportAggregateItem>> GetAggregatedAsync(ReportDateAggregate dateAggregate)
        {
            if (this.query == null)
            {
                await this.PrepareQuery();
            }

            var groupedFields = new
            {
                Client = this.groupingFields.Contains("Client"),
                ServiceLevel = this.groupingFields.Contains("ServiceLevel"),
                Brand = this.groupingFields.Contains("Brand"),
                Model = this.groupingFields.Contains("Model"),
                RepairedBy = this.groupingFields.Contains("RepairedBy"),
                HandpieceStatus = this.groupingFields.Contains("HandpieceStatus"),
            };

            switch (dateAggregate)
            {
                case ReportDateAggregate.Yearly:
                {
                    var items = await this.query
                        .GroupBy(x => new
                        {
                            ClientId                = groupedFields.Client ? x.ClientId : x.NullId,
                            ClientName              = groupedFields.Client ? x.ClientName : String.Empty,
                            ServiceLevelName        = groupedFields.ServiceLevel ? x.ServiceLevelName : String.Empty,
                            Brand                   = groupedFields.Brand ? x.Brand : String.Empty,
                            Model                   = groupedFields.Model ? x.Model : String.Empty,
                            RepairedByName          = groupedFields.RepairedBy ? x.RepairedByName : String.Empty,
                            HandpieceStatus         = groupedFields.HandpieceStatus ? x.HandpieceStatus : HandpieceStatus.None,
                            x.Year,
                        })
                        .Select(x => new
                        {
                            ClientId                = x.Key.ClientId,
                            ClientName              = x.Key.ClientName,
                            ServiceLevelName        = x.Key.ServiceLevelName,
                            Brand                   = x.Key.Brand,
                            Model                   = x.Key.Model,
                            RepairedByName          = x.Key.RepairedByName,
                            HandpieceStatus         = x.Key.HandpieceStatus,
                            Year                    = x.Key.Year,
                            RatingAverage           = x.Average(y => y.Rating),
                            CostSum                 = x.Sum(y => y.Cost),
                            CostAverage             = x.Average(y => y.Cost),
                            HandpiecesCount         = x.Sum(y => y.Handpieces),
                            UnrepairedCount         = x.Sum(y => y.Unrepaired),
                            ReturnUnrepairedCount   = x.Sum(y => y.ReturnUnrepaired),
                            TurnaroundAverage       = x.Average(y => y.Turnaround),
                            WarrantyCount           = x.Sum(y => y.Warranty),
                        })
                        .ToListAsync();

                    return items.GroupBy(x => new
                        {
                            x.ClientId,
                            x.ClientName,
                            x.ServiceLevelName,
                            x.Brand,
                            x.Model,
                            x.RepairedByName,
                            x.HandpieceStatus,
                        })
                        .Select(x => new GlobalHandpiecesReportAggregateItem
                        {
                            ClientId                = x.Key.ClientId,
                            ClientName              = x.Key.ClientName,
                            ServiceLevelName        = x.Key.ServiceLevelName,
                            Brand                   = x.Key.Brand,
                            Model                   = x.Key.Model,
                            RepairedByName          = x.Key.RepairedByName,
                            HandpieceStatus         = x.Key.HandpieceStatus,
                            RatingAverage           = x.ToDictionary(y => $"{y.Year}", y => y.RatingAverage),
                            CostSum                 = x.ToDictionary(y => $"{y.Year}", y => y.CostSum),
                            CostAverage             = x.ToDictionary(y => $"{y.Year}", y => y.CostAverage),
                            UnrepairedPercent       = x.ToDictionary(y => $"{y.Year}", y => (Decimal)y.UnrepairedCount / y.HandpiecesCount),
                            ReturnUnrepairedPercent = x.ToDictionary(y => $"{y.Year}", y => (Decimal)y.ReturnUnrepairedCount / y.HandpiecesCount),
                            HandpiecesCount         = x.ToDictionary(y => $"{y.Year}", y => y.HandpiecesCount),
                            TurnaroundAverage       = x.ToDictionary(y => $"{y.Year}", y => y.TurnaroundAverage),
                            WarrantyCount           = x.ToDictionary(y => $"{y.Year}", y => y.WarrantyCount),
                        })
                        .ToList();
                }

                case ReportDateAggregate.Quarterly:
                {
                    var items = await this.query
                        .GroupBy(x => new
                        {
                            ClientId                = groupedFields.Client ? x.ClientId : x.NullId,
                            ClientName              = groupedFields.Client ? x.ClientName : String.Empty,
                            ServiceLevelName        = groupedFields.ServiceLevel ? x.ServiceLevelName : String.Empty,
                            Brand                   = groupedFields.Brand ? x.Brand : String.Empty,
                            Model                   = groupedFields.Model ? x.Model : String.Empty,
                            RepairedByName          = groupedFields.RepairedBy ? x.RepairedByName : String.Empty,
                            HandpieceStatus         = groupedFields.HandpieceStatus ? x.HandpieceStatus : HandpieceStatus.None,
                            x.Year, x.Quarter,
                        })
                        .Select(x => new
                        {
                            ClientId                = x.Key.ClientId,
                            ClientName              = x.Key.ClientName,
                            ServiceLevelName        = x.Key.ServiceLevelName,
                            Brand                   = x.Key.Brand,
                            Model                   = x.Key.Model,
                            RepairedByName          = x.Key.RepairedByName,
                            HandpieceStatus         = x.Key.HandpieceStatus,
                            Year                    = x.Key.Year,
                            Quarter                 = x.Key.Quarter,
                            RatingAverage           = x.Average(y => y.Rating),
                            CostSum                 = x.Sum(y => y.Cost),
                            CostAverage             = x.Average(y => y.Cost),
                            HandpiecesCount         = x.Sum(y => y.Handpieces),
                            UnrepairedCount         = x.Sum(y => y.Unrepaired),
                            ReturnUnrepairedCount   = x.Sum(y => y.ReturnUnrepaired),
                            TurnaroundAverage       = x.Average(y => y.Turnaround),
                            WarrantyCount           = x.Sum(y => y.Warranty),
                        })
                        .ToListAsync();

                    return items.GroupBy(x => new
                        {
                            x.ClientId,
                            x.ClientName,
                            x.ServiceLevelName,
                            x.Brand,
                            x.Model,
                            x.RepairedByName,
                            x.HandpieceStatus,
                        })
                        .Select(x => new GlobalHandpiecesReportAggregateItem
                        {
                            ClientId                = x.Key.ClientId,
                            ClientName              = x.Key.ClientName,
                            ServiceLevelName        = x.Key.ServiceLevelName,
                            Brand                   = x.Key.Brand,
                            Model                   = x.Key.Model,
                            RepairedByName          = x.Key.RepairedByName,
                            HandpieceStatus         = x.Key.HandpieceStatus,
                            RatingAverage           = x.ToDictionary(y => $"{y.Year}-Q{y.Quarter}", y => y.RatingAverage),
                            CostSum                 = x.ToDictionary(y => $"{y.Year}-Q{y.Quarter}", y => y.CostSum),
                            CostAverage             = x.ToDictionary(y => $"{y.Year}-Q{y.Quarter}", y => y.CostAverage),
                            UnrepairedPercent       = x.ToDictionary(y => $"{y.Year}-Q{y.Quarter}", y => (Decimal)y.UnrepairedCount / y.HandpiecesCount),
                            ReturnUnrepairedPercent = x.ToDictionary(y => $"{y.Year}-Q{y.Quarter}", y => (Decimal)y.ReturnUnrepairedCount / y.HandpiecesCount),
                            HandpiecesCount         = x.ToDictionary(y => $"{y.Year}-Q{y.Quarter}", y => y.HandpiecesCount),
                            TurnaroundAverage       = x.ToDictionary(y => $"{y.Year}-Q{y.Quarter}", y => y.TurnaroundAverage),
                            WarrantyCount           = x.ToDictionary(y => $"{y.Year}-Q{y.Quarter}", y => y.WarrantyCount),
                        })
                        .ToList();
                }

                case ReportDateAggregate.Monthly:
                {
                    var items = await this.query
                        .GroupBy(x => new
                        {
                            ClientId                = groupedFields.Client ? x.ClientId : x.NullId,
                            ClientName              = groupedFields.Client ? x.ClientName : String.Empty,
                            ServiceLevelName        = groupedFields.ServiceLevel ? x.ServiceLevelName : String.Empty,
                            Brand                   = groupedFields.Brand ? x.Brand : String.Empty,
                            Model                   = groupedFields.Model ? x.Model : String.Empty,
                            RepairedByName          = groupedFields.RepairedBy ? x.RepairedByName : String.Empty,
                            HandpieceStatus         = groupedFields.HandpieceStatus ? x.HandpieceStatus : HandpieceStatus.None,
                            x.Year, x.Quarter, x.Month,
                        })
                        .Select(x => new
                        {
                            ClientId                = x.Key.ClientId,
                            ClientName              = x.Key.ClientName,
                            ServiceLevelName        = x.Key.ServiceLevelName,
                            Brand                   = x.Key.Brand,
                            Model                   = x.Key.Model,
                            RepairedByName          = x.Key.RepairedByName,
                            HandpieceStatus         = x.Key.HandpieceStatus,
                            Year                    = x.Key.Year,
                            Quarter                 = x.Key.Quarter,
                            Month                   = x.Key.Month,
                            RatingAverage           = x.Average(y => y.Rating),
                            CostSum                 = x.Sum(y => y.Cost),
                            CostAverage             = x.Average(y => y.Cost),
                            HandpiecesCount         = x.Sum(y => y.Handpieces),
                            UnrepairedCount         = x.Sum(y => y.Unrepaired),
                            ReturnUnrepairedCount   = x.Sum(y => y.ReturnUnrepaired),
                            TurnaroundAverage       = x.Average(y => y.Turnaround),
                            WarrantyCount           = x.Sum(y => y.Warranty),
                        })
                        .ToListAsync();

                    return items.GroupBy(x => new
                        {
                            x.ClientId,
                            x.ClientName,
                            x.ServiceLevelName,
                            x.Brand,
                            x.Model,
                            x.RepairedByName,
                            x.HandpieceStatus,
                        })
                        .Select(x => new GlobalHandpiecesReportAggregateItem
                        {
                            ClientId                = x.Key.ClientId,
                            ClientName              = x.Key.ClientName,
                            ServiceLevelName        = x.Key.ServiceLevelName,
                            Brand                   = x.Key.Brand,
                            Model                   = x.Key.Model,
                            RepairedByName          = x.Key.RepairedByName,
                            HandpieceStatus         = x.Key.HandpieceStatus,
                            RatingAverage           = x.ToDictionary(y => $"{y.Year}-{y.Month}", y => y.RatingAverage),
                            CostSum                 = x.ToDictionary(y => $"{y.Year}-{y.Month}", y => y.CostSum),
                            CostAverage             = x.ToDictionary(y => $"{y.Year}-{y.Month}", y => y.CostAverage),
                            UnrepairedPercent       = x.ToDictionary(y => $"{y.Year}-{y.Month}", y => (Decimal)y.UnrepairedCount / y.HandpiecesCount),
                            ReturnUnrepairedPercent = x.ToDictionary(y => $"{y.Year}-{y.Month}", y => (Decimal)y.ReturnUnrepairedCount / y.HandpiecesCount),
                            HandpiecesCount         = x.ToDictionary(y => $"{y.Year}-{y.Month}", y => y.HandpiecesCount),
                            TurnaroundAverage       = x.ToDictionary(y => $"{y.Year}-{y.Month}", y => y.TurnaroundAverage),
                            WarrantyCount           = x.ToDictionary(y => $"{y.Year}-{y.Month}", y => y.WarrantyCount),
                        })
                        .ToList();
                }

                case ReportDateAggregate.Weekly:
                {
                    var items = await this.query
                        .GroupBy(x => new
                        {
                            ClientId                = groupedFields.Client ? x.ClientId : x.NullId,
                            ClientName              = groupedFields.Client ? x.ClientName : String.Empty,
                            ServiceLevelName        = groupedFields.ServiceLevel ? x.ServiceLevelName : String.Empty,
                            Brand                   = groupedFields.Brand ? x.Brand : String.Empty,
                            Model                   = groupedFields.Model ? x.Model : String.Empty,
                            RepairedByName          = groupedFields.RepairedBy ? x.RepairedByName : String.Empty,
                            HandpieceStatus         = groupedFields.HandpieceStatus ? x.HandpieceStatus : HandpieceStatus.None,
                            x.Year, x.Week,
                        })
                        .Select(x => new
                        {
                            ClientId                = x.Key.ClientId,
                            ClientName              = x.Key.ClientName,
                            ServiceLevelName        = x.Key.ServiceLevelName,
                            Brand                   = x.Key.Brand,
                            Model                   = x.Key.Model,
                            RepairedByName          = x.Key.RepairedByName,
                            HandpieceStatus         = x.Key.HandpieceStatus,
                            Year                    = x.Key.Year,
                            Week                    = x.Key.Week,
                            RatingAverage           = x.Average(y => y.Rating),
                            CostSum                 = x.Sum(y => y.Cost),
                            CostAverage             = x.Average(y => y.Cost),
                            HandpiecesCount         = x.Sum(y => y.Handpieces),
                            UnrepairedCount         = x.Sum(y => y.Unrepaired),
                            ReturnUnrepairedCount   = x.Sum(y => y.ReturnUnrepaired),
                            TurnaroundAverage       = x.Average(y => y.Turnaround),
                            WarrantyCount           = x.Sum(y => y.Warranty),
                        })
                        .ToListAsync();

                    return items.GroupBy(x => new
                        {
                            x.ClientId,
                            x.ClientName,
                            x.ServiceLevelName,
                            x.Brand,
                            x.Model,
                            x.RepairedByName,
                            x.HandpieceStatus,
                        })
                        .Select(x => new GlobalHandpiecesReportAggregateItem
                        {
                            ClientId                = x.Key.ClientId,
                            ClientName              = x.Key.ClientName,
                            ServiceLevelName        = x.Key.ServiceLevelName,
                            Brand                   = x.Key.Brand,
                            Model                   = x.Key.Model,
                            RepairedByName          = x.Key.RepairedByName,
                            HandpieceStatus         = x.Key.HandpieceStatus,
                            RatingAverage           = x.ToDictionary(y => $"{y.Year}-W{y.Week}", y => y.RatingAverage),
                            CostSum                 = x.ToDictionary(y => $"{y.Year}-W{y.Week}", y => y.CostSum),
                            CostAverage             = x.ToDictionary(y => $"{y.Year}-W{y.Week}", y => y.CostAverage),
                            UnrepairedPercent       = x.ToDictionary(y => $"{y.Year}-W{y.Week}", y => (Decimal)y.UnrepairedCount / y.HandpiecesCount),
                            ReturnUnrepairedPercent = x.ToDictionary(y => $"{y.Year}-W{y.Week}", y => (Decimal)y.ReturnUnrepairedCount / y.HandpiecesCount),
                            HandpiecesCount         = x.ToDictionary(y => $"{y.Year}-W{y.Week}", y => y.HandpiecesCount),
                            TurnaroundAverage       = x.ToDictionary(y => $"{y.Year}-W{y.Week}", y => y.TurnaroundAverage),
                            WarrantyCount           = x.ToDictionary(y => $"{y.Year}-W{y.Week}", y => y.WarrantyCount),
                        })
                        .ToList();
                }

                case ReportDateAggregate.Daily:
                {
                    var items = await this.query
                        .GroupBy(x => new
                        {
                            ClientId                = groupedFields.Client ? x.ClientId : x.NullId,
                            ClientName              = groupedFields.Client ? x.ClientName : String.Empty,
                            ServiceLevelName        = groupedFields.ServiceLevel ? x.ServiceLevelName : String.Empty,
                            Brand                   = groupedFields.Brand ? x.Brand : String.Empty,
                            Model                   = groupedFields.Model ? x.Model : String.Empty,
                            RepairedByName          = groupedFields.RepairedBy ? x.RepairedByName : String.Empty,
                            HandpieceStatus         = groupedFields.HandpieceStatus ? x.HandpieceStatus : HandpieceStatus.None,
                            x.Year, x.Quarter, x.Month, x.Date,
                        })
                        .Select(x => new
                        {
                            ClientId                = x.Key.ClientId,
                            ClientName              = x.Key.ClientName,
                            ServiceLevelName        = x.Key.ServiceLevelName,
                            Brand                   = x.Key.Brand,
                            Model                   = x.Key.Model,
                            RepairedByName          = x.Key.RepairedByName,
                            HandpieceStatus         = x.Key.HandpieceStatus,
                            Year                    = x.Key.Year,
                            Quarter                 = x.Key.Quarter,
                            Month                   = x.Key.Month,
                            Date                    = x.Key.Date,
                            RatingAverage           = x.Average(y => y.Rating),
                            CostSum                 = x.Sum(y => y.Cost),
                            CostAverage             = x.Average(y => y.Cost),
                            HandpiecesCount         = x.Sum(y => y.Handpieces),
                            UnrepairedCount         = x.Sum(y => y.Unrepaired),
                            ReturnUnrepairedCount   = x.Sum(y => y.ReturnUnrepaired),
                            TurnaroundAverage       = x.Average(y => y.Turnaround),
                            WarrantyCount           = x.Sum(y => y.Warranty),
                        })
                        .ToListAsync();

                    return items.GroupBy(x => new
                        {
                            x.ClientId,
                            x.ClientName,
                            x.ServiceLevelName,
                            x.Brand,
                            x.Model,
                            x.RepairedByName,
                            x.HandpieceStatus,
                        })
                        .Select(x => new GlobalHandpiecesReportAggregateItem
                        {
                            ClientId                = x.Key.ClientId,
                            ClientName              = x.Key.ClientName,
                            ServiceLevelName        = x.Key.ServiceLevelName,
                            Brand                   = x.Key.Brand,
                            Model                   = x.Key.Model,
                            RepairedByName          = x.Key.RepairedByName,
                            HandpieceStatus         = x.Key.HandpieceStatus,
                            RatingAverage           = x.ToDictionary(y => $"{y.Date:yyyy-MM-dd}", y => y.RatingAverage),
                            CostSum                 = x.ToDictionary(y => $"{y.Date:yyyy-MM-dd}", y => y.CostSum),
                            CostAverage             = x.ToDictionary(y => $"{y.Date:yyyy-MM-dd}", y => y.CostAverage),
                            UnrepairedPercent       = x.ToDictionary(y => $"{y.Date:yyyy-MM-dd}", y => (Decimal)y.UnrepairedCount / y.HandpiecesCount),
                            ReturnUnrepairedPercent = x.ToDictionary(y => $"{y.Date:yyyy-MM-dd}", y => (Decimal)y.ReturnUnrepairedCount / y.HandpiecesCount),
                            HandpiecesCount         = x.ToDictionary(y => $"{y.Date:yyyy-MM-dd}", y => y.HandpiecesCount),
                            TurnaroundAverage       = x.ToDictionary(y => $"{y.Date:yyyy-MM-dd}", y => y.TurnaroundAverage),
                            WarrantyCount           = x.ToDictionary(y => $"{y.Date:yyyy-MM-dd}", y => y.WarrantyCount),
                        })
                        .ToList();
                }

                default:
                    throw new ArgumentOutOfRangeException(nameof(dateAggregate), dateAggregate, null);
            }
        }
    }
}
