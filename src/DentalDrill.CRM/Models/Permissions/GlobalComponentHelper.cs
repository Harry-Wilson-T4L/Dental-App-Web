using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DentalDrill.CRM.Models.Permissions
{
    public static class GlobalComponentHelper
    {
        public static IReadOnlyList<GlobalComponent> GetAllFlags()
        {
            return new GlobalComponent[]
            {
                GlobalComponent.User,
                GlobalComponent.Employee,
                GlobalComponent.EmployeeRole,
                GlobalComponent.Workshop,
                GlobalComponent.Corporate,
                GlobalComponent.PickupRequest,
                GlobalComponent.CorporatePricing,
                GlobalComponent.Feedback,
                GlobalComponent.Report,
                GlobalComponent.EmailTemplate,
                GlobalComponent.EmailReminder,
                GlobalComponent.HandpiecesDirectory,
                GlobalComponent.HandpiecesStore,
                GlobalComponent.HandpiecesOrder,
                GlobalComponent.State,
                GlobalComponent.Zone,
                GlobalComponent.DiagnosticChecklist,
                GlobalComponent.ProblemOption,
                GlobalComponent.ReturnEstimate,
                GlobalComponent.ServiceLevel,
                GlobalComponent.Tutorial,
            };
        }

        public static IEnumerable<SelectListItem> MakeSelectList()
        {
            return new SelectList(GlobalComponentHelper.GetAllFlags().Select(x => Tuple.Create(x, x.ToFlagDisplayString())), "Item1", "Item2");
        }

        public static String ToDisplayString(this GlobalComponent value)
        {
            return value switch
            {
                GlobalComponent.All => "All",
                GlobalComponent.None => "None",
                _ => String.Join(", ", value.SplitValue().Select(x => x.ToFlagDisplayString())),
            };
        }

        public static IReadOnlyList<GlobalComponent> SplitValue(this GlobalComponent value)
        {
            return GlobalComponentHelper.GetAllFlags().Where(flag => (value & flag) == flag).ToList();
        }

        public static GlobalComponent CombineValue(this IReadOnlyList<GlobalComponent> values)
        {
            return (values ?? Array.Empty<GlobalComponent>()).Aggregate(GlobalComponent.None, (acc, x) => acc | x);
        }

        private static String ToFlagDisplayString(this GlobalComponent value)
        {
            return value switch
            {
                GlobalComponent.User => "User",
                GlobalComponent.Employee => "Employee",
                GlobalComponent.EmployeeRole => "Employee role",
                GlobalComponent.Workshop => "Workshop",
                GlobalComponent.Corporate => "Corporate",
                GlobalComponent.PickupRequest => "Pickup request",
                GlobalComponent.CorporatePricing => "Corporate pricing",
                GlobalComponent.Feedback => "Feedback",
                GlobalComponent.Report => "Report",
                GlobalComponent.EmailTemplate => "Email template",
                GlobalComponent.EmailReminder => "Email reminder",
                GlobalComponent.HandpiecesDirectory => "Handpiece directory",
                GlobalComponent.HandpiecesStore => "Handpiece store",
                GlobalComponent.HandpiecesOrder => "Handpiece order",
                GlobalComponent.State => "State",
                GlobalComponent.Zone => "Zone",
                GlobalComponent.DiagnosticChecklist => "Diagnostic checklist",
                GlobalComponent.ProblemOption => "Problem option",
                GlobalComponent.ReturnEstimate => "Return estimate",
                GlobalComponent.ServiceLevel => "Service level",
                GlobalComponent.Tutorial => "Tutorial",
                _ => value.ToString(),
            };
        }
    }
}
