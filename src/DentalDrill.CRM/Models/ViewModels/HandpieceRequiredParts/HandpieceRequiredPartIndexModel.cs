using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DentalDrill.CRM.Models.ViewModels.HandpieceRequiredParts
{
    public class HandpieceRequiredPartIndexModel
    {
        [BindNever]
        public Handpiece Handpiece { get; set; }

        [BindNever]
        public List<InventorySKU> AvailableSKUs { get; set; }
    }
}
