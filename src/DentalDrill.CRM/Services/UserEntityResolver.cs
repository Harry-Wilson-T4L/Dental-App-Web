using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DentalDrill.CRM.Domain;
using DentalDrill.CRM.Domain.Abstractions;
using DentalDrill.CRM.Models;
using DevGuild.AspNetCore.Services.Data;
using DevGuild.AspNetCore.Services.Identity;
using DevGuild.AspNetCore.Services.Identity.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DentalDrill.CRM.Services
{
    public class UserEntityResolver
    {
        private readonly IAuthenticatedUserAccessorService<ApplicationUser> userAccessorService;
        private readonly IRepository repository;
        private readonly UserManager<ApplicationUser> userManager;

        public UserEntityResolver(IAuthenticatedUserAccessorService<ApplicationUser> userAccessorService, IRepository repository, UserManager<ApplicationUser> userManager)
        {
            this.userAccessorService = userAccessorService;
            this.repository = repository;
            this.userManager = userManager;
        }

        public async Task<Object> ResolveCurrentUserEntity()
        {
            var user = await this.userAccessorService.GetUserAsync();
            if (user == null)
            {
                return null;
            }

            return await this.ResolveUserEntity(user);
        }

        public async Task<Object> ResolveUserEntity(ApplicationUser user)
        {
            var roles = await this.userManager.GetRolesAsync(user);

            if (roles.Any(x => x == ApplicationRoles.Administrator))
            {
                return new Administrator();
            }

            if (roles.Any(x => x == ApplicationRoles.OfficeAdministrator || x == ApplicationRoles.WorkshopTechnician || x == ApplicationRoles.CompanyAdministrator || x == ApplicationRoles.CompanyManager))
            {
                var employee = await this.repository.QueryWithoutTracking<Employee>("ApplicationUser").SingleAsync(x => x.ApplicationUserId == user.Id);
                return employee;
            }

            if (roles.Any(x => x == ApplicationRoles.Corporate))
            {
                var corporate = await this.repository.QueryWithoutTracking<Corporate>().SingleAsync(x => x.UserId == user.Id);
                return corporate;
            }

            if (roles.Any(x => x == ApplicationRoles.Client))
            {
                var client = await this.repository.QueryWithoutTracking<Client>().SingleAsync(x => x.UserId == user.Id);
                return client;
            }

            return null;
        }

        public async Task<Object> ResolvePrincipal(ClaimsPrincipal principal)
        {
            if (!principal.Identity.IsAuthenticated)
            {
                return null;
            }

            var user = await this.userManager.GetUserAsync(principal);
            if (user == null)
            {
                return null;
            }

            return await this.ResolveUserEntity(user);
        }

        public async Task<IEmployeeAccess> GetEmployeeAccessAsync()
        {
            var user = await this.userAccessorService.GetUserAsync();
            if (user == null)
            {
                return new EmployeeAccessNullDomainModel();
            }

            var roles = await this.userManager.GetRolesAsync(user);

            if (roles.Any(x => x == ApplicationRoles.Administrator))
            {
                var workshops = await this.repository.QueryWithoutTracking<Workshop>().ToListAsync();
                var jobTypes = await this.repository.QueryWithoutTracking<JobType>().ToListAsync();

                return new EmployeeAccessAdminDomainModel(workshops, jobTypes);
            }

            if (roles.Any(x => x == ApplicationRoles.OfficeAdministrator || x == ApplicationRoles.WorkshopTechnician || x == ApplicationRoles.CompanyAdministrator || x == ApplicationRoles.CompanyManager))
            {
                var employee = await this.repository.QueryWithoutTracking<Employee>("ApplicationUser").SingleAsync(x => x.ApplicationUserId == user.Id);
                var role = await this.repository.QueryWithoutTracking<EmployeeRole>()
                    .Include(x => x.WorkshopRoles).ThenInclude(x => x.WorkshopRole).ThenInclude(x => x.JobTypePermissions).ThenInclude(x => x.StatusPermissions)
                    .Include(x => x.WorkshopRoles).ThenInclude(x => x.WorkshopRole).ThenInclude(x => x.JobTypePermissions).ThenInclude(x => x.JobExceptions)
                    .Include(x => x.WorkshopRoles).ThenInclude(x => x.WorkshopRole).ThenInclude(x => x.JobTypePermissions).ThenInclude(x => x.HandpieceExceptions)
                    .SingleOrDefaultAsync(x => x.Id == employee.RoleId);

                return new EmployeeAccessDomainModel(role);
            }

            if (roles.Any(x => x == ApplicationRoles.Corporate))
            {
                return new EmployeeAccessNullDomainModel();
            }

            if (roles.Any(x => x == ApplicationRoles.Client))
            {
                return new EmployeeAccessNullDomainModel();
            }

            return null;
        }
    }
}
