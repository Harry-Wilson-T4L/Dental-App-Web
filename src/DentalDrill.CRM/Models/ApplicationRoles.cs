using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DentalDrill.CRM.Models
{
    public static class ApplicationRoles
    {
        public const String Administrator = nameof(ApplicationRoles.Administrator);

        public const String CompanyAdministrator = nameof(ApplicationRoles.CompanyAdministrator);

        public const String OfficeAdministrator = nameof(ApplicationRoles.OfficeAdministrator);

        public const String WorkshopTechnician = nameof(ApplicationRoles.WorkshopTechnician);

        public const String CompanyManager = nameof(ApplicationRoles.CompanyManager);

        public const String Employee = nameof(ApplicationRoles.Employee);

        public const String Client = nameof(ApplicationRoles.Client);

        public const String Corporate = nameof(ApplicationRoles.Corporate);

        public static class Combined
        {
            public const String Employee = nameof(ApplicationRoles.Administrator) + "," +
                                           nameof(ApplicationRoles.Employee);

            public const String Administrator = nameof(ApplicationRoles.Administrator) + "," +
                                                nameof(ApplicationRoles.CompanyAdministrator);

            public const String Staff = nameof(ApplicationRoles.Administrator) + "," +
                                        nameof(ApplicationRoles.CompanyAdministrator) + "," +
                                        nameof(ApplicationRoles.OfficeAdministrator) + "," +
                                        nameof(ApplicationRoles.WorkshopTechnician) + "," +
                                        nameof(ApplicationRoles.CompanyManager);

            public const String Office = nameof(ApplicationRoles.Administrator) + "," +
                                         nameof(ApplicationRoles.CompanyAdministrator) + "," +
                                         nameof(ApplicationRoles.OfficeAdministrator);

            public const String InventoryManagers = nameof(ApplicationRoles.Administrator) + "," +
                                                    nameof(ApplicationRoles.CompanyAdministrator) + "," +
                                                    nameof(ApplicationRoles.CompanyManager);

            public const String Workshop = nameof(ApplicationRoles.Administrator) + "," +
                                           nameof(ApplicationRoles.CompanyAdministrator) + "," +
                                           nameof(ApplicationRoles.WorkshopTechnician);
        }
    }
}
