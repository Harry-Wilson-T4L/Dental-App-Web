using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DentalDrill.CRM.Models.Import;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DentalDrill.CRM.Models.ViewModels
{
    public class ImportHandpiecesViewModel
    {
        public IFormFile File { get; set; }

        public IFormFile MappingFile { get; set; }

        public IFormFile TechsMappingFile { get; set; }

        public IFormFile RatingsMappingFile { get; set; }

        public String Confirmation { get; set; }

        [BindNever]
        public IReadOnlyList<HandpieceImportModel> ImportedHandpieces { get; set; }

        [BindNever]
        public IReadOnlyList<JobImportModel> ImportedJobs { get; set; }
    }
}
