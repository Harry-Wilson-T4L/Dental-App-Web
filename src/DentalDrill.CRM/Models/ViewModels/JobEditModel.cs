using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using DentalDrill.CRM.Domain.Abstractions;
using DentalDrill.CRM.Models.Permissions;
using DentalDrill.CRM.Models.Workflow;
using DevGuild.AspNetCore.ObjectModel;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DentalDrill.CRM.Models.ViewModels
{
    public class JobEditModel : IEditModelOriginalEntity<Job>
    {
        [BindNever]
        public Job Original { get; set; }

        [BindNever]
        public List<Workshop> Workshops { get; set; }

        [BindNever]
        public List<Client> Clients { get; set; }

        [BindNever]
        public JobActionsList ActionsList { get; set; }

        [BindNever]
        public PropertiesAccessControlList<Job> AccessControl { get; set; }

        [BindNever]
        public IEmployeeAccess Access { get; set; }

        [Display(Name = "Job Type")]
        public JobType JobType { get; set; }

        [Display(Name = "#")]
        public Int64 JobNumber { get; set; }

        [Display(Name = "Workshop")]
        public Guid? WorkshopId { get; set; }

        [Display(Name = "Client")]
        public Guid? ClientId { get; set; }

        [BindNever]
        public Client SelectedClient { get; set; }

        [Display(Name = "Received")]
        public DateTime Received { get; set; }

        [Display(Name = "Comment")]
        public String Comment { get; set; }

        [Display(Name = "Has warning")]
        public Boolean HasWarning { get; set; }

        [Display(Name = "Approved by")]
        public String ApprovedBy { get; set; }

        [Display(Name = "Approved on")]
        public DateTime? ApprovedOn { get; set; }

        [Display(Name = "Rate plan")]
        public JobRatePlan RatePlan { get; set; }
    }
}
