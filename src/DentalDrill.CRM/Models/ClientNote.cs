using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DentalDrill.CRM.Models
{
    public class ClientNote : IClientDependent
    {
        public Guid Id { get; set; }

        [Display(Name = "Created on")]
        public DateTime Created { get; set; }

        [Required]
        [Display(Name = "Note")]
        public String Text { get; set; }

        [Display(Name = "DDS Staff")]
        public Guid CreatorId { get; set; }

        [Display(Name = "DDS Staff")]
        public Employee Creator { get; set; }

        public Guid ClientId { get; set; }

        public Client Client { get; set; }
    }
}
