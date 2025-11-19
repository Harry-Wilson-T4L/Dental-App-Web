using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using DentalDrill.CRM.Domain.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DentalDrill.CRM.Models.ViewModels.InventoryMovements
{
    public class InventoryMovementCreateForSKUModel
    {
        [BindNever]
        public InventorySKU SKU { get; set; }

        [BindNever]
        public IWorkshop PreselectedWorkshop { get; set; }

        [BindNever]
        public IReadOnlyList<IWorkshop> AvailableWorkshops { get; set; }

        [BindNever]
        public InventoryMovementStatus[] AvailableStatuses { get; } =
        {
            InventoryMovementStatus.Completed,
        };

        [Display(Name = "Movement Type")]
        public InventoryMovementCreateType Type { get; set; }

        [Display(Name = "From Workshop")]
        public Guid? FromWorkshop { get; set; }

        [Display(Name = "To Workshop")]
        public Guid? ToWorkshop { get; set; }

        [Required]
        [Display(Name = "Movement Quantity")]
        public Decimal? Quantity { get; set; }

        [Display(Name = "Movement Status")]
        public InventoryMovementStatus Status { get; set; }

        [Display(Name = "Comment")]
        public String Comment { get; set; }

        [MemberNotNullWhen(true, "Quantity")]
        public Boolean Validate(ModelStateDictionary modelState)
        {
            switch (this.Type)
            {
                case InventoryMovementCreateType.Order:
                    if (this.ToWorkshop == null)
                    {
                        modelState.AddModelError<InventoryMovementCreateForSKUModel>(x => x.ToWorkshop, "Required");
                    }

                    return modelState.IsValid;
                case InventoryMovementCreateType.Found:
                    if (this.ToWorkshop == null)
                    {
                        modelState.AddModelError<InventoryMovementCreateForSKUModel>(x => x.ToWorkshop, "Required");
                    }

                    return modelState.IsValid;
                case InventoryMovementCreateType.Lost:
                    if (this.FromWorkshop == null)
                    {
                        modelState.AddModelError<InventoryMovementCreateForSKUModel>(x => x.FromWorkshop, "Required");
                    }

                    return modelState.IsValid;
                case InventoryMovementCreateType.MoveBetweenWorkshops:
                    if (this.FromWorkshop == null)
                    {
                        modelState.AddModelError<InventoryMovementCreateForSKUModel>(x => x.FromWorkshop, "Required");
                    }

                    if (this.ToWorkshop == null)
                    {
                        modelState.AddModelError<InventoryMovementCreateForSKUModel>(x => x.ToWorkshop, "Required");
                    }

                    return modelState.IsValid;

                case InventoryMovementCreateType.MoveFromAnotherWorkshop:
                    if (this.FromWorkshop == null)
                    {
                        modelState.AddModelError<InventoryMovementCreateForSKUModel>(x => x.FromWorkshop, "Required");
                    }

                    return modelState.IsValid;

                case InventoryMovementCreateType.MoveToAnotherWorkshop:
                    if (this.ToWorkshop == null)
                    {
                        modelState.AddModelError<InventoryMovementCreateForSKUModel>(x => x.ToWorkshop, "Required");
                    }

                    return modelState.IsValid;
                    
                default:
                    modelState.AddModelError<InventoryMovementCreateForSKUModel>(x => x.Type, "Invalid type");
                    return false;
                }
        }
    }
}
