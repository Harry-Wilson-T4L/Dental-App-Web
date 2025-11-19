using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DentalDrill.CRM.Models.Permissions;

namespace DentalDrill.CRM.Models.ViewModels
{
    public class HandpieceDetailsModel
    {
        public Guid Id { get; set; }

        public Handpiece Entity { get; set; }

        public Job Job { get; set; }

        public Handpiece Previous { get; set; }

        public Handpiece Next { get; set; }

        public PropertiesAccessControlList<Handpiece> PropertiesAccessControl { get; set; }
    }
}
