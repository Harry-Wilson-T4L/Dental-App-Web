using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DentalDrill.CRM.Models.ViewModels
{
    public class HandpieceModelReadModel
    {
        public Guid Id { get; set; }

        public Guid BrandId { get; set; }

        public String BrandName { get; set; }

        public String Name { get; set; }

        public String Description { get; set; }

        public Guid? ImageId { get; set; }

        public String ImageUrl { get; set; }
    }
}
