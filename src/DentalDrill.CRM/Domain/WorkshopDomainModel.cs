using System;
using DentalDrill.CRM.Domain.Abstractions;
using DentalDrill.CRM.Models;

namespace DentalDrill.CRM.Domain
{
    public class WorkshopDomainModel : IWorkshop
    {
        private readonly Guid id;
        private readonly Workshop workshop;

        public WorkshopDomainModel(Workshop workshop)
        {
            this.id = workshop.Id;
            this.workshop = workshop;
        }

        public Guid Id => this.id;

        public String Name => this.workshop.Name;

        public String Description => this.workshop.Description;
    }
}
