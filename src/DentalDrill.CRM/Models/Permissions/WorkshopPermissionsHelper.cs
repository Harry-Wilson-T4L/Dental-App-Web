using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DentalDrill.CRM.Models.Permissions
{
    public static class WorkshopPermissionsHelper
    {
        public static IReadOnlyList<WorkshopPermissions> GetAllFlags()
        {
            return new WorkshopPermissions[]
            {
                WorkshopPermissions.AccessWorkshop,
                WorkshopPermissions.CreateJob,
            };
        }

        public static IEnumerable<SelectListItem> MakeSelectList()
        {
            return new SelectList(WorkshopPermissionsHelper.GetAllFlags().Select(x => Tuple.Create(x, x.ToFlagDisplayString())), "Item1", "Item2");
        }

        public static String ToDisplayString(this WorkshopPermissions value)
        {
            return value switch
            {
                WorkshopPermissions.All => "All",
                WorkshopPermissions.None => "None",
                _ => String.Join(", ", value.SplitValue().Select(x => x.ToFlagDisplayString())),
            };
        }

        public static IReadOnlyList<WorkshopPermissions> SplitValue(this WorkshopPermissions value)
        {
            return WorkshopPermissionsHelper.GetAllFlags().Where(flag => (value & flag) == flag).ToList();
        }

        public static WorkshopPermissions CombineValue(this IReadOnlyList<WorkshopPermissions> values)
        {
            return values.Aggregate(WorkshopPermissions.None, (acc, x) => acc | x);
        }

        private static String ToFlagDisplayString(this WorkshopPermissions value)
        {
            return value switch
            {
                WorkshopPermissions.AccessWorkshop => "Access workshop",
                WorkshopPermissions.CreateJob => "Create job",
                _ => value.ToString(),
            };
        }
    }
}
