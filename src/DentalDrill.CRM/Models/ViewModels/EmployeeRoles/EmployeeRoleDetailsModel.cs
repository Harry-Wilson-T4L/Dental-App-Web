using System;
using DentalDrill.CRM.Domain.Abstractions;

namespace DentalDrill.CRM.Models.ViewModels.EmployeeRoles
{
    public class EmployeeRoleDetailsModel
    {
        public Guid Id => this.Entity.Id;

        public EmployeeRole Entity { get; set; }

        public IEmployeeAccess Access { get; set; }
    }
}
