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
    public class GlobalBatchResultReportsDomainModel
    {
        private readonly IServiceProvider serviceProvider;

        private readonly DateTime from;
        private readonly DateTime to;

        private IQueryable<GlobalBatchResultReportItem> query;

        public GlobalBatchResultReportsDomainModel(IServiceProvider serviceProvider, DateTime from, DateTime to)
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
  j.[Id] as [JobId],
  j.[JobNumber],
  min(cast(h.[CompletedOn] as date)) as [CompletedFirst],
  count(distinct h.[Id]) as [CountOfHandpieces],
  count(distinct cast(h.[CompletedOn] as date)) as [CountOfDistinctDates],
  string_agg(cast(h.[CompletedOn] as date), ',') within group (order by h.[CompletedOn]) as [ListOfDates]
from [Jobs] j
  inner join [Handpieces] h on h.[JobId] = j.[Id]
where j.[Received] between @from and @to
group by j.[Id], j.[JobNumber]
having count(distinct cast(h.[CompletedOn] as date)) > 1");

            var queryParameters = new List<Object>();
            queryParameters.Add(new SqlParameter("from", SqlDbType.Date) { Value = this.from });
            queryParameters.Add(new SqlParameter("to", SqlDbType.Date) { Value = this.to });

            this.query = dbContext.Set<GlobalBatchResultReportItem>().FromSqlRaw(queryText.ToString(), queryParameters.ToArray());
            return Task.CompletedTask;
        }

        public async Task<List<GlobalBatchResultReportItem>> GetDataAsync()
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
