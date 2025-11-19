using System;
using System.Collections.Generic;
using DentalDrill.CRM.Models;
using DentalDrill.CRM.Services.RandomData;

namespace DentalDrill.CRM.Services.Generation
{
    public class JobStatusOptions
    {
        public JobStatus JobStatus { get; set; }

        public Double Weight { get; set; }

        public List<RandomValueDescriptor<HandpieceStatus>> HandpieceStatuses { get; set; }
    }
}