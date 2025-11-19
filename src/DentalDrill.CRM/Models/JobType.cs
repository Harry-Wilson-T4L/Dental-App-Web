using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DentalDrill.CRM.Models
{
    public class JobType
    {
        [Required]
        [MaxLength(20)]
        [Key]
        public String Id { get; set; }

        [Required]
        [MaxLength(200)]
        public String Name { get; set; }

        [Required]
        [MaxLength(50)]
        public String ShortName { get; set; }

        public ICollection<Job> Jobs { get; set; }
    }
}
