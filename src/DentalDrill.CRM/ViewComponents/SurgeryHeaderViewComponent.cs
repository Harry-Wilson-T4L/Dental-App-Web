using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DentalDrill.CRM.Models;
using DentalDrill.CRM.Models.ComponentModels;
using DentalDrill.CRM.Services;
using DevGuild.AspNetCore.Services.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DentalDrill.CRM.ViewComponents
{
    public class SurgeryHeaderViewComponent : ViewComponent
    {
        private readonly IRepository repository;
        private readonly UserEntityResolver userResolver;

        public SurgeryHeaderViewComponent(IRepository repository, UserEntityResolver userResolver)
        {
            this.repository = repository;
            this.userResolver = userResolver;
        }

        public async Task<IViewComponentResult> InvokeAsync(Guid? specificClient, Guid? specificCorporate)
        {
            var user = await this.userResolver.ResolveCurrentUserEntity();
            Guid? clientId = null;
            Guid? corporateId = null;
            switch (user)
            {
                case Client client:
                    clientId = client.Id;
                    break;
                case Corporate corporate:
                    corporateId = corporate.Id;
                    break;
                case Administrator _:
                case Employee _:
                    clientId = specificClient;
                    corporateId = specificCorporate;
                    break;
                default:
                    throw new InvalidOperationException("Unable to determine client");
            }

            if (corporateId != null)
            {
                var resolvedCorporate = await this.repository.QueryWithoutTracking<Corporate>().SingleAsync(x => x.Id == corporateId);
                var appearance = await this.repository.QueryWithoutTracking<CorporateAppearance>("Logo").SingleOrDefaultAsync(x => x.CorporateId == corporateId);

                var model = new CorporateSurgeryHeaderViewComponentModel
                {
                    Corporate = resolvedCorporate,
                    CorporateAppearance = appearance ?? CorporateAppearance.Default(),
                };

                return this.View("Corporate", model);
            }
            else if (clientId != null)
            {
                var resolvedClient = await this.repository.QueryWithoutTracking<Client>().SingleAsync(x => x.Id == clientId);
                var appearance = await this.repository.QueryWithoutTracking<ClientAppearance>("Logo").SingleOrDefaultAsync(x => x.ClientId == clientId);

                var model = new SurgeryHeaderViewComponentModel
                {
                    Client = resolvedClient,
                    ClientAppearance = appearance ?? ClientAppearance.Default(),
                };

                return this.View(model);
            }
            else
            {
                throw new InvalidOperationException("Unable to resolve neither client nor corporate");
            }
        }
    }
}
