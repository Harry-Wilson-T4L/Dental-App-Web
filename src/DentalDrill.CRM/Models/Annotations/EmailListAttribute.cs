using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DentalDrill.CRM.Models.Annotations
{
    public class EmailListAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(Object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success;
            }

            if (value is String stringValue)
            {
                stringValue = stringValue.Trim();
                if (String.IsNullOrEmpty(stringValue))
                {
                    return ValidationResult.Success;
                }

                var parts = stringValue.Split(',', StringSplitOptions.RemoveEmptyEntries);
                foreach (var part in parts)
                {
                    var trimmedPart = part.Trim();
                    if (!String.IsNullOrEmpty(trimmedPart) && !this.IsValidEmail(trimmedPart))
                    {
                        return new ValidationResult(this.FormatErrorMessage(validationContext.DisplayName));
                    }
                }

                return ValidationResult.Success;
            }

            throw new InvalidOperationException("Type of the value is invalid");
        }

        private Boolean IsValidEmail(String email)
        {
            return new EmailAddressAttribute().IsValid(email);
        }
    }
}
