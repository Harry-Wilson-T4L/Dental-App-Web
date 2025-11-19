using System;
using DentalDrill.CRM.Domain.Abstractions;

namespace DentalDrill.CRM.Models.ViewModels.Corporates
{
    public class CorporateDetailsModel
    {
        public Guid Id => this.Entity.Id;

        public Corporate Entity { get; set; }

        public IEmployeeAccess Access { get; set; }
    }
}
