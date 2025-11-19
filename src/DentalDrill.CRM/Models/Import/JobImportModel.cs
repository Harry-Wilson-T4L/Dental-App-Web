using System;
using System.Collections.Generic;

namespace DentalDrill.CRM.Models.Import
{
    public class JobImportModel
    {
        public Int32 RelativeNo { get; set; }

        public Client Client { get; set; }

        public DateTime Received { get; set; }

        public List<HandpieceImportModel> Handpieces { get; set; }
    }
}