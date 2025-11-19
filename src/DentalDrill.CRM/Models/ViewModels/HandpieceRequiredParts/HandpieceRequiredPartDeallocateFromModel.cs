using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DentalDrill.CRM.Domain.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DentalDrill.CRM.Models.ViewModels.HandpieceRequiredParts
{
    public class HandpieceRequiredPartDeallocateFromModel
    {
        [BindNever]
        public IHandpiece Handpiece { get; set; }

        [BindNever]
        public IInventorySKU SKU { get; set; }

        [BindNever]
        public IHandpieceRequiredPart Part { get; set; }

        [BindNever]
        public IInventoryRepairMovement Repair { get; set; }

        [BindNever]
        public Decimal ShelfQuantity { get; set; }

        [BindNever]
        public Decimal AllocatedQuantity { get; set; }

        public List<Guid> ReallocateTo { get; set; } = new List<Guid>();
    }
}
