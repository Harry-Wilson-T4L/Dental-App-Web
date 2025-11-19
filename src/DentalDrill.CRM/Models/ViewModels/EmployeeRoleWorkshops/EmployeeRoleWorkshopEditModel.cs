using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DentalDrill.CRM.Models.ViewModels.EmployeeRoleWorkshops
{
    public class EmployeeRoleWorkshopEditModel
    {
        [BindNever]
        public EmployeeRoleWorkshop Original { get; set; }

        [BindNever]
        public EmployeeRole Parent { get; set; }

        [BindNever]
        public List<Workshop> Workshops { get; set; }

        [BindNever]
        public List<WorkshopRole> WorkshopRoles { get; set; }

        [Display(Name = "Workshop")]
        public Guid WorkshopId { get; set; }

        [Display(Name = "Workshop role")]
        public Guid WorkshopRoleId { get; set; }

        [Display(Name = "Notifications role")]
        public EmployeeType EmployeeType { get; set; }
    }
}
