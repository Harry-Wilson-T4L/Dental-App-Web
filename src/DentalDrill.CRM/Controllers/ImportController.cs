using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DentalDrill.CRM.Models;
using DentalDrill.CRM.Models.ViewModels;
using DentalDrill.CRM.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DentalDrill.CRM.Controllers
{
    [Authorize(Roles = ApplicationRoles.Combined.Administrator)]
    public class ImportController : Controller
    {
        private readonly ImportService importService;

        public ImportController(ImportService importService)
        {
            this.importService = importService;
        }

        public IActionResult Index()
        {
            return this.View();
        }

        public IActionResult ACT()
        {
            return this.View(new ImportClientsViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ACT(ImportClientsViewModel model)
        {
            if (model.File == null || model.MappingFile == null)
            {
                return this.View(model);
            }

            using (var stream = model.File.OpenReadStream())
            using (var numbersStream = model.MappingFile.OpenReadStream())
            {
                var clients = this.importService.ParseClients(stream)
                    .Where(x => !String.IsNullOrEmpty(x.Contact) || !String.IsNullOrEmpty(x.Company) || !String.IsNullOrEmpty(x.City))
                    .ToList();
                var numbers = this.importService.ParseClientNumbers(numbersStream);

                foreach (var client in clients)
                {
                    if (numbers.TryGetValue(client.UniqueId, out var clientNumber))
                    {
                        client.ClientNumber = clientNumber;
                    }
                    else
                    {
                        client.ClientNumber = null;
                    }
                }

                model.ImportedClients = clients;

                if (model.Confirmation == "Confirm")
                {
                    await this.importService.ExecuteClientsImport(clients);
                }
            }

            return this.View(model);
        }

        public IActionResult Handpieces()
        {
            return this.View(new ImportHandpiecesViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Handpieces(ImportHandpiecesViewModel model)
        {
            if (model.File == null || model.MappingFile == null || model.TechsMappingFile == null || model.RatingsMappingFile == null)
            {
                return this.View(model);
            }

            using (var stream = model.File.OpenReadStream())
            using (var mappingStream = model.MappingFile.OpenReadStream())
            using (var techsMappingStream = model.TechsMappingFile.OpenReadStream())
            using (var ratingsStream = model.RatingsMappingFile.OpenReadStream())
            {
                var handpieces = this.importService.ParseHandpieces(stream);
                handpieces = await this.importService.ResolveClients(handpieces, mappingStream);
                handpieces = this.importService.FilterClientlessHandpieces(handpieces);
                handpieces = await this.importService.ResolveTechs(handpieces, techsMappingStream);
                handpieces = await this.importService.ResolveRating(handpieces, ratingsStream);
                handpieces = this.importService.ResolveDates(handpieces);

                model.ImportedHandpieces = handpieces;
                model.ImportedJobs = this.importService.GroupIntoJobs(handpieces);

                if (model.Confirmation == "Confirm")
                {
                    await this.importService.ExecuteHandpiecesImport(model.ImportedJobs);
                }
            }

            return this.View(model);
        }
    }
}
