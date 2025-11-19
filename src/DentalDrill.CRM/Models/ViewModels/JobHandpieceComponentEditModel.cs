using System;
using System.ComponentModel.DataAnnotations;

namespace DentalDrill.CRM.Models.ViewModels
{
    public class JobHandpieceComponentEditModel
    {
        [Required(ErrorMessage = "Field is required")]
        [Display(Name = "Brand")]
        public String Brand { get; set; }

        [Required(ErrorMessage = "Field is required")]
        [Display(Name = "Model")]
        public String MakeAndModel { get; set; }

        [Required(ErrorMessage = "Field is required")]
        [Display(Name = "Serial #")]
        public String Serial { get; set; }
    }
}
