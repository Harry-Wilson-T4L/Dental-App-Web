using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DentalDrill.CRM.Data;
using DentalDrill.CRM.Models.ViewModels.Reports.Corporate;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DentalDrill.CRM.Domain.Reporting
{
    public class CorporateStatusesReportsDomainModel
    {
        private readonly IServiceProvider serviceProvider;
        private readonly CorporateDomainModel corporate;

        private readonly DateTime from;
        private readonly DateTime to;
        private readonly IReadOnlyList<Guid> clients;

        private IQueryable<CorporateSurgeryReportStatusItem> query;

        public CorporateStatusesReportsDomainModel(IServiceProvider serviceProvider, CorporateDomainModel corporate, DateTime from, DateTime to, IReadOnlyList<Guid> clients)
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
  [Clients].[Id] as [ClientId],
  [Clients].[Name] as [ClientName],
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

            this.query = dbContext.Set<CorporateSurgeryReportStatusItem>().FromSqlRaw(queryText.ToString(), queryParameters.ToArray());
            return Task.CompletedTask;
        }

        public async Task<List<CorporateSurgeryReportStatusItem>> GetSurgeriesForEntirePeriodAsync()
        {
            return await this.query
                .GroupBy(x => new { x.ClientId, x.ClientName })
                .Select(x => new CorporateSurgeryReportStatusItem
                {
                    ClientId = x.Key.ClientId,
                    ClientName = x.Key.ClientName,
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
