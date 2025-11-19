using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using DevGuild.AspNetCore.Services.ModelMapping.Annotations;

namespace DentalDrill.CRM.Models.ViewModels
{
    public class RepairDetailsViewModel
    {
        [PropertyMapping(PropertyMappingMode.ToDetails)]
        public Guid Id { get; set; }

        [Display(Name = "Estimate #")]
        public Int64 JobNumber { get; set; }

        public Int32 StatusNumber { get; set; }

        public String StatusDescription { get; set; }

        [Display(Name = "Client")]
        public String ClientName { get; set; }

        [Display(Name = "Make & Model")]
        public String MakeAndModel { get; set; }

        [Display(Name = "Serial #")]
        public String Serial { get; set; }

        [Display(Name = "Service Level")]
        public String ServiceLevel { get; set; }

        [Display(Name = "Received Date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime Received { get; set; }

        [Display(Name = "Rating")]
        public Int32 Rating { get; set; }

        [Display(Name = "Problem Description")]
        public String ProblemDescription { get; set; }

        [Display(Name = "Parts #")]
        public String Parts { get; set; }

        [Display(Name = "Diagnostic Report")]
        public String DiagnosticReport { get; set; }

        [Display(Name = "Comment / Approved By")]
        public String Comment { get; set; }

        [Display(Name = "Turnaround (days)")]
        public Int32 TurnAround { get; set; }

        [Display(Name = "Cost of repair")]
        public Decimal CostOfRepair { get; set; }
    }
}
