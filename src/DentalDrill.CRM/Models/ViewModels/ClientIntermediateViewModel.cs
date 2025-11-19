using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using DevGuild.AspNetCore.Services.ModelMapping.Annotations;

namespace DentalDrill.CRM.Models.ViewModels
{
    public class ClientIntermediateViewModel
    {
        [PropertyMapping(PropertyMappingMode.ToDetails)]
        public Guid Id { get; set; }

        [Display(Name = "Zone")]
        public String Zone { get; set; }

        [Display(Name = "Surgery Name")]
        public String Name { get; set; }

        [Display(Name = "Principal Dentist")]
        public String PrincipalDentist { get; set; }

        [Display(Name = "Phone")]
        public String Phone { get; set; }

        [Display(Name = "Address")]
        public String Address { get; set; }

        [Display(Name = "Suburb")]
        public String Suburb { get; set; }

        [Display(Name = "State")]
        public String State { get; set; }

        public String Postcode { get; set; }

        [Display(Name = "Email")]
        public String Email { get; set; }

        [Display(Name = "Number Of HPs")]
        public IEnumerable<Handpiece> Handpieces { get; set; }

        public Int32 TotalRating { get; set; }

        public Int32 CountRating { get; set; }
    }
}
