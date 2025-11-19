using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DentalDrill.CRM.Models.ViewModels
{
    public class OrderCourierAustraliaViewModel
    {
        [Display(Name = "Surgery")]
        public Int32? ClientNo { get; set; }

        [BindNever]
        [Display(Name = "Surgery")]
        public Client ClientEntity { get; set; }

        [Required]
        [MaxLength(100)]
        [Display(Name = "Practice Name")]
        public String PracticeName { get; set; }

        [Required]
        [MaxLength(100)]
        [Display(Name = "Contact Person")]
        public String ContactPerson { get; set; }

        [Required]
        [MaxLength(256)]
        [EmailAddress]
        [Display(Name = "Email")]
        public String Email { get; set; }

        [Required]
        [MaxLength(20)]
        [Display(Name = "Phone")]
        public String Phone { get; set; }

        [Required]
        [MaxLength(30)]
        [Display(Name = "Pick-up Address")]
        public String AddressLine1 { get; set; }

        [MaxLength(30)]
        [Display(Name = "Address Line 2")]
        public String AddressLine2 { get; set; }

        [Required]
        [MaxLength(50)]
        [Display(Name = "Suburb")]
        public String Suburb { get; set; }

        [Required]
        [MaxLength(25)]
        [Display(Name = "State")]
        public String State { get; set; }

        [Required]
        [MaxLength(10)]
        [Display(Name = "Postcode")]
        public String Postcode { get; set; }

        [Required]
        [Display(Name = "No. of Handpieces")]
        public Int32? NumberOfHandpieces { get; set; }

        public Boolean PackageIsReady { get; set; }

        [MaxLength(90)]
        [Display(Name = "Any additional comments")]
        public String Comment { get; set; }
    }
}
