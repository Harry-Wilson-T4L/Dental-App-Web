using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DentalDrill.CRM.Models.QueryModels
{
    public class ClientIndexItem
    {
        public Guid Id { get; set; }

        public Guid ZoneId { get; set; }

        public String ZoneName { get; set; }

        public String Name { get; set; }

        public String PrincipalDentist { get; set; }

        public String Phone { get; set; }

        public String Address { get; set; }

        public String Suburb { get; set; }

        public Guid StateId { get; set; }

        public String StateName { get; set; }

        public String Postcode { get; set; }

        public String Email { get; set; }

        public Int32 HandpiecesCount { get; set; }

        public Decimal? AverageRating { get; set; }
    }
}
