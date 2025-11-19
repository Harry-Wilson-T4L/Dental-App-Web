using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DentalDrill.CRM.Models
{
    public class Handpiece
    {
        public Guid Id { get; set; }

        public HandpieceStatus HandpieceStatus { get; set; }

        [Required]
        [MaxLength(100)]
        public String Brand { get; set; }

        [Required]
        [MaxLength(200)]
        public String MakeAndModel { get; set; }

        [MaxLength(200)]
        public String OldMakeAndModel { get; set; }

        [Required]
        [MaxLength(200)]
        public String Serial { get; set; }

        public ICollection<HandpieceComponent> Components { get; set; }

        public String ComponentsText { get; set; }

        public Guid ClientHandpieceId { get; set; }

        public ClientHandpiece ClientHandpiece { get; set; }

        public Int32 Rating { get; set; }

        public HandpieceSpeed SpeedType { get; set; }

        public Int32? Speed { get; set; }

        [StringLength(200)]
        public String Parts { get; set; }

        public HandpiecePartsVersion PartsVersion { get; set; }

        public ICollection<HandpieceRequiredPart> PartsRequired { get; set; }

        public HandpiecePartsStockStatus PartsOutOfStock { get; set; }

        public String PartsComment { get; set; }

        public Boolean PartsOrdered { get; set; }

        public Boolean PartsRestocked { get; set; }

        public String ProblemDescription { get; set; }

        public String DiagnosticReport { get; set; }

        public String DiagnosticOther { get; set; }

        public Guid? ReturnById { get; set; }

        public ReturnEstimate ReturnBy { get; set; }

        public Decimal? CostOfRepair { get; set; }

        public Guid JobId { get; set; }

        public Job Job { get; set; }

        public Int32 JobPosition { get; set; }

        public Guid? ServiceLevelId { get; set; }

        public ServiceLevel ServiceLevel { get; set; }

        public ICollection<HandpieceDiagnostic> SelectedDiagnosticCheckItems { get; set; }

        public ICollection<HandpieceImage> Images { get; set; }

        public Guid CreatorId { get; set; }

        public Employee Creator { get; set; }

        public Guid? EstimatedById { get; set; }

        public Employee EstimatedBy { get; set; }

        public DateTime? EstimatedOn { get; set; }

        public Guid? ApprovedById { get; set; }

        public Employee ApprovedBy { get; set; }

        public DateTime? ApprovedOn { get; set; }

        public Guid? RepairedById { get; set; }

        public Employee RepairedBy { get; set; }

        public DateTime? RepairedOn { get; set; }

        public String InternalComment { get; set; }

        public String PublicComment { get; set; }

        [MaxLength(200)]
        public String ImportKey { get; set; }

        public DateTime? CompletedOn { get; set; }

        public ICollection<HandpieceChange> Changes { get; set; }
    }
}
