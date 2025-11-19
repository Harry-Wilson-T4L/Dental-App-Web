using System;
using DevGuild.AspNetCore.Services.ModelMapping.Annotations;

namespace DentalDrill.CRM.Models.ViewModels.Workshops
{
    public class WorkshopViewModel
    {
        [PropertyMapping(PropertyMappingMode.ToDetails)]
        public Guid Id { get; set; }

        public String Name { get; set; }

        public String Description { get; set; }
    }
}
