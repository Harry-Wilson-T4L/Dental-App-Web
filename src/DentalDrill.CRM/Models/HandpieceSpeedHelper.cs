using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DentalDrill.CRM.Models
{
    public static class HandpieceSpeedHelper
    {
        public static IEnumerable<SelectListItem> MakeSelectList()
        {
            return new SelectList(
                new[]
                {
                    Tuple.Create(HandpieceSpeed.Other, "Other"),
                    Tuple.Create(HandpieceSpeed.LowSpeed, "Low speed"),
                    Tuple.Create(HandpieceSpeed.HighSpeed, "High speed"),
                }, "Item1", "Item2");
        }

        public static String ToDisplayString(this HandpieceSpeed value)
        {
            switch (value)
            {
                case HandpieceSpeed.Other:
                    return "Other";
                case HandpieceSpeed.LowSpeed:
                    return "Low speed";
                case HandpieceSpeed.HighSpeed:
                    return "High speed";
                default:
                    throw new InvalidOperationException();
            }
        }
    }
}
