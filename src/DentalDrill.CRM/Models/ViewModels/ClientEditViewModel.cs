using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DentalDrill.CRM.Domain.Abstractions;
using DentalDrill.CRM.Models.Annotations;
using DevGuild.AspNetCore.ObjectModel;
using DevGuild.AspNetCore.Services.ModelMapping.Annotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DentalDrill.CRM.Models.ViewModels
{
    public class ClientEditViewModel : IEditModelOriginalEntity<Client>
    {
        [BindNever]
        public Client Original { get; set; }

        [BindNever]
        public IEmployeeAccessClient Access { get; set; }

        [BindNever]
        public List<SelectItemViewModel> States { get; set; }

        [BindNever]
        public List<SelectItemViewModel> Zones { get; set; }

        [BindNever]
        public List<Corporate> Corporates { get; set; }

        [BindNever]
        public List<CorporatePricingCategory> PricingCategories { get; set; }

        [BindNever]
        public List<Workshop> Workshops { get; set; }

        [Required]
        [Display(Name = "Client #")]
        public Int32? ClientNo { get; set; }

        [Required]
        [Display(Name = "Surgery Name")]
        public String Name { get; set; }

        [Display(Name = "Other contact")]
        public String OtherContact { get; set; }

        [EmailAddress]
        [Display(Name = "Main email")]
        public String Email { get; set; }

        [EmailList]
        [Display(Name = "Other emails")]
        public String SecondaryEmails { get; set; }

        [Display(Name = "Main phone")]
        public String Phone { get; set; }

        [Display(Name = "Other phones")]
        public String SecondaryPhones { get; set; }

        [Display(Name = "Address")]
        public String Address { get; set; }

        [Display(Name = "Suburb")]
        public String Suburb { get; set; }

        [MaxLength(4)]
        [MinLength(4)]
        [Display(Name = "Postal")]
        public String PostCode { get; set; }

        [Display(Name = "Princ. dentist")]
        public String PrincipalDentist { get; set; }

        [Display(Name = "Opening hours")]
        public String OpeningHours { get; set; }

        [Display(Name = "Brands")]
        public String Brands { get; set; }

        [Display(Name = "Comment")]
        public String Comment { get; set; }

        [Display(Name = "State")]
        public Guid StateId { get; set; }

        [Display(Name = "Zone")]
        public Guid ZoneId { get; set; }

        [Display(Name = "Corporate")]
        public Guid? CorporateId { get; set; }

        [Display(Name = "Pricing category")]
        public Guid? PricingCategoryId { get; set; }

        [Display(Name = "Primary workshop")]
        public Guid? PrimaryWorkshopId { get; set; }
    }
}
