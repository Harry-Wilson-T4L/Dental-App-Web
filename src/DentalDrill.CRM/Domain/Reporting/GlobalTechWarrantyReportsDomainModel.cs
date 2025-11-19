using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DentalDrill.CRM.Data;
using DentalDrill.CRM.Models.ViewModels.Reports.Global;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DentalDrill.CRM.Domain.Reporting
{
    public class GlobalTechWarrantyReportsDomainModel
    {
        private readonly IServiceProvider serviceProvider;

        private readonly DateTime from;
        private readonly DateTime to;

        private IQueryable<GlobalTechWarrantyReportItem> query;

        public GlobalTechWarrantyReportsDomainModel(IServiceProvider serviceProvider, DateTime from, DateTime to)
        {
            this.serviceProvider = serviceProvider;

            this.from = from;
            this.to = to;
        }

        public Task PrepareQueryAsync()
        {
            var dbContext = this.serviceProvider.GetService<ApplicationDbContext>();
            var queryText = new StringBuilder();
            queryText.Append(@"select
  actual.[HandpieceId],
  actual.[JobId],
  actual.[JobNumber],
  actual.[JobReceived],
  actual.[RepairedById],
  actual.[RepairedByName],
  actual.[Brand],
  actual.[Model],
  actual.[Serial],
  1 as [HandpieceCount],
  iif(actual.[ServiceLevelName] = 'Warranty', 1, 0) as [Warranty],
  datediff(day, previous.[CompletedOn], actual.[JobReceived]) as [DaysPassed]
from (
	select
	  h.[Id] as [HandpieceId],
	  j.[Id] as [JobId],
	  j.[JobNumber] as [JobNumber],
	  j.[Received] as [JobReceived],
	  j.[ClientId] as [ClientId],
	  h.[Brand] as [Brand],
	  h.[MakeAndModel] as [Model],
	  h.[Serial] as [Serial],
	  h.[RepairedById] as [RepairedById],
	  rep.[FirstName] + ' ' + rep.[LastName] as [RepairedByName],
	  sl.[Name] as [ServiceLevelName]
	from [Handpieces] h
	inner join [Jobs] j on h.[JobId] = j.[Id]
	left join [Employees] rep on h.[RepairedById] = rep.[Id]
	left join [ServiceLevels] sl on h.[ServiceLevelId] = sl.[Id]
) actual
outer apply (
select top 1
  h.[Id] as [HandpieceId],
  j.[Id] as [JobId],
  j.[JobNumber] as [JobNumber],
  j.[Received] as [JobReceived],
  j.[ClientId] as [ClientId],
  h.[Brand] as [Brand],
  h.[MakeAndModel] as [Model],
  h.[Serial] as [Serial],
  h.[CompletedOn] as [CompletedOn]
from [Handpieces] h inner join [Jobs] j on h.[JobId] = j.[Id]
where j.[ClientId] = actual.[ClientId]
  and h.[Brand] = actual.[Brand]
  and h.[MakeAndModel] = actual.[Model]
  and h.[Serial] = actual.[Serial]
  and j.[Id] <> actual.[JobId]
  and j.[Received] < actual.[JobReceived]
  and h.[HandpieceStatus] = 60
) previous
where actual.[JobReceived] between @from and @to
");

            var queryParameters = new List<Object>();
            queryParameters.Add(new SqlParameter("from", SqlDbType.Date) { Value = this.from });
            queryParameters.Add(new SqlParameter("to", SqlDbType.Date) { Value = this.to });

            this.query = dbContext.Set<GlobalTechWarrantyReportItem>().FromSqlRaw(queryText.ToString(), queryParameters.ToArray());
            return Task.CompletedTask;
        }

        public async Task<List<GlobalTechWarrantyReportItem>> GetDataAsync()
        {
            if (this.query == null)
            {
                await this.PrepareQueryAsync();
            }

            var items = await this.query.ToListAsync();
            return items;
        }
    }
}
