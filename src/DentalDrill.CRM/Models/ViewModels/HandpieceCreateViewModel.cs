using System;
using System.ComponentModel.DataAnnotations;

namespace DentalDrill.CRM.Models.ViewModels
{
    public class HandpieceCreateViewModel
    {
        [Display(Name = "#")]
        public Int32 Position { get; set; }

        public Int32? PositionSuffix { get; set; }

        [Display(Name = "Brand")]
        [Required]
        public String Brand { get; set; }

        [Display(Name = "Model")]
        [Required]
        public String MakeAndModel { get; set; }

        [Display(Name = "Serial #")]
        [Required]
        public String Serial { get; set; }

        [Display(Name = "Problem")]
        public String ProblemDescription { get; set; }
    }
}
