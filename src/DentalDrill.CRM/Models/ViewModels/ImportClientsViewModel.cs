using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DentalDrill.CRM.Models.Import;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DentalDrill.CRM.Models.ViewModels
{
    public class ImportClientsViewModel
    {
        public IFormFile File { get; set; }

        public IFormFile MappingFile { get; set; }

        public String Confirmation { get; set; }

        [BindNever]
        public IReadOnlyList<ClientImportModel> ImportedClients { get; set; }
    }
}
