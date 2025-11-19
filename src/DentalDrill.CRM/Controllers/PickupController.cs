using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DentalDrill.CRM.Models;
using DentalDrill.CRM.Models.ViewModels;
using DentalDrill.CRM.Services;
using DevGuild.AspNetCore.Controllers.Mvc.Filters;
using DevGuild.AspNetCore.Services.Data;
using DevGuild.AspNetCore.Services.Identity;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DentalDrill.CRM.Controllers
{
    public class PickupController : Controller
    {
        private readonly IRepository repository;
        private readonly UserEntityResolver userResolver;
        private readonly IAuthenticatedUserAccessorService<ApplicationUser> userAccessor;

        public PickupController(IRepository repository, UserEntityResolver userResolver, IAuthenticatedUserAccessorService<ApplicationUser> userAccessor)
        {
            this.repository = repository;
            this.userResolver = userResolver;
            this.userAccessor = userAccessor;
        }

        public async Task<IActionResult> Index(Guid? client)
        {
            var clientEntity = client.HasValue ? await this.repository.Query<Client>().SingleOrDefaultAsync(x => x.Id == client) : null;
            var model = new OrderCourierCombinedWrapperModel
            {
                Selected = PickupRequestType.Australia,
            };

            if (clientEntity != null)
            {
                await this.FillFromEntity(model, clientEntity, asAdmin: true);
            }
            else
            {
                await this.FillRemainingAsync(model);
            }

            return this.View(model);
        }

        [AjaxPost]
        public async Task<IActionResult> ReadAvailableSurgeries([DataSourceRequest] DataSourceRequest request)
        {
            var query = this.repository.Query<Client>("State");

            var user = await this.userResolver.ResolveCurrentUserEntity();
            switch (user)
            {
                case Employee employee:
                case Administrator _:
                    break;
                case Corporate corporate:
                    query = query.Where(x => x.CorporateId == corporate.Id);
                    break;
                default:
                    return this.NotFound();
            }

            var result = await query.Select(x => new
            {
                x.ClientNo,
                x.FullName,
                x.Name,
                x.Address,
                x.PrincipalDentist,
                x.Suburb,
                x.PostCode,
                StateName = x.StateId != null ? x.State.Name : "",
                x.Email,
                x.Phone,
            }).ToDataSourceResultAsync(request);
            return this.Json(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> GreaterSydney(OrderCourierGreaterSydneyWrapperModel model)
        {
            if (this.ModelState.IsValid)
            {
                var request = await this.CreateBlankRequest(PickupRequestType.GreaterSydney);

                request.ClientId = await this.ResolveClientId(request.ClientId, model.GreaterSydney.ClientNo);
                request.PracticeName = model.GreaterSydney.PracticeName;
                request.ContactPerson = model.GreaterSydney.ContactPerson;
                request.Email = model.GreaterSydney.Email;
                request.Phone = model.GreaterSydney.Phone;
                request.AddressLine1 = model.GreaterSydney.AddressLine1;
                request.AddressLine2 = model.GreaterSydney.AddressLine2;
                request.Suburb = model.GreaterSydney.Suburb;
                request.State = "NSW";
                request.Postcode = model.GreaterSydney.Postcode;
                request.Country = "Australia";

                request.HandpiecesCount = model.GreaterSydney.NumberOfHandpieces ?? throw new InvalidOperationException();
                request.Comment = model.GreaterSydney.Comment;

                await this.repository.InsertAsync(request);
                await this.repository.SaveChangesAsync();

                return this.RedirectToAction("Success");
            }

            var combined = new OrderCourierCombinedWrapperModel
            {
                Selected = PickupRequestType.GreaterSydney,
                GreaterSydney = model.GreaterSydney,
            };

            await this.FillRemainingAsync(combined);
            return this.View("Index", combined);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Australia(OrderCourierAustraliaWrapperModel model)
        {
            if (!model.Australia.PackageIsReady)
            {
                this.ModelState.AddModelError("Australia.PackageIsReady", "Package must be ready for pickup");
            }

            if (this.ModelState.IsValid)
            {
                var request = await this.CreateBlankRequest(PickupRequestType.Australia);

                request.ClientId = await this.ResolveClientId(request.ClientId, model.Australia.ClientNo);
                request.PracticeName = model.Australia.PracticeName;
                request.ContactPerson = model.Australia.ContactPerson;
                request.Email = model.Australia.Email;
                request.Phone = model.Australia.Phone;
                request.AddressLine1 = model.Australia.AddressLine1;
                request.AddressLine2 = model.Australia.AddressLine2;
                request.Suburb = model.Australia.Suburb;
                request.State = model.Australia.State;
                request.Postcode = model.Australia.Postcode;
                request.Country = "Australia";

                request.HandpiecesCount = model.Australia.NumberOfHandpieces ?? throw new InvalidOperationException();
                request.Comment = model.Australia.Comment;

                await this.repository.InsertAsync(request);
                await this.repository.SaveChangesAsync();

                return this.RedirectToAction("Success");
            }

            var combined = new OrderCourierCombinedWrapperModel
            {
                Selected = PickupRequestType.Australia,
                Australia = model.Australia,
            };

            await this.FillRemainingAsync(combined);
            return this.View("Index", combined);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> NewZealand(OrderCourierNewZealandWrapperModel model)
        {
            if (!model.NewZealand.PackageIsReady)
            {
                this.ModelState.AddModelError("NewZealand.PackageIsReady", "Package must be ready for pickup");
            }

            if (this.ModelState.IsValid)
            {
                var request = await this.CreateBlankRequest(PickupRequestType.NewZealand);

                request.ClientId = await this.ResolveClientId(request.ClientId, model.NewZealand.ClientNo);
                request.PracticeName = model.NewZealand.PracticeName;
                request.ContactPerson = model.NewZealand.ContactPerson;
                request.Email = model.NewZealand.Email;
                request.Phone = model.NewZealand.Phone;
                request.AddressLine1 = model.NewZealand.AddressLine1;
                request.AddressLine2 = model.NewZealand.AddressLine2;
                request.Suburb = model.NewZealand.Suburb;
                request.State = model.NewZealand.Locality;
                request.Postcode = model.NewZealand.Island;
                request.Country = "New Zealand";

                request.HandpiecesCount = model.NewZealand.NumberOfHandpieces ?? throw new InvalidOperationException();
                request.Comment = model.NewZealand.Comment;

                await this.repository.InsertAsync(request);
                await this.repository.SaveChangesAsync();

                return this.RedirectToAction("Success");
            }

            var combined = new OrderCourierCombinedWrapperModel
            {
                Selected = PickupRequestType.NewZealand,
                NewZealand = model.NewZealand,
            };

            await this.FillRemainingAsync(combined);
            return this.View("Index", combined);
        }

        public async Task<IActionResult> Queensland(OrderCourierQueenslandWrapperModel model)
        {
            if (this.ModelState.IsValid)
            {
                var request = await this.CreateBlankRequest(PickupRequestType.Queensland);

                request.ClientId = await this.ResolveClientId(request.ClientId, model.Queensland.ClientNo);
                request.PracticeName = model.Queensland.PracticeName;
                request.ContactPerson = model.Queensland.ContactPerson;
                request.Email = model.Queensland.Email;
                request.Phone = model.Queensland.Phone;
                request.AddressLine1 = model.Queensland.AddressLine1;
                request.AddressLine2 = model.Queensland.AddressLine2;
                request.Suburb = model.Queensland.Suburb;
                request.State = "QLD";
                request.Postcode = model.Queensland.Postcode;
                request.Country = "Australia";

                request.HandpiecesCount = model.Queensland.NumberOfHandpieces ?? throw new InvalidOperationException();
                request.Comment = model.Queensland.Comment;

                await this.repository.InsertAsync(request);
                await this.repository.SaveChangesAsync();

                return this.RedirectToAction("Success");
            }

            var combined = new OrderCourierCombinedWrapperModel
            {
                Selected = PickupRequestType.Queensland,
                Queensland = model.Queensland,
            };

            await this.FillRemainingAsync(combined);
            return this.View("Index", combined);
        }

        public IActionResult Success()
        {
            return this.View();
        }

        private async Task FillFromEntity(OrderCourierCombinedWrapperModel model, Client client, Boolean asAdmin = false)
        {
            if (asAdmin)
            {
                var user = await this.userResolver.ResolveCurrentUserEntity();
                switch (user)
                {
                    case Employee employee:
                    case Administrator _:
                        break;
                    case Corporate corporate:
                        if (client.CorporateId != corporate.Id)
                        {
                            throw new InvalidOperationException("Can't access specified client");
                        }

                        break;
                    case Client userClient:
                        if (userClient.Id != client.Id)
                        {
                            throw new InvalidOperationException("Can't access specified client");
                        }

                        break;
                    default:
                        throw new InvalidOperationException("Can't access specified client");
                }
            }

            var state = await this.repository.Query<State>().SingleOrDefaultAsync(x => x.Id == client.StateId);

            if (model.GreaterSydney == null)
            {
                model.GreaterSydney = new OrderCourierGreaterSydneyViewModel
                {
                    ClientNo = client.ClientNo,
                    PracticeName = client.Name,
                    AddressLine1 = client.Address,
                    ContactPerson = client.PrincipalDentist,
                    Suburb = client.Suburb,
                    Postcode = client.PostCode,
                    Email = client.Email,
                    Phone = client.Phone,
                };
            }

            if (model.Australia == null)
            {
                model.Australia = new OrderCourierAustraliaViewModel
                {
                    ClientNo = client.ClientNo,
                    PracticeName = client.Name,
                    AddressLine1 = client.Address,
                    ContactPerson = client.PrincipalDentist,
                    Suburb = client.Suburb,
                    Postcode = client.PostCode,
                    State = state?.Name,
                    Email = client.Email,
                    Phone = client.Phone,
                };
            }

            if (model.NewZealand == null)
            {
                model.NewZealand = new OrderCourierNewZealandViewModel
                {
                    ClientNo = client.ClientNo,
                    PracticeName = client.Name,
                    AddressLine1 = client.Address,
                    ContactPerson = client.PrincipalDentist,
                    Suburb = client.Suburb,
                    Email = client.Email,
                    Phone = client.Phone,
                };
            }

            if (model.Queensland == null)
            {
                model.Queensland = new OrderCourierQueenslandViewModel
                {
                    ClientNo = client.ClientNo,
                    PracticeName = client.Name,
                    AddressLine1 = client.Address,
                    ContactPerson = client.PrincipalDentist,
                    Suburb = client.Suburb,
                    Postcode = client.PostCode,
                    Email = client.Email,
                    Phone = client.Phone,
                };
            }
        }

        private async Task FillRemainingAsync(OrderCourierCombinedWrapperModel model)
        {
            var user = await this.userResolver.ResolveCurrentUserEntity();
            if (user is Client client)
            {
                await this.FillFromEntity(model, client);
            }
            else
            {
                if (model.GreaterSydney == null)
                {
                    model.GreaterSydney = new OrderCourierGreaterSydneyViewModel();
                }

                if (model.Australia == null)
                {
                    model.Australia = new OrderCourierAustraliaViewModel();
                }

                if (model.NewZealand == null)
                {
                    model.NewZealand = new OrderCourierNewZealandViewModel();
                }

                if (model.Queensland == null)
                {
                    model.Queensland = new OrderCourierQueenslandViewModel();
                }
            }

            if (model.GreaterSydney.ClientNo != null && model.GreaterSydney.ClientEntity == null)
            {
                model.GreaterSydney.ClientEntity = await this.repository.Query<Client>().SingleOrDefaultAsync(x => x.ClientNo == model.GreaterSydney.ClientNo);
            }

            if (model.Australia.ClientNo != null && model.Australia.ClientEntity == null)
            {
                model.Australia.ClientEntity = await this.repository.Query<Client>().SingleOrDefaultAsync(x => x.ClientNo == model.Australia.ClientNo);
            }

            if (model.NewZealand.ClientNo != null && model.Australia.ClientEntity == null)
            {
                model.NewZealand.ClientEntity = await this.repository.Query<Client>().SingleOrDefaultAsync(x => x.ClientNo == model.NewZealand.ClientNo);
            }

            if (model.Queensland.ClientNo != null && model.Queensland.ClientEntity == null)
            {
                model.Queensland.ClientEntity = await this.repository.Query<Client>().SingleOrDefaultAsync(x => x.ClientNo == model.Queensland.ClientNo);
            }
        }

        private async Task<PickupRequest> CreateBlankRequest(PickupRequestType type)
        {
            var appUser = await this.userAccessor.GetUserAsync();
            var request = new PickupRequest
            {
                Type = type,
                CreatedOn = DateTime.UtcNow,
                Status = PickupRequestStatus.Created,
            };

            if (appUser != null)
            {
                request.CreatedById = appUser.Id;
                var user = await this.userResolver.ResolveCurrentUserEntity();
                switch (user)
                {
                    case Employee employee:
                        request.EmployeeId = employee.Id;
                        break;
                    case Client client:
                        request.ClientId = client.Id;
                        break;
                    case Corporate corporate:
                        request.CorporateId = corporate.Id;
                        break;
                }
            }

            return request;
        }

        private async Task<Guid?> ResolveClientId(Guid? currentValue, Int32? clientNo)
        {
            if (currentValue.HasValue)
            {
                return currentValue;
            }

            var user = await this.userResolver.ResolveCurrentUserEntity();
            switch (user)
            {
                case Employee employee:
                {
                    var surgery = await this.repository.Query<Client>().SingleOrDefaultAsync(x => x.ClientNo == clientNo);
                    return surgery?.Id;
                }

                case Corporate corporate:
                {
                    var surgery = await this.repository.Query<Client>().SingleOrDefaultAsync(x => x.CorporateId == corporate.Id && x.ClientNo == clientNo);
                    return surgery?.Id;
                }

                default:
                    return null;
            }
        }
    }
}
