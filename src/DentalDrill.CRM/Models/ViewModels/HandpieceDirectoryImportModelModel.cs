using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DentalDrill.CRM.Models.ViewModels
{
    public class HandpieceDirectoryImportModelModel
    {
        public String NormalizedName { get; set; }

        public Boolean IsImported { get; set; }

        public String CanonicalName { get; set; }

        [BindNever]
        public List<String> AlternateNames { get; set; }

        [BindNever]
        public HandpieceModel ExistingModel { get; set; }
    }
}