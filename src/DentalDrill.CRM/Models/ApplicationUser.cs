using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevGuild.AspNetCore.Services.Identity.Models;
using Microsoft.AspNetCore.Identity;

namespace DentalDrill.CRM.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : User
    {
        public Boolean MustChangePasswordAtNextLogin { get; set; }
    }
}
