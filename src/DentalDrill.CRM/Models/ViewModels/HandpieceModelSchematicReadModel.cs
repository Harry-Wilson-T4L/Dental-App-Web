using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DentalDrill.CRM.Models.ViewModels
{
    public class HandpieceModelSchematicReadModel
    {
        public Guid Id { get; set; }

        public HandpieceModelSchematicType Type { get; set; }

        public String Title { get; set; }

        public Boolean Display { get; set; }

        public String ThumbnailUrl { get; set; }
    }
}
