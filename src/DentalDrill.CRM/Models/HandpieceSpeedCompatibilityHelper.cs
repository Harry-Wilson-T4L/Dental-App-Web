using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DentalDrill.CRM.Models
{
    public static class HandpieceSpeedCompatibilityHelper
    {
        public static IEnumerable<SelectListItem> MakeSelectList()
        {
            return new SelectList(
                new[]
                {
                    Tuple.Create(HandpieceSpeedCompatibility.Other, "Other"),
                    Tuple.Create(HandpieceSpeedCompatibility.LowSpeed, "Low speed"),
                    Tuple.Create(HandpieceSpeedCompatibility.HighSpeed, "High speed"),
                }, "Item1", "Item2");
        }

        public static String ToDisplayString(this HandpieceSpeedCompatibility value)
        {
            switch (value)
            {
                case HandpieceSpeedCompatibility.All:
                    return "All";
                case HandpieceSpeedCompatibility.None:
                    return "None";
            }

            return String.Join(", ", value.SplitValue().Select(PrimitiveValueToDisplayString));

            String PrimitiveValueToDisplayString(HandpieceSpeedCompatibility primitiveValue)
            {
                return primitiveValue switch
                {
                    HandpieceSpeedCompatibility.Other => "Other",
                    HandpieceSpeedCompatibility.LowSpeed => "Low speed",
                    HandpieceSpeedCompatibility.HighSpeed => "High speed",
                    _ => throw new InvalidOperationException(),
                };
            }
        }

        public static HandpieceSpeedCompatibility[] SplitValue(this HandpieceSpeedCompatibility value)
        {
            var list = new List<HandpieceSpeedCompatibility>();
            if (value.HasFlag(HandpieceSpeedCompatibility.Other))
            {
                list.Add(HandpieceSpeedCompatibility.Other);
            }

            if (value.HasFlag(HandpieceSpeedCompatibility.LowSpeed))
            {
                list.Add(HandpieceSpeedCompatibility.LowSpeed);
            }

            if (value.HasFlag(HandpieceSpeedCompatibility.HighSpeed))
            {
                list.Add(HandpieceSpeedCompatibility.HighSpeed);
            }

            return list.ToArray();
        }

        public static HandpieceSpeedCompatibility CombineValue(this HandpieceSpeedCompatibility[] value)
        {
            if (value == null)
            {
                return HandpieceSpeedCompatibility.None;
            }

            return value.Aggregate(HandpieceSpeedCompatibility.None, (acc, x) => acc | x);
        }

        public static HandpieceSpeedCompatibility? CombineValueOrDefault(this HandpieceSpeedCompatibility[] value)
        {
            if (value == null || value.Length == 0)
            {
                return null;
            }

            return value.Aggregate(HandpieceSpeedCompatibility.None, (acc, x) => acc | x);
        }

        public static IReadOnlyList<HandpieceSpeed> ToSupportedHandpieceSpeed(this HandpieceSpeedCompatibility value)
        {
            var split = value.SplitValue();
            return split.Select(x => x switch
            {
                HandpieceSpeedCompatibility.Other => HandpieceSpeed.Other,
                HandpieceSpeedCompatibility.LowSpeed => HandpieceSpeed.LowSpeed,
                HandpieceSpeedCompatibility.HighSpeed => HandpieceSpeed.HighSpeed,
                _ => throw new ArgumentOutOfRangeException(nameof(x), x, null),
            }).ToList();
        }
    }
}
