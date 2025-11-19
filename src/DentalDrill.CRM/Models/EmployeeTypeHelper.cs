using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DentalDrill.CRM.Models
{
    public static class EmployeeTypeHelper
    {
        public static IEnumerable<SelectListItem> MakeSelectList()
        {
            return new SelectList(
                new[]
                {
                    Tuple.Create(EmployeeType.WorkshopTechnician, "Workshop technician"),
                    Tuple.Create(EmployeeType.OfficeAdministrator, "Office administrator"),
                    Tuple.Create(EmployeeType.CompanyAdministrator, "Company administrator"),
                    Tuple.Create(EmployeeType.CompanyManager, "Company manager"),
                },
                "Item1",
                "Item2");
        }

        public static String ToDisplayString(this EmployeeType value)
        {
            switch (value)
            {
                case EmployeeType.WorkshopTechnician:
                    return "Workshop technician";
                case EmployeeType.OfficeAdministrator:
                    return "Office administrator";
                case EmployeeType.CompanyAdministrator:
                    return "Company administrator";
                case EmployeeType.CompanyManager:
                    return "Company manager";
                default:
                    throw new ArgumentOutOfRangeException(nameof(value), value, null);
            }
        }
    }
}
