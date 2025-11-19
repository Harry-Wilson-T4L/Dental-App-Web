using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Data.SqlClient;

namespace DentalDrill.CRM.Models.ViewModels
{
    public class SurgeryMaintenanceHandpieceFilter
    {
        public String JobNumber { get; set; }

        public String MakeAndModel { get; set; }

        public String Serial { get; set; }

        public HandpieceSpeed? SpeedType { get; set; }

        public DateTime? ReceivedFrom { get; set; }

        public DateTime? ReceivedTo { get; set; }

        public IQueryable<Handpiece> ApplyToQuery(IQueryable<Handpiece> query)
        {
            if (!String.IsNullOrEmpty(this.JobNumber))
            {
                query = query.Where(x => x.Job.JobNumber.ToString().Contains(this.JobNumber));
            }

            if (!String.IsNullOrEmpty(this.MakeAndModel))
            {
                query = query.Where(x => x.MakeAndModel.Contains(this.MakeAndModel));
            }

            if (!String.IsNullOrEmpty(this.Serial))
            {
                query = query.Where(x => x.Serial.Contains(this.Serial));
            }

            if (this.SpeedType.HasValue)
            {
                query = query.Where(x => x.SpeedType == this.SpeedType);
            }

            if (this.ReceivedFrom.HasValue)
            {
                query = query.Where(x => x.Job.Received >= this.ReceivedFrom);
            }

            if (this.ReceivedTo.HasValue)
            {
                query = query.Where(x => x.Job.Received <= this.ReceivedTo);
            }

            return query;
        }

        public (String QueryText, Object[] QueryParameters) GenerateWhereCondition(String jobAlias, String handpieceAlias, Boolean prefixWithAnd)
        {
            var queryText = new StringBuilder();
            var queryParameters = new List<Object>();
            if (!String.IsNullOrEmpty(this.JobNumber))
            {
                var jobNumberRegex = new Regex(@"^(?<jn>\d+)($|[-A-Za-z][-A-Za-z0-9]*$)");
                var jobNumberMatch = jobNumberRegex.Match(this.JobNumber);
                if (jobNumberMatch.Success)
                {
                    var jobNumber = Int32.Parse(jobNumberMatch.Groups["jn"].Value);
                    if (prefixWithAnd || queryText.Length > 0)
                    {
                        queryText.Append(" and ");
                    }

                    queryText.Append($"{jobAlias}.[JobNumber] = @filterJobNumber");
                    queryParameters.Add(new SqlParameter("filterJobNumber", SqlDbType.Int) { Value = jobNumber });
                }
            }

            if (!String.IsNullOrEmpty(this.MakeAndModel))
            {
                if (prefixWithAnd || queryText.Length > 0)
                {
                    queryText.Append(" and ");
                }

                queryText.Append($"{handpieceAlias}.[MakeAndModel] like @filterMakeAndModel");
                queryParameters.Add(new SqlParameter("filterMakeAndModel", SqlDbType.NVarChar) { Value = $"%{this.MakeAndModel}%" });
            }

            if (!String.IsNullOrEmpty(this.Serial))
            {
                if (prefixWithAnd || queryText.Length > 0)
                {
                    queryText.Append(" and ");
                }

                queryText.Append($"{handpieceAlias}.[Serial] like @filterSerial");
                queryParameters.Add(new SqlParameter("filterSerial", SqlDbType.NVarChar) { Value = $"%{this.Serial}%" });
            }

            if (this.SpeedType.HasValue)
            {
                if (prefixWithAnd || queryText.Length > 0)
                {
                    queryText.Append(" and ");
                }

                queryText.Append($"{handpieceAlias}.[SpeedType] = @filterSpeedType");
                queryParameters.Add(new SqlParameter("filterSpeedType", SqlDbType.Int) { Value = (Int32)this.SpeedType.Value });
            }

            if (this.ReceivedFrom.HasValue)
            {
                if (prefixWithAnd || queryText.Length > 0)
                {
                    queryText.Append(" and ");
                }

                queryText.Append($"{jobAlias}.[Received] >= @filterReceivedFrom");
                queryParameters.Add(new SqlParameter("filterReceivedFrom", SqlDbType.DateTime2) { Value = this.ReceivedFrom.Value });
            }

            if (this.ReceivedTo.HasValue)
            {
                if (prefixWithAnd || queryText.Length > 0)
                {
                    queryText.Append(" and ");
                }

                queryText.Append($"{jobAlias}.[Received] <= @filterReceivedTo");
                queryParameters.Add(new SqlParameter("filterReceivedTo", SqlDbType.DateTime2) { Value = this.ReceivedTo.Value });
            }

            return (queryText.ToString(), queryParameters.ToArray());
        }
    }
}
