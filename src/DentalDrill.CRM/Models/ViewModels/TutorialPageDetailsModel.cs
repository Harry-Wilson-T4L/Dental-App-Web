using System;
using DentalDrill.CRM.Domain.Abstractions;

namespace DentalDrill.CRM.Models.ViewModels
{
    public class TutorialPageDetailsModel
    {
        public String Id => this.Entity.Id;

        public TutorialPage Entity { get; set; }

        public IEmployeeAccess Access { get; set; }
    }
}
