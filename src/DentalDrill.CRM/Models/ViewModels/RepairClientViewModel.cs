using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using DevGuild.AspNetCore.Services.ModelMapping.Annotations;

namespace DentalDrill.CRM.Models.ViewModels
{
    public class RepairClientViewModel
    {
        [PropertyMapping(PropertyMappingMode.ToDetails)]
        public Guid Id { get; set; }

        [Display(Name = "SID")]
        public Int64 JobNumber { get; set; }

        [Display(Name = "Model")]
        public String MakeAndModel { get; set; }

        [Display(Name = "Serial #")]
        public String Serial { get; set; }

        [Display(Name = "Diagnostic Report")]
        public String DiagnosticReport { get; set; }

        [Display(Name = "Service")]
        public String Service { get; set; }

        [Display(Name = "Status")]
        public HandpieceStatus HandpieceStatus { get; set; }

        [Display(Name = "Rating")]
        public Int32 Rating { get; set; }

        [Display(Name = "Received date")]
        public DateTime Received { get; set; }
    }
}
