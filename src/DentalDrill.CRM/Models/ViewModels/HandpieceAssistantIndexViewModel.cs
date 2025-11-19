using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DentalDrill.CRM.Models.ViewModels
{
    public class HandpieceAssistantIndexViewModel
    {
        public Guid Id { get; set; }

        public HandpieceBrand Brand { get; set; }

        public HandpieceModel Model { get; set; }

        public List<HandpieceModelSchematicReadModel> Schematics { get; set; }
    }
}
