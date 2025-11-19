using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DentalDrill.CRM.Models.ViewModels
{
    public class NotificationFeedModel
    {
        public Boolean Init { get; set; }

        public Boolean LinkFullVersion { get; set; }

        public Int32? PageSize { get; set; }
    }
}
