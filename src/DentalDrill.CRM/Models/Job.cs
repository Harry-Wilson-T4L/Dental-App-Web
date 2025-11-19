using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace DentalDrill.CRM.Models
{
    public class Job
    {
        public Guid Id { get; set; }

        [Display(Name = "Workshop")]
        public Guid WorkshopId { get; set; }

        [Display(Name = "Workshop")]
        public Workshop Workshop { get; set; }

        [Required]
        public String JobTypeId { get; set; }

        public JobType JobType { get; set; }

        public Int64 JobNumber { get; set; }

        [Column(TypeName = "date")]
        [Display(Name = "Received")]
        public DateTime Received { get; set; }

        [Display(Name = "Client")]
        public Guid ClientId { get; set; }

        [Display(Name = "Client")]
        public Client Client { get; set; }

        [Display(Name = "Comment")]
        public String Comment { get; set; }

        [Display(Name = "Status")]
        public JobStatus Status { get; set; }

        [Display(Name = "Has warning")]
        public Boolean HasWarning { get; set; }

        [Display(Name = "Creator")]
        public Guid CreatorId { get; set; }

        [Display(Name = "Creator")]
        public Employee Creator { get; set; }

        [Display(Name = "Approved by")]
        public String ApprovedBy { get; set; }

        [Display(Name = "Approved on")]
        public DateTime? ApprovedOn { get; set; }

        [Display(Name = "Discount")]
        public JobRatePlan RatePlan { get; set; }

        public ICollection<Handpiece> Handpieces { get; set; }

        public ICollection<JobInvoice> Invoices { get; set; }

        public ICollection<JobChange> Changes { get; set; }

        public static String ComputeStatusConfig(JobStatus status, IEnumerable<Handpiece> handpieces)
        {
            var statusConfigBuilder = new StringBuilder();
            var jobIndicator = status.ToIndicatorValue();
            statusConfigBuilder.Append(jobIndicator);
            var statusesMap = handpieces
                .Where(x => !(x.HandpieceStatus == HandpieceStatus.TradeIn || x.HandpieceStatus == HandpieceStatus.ReturnUnrepaired || x.HandpieceStatus == HandpieceStatus.Unrepairable || x.HandpieceStatus == HandpieceStatus.Cancelled))
                .Select(x => new { Entity = x, Indicator = x.HandpieceStatus.ToInternalVisualisationNumber() })
                .GroupBy(x => x.Indicator)
                .ToDictionary(x => x.Key, x => x.Count());
            for (var i = 1; i <= 7; i++)
            {
                statusConfigBuilder.AppendFormat(";{0}", statusesMap.TryGetValue(i, out var count) ? count : 0);
            }

            return statusConfigBuilder.ToString();
        }

        public String ComputeStatusConfig()
        {
            return Job.ComputeStatusConfig(this.Status, this.Handpieces);
        }
    }
}
