using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DentalDrill.CRM.Models.Permissions
{
    public static class HandpieceEntityComponentHelper
    {
        public static IReadOnlyList<HandpieceEntityComponent> GetAllFlags()
        {
            return new HandpieceEntityComponent[]
            {
                HandpieceEntityComponent.MaintenanceHistory,
                HandpieceEntityComponent.Image,
                HandpieceEntityComponent.Note,
                HandpieceEntityComponent.History,
            };
        }

        public static IReadOnlyList<HandpieceEntityComponent> GetSupportedFlags(String jobTypeId)
        {
            return jobTypeId switch
            {
                JobTypes.Estimate => HandpieceEntityComponentHelper.GetAllFlags(),
                JobTypes.Sale => HandpieceEntityComponentHelper.GetAllFlags(),
                _ => throw new NotSupportedException($"Job type {jobTypeId} is not supported"),
            };
        }

        public static IEnumerable<SelectListItem> MakeSelectList()
        {
            return new SelectList(HandpieceEntityComponentHelper.GetAllFlags().Select(x => Tuple.Create(x, x.ToFlagDisplayString())), "Item1", "Item2");
        }

        public static String ToDisplayString(this HandpieceEntityComponent value)
        {
            return value switch
            {
                HandpieceEntityComponent.All => "All",
                HandpieceEntityComponent.None => "None",
                _ => String.Join(", ", value.SplitValue().Select(x => x.ToFlagDisplayString())),
            };
        }

        public static IReadOnlyList<HandpieceEntityComponent> SplitValue(this HandpieceEntityComponent value)
        {
            return HandpieceEntityComponentHelper.GetAllFlags().Where(flag => (value & flag) == flag).ToList();
        }

        public static HandpieceEntityComponent CombineValue(this IReadOnlyList<HandpieceEntityComponent> values)
        {
            return (values ?? Array.Empty<HandpieceEntityComponent>()).Aggregate(HandpieceEntityComponent.None, (acc, x) => acc | x);
        }

        private static String ToFlagDisplayString(this HandpieceEntityComponent value)
        {
            return value switch
            {
                HandpieceEntityComponent.MaintenanceHistory => "Maintenance history",
                HandpieceEntityComponent.Image => "Image",
                HandpieceEntityComponent.Note => "Note",
                HandpieceEntityComponent.History => "History",
                _ => value.ToString(),
            };
        }
    }
}
