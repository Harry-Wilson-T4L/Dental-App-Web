using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DentalDrill.CRM.Models.ViewModels
{
    public class HandpieceStoreReadModel
    {
        public Guid Id { get; set; }

        public String Brand { get; set; }

        public String Model { get; set; }

        public Decimal Price { get; set; }

        public String ThumbnailUrl { get; set; }
    }
}
