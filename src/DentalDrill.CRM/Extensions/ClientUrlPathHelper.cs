using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalDrill.CRM.Extensions
{
    public static class ClientUrlPathHelper
    {
        private static readonly HashSet<Char> AllowedCharacters = new HashSet<Char> { '-', '.', '_', '~' };

        private static readonly Dictionary<Char, Char> ReplacedCharacters = new Dictionary<Char, Char>
        {
            [' '] = '-',
        };

        public static String ConvertToUrlPath(String name)
        {
            if (String.IsNullOrEmpty(name))
            {
                return String.Empty;
            }

            var sb = new StringBuilder();
            foreach (var character in name)
            {
                if (character <= 0x007a && Char.IsLetterOrDigit(character))
                {
                    sb.Append(character);
                }
                else if (ClientUrlPathHelper.AllowedCharacters.Contains(character))
                {
                    sb.Append(character);
                }
                else if (ClientUrlPathHelper.ReplacedCharacters.TryGetValue(character, out var replacement))
                {
                    sb.Append(replacement);
                }
            }

            return sb.ToString();
        }

        public static String ConvertToUsername(String name)
        {
            if (String.IsNullOrEmpty(name))
            {
                return String.Empty;
            }

            var sb = new StringBuilder();
            foreach (var character in name)
            {
                if (character <= 0x007a && Char.IsLetterOrDigit(character))
                {
                    sb.Append(character);
                }
            }

            return sb.ToString();
        }
    }
}
