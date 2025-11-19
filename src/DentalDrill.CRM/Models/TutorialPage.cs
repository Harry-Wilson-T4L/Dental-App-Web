using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DentalDrill.CRM.Models
{
    public class TutorialPage
    {
        [Required]
        [MaxLength(50)]
        public String Id { get; set; }

        [Required]
        [MaxLength(200)]
        public String Title { get; set; }

        public ICollection<TutorialVideo> Videos { get; set; }
    }
}
