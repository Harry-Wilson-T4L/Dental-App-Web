using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalDrill.CRM.Services
{
    public class NameNormalizationService : INameNormalizationService
    {
        private static readonly HashSet<Char> AllowedCharacters = new HashSet<Char>("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789");

        public String NormalizeName(String originalName)
        {
            var sb = new StringBuilder(originalName.Length, originalName.Length);
            for (var i = 0; i < originalName.Length; i++)
            {
                var character = Char.ToUpperInvariant(originalName[i]);
                if (Char.IsWhiteSpace(character))
                {
                    continue;
                }

                if (NameNormalizationService.AllowedCharacters.Contains(character))
                {
                    sb.Append(character);
                    continue;
                }

                sb.Append("_");
            }

            return sb.ToString();
        }
    }
}
