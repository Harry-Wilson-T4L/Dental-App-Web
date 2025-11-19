using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DentalDrill.CRM.Models.ViewModels
{
    public class JobCreateModel
    {
        [BindNever]
        public List<Client> Clients { get; set; }

        [BindNever]
        public JobType Type { get; set; }

        [BindNever]
        [Display(Name = "Expected #")]
        public Int64 ExpectedId { get; set; }

        [Required]
        [Display(Name = "Workshop")]
        public Guid? WorkshopId { get; set; }

        [Required]
        [Display(Name = "Client")]
        public Guid? ClientId { get; set; }

        [Display(Name = "Received")]
        public DateTime Received { get; set; }

        [Display(Name = "Comment")]
        public String Comment { get; set; }

        [Display(Name = "Has warning")]
        public Boolean HasWarning { get; set; }

        // JSON-encoded list of handpieces
        public String Handpieces { get; set; }

        [BindNever]
        public List<Workshop> Workshops { get; set; }

        [BindNever]
        public List<JobCreateModelHandpiece> HandpieceList { get; set; } = new();
    }
}
