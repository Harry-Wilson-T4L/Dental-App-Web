using System.Diagnostics;
using System.Threading.Tasks;
using DentalDrill.CRM.Models;
using DentalDrill.CRM.Models.ViewModels;
using DentalDrill.CRM.Services;
using DevGuild.AspNetCore.Services.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DentalDrill.CRM.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserEntityResolver userEntityResolver;
        private readonly IRepository repository;

        public HomeController(UserEntityResolver userEntityResolver, IRepository repository)
        {
            this.userEntityResolver = userEntityResolver;
            this.repository = repository;
        }

        public async Task<IActionResult> Index()
        {
            var userEntity = await this.userEntityResolver.ResolveCurrentUserEntity();
            switch (userEntity)
            {
                case Administrator _:
                    return this.RedirectToAction("Index", "Clients");
                case Employee employee:
                    var access = await this.userEntityResolver.GetEmployeeAccessAsync();
                    if (access.CanAccessClients())
                    {
                        return this.RedirectToAction("Index", "Clients");
                    }

                    if (access.CanAccessWorkshops())
                    {
                        return this.RedirectToAction("Index", "Jobs");
                    }

                    if (access.CanAccessInventory())
                    {
                        return this.RedirectToAction("Index", "InventoryMovements");
                    }

                    return this.View();

                case Corporate corporate:
                    return this.RedirectToAction("Index", "CorporateSurgeries", new { id = corporate.UrlPath });

                case Client client:
                    return this.RedirectToAction("Index", "Surgeries", new { id = client.UrlPath });

                default:
                    // return this.NotFound();
                    return this.View();
            }
        }

        [Authorize]
        public async Task<IActionResult> SurgeryTutorials()
        {
            var page = await this.repository.QueryWithoutTracking<TutorialPage>()
                .Include(x => x.Videos)
                .SingleOrDefaultAsync(x => x.Id == "Surgery");
            if (page == null)
            {
                return this.NotFound();
            }

            return this.View("Tutorials", page);
        }

        [Authorize]
        public async Task<IActionResult> HubTutorial()
        {
            var page = await this.repository.QueryWithoutTracking<TutorialPage>()
                .Include(x => x.Videos)
                .SingleOrDefaultAsync(x => x.Id == "Hub");
            if (page == null)
            {
                return this.NotFound();
            }

            return this.View("Tutorials", page);
        }

        public IActionResult Error()
        {
            return this.View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
        }
    }
}
