using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace DentalDrill.CRM.Models.Permissions
{
    public static class ClientEntityFieldHelper
    {
        public static IReadOnlyList<ClientEntityField> GetAllFlags()
        {
            return new ClientEntityField[]
            {
                ClientEntityField.ClientNo,
                ClientEntityField.Name,
                ClientEntityField.PrincipalDentist,
                ClientEntityField.Corporate,
                ClientEntityField.MainEmail,
                ClientEntityField.OtherEmails,
                ClientEntityField.MainPhone,
                ClientEntityField.OtherPhones,
                ClientEntityField.Address,
                ClientEntityField.OtherContact,
                ClientEntityField.OpeningHours,
                ClientEntityField.Brands,
                ClientEntityField.PricingCategory,
                ClientEntityField.Comment,
                ClientEntityField.PrimaryWorkshop,
            };
        }

        public static IEnumerable<SelectListItem> MakeSelectList()
        {
            return new SelectList(ClientEntityFieldHelper.GetAllFlags().Select(x => Tuple.Create(x, x.ToFlagDisplayString())), "Item1", "Item2");
        }

        public static String ToDisplayString(this ClientEntityField value)
        {
            return value switch
            {
                ClientEntityField.All => "All",
                ClientEntityField.None => "None",
                _ => String.Join(", ", value.SplitValue().Select(x => x.ToFlagDisplayString())),
            };
        }

        public static IReadOnlyList<ClientEntityField> SplitValue(this ClientEntityField value)
        {
            return ClientEntityFieldHelper.GetAllFlags().Where(flag => (value & flag) == flag).ToList();
        }

        public static ClientEntityField CombineValue(this IReadOnlyList<ClientEntityField> values)
        {
            return (values ?? Array.Empty<ClientEntityField>()).Aggregate(ClientEntityField.None, (acc, x) => acc | x);
        }

        private static String ToFlagDisplayString(this ClientEntityField value)
        {
            return value switch
            {
                ClientEntityField.ClientNo => "Client #",
                ClientEntityField.Name => "Name",
                ClientEntityField.PrincipalDentist => "Principal dentist",
                ClientEntityField.Corporate => "Corporate",
                ClientEntityField.MainEmail => "Main email",
                ClientEntityField.OtherEmails => "Other emails",
                ClientEntityField.MainPhone => "Main phone",
                ClientEntityField.OtherPhones => "Other phones",
                ClientEntityField.Address => "Address",
                ClientEntityField.OtherContact => "Other contact",
                ClientEntityField.OpeningHours => "Opening hours",
                ClientEntityField.Brands => "Brands",
                ClientEntityField.PricingCategory => "Pricing category",
                ClientEntityField.Comment => "Comment",
                ClientEntityField.PrimaryWorkshop => "Primary workshop",
                _ => value.ToString(),
            };
        }
    }
}
