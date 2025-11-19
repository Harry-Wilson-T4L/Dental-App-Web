using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DentalDrill.CRM.Models.ViewModels
{
    public class OrderCourierCombinedWrapperModel
    {
        public PickupRequestType? Selected { get; set; }

        public OrderCourierGreaterSydneyViewModel GreaterSydney { get; set; }

        public OrderCourierAustraliaViewModel Australia { get; set; }

        public OrderCourierNewZealandViewModel NewZealand { get; set; }

        public OrderCourierQueenslandViewModel Queensland { get; set; }
    }
}
