using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DentalDrill.CRM.Models
{
    public class Corporate
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(100)]
        public String Name { get; set; }

        public ICollection<Client> Clients { get; set; }

        public Guid? UserId { get; set; }

        public ApplicationUser User { get; set; }

        public Int32 CorporateNo { get; set; }

        [Required]
        [MaxLength(200)]
        public String UrlPath { get; set; }
    }
}
