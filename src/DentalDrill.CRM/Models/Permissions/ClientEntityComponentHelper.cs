using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DentalDrill.CRM.Models.Permissions
{
    public static class ClientEntityComponentHelper
    {
        public static IReadOnlyList<ClientEntityComponent> GetAllFlags()
        {
            return new ClientEntityComponent[]
            {
                ClientEntityComponent.Client,
                ClientEntityComponent.Note,
                ClientEntityComponent.Callback,
                ClientEntityComponent.Repair,
                ClientEntityComponent.Email,
                ClientEntityComponent.Invoice,
                ClientEntityComponent.Feedback,
                ClientEntityComponent.Attachment,
                ClientEntityComponent.Appearance,
                ClientEntityComponent.Access,
            };
        }

        public static IEnumerable<SelectListItem> MakeSelectList()
        {
            return new SelectList(ClientEntityComponentHelper.GetAllFlags().Select(x => Tuple.Create(x, x.ToFlagDisplayString())), "Item1", "Item2");
        }

        public static String ToDisplayString(this ClientEntityComponent value)
        {
            return value switch
            {
                ClientEntityComponent.All => "All",
                ClientEntityComponent.None => "None",
                _ => String.Join(", ", value.SplitValue().Select(x => x.ToFlagDisplayString())),
            };
        }

        public static IReadOnlyList<ClientEntityComponent> SplitValue(this ClientEntityComponent value)
        {
            return ClientEntityComponentHelper.GetAllFlags().Where(flag => (value & flag) == flag).ToList();
        }

        public static ClientEntityComponent CombineValue(this IReadOnlyList<ClientEntityComponent> values)
        {
            return (values ?? Array.Empty<ClientEntityComponent>()).Aggregate(ClientEntityComponent.None, (acc, x) => acc | x);
        }

        private static String ToFlagDisplayString(this ClientEntityComponent value)
        {
            return value switch
            {
                ClientEntityComponent.Client => "Client",
                ClientEntityComponent.Note => "Note",
                ClientEntityComponent.Callback => "Callback",
                ClientEntityComponent.Repair => "Repair",
                ClientEntityComponent.Email => "Email",
                ClientEntityComponent.Invoice => "Invoice",
                ClientEntityComponent.Feedback => "Feedback",
                ClientEntityComponent.Attachment => "Attachment",
                ClientEntityComponent.Appearance => "Appearance",
                ClientEntityComponent.Access => "Access",
                _ => value.ToString(),
            };
        }
    }
}
