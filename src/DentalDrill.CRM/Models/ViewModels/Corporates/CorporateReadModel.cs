using System;

namespace DentalDrill.CRM.Models.ViewModels.Corporates
{
    public class CorporateReadModel
    {
        public Guid Id { get; set; }

        public String Name { get; set; }

        public Int32 ClientsCount { get; set; }

        public String UserName { get; set; }

        public String UserEmail { get; set; }
    }
}
