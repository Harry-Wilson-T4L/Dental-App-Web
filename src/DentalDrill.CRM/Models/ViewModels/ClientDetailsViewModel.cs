using System;
using System.Collections.Generic;
using DentalDrill.CRM.Domain.Abstractions;
using DevGuild.AspNetCore.Services.ModelMapping.Annotations;

namespace DentalDrill.CRM.Models.ViewModels
{
    public class ClientDetailsViewModel
    {
        [PropertyMapping(PropertyMappingMode.ToDetails)]
        public Guid Id { get; set; }

        public Client Entity { get; set; }

        public IEmployeeAccess AllAccess { get; set; }

        public IEmployeeAccessClient Access { get; set; }

        public List<ClientDetailsWorkshopStatistics> Workshops { get; set; } = new();
    }
}
