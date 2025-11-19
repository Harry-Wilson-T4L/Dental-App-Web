using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DentalDrill.CRM.Models.ViewModels.WorkshopRoleJobTypes
{
    public class WorkshopRoleJobTypeCreateModel
    {
        [BindNever]
        public WorkshopRole Parent { get; set; }

        [BindNever]
        public List<JobType> JobTypes { get; set; }

        public String JobTypeId { get; set; }
    }
}
