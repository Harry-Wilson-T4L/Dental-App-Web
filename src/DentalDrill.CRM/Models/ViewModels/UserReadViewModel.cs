using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DentalDrill.CRM.Models.ViewModels
{
    public class UserReadViewModel
    {
        public Guid Id { get; set; }

        public String UserName { get; set; }

        public String Email { get; set; }

        public String Roles { get; set; }
    }
}
