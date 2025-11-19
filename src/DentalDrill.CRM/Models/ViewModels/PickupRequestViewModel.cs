using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DentalDrill.CRM.Models.ViewModels
{
    public class PickupRequestViewModel
    {
        public Guid Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public String CreatedBy { get; set; }

        public String Type { get; set; }

        public String Status { get; set; }

        public String PracticeName { get; set; }

        public String Contact { get; set; }

        public String Address { get; set; }

        public Int32 HandpiecesCount { get; set; }
    }
}
