using System;
using System.ComponentModel.DataAnnotations;
using DevGuild.AspNetCore.ObjectModel;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DentalDrill.CRM.Models.ViewModels
{
    public class ClientNoteEditViewModel : IEditModelOriginalEntity<ClientNote>
    {
        [BindNever]
        public Client Parent { get; set; }

        [BindNever]
        public ClientNote Original { get; set; }

        public Guid Id { get; set; }

        [Required]
        [Display(Name = "Note")]
        public String Text { get; set; }
    }
}
