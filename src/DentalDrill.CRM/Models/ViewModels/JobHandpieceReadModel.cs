using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DentalDrill.CRM.Models.ViewModels
{
    public class JobHandpieceReadModel
    {
        public Guid Id { get; set; }

        public Guid JobId { get; set; }

        public Int32 JobPosition { get; set; }

        [Display(Name = "Brand")]
        public String Brand { get; set; }

        [Display(Name = "Make & Model")]
        public String MakeAndModel { get; set; }

        [Display(Name = "Serial")]
        public String Serial { get; set; }

        public List<JobHandpieceComponentEditModel> Components { get; set; } = new List<JobHandpieceComponentEditModel>();

        [Display(Name = "Diagnostic Report")]
        public String DiagnosticReport { get; set; }

        [Display(Name = "Service level")]
        public String ServiceLevel { get; set; }

        public HandpieceStatus StatusId { get; set; }

        [Display(Name = "Status")]
        public String Status { get; set; }

        [Display(Name = "Rating")]
        public Int32 Rating { get; set; }

        [Display(Name = "Estimated by")]
        public String EstimatedBy { get; set; }

        [Display(Name = "Repaired by")]
        public String RepairedBy { get; set; }

        [Display(Name = "Parts")]
        public String Parts { get; set; }

        [Display(Name = "Parts out of stock")]
        public HandpiecePartsStockStatus PartsOutOfStock { get; set; }

        [Display(Name = "Cost")]
        public Decimal CostOfRepair { get; set; }

        [Display(Name = "Int. comment")]
        public String InternalComment { get; set; }
    }
}
