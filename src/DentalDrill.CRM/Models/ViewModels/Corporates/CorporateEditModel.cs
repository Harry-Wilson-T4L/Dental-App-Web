using System;
using System.ComponentModel.DataAnnotations;
using DevGuild.AspNetCore.ObjectModel;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DentalDrill.CRM.Models.ViewModels.Corporates
{
    public class CorporateEditModel : IEditModelOriginalEntity<Corporate>
    {
        [BindNever]
        public Corporate Original { get; set; }

        [Required]
        [MaxLength(100)]
        public String Name { get; set; }
    }
}
