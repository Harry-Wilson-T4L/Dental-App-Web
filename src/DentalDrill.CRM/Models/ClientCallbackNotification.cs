using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DentalDrill.CRM.Models
{
    public enum ClientCallbackNotificationStatus
    {
        New,
        Done,
        Active,
    }

    public class ClientCallbackNotification
    {
        public Guid Id { get; set; }

        [Display(Name = "Surgery")]
        public Guid ClientId { get; set; }

        [Display(Name = "Surgery")]
        public Client Client { get; set; }

        [Display(Name = "Created By")]
        public Guid CreatedById { get; set; }

        [Display(Name = "Created By")]
        public Employee CreatedBy { get; set; }

        [Display(Name = "Assigned To")]
        public Guid? AssignedToId { get; set; }

        [Display(Name = "Assigned To")]
        public Employee AssignedTo { get; set; }

        [Display(Name = "Status")]
        public ClientCallbackNotificationStatus Status { get; set; }

        [Display(Name = "Created On")]
        public DateTime CreatedOn { get; set; }

        [Display(Name = "Completed On")]
        public DateTime? CompletedOn { get; set; }

        [Display(Name = "Call Date Time")]
        public DateTime? CallDateTime { get; set; }

        [Display(Name = "Note")]
        public String Note { get; set; }
    }
}
