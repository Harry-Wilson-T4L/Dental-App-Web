using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DentalDrill.CRM.Models;
using DevGuild.AspNetCore.Services.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace DentalDrill.CRM.Services
{
    public class ApplicationClaimsPrincipalFactory : UserClaimsPrincipalFactory<ApplicationUser, Role>
    {
        public ApplicationClaimsPrincipalFactory(UserManager<ApplicationUser> userManager, RoleManager<Role> roleManager, IOptions<IdentityOptions> options)
            : base(userManager, roleManager, options)
        {
        }

        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(ApplicationUser user)
        {
            var claims = await base.GenerateClaimsAsync(user);
            if (user.MustChangePasswordAtNextLogin)
            {
                claims.AddClaim(new Claim("MustChangePasswordAtNextLogin", "true"));
            }

            return claims;
        }
    }
}
