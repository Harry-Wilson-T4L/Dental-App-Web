using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DentalDrill.CRM.Models.ViewModels
{
    public class CalendarWeekViewModel
    {
        public Guid Id { get; set; }

        public Int32 Year { get; set; }

        public Int32 Week { get; set; }

        public DateTime RangeStart { get; set; }

        public DateTime RangeEnd { get; set; }
    }
}
