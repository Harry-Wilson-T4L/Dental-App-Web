using System;
using System.ComponentModel.DataAnnotations;

namespace DentalDrill.CRM.Models
{
    public class ClientRepairedItemOverride
    {
        public Guid Id { get; set; }

        public Guid ClientId { get; set; }

        public Client Client { get; set; }

        [Required]
        [MaxLength(100)]
        public String Brand { get; set; }

        [Required]
        [MaxLength(200)]
        public String Model { get; set; }

        [Required]
        [MaxLength(200)]
        public String Serial { get; set; }

        public Boolean Disabled { get; set; }

        public Int32 RemindersCount { get; set; }

        public Guid? RemindersLastHandpieceId { get; set; }

        public DateTime? RemindersLastDateTime { get; set; }

        public Int32 TotalRemindersCount { get; set; }
    }
}
