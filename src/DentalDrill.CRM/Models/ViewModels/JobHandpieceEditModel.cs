using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using DentalDrill.CRM.Domain.Abstractions;
using DentalDrill.CRM.ModelBinding;
using DentalDrill.CRM.Models.Permissions;
using DentalDrill.CRM.Models.ViewModels.HandpieceRequiredParts;
using DevGuild.AspNetCore.ObjectModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DentalDrill.CRM.Models.ViewModels
{
    public class JobHandpieceEditModel : IEditModelOriginalEntity<HandpieceDetailsModel>
    {
        [BindNever]
        public Job Parent { get; set; }

        [BindNever]
        public HandpieceDetailsModel Original { get; set; }

        [BindNever]
        public List<JobHandpieceServiceLevel> ServiceLevels { get; set; }

        [BindNever]
        public List<ReturnEstimate> ReturnEstimates { get; set; }

        [BindNever]
        public IEmployeeAccess Access { get; set; }

        [BindNever]
        public PropertiesAccessControlList<Handpiece> PropertiesAccessControl { get; set; }

        [BindNever]
        public List<HandpieceRequiredPartReadModel> RequiredParts { get; set; }

        [BindNever]
        public Dictionary<Guid, DiagnosticCheckItem> AllDiagnosticCheckItems { get; set; }

        public Guid? ClientHandpieceId { get; set; }

        public String Brand { get; set; }

        [Display(Name = "Model")]
        public String MakeAndModel { get; set; }

        [Display(Name = "Serial #")]
        public String Serial { get; set; }

        public List<JobHandpieceComponentEditModel> Components { get; set; } = new List<JobHandpieceComponentEditModel>();

        [Display(Name = "Service Level")]
        public Guid? ServiceLevelId { get; set; }

        [Display(Name = "Rating")]
        public Int32 Rating { get; set; }

        [Display(Name = "Problem Description")]
        public String ProblemDescription { get; set; }

        public List<Guid> DiagnosticReportChecked { get; set; } = new List<Guid>();

        public String DiagnosticReportOther { get; set; }

        [Display(Name = "Parts #")]
        public String Parts { get; set; }

        [Display(Name = "Parts stock")]
        public HandpiecePartsStockStatus PartsOutOfStock { get; set; }

        [Display(Name = "Parts Comment")]
        public String PartsComment { get; set; }

        [Display(Name = "Ordered")]
        public Boolean PartsOrdered { get; set; }

        [Display(Name = "Restocked")]
        public Boolean PartsRestocked { get; set; }

        [Display(Name = "Turnaround")]
        public Guid? ReturnById { get; set; }

        [Display(Name = "Cost")]
        public Decimal? CostOfRepair { get; set; }

        [Display(Name = "Speed (type)")]
        public HandpieceSpeed SpeedType { get; set; }

        [Display(Name = "RPM")]
        public Int32? Speed { get; set; }

        [Display(Name = "Internal comment")]
        public String InternalComment { get; set; }

        [Display(Name = "Public comment")]
        public String PublicComment { get; set; }

        public HandpieceStatus? ChangeStatus { get; set; }

        public IReadOnlyList<HandpieceStatus> PossibleStatuses { get; set; }
    }
}
