using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DentalDrill.CRM.Models.ViewModels
{
    public class HandpieceImagePreviewModel
    {
        [BindNever]
        public List<HandpieceImageViewModel> Images { get; set; }

        [BindNever]
        public Guid? SelectedImage { get; set; }
    }
}
