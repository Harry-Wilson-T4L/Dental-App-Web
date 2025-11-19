using System;
using System.ComponentModel.DataAnnotations;

namespace DentalDrill.CRM.Models.ViewModels
{
    public class ClientCallbackNotificationReadModel
    {
        public Guid Id { get; set; }

        [Display(Name = "Created On")]
        public DateTime CreatedOn { get; set; }

        [Display(Name = "Assigned To")]
        public String AssignedToName { get; set; }

        [Display(Name = "Date and time of the call")]
        public DateTime? CallDateTime { get; set; }

        [Display(Name = "Note")]
        public String Note { get; set; }

        [Display(Name = "Created By")]
        public String CreatedByName { get; set; }
    }
}