using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DentalDrill.CRM.Models.ViewModels
{
    public class JobsGenerateViewModel
    {
        [BindNever]
        public List<Corporate> Corporates { get; set; }

        [BindNever]
        public List<Client> Clients { get; set; }

        [BindNever]
        public List<Employee> Employees { get; set; }

        public List<Guid> SelectedCorporates { get; set; } = new List<Guid>();

        public List<Guid> SelectedClients { get; set; } = new List<Guid>();

        public Guid SelectedEmployee { get; set; }

        public Int32 Quantity { get; set; }

        public Int32 Year { get; set; }

        public String BrandsConfig { get; set; }

        public String CostsConfig { get; set; }

        public String RatingsConfig { get; set; }

        public String StatusesConfig { get; set; }

        public String DatesConfig { get; set; }

        public IFormFile ImagesArchive { get; set; }
    }
}
