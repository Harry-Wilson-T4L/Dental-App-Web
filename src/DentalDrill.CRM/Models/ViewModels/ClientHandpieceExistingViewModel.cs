using System;
using System.Collections.Generic;

namespace DentalDrill.CRM.Models.ViewModels
{
    public class ClientHandpieceExistingViewModel
    {
        public Guid Id { get; set; }

        public ClientHandpieceExistingType Type { get; set; }

        public String Brand { get; set; }

        public String Model { get; set; }

        public String Serial { get; set; }

        public String MainText { get; set; }

        public ICollection<ClientHandpieceComponentExistingViewModel> Components { get; set; }

        public String ComponentsText { get; set; }

        public DateTime? LastRepairDate { get; set; }

        public String LastRepairStatus { get; set; }

        public String LastRepairServiceLevelName { get; set; }
    }
}
