using System;
using DentalDrill.CRM.Domain.Abstractions;

namespace DentalDrill.CRM.Models.ViewModels.WorkshopRoles
{
    public class WorkshopRoleDetailsModel
    {
        public Guid Id => this.Entity.Id;

        public WorkshopRole Entity { get; set; }

        public IEmployeeAccess Access { get; set; }
    }
}
