using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DentalDrill.CRM.Models.Permissions
{
    public static class HandpieceEntityFieldHelper
    {
        public static IReadOnlyList<HandpieceEntityField> GetAllFlags()
        {
            return new HandpieceEntityField[]
            {
                HandpieceEntityField.Brand,
                HandpieceEntityField.Model,
                HandpieceEntityField.Serial,
                HandpieceEntityField.Components,
                HandpieceEntityField.Rating,
                HandpieceEntityField.SpeedType,
                HandpieceEntityField.Speed,
                HandpieceEntityField.Parts,
                HandpieceEntityField.PartsOutOfStock,
                HandpieceEntityField.PartsComment,
                HandpieceEntityField.PartsOrdered,
                HandpieceEntityField.PartsRestocked,
                HandpieceEntityField.ProblemDescription,
                HandpieceEntityField.DiagnosticReport,
                HandpieceEntityField.ReturnBy,
                HandpieceEntityField.CostOfRepair,
                HandpieceEntityField.ServiceLevel,
                HandpieceEntityField.EstimatedBy,
                HandpieceEntityField.InternalComment,
                HandpieceEntityField.PublicComment,
            };
        }

        public static IReadOnlyList<HandpieceEntityField> GetSupportedFlags(String jobTypeId)
        {
            return jobTypeId switch
            {
                JobTypes.Estimate => HandpieceEntityFieldHelper.GetAllFlags(),
                JobTypes.Sale => (HandpieceEntityField.Brand |
                                  HandpieceEntityField.Model |
                                  HandpieceEntityField.Serial |
                                  HandpieceEntityField.Components |
                                  HandpieceEntityField.CostOfRepair |
                                  HandpieceEntityField.InternalComment |
                                  HandpieceEntityField.PublicComment).SplitValue(),
                _ => throw new NotSupportedException($"Job type {jobTypeId} is not supported"),
            };
        }

        public static IEnumerable<SelectListItem> MakeSelectList()
        {
            return new SelectList(HandpieceEntityFieldHelper.GetAllFlags().Select(x => Tuple.Create(x, x.ToFlagDisplayString())), "Item1", "Item2");
        }

        public static String ToDisplayString(this HandpieceEntityField value)
        {
            return value switch
            {
                HandpieceEntityField.All => "All",
                HandpieceEntityField.None => "None",
                _ => String.Join(", ", value.SplitValue().Select(x => x.ToFlagDisplayString())),
            };
        }

        public static IReadOnlyList<HandpieceEntityField> SplitValue(this HandpieceEntityField value)
        {
            return HandpieceEntityFieldHelper.GetAllFlags().Where(flag => (value & flag) == flag).ToList();
        }

        public static HandpieceEntityField CombineValue(this IReadOnlyList<HandpieceEntityField> values)
        {
            return values.Aggregate(HandpieceEntityField.None, (acc, x) => acc | x);
        }

        private static String ToFlagDisplayString(this HandpieceEntityField value)
        {
            return value switch
            {
                HandpieceEntityField.Brand => "Brand",
                HandpieceEntityField.Model => "Model",
                HandpieceEntityField.Serial => "Serial",
                HandpieceEntityField.Components => "Components",
                HandpieceEntityField.Rating => "Rating",
                HandpieceEntityField.SpeedType => "Speed type",
                HandpieceEntityField.Speed => "Speed",
                HandpieceEntityField.Parts => "Parts",
                HandpieceEntityField.PartsOutOfStock => "Parts out of stock (legacy)",
                HandpieceEntityField.PartsComment => "Parts comment",
                HandpieceEntityField.PartsOrdered => "Parts ordered (legacy)",
                HandpieceEntityField.PartsRestocked => "Parts restocked (legacy)",
                HandpieceEntityField.ProblemDescription => "Problem description",
                HandpieceEntityField.DiagnosticReport => "Diagnostic report",
                HandpieceEntityField.ReturnBy => "Return by",
                HandpieceEntityField.CostOfRepair => "Cost of repair",
                HandpieceEntityField.ServiceLevel => "Service level",
                HandpieceEntityField.InternalComment => "Internal comment",
                HandpieceEntityField.PublicComment => "Public comment",
                HandpieceEntityField.EstimatedBy => "Estimated by",
                _ => value.ToString(),
            };
        }
    }
}
