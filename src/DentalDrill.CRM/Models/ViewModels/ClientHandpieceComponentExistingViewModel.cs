using System;

namespace DentalDrill.CRM.Models.ViewModels
{
    public class ClientHandpieceComponentExistingViewModel
    {
        public Guid Id { get; set; }

        public Int32 OrderNo { get; set; }

        public String Brand { get; set; }

        public String Model { get; set; }

        public String Serial { get; set; }
    }
}
