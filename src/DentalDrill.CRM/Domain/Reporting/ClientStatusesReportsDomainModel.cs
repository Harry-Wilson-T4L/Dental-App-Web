using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DentalDrill.CRM.Data;
using DentalDrill.CRM.Domain.Abstractions;
using DentalDrill.CRM.Domain.Abstractions.Reporting;
using DentalDrill.CRM.Models.ViewModels.Reports.Surgery;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DentalDrill.CRM.Domain.Reporting
{
    public class ClientStatusesReportsDomainModel : IClientStatusesReports
    {
        private readonly IServiceProvider serviceProvider;
        private readonly IClient client;

        private readonly DateTime from;
        private readonly DateTime to;

        private IQueryable<SurgeryReportStatusItem> query;

        public ClientStatusesReportsDomainModel(IServiceProvider serviceProvider, IClient client, DateTime from, DateTime to)
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
  iif([Handpieces].[HandpieceStatus] in (10), 1, 0) as [StatusReceived],
  iif([Handpieces].[HandpieceStatus] in (20), 1, 0) as [StatusBeingEstimated],
  iif([Handpieces].[HandpieceStatus] in (30, 31), 1, 0) as [StatusWaitingForApproval],
  iif([Handpieces].[HandpieceStatus] in (35), 1, 0) as [StatusEstimateSent],
  iif([Handpieces].[HandpieceStatus] in (40, 41), 1, 0) as [StatusBeingRepaired],
  iif([Handpieces].[HandpieceStatus] in (50), 1, 0) as [StatusReadyToReturn],
  iif([Handpieces].[HandpieceStatus] in (60), 1, 0) as [StatusSentComplete],
  iif([Handpieces].[HandpieceStatus] in (51, 70, 71, 42), 1, 0) as [StatusCancelled],
  1 as [StatusAny]
from [Handpieces]
  inner join [Jobs] on [Handpieces].[JobId] = [Jobs].[Id]
where [Jobs].[ClientId] = @clientId
  and [Jobs].[Received] between @from and @to");

            var queryParameters = new List<Object>();
            queryParameters.Add(new SqlParameter("clientId", SqlDbType.UniqueIdentifier) { Value = this.client.Id });
            queryParameters.Add(new SqlParameter("from", SqlDbType.Date) { Value = this.from });
            queryParameters.Add(new SqlParameter("to", SqlDbType.Date) { Value = this.to });

            this.query = dbContext.Set<SurgeryReportStatusItem>().FromSqlRaw(queryText.ToString(), queryParameters.ToArray());
            return Task.CompletedTask;
        }

        public async Task<List<SurgeryReportStatusItem>> GetBrandsForEntirePeriodAsync()
        {
            if (this.query == null)
            {
                await this.PrepareQuery();
            }

            return await this.query
                .GroupBy(x => new { x.Brand })
                .Select(x => new SurgeryReportStatusItem
                {
                    Brand = x.Key.Brand,
                    StatusReceived = x.Sum(y => y.StatusReceived),
                    StatusBeingEstimated = x.Sum(y => y.StatusBeingEstimated),
                    StatusWaitingForApproval = x.Sum(y => y.StatusWaitingForApproval),
                    StatusEstimateSent = x.Sum(y => y.StatusEstimateSent),
                    StatusBeingRepaired = x.Sum(y => y.StatusBeingRepaired),
                    StatusReadyToReturn = x.Sum(y => y.StatusReadyToReturn),
                    StatusSentComplete = x.Sum(y => y.StatusSentComplete),
                    StatusCancelled = x.Sum(y => y.StatusCancelled),
                    StatusAny = x.Sum(y => y.StatusAny),
                })
                .ToListAsync();
        }
    }
}
