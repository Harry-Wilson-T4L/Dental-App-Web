using System;
using System.Collections.Generic;
using DentalDrill.CRM.Models;

namespace DentalDrill.CRM.Services.Generation
{
    public class EntitiesGenerationRuntimeSettings
    {
        public Int32 NumberOfHandpieces { get; set; }

        public Employee Employee { get; set; }

        public List<Client> Clients { get; set; }

        public Int32 Year { get; set; }
    }
}