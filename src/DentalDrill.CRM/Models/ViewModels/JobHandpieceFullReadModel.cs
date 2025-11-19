using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DentalDrill.CRM.Models.ViewModels
{
    public class JobHandpieceFullReadModel
    {
        public Guid Id { get; set; }

        public Guid JobId { get; set; }

        public String JobTypeId { get; set; }

        public String JobTypeName { get; set; }

        public Int64 JobNumber { get; set; }

        public String JobNumberString { get; set; }

        public Guid WorkshopId { get; set; }

        public String WorkshopName { get; set; }

        public Guid ClientId { get; set; }

        public String ClientName { get; set; }

        [Display(Name = "Brand")]
        public String Brand { get; set; }

        [Display(Name = "Model")]
        public String MakeAndModel { get; set; }

        [Display(Name = "Serial")]
        public String Serial { get; set; }

        public IEnumerable<HandpieceComponent> Components { get; set; }

        [Display(Name = "Diagnostic Report")]
        public String DiagnosticReport { get; set; }

        public Guid? ServiceLevelId { get; set; }

        [Display(Name = "Service level")]
        public String ServiceLevelName { get; set; }

        public JobStatus JobStatus { get; set; }

        public String JobStatusName { get; set; }

        public HandpieceStatus HandpieceStatus { get; set; }

        [Display(Name = "Status")]
        public String HandpieceStatusName { get; set; }

        [Display(Name = "Rating")]
        public Int32 Rating { get; set; }

        [Display(Name = "Received")]
        public DateTime Received { get; set; }

        [Display(Name = "Estimated by")]
        public String EstimatedBy { get; set; }

        [Display(Name = "Estimated by")]
        public String EstimatedByFullName { get; set; }

        [Display(Name = "Repaired by")]
        public String RepairedBy { get; set; }

        [Display(Name = "Repaired by")]
        public String RepairedByFullName { get; set; }

        public HandpieceSpeed SpeedType { get; set; }

        public String SpeedTypeName { get; set; }

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
