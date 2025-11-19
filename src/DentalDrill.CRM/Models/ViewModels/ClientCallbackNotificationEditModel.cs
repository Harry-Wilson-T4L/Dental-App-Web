using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using DevGuild.AspNetCore.ObjectModel;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DentalDrill.CRM.Models.ViewModels
{
    public class ClientCallbackNotificationEditModel : IEditModelOriginalEntity<ClientCallbackNotification>
    {
        [BindNever]
        public Client Parent { get; set; }

        [BindNever]
        public ClientCallbackNotification Original { get; set; }

        [BindNever]
        public List<Employee> Employees { get; set; }

        [Display(Name = "Assigned To")]
        public Guid? AssignedToId { get; set; }

        [Display(Name = "Callback on")]
        public DateTime? CallDateTime { get; set; }

        [Display(Name = "Note")]
        public String Note { get; set; }
    }
}
