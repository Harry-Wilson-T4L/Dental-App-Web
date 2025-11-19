using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevGuild.AspNetCore.Services.Identity.Models;

namespace DentalDrill.CRM.Models.ViewModels
{
    public class UserDetailsViewModel
    {
        public ApplicationUser User { get; set; }

        public List<Role> Roles { get; set; }

        public String GetRolesNames()
        {
            if (this.Roles == null || this.Roles.Count == 0)
            {
                return String.Empty;
            }

            return String.Join(", ", this.Roles.Select(x => x.Name));
        }
    }
}
