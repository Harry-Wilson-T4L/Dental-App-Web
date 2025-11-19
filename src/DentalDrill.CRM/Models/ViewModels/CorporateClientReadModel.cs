using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using DevGuild.AspNetCore.Services.ModelMapping.Annotations;

namespace DentalDrill.CRM.Models.ViewModels
{
    public class CorporateClientReadModel
    {
        public Guid Id { get; set; }

        public Guid? CorporateId { get; set; }

        [Display(Name = "Zone")]
        public Guid ZoneId { get; set; }

        [Display(Name = "Zone")]
        public String ZoneName { get; set; }

        [Display(Name = "Name")]
        public String Name { get; set; }

        [Display(Name = "Dentist")]
        public String PrincipalDentist { get; set; }

        [Display(Name = "Phone")]
        public String Phone { get; set; }

        [Display(Name = "Address")]
        public String Address { get; set; }

        [Display(Name = "Suburb")]
        public String Suburb { get; set; }

        [Display(Name = "State")]
        public Guid StateId { get; set; }

        [Display(Name = "State")]
        public String StateName { get; set; }

        [Display(Name = "Post")]
        public String Postcode { get; set; }

        [Display(Name = "Email")]
        public String Email { get; set; }

        [Display(Name = "#HPs")]
        public Int32 HandpiecesCount { get; set; }

        [Display(Name = "Rtg")]
        public Decimal? AverageRating { get; set; }
    }
}
