using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DentalDrill.CRM.Models.ViewModels
{
    public class ClientNoteViewModel
    {
        public Guid Id { get; set; }

        [Display(Name = "Created on")]
        public DateTime Created { get; set; }

        [Display(Name = "Note")]
        public String Text { get; set; }

        [Display(Name = "DDS Staff")]
        public String CreatorName { get; set; }
    }
}
