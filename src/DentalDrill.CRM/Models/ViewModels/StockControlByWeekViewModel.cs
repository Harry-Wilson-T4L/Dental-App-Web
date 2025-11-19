using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DentalDrill.CRM.Models.ViewModels
{
    public class StockControlByWeekViewModel
    {
        public CalendarWeek Week { get; set; }

        public CalendarWeek PreviousWeek { get; set; }

        public CalendarWeek NextWeek { get; set; }
    }
}
