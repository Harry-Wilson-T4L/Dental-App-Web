using System;
using System.ComponentModel.DataAnnotations;

namespace DentalDrill.CRM.Models
{
    public class EmployeeRoleWorkshop
    {
        public Guid Id { get; set; }

        public Guid EmployeeRoleId { get; set; }

        public EmployeeRole EmployeeRole { get; set; }

        [Display(Name = "Workshop")]
        public Guid WorkshopId { get; set; }

        [Display(Name = "Workshop")]
        public Workshop Workshop { get; set; }

        [Display(Name = "Workshop role")]
        public Guid WorkshopRoleId { get; set; }

        [Display(Name = "Workshop role")]
        public WorkshopRole WorkshopRole { get; set; }

        [Display(Name = "Notifications role")]
        public EmployeeType EmployeeType { get; set; }
    }
}
