using System.Collections.Generic;
using System.Linq;

namespace DentalDrill.CRM.Models.ViewModels.Inventory
{
    internal static class InventorySKUWorkshopOptionsExtensions
    {
        public static void ApplyWorkshopOptions(this InventorySKU entity, IReadOnlyList<InventorySKUWorkshopOptionsEditModel> workshopOptions)
        {
            entity.WorkshopOptions ??= new List<InventorySKUWorkshopOptions>();
            workshopOptions ??= new List<InventorySKUWorkshopOptionsEditModel>();

            foreach (var existingOption in entity.WorkshopOptions)
            {
                var matchingUpdate = workshopOptions.SingleOrDefault(x => x.WorkshopId == existingOption.WorkshopId);
                existingOption.WarningQuantity = matchingUpdate?.WarningQuantity;
            }

            var newOptions = workshopOptions.Where(x => entity.WorkshopOptions.All(y => y.WorkshopId != x.WorkshopId)).ToList();
            foreach (var newOption in newOptions)
            {
                entity.WorkshopOptions.Add(new InventorySKUWorkshopOptions
                {
                    WorkshopId = newOption.WorkshopId,
                    WarningQuantity = newOption.WarningQuantity,
                });
            }
        }
    }
}
